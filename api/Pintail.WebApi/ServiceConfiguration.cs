using System.Reflection;
using Microsoft.AspNetCore.WebSockets;
using Pintail.WebApi.Routes;

namespace Pintail.WebApi;

public static class ServiceConfiguration {
  public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment) {
    services
      .AddCustomWebApi()
      .AddApplicationServices()
      .AddOptions(configuration)
      .AddOpenApi()
      .AddHealthChecks();

    return services;
  }

  private static IServiceCollection AddCustomWebApi(this IServiceCollection services) {
    services.AddWebSockets(options => options.KeepAliveInterval = TimeSpan.FromSeconds(120));
    services.AddControllers();

    return services;
  }

  private static IServiceCollection AddApplicationServices(this IServiceCollection services) {
    services.AddHttpContextAccessor();
    return services;
  }

  private static IServiceCollection AddOpenApi(this IServiceCollection services) {
    services.AddEndpointsApiExplorer();
    services.AddOpenApiDocument(opt => {
      opt.Title = "Pintail API";
      opt.Description = "Pintail API";
      opt.Version = "v1";
      opt.DocumentName = "v1";
    });

    return services;
  }

  private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration) {
    // TODO: Setup option configuration.
    return services;
  }

  public static void UseRouteGroups(this WebApplication app) {
    var routeBundles = Assembly.GetExecutingAssembly().GetTypes()
      .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i == typeof(IRouteBundle)));

    using var scope = app.Services.CreateScope();

    foreach (var routeBundle in routeBundles) {
      var routeBundleInstance = ActivatorUtilities.CreateInstance(scope.ServiceProvider, routeBundle);
      var method = routeBundle.GetMethod("RegisterRoutes");
      method?.Invoke(routeBundleInstance, new object[] { app });
    }
  }
}