using Pintail.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pintail.Infrastructure;

public static class ServiceConfiguration {

  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration) {
    services
      .ConfigureEntityFramework(configuration)
      .ConfigureRepository()
      .ConfigureSeed();

    return services;
  }

  private static IServiceCollection ConfigureEntityFramework(this IServiceCollection services, IConfiguration configuration) {
    // TODO: setup sqlite connection.
    services.AddDbContext<PintailDbContext>(options => {
      options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
    });
    return services;
  }

  private static IServiceCollection ConfigureRepository(this IServiceCollection services) {
    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

    return services;
  }

  private static IServiceCollection ConfigureSeed(this IServiceCollection services) {
    services.AddTransient<IDataSeeder, DataSeeder>();

    return services;
  }
}