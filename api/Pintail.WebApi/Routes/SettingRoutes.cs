using Pintail.BusinessLogic.Settings;
using MediatR;

public interface IRouteBundle {
  public void RegisterRoutes(WebApplication app);
}
public class SettingRoutes : IRouteBundle {

  public void RegisterRoutes(WebApplication app) {
    var group = app.MapGroup("/api/settings").WithTags("Settings");

    group.MapGet("/", GetSettingsRoute);

  }

  public async Task<SettingDto> GetSettingsRoute(IMediator mediator) => await mediator.Send(new GetSettings.Query());

}