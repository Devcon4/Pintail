namespace Pintail.WebApi;

public static class ConfigurationBulderExtensions {
  public static IConfigurationBuilder AddConfigFiles(this IConfigurationBuilder config) {
    config.AddJsonFile("settings/appsettings.json", optional : true, reloadOnChange : true);
    config.AddJsonFile("secrets/appsettings.json", optional : true, reloadOnChange : true);
    return config;
  }
}