using Ardalis.GuardClauses;
using Ardalis.Specification;
using Pintail.Domain.Aggregates.CheckAggregate;
using Pintail.Domain.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Pintail.BusinessLogic.Settings;

public class GetSettings {
  public record Query : IRequest<SettingDto> {
    public int Count { get; init; }

    public Query(int count = 100) {
      Count = Guard.Against.OutOfRange(count, nameof(count), 1, 10000);
    }
  }

  public class Handler : IRequestHandler<Query, SettingDto> {
    private readonly ILogger<Handler> _logger;
    private readonly IOptionsMonitor<SettingOptions> _settings;

    public Handler(IOptionsMonitor<SettingOptions> settings, ILogger<Handler> logger) {
      _logger = logger;
      _settings = settings;
    }

    public async Task<SettingDto> Handle(Query request, CancellationToken cancellationToken) {
      _logger.LogInformation("Getting settings!");
      var setting = _settings.CurrentValue;

      return await Task.FromResult(new SettingDto(setting.AppName));
    }
  }
}