using Pintail.Domain.Aggregates.SiteAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Pintail.Infrastructure;

public class DataSeeder : IDataSeeder {
  private readonly PintailDbContext _dbContext;
  private readonly ILogger<DataSeeder> _logger;

  public DataSeeder(PintailDbContext dbContext, ILogger<DataSeeder> logger) {
    _dbContext = dbContext;
    _logger = logger;
  }

  public async Task SeedAsync() {
    _logger.LogInformation("Migrating database...");
    await _dbContext.Database.MigrateAsync();
    _logger.LogInformation("Database migration completed.");

    _logger.LogInformation("Seeding database...");
    // await InsertIFNotEmpty(GetSites());

    _logger.LogInformation("Database seeding completed.");
  }

  public async Task InsertIFNotEmpty<T>(List<T> entities) where T : class {
    if (!await _dbContext.Set<T>().AnyAsync()) {
      await _dbContext.Set<T>().AddRangeAsync(entities);
      await _dbContext.SaveChangesAsync();
    }
  }

  // public List<Site> GetSites() => new() {
  //   new Site(Guid.NewGuid(), "Site 1", new Uri("http://localhost:4530")),
  // };

}