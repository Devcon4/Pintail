using Pintail.BusinessLogic.Sites;
using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Pintail.WebApi.Controllers;

public class SiteRoutes : IRouteBundle {
  public void RegisterRoutes(WebApplication app) {
    var group = app.MapGroup("/api/sites").WithTags("Sites");

    group.MapGet("", GetSitesRoute);
    group.MapGet("{id}", GetSiteRoute);
  }

  public async Task<IEnumerable<SiteDto>> GetSitesRoute(IMediator mediator) => await mediator.Send(new GetSites.Query());
  public async Task<SiteDto> GetSiteRoute(IMediator mediator, Guid id) => await mediator.Send(new GetSite.Query(id));
}

// MVC controller version of above class
[ApiController]
[Route("api/sites")]
public class SiteController : ControllerBase {
  private readonly IMediator _mediator;

  public SiteController(IMediator mediator) {
    _mediator = mediator;
  }

  [HttpGet]
  public async Task<IEnumerable<SiteDto>> GetSitesRoute() => await _mediator.Send(new GetSites.Query());

  [HttpGet("{id}")]
  public async Task<SiteDto> GetSiteRoute(Guid id) => await _mediator.Send(new GetSite.Query(id));
}
