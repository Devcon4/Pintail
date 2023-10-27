using MediatR;
using Pintail.BusinessLogic.Boards;

namespace Pintail.WebApi.Routes;

public class BoardRoutes : IRouteBundle {
  public void RegisterRoutes(WebApplication app) {
    var group = app.MapGroup("/api/boards").WithTags("Boards");

    group.MapGet("", GetBoardsRoute);
    group.MapGet("{key}", GetBoardRoute);
  }

  public async Task<IEnumerable<BoardDto>> GetBoardsRoute(IMediator mediator) => await mediator.Send(new GetBoards.Query());
  public async Task<BoardDto> GetBoardRoute(IMediator mediator, string key) => await mediator.Send(new GetBoard.Query(key));
}