using System.Threading.Channels;
using Pintail.BusinessLogic.Checks;
using Pintail.Domain.Aggregates.CheckAggregate;
using Pintail.Domain.Aggregates.SiteAggregate;
using Pintail.Domain.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Pintail.BusinessLogic.Sites;

public interface IPintailConfig { }

public class PintailConfig {
  public List<SiteConfig> Sites { get; set; }
}

public class SiteJobConfig : IPintailConfig { }

public class SiteConfig {
  public string Name { get; set; }
  public Uri Url { get; set; }
}

public class SiteJob : IHostedService {
  private readonly IServiceProvider _services;
  private readonly IPintailJobQueue<SiteJobConfig> _siteQueue;

  public SiteJob(IServiceProvider services, IPintailJobQueue<SiteJobConfig> siteQueue) {
    _services = services;
    _siteQueue = siteQueue;
  }

  public async Task StartAsync(CancellationToken cancellationToken) {
    using var scope = _services.CreateScope();
    var siteRepository = scope.ServiceProvider.GetRequiredService<IRepository<Site>>();
    var SiteLogger = scope.ServiceProvider.GetRequiredService<ILogger<SiteJob>>();
    SiteLogger.LogInformation("Starting SiteJob");

    await UpdateSitesFromConfig(cancellationToken);

    await _siteQueue.EnqueueAsync(async(config, token) => {
      var timer = new Timer(async _ => await CleanupJobs(token), null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
      await Task.CompletedTask;
    }, cancellationToken);

    var sites = await siteRepository.ListAsync(cancellationToken);

    foreach (var site in sites) {
      await _siteQueue.EnqueueAsync(async(config, token) => {
        var timer = new Timer(async _ => await SiteWork(site), null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        await Task.CompletedTask;
      }, cancellationToken);
    }
  }

  public async Task CleanupJobs(CancellationToken cancellationToken) {
    using var scope = _services.CreateScope();
    var checkRepository = scope.ServiceProvider.GetRequiredService<IRepository<Check>>();
    var checkLogger = scope.ServiceProvider.GetRequiredService<ILogger<SiteJob>>();

    var checks = await checkRepository.ListAsync(new GetStartedChecksSpec(DateTime.UtcNow.AddMinutes(-10)), cancellationToken);

    foreach (var check in checks) {
      checkLogger.LogInformation("Marking check {CheckId} as failed", check.Id);
      check.SetFailed();
      await checkRepository.UpdateAsync(check, cancellationToken);
    }
  }

  public async Task UpdateSitesFromConfig(CancellationToken cancellationToken) {
    using var scope = _services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<SiteJob>>();
    var siteRepository = scope.ServiceProvider.GetRequiredService<IRepository<Site>>();
    var pintailConfig = scope.ServiceProvider.GetRequiredService<IOptions<PintailConfig>>();
    var siteConfigs = pintailConfig.Value.Sites ?? new List<SiteConfig>();

    logger.LogInformation("Updating sites from config, {SiteCount} sites found", siteConfigs.Count());

    var existingSites = await siteRepository.ListAsync(cancellationToken);

    var adds = siteConfigs
      .Where(config => !existingSites.Any(site => site.Url == config.Url))
      .Select(config => new Site(Guid.NewGuid(), config.Name, config.Url, siteConfigs.FindIndex(originalConfig => originalConfig.Url == config.Url)));

    var deletes = existingSites
      .Where(site => !siteConfigs.Any(config => config.Url == site.Url));

    var updates = existingSites
      .Where(site => siteConfigs.Any(config => config.Url == site.Url))
      .Select(site => {
        var config = siteConfigs.First(config => config.Url == site.Url);
        site.UpdateName(config.Name);
        site.UpdateOrdinal(siteConfigs.FindIndex(originalConfig => originalConfig.Url == config.Url));
        return site;
      });

    await siteRepository.DeleteRangeAsync(deletes, cancellationToken);
    await siteRepository.AddRangeAsync(adds, cancellationToken);
    await siteRepository.UpdateRangeAsync(updates, cancellationToken);
    await siteRepository.SaveChangesAsync(cancellationToken);

    logger.LogInformation("Sites updated: +{Adds} added, -{Deletes} deleted, ~{Updates} updated", adds.Count(), deletes.Count(), updates.Count());
  }

  public async Task SiteWork(Site site) {
    using var scope = _services.CreateScope();
    var SiteLogger = scope.ServiceProvider.GetRequiredService<ILogger<SiteJob>>();
    var checkRepository = scope.ServiceProvider.GetRequiredService<IRepository<Check>>();
    var httpFac = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
    var http = httpFac.CreateClient();

    var check = new Check(site.Id);
    var l = SiteLogger.BeginScope(new { CheckId = check.Id, SiteId = site.Id });
    SiteLogger.LogInformation("Job Started");

    await checkRepository.AddAsync(check);
    await checkRepository.SaveChangesAsync();

    var req = new HttpRequestMessage(HttpMethod.Get, site.Url);

    HttpResponseMessage? res = null;

    try {
      res = await http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None);
    } catch (Exception ex) {
      SiteLogger.LogError(ex, "{Message}", ex.Message);
      check.SetFailed();
    }

    check = await checkRepository.GetByIdAsync(check.Id);

    if (check == null) {
      SiteLogger.LogError("Check {CheckId} not found", check?.Id);
      throw new Exception("Check not found");
    }

    if (res != null && res.IsSuccessStatusCode) {
      SiteLogger.LogInformation("Job Code: {StatusCode}", res.StatusCode);
      check.SetCompleted();
    }

    if (res != null && !res.IsSuccessStatusCode) {
      SiteLogger.LogError("Job Code: {StatusCode}", res.StatusCode);
      check.SetFailed();
    }

    await checkRepository.UpdateAsync(check);
    await checkRepository.SaveChangesAsync();
    SiteLogger.LogInformation("Job Status: {Status}", check.Status.Value);
    SiteLogger.LogInformation("Job Ended");

    l?.Dispose();
  }

  public async Task StopAsync(CancellationToken cancellationToken) {
    using var scope = _services.CreateScope();

    var logger = scope.ServiceProvider.GetRequiredService<ILogger<SiteJob>>();
    logger.LogInformation("SiteJob is stopping.");
    await Task.CompletedTask;
  }
}

public interface IPintailJob<TConfig> : IHostedService { }

public class PintailJob<TConfig> : IPintailJob<TConfig> where TConfig : class, IPintailConfig {
  private readonly TConfig _config;
  private readonly ILogger<PintailJob<TConfig>> _logger;
  private readonly IPintailJobQueue<TConfig> _checkQueue;

