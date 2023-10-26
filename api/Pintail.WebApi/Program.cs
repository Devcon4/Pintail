using Pintail.BusinessLogic;
using Pintail.Infrastructure;
using Pintail.WebApi;
using Serilog;
using Serilog.Events;

// First stage logger config
Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
  .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
  .WriteTo.Console()
  .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfigFiles();

builder.Host.UseSerilog((context, services, configuration) => configuration
  .ReadFrom.Configuration(context.Configuration)
  .ReadFrom.Services(services));

builder.Services
  .AddBusinessLogicServices(builder.Configuration)
  .AddInfrastructureServices(builder.Configuration)
  .AddWebApiServices(builder.Configuration, builder.Environment);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseOpenApi();

  app.UseSwaggerUi3();
  app.UseReDoc(opt => opt.Path = "/redoc");
}

app.MapFallbackToFile("index.html");
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseRouteGroups();

app.UseAuthorization();

app.UseWebSockets();
app.MapHealthChecks("/health");
app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var loggingFactory = services.GetRequiredService<ILoggerFactory>();
var logger = loggingFactory.CreateLogger<Program>();

try {
  var seeders = services.GetServices<IDataSeeder>();
  foreach (var seeder in seeders) {
    await seeder.SeedAsync();
  }

  var devOnlySeeders = services.GetServices<IDevOnlyDataSeeder>();
  foreach (var seeder in devOnlySeeders) {
    await seeder.DevOnlySeedAsync();
  }
} catch (Exception ex) {
  logger.LogError(ex, "An error occurred while migrating or seeding the database.");
  throw;
}

app.Run();