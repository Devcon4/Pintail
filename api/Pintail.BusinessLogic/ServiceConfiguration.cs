using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace Pintail.BusinessLogic;

public static class ServiceConfiguration {
  public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services, IConfiguration configuration) {
    services.AddHttpClient();


    services.AddMediatR(config => {
      config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    });

    return services;
  }
}