  public PintailJob(IOptions<TConfig> config, ILogger<PintailJob<TConfig>> logger, IPintailJobQueue<TConfig> checkQueue) {
    _config = config.Value;
    _logger = logger;
    _checkQueue = checkQueue;
  }

  public Task StopAsync(CancellationToken cancellationToken) {
    _logger.LogInformation("Job is stopping.");
    return Task.CompletedTask;
  }

  public async Task StartAsync(CancellationToken stoppingToken) {
    _logger.LogInformation("Job is starting.");

    var job = new Task(async() => {
      while (!stoppingToken.IsCancellationRequested) {
        _logger.LogInformation("Job is running.");
        var currentItem = await _checkQueue.DequeueAsync(stoppingToken);

        try {
          await currentItem(_config, stoppingToken);
        } catch (Exception ex) {
          _logger.LogError(ex, "Error occurred executing {Check}.", nameof(currentItem));
        }
      }
    }, stoppingToken);
    job.Start();

    await Task.CompletedTask;
  }
}

public interface IPintailJobQueue<TConfig> {
  Task EnqueueAsync(Func<TConfig, CancellationToken, ValueTask> item, CancellationToken cancellationToken);
  Task<Func<TConfig, CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
}

public class PintailJobQueue<TConfig> : IPintailJobQueue<TConfig> where TConfig : class, IPintailConfig {
  private readonly Channel<Func<TConfig, CancellationToken, ValueTask>> _queue;

  public PintailJobQueue() : this(100) { }

  public PintailJobQueue(int capacity) {
    var options = new BoundedChannelOptions(capacity) {
      FullMode = BoundedChannelFullMode.Wait,
    };

    _queue = Channel.CreateBounded<Func<TConfig, CancellationToken, ValueTask>>(options);
  }

  public async Task EnqueueAsync(Func<TConfig, CancellationToken, ValueTask> item, CancellationToken cancellationToken) {
    await _queue.Writer.WriteAsync(item, cancellationToken);
  }

  public async Task<Func<TConfig, CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken) {
    return await _queue.Reader.ReadAsync(cancellationToken);
  }
}