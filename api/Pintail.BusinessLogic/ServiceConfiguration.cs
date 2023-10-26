using System.Reflection;
using Pintail.BusinessLogic.Sites;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace Pintail.BusinessLogic;

public static class ServiceConfiguration {
  public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services, IConfiguration configuration) {
    services.AddSingleton<IPintailJobQueue<SiteJobConfig>, PintailJobQueue<SiteJobConfig>>();
    services.AddHostedService<PintailJob<SiteJobConfig>>();
    services.AddHostedService<SiteJob>();
    services.AddHttpClient();

    services.Configure<SettingOptions>(configuration.GetSection("Pintail:Settings"));

    services.AddMediatR(config => {
      config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    });

    return services;
  }
}