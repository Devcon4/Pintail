using MediatR;
using Pintail.BusinessLogic.Boards;

namespace Pintail.WebApi.Routes;

public class BoardRoutes : IRouteBundle {
  public void RegisterRoutes(WebApplication app) {
    var group = app.MapGroup("/api/boards").WithTags("Boards");

    group.MapGet("", GetBoardsRoute);
    group.MapGet("{key}", GetBoardRoute);
    group.MapPost("", CreateBoardRoute);
    group.MapDelete("{id}", DeleteBoardRoute);
  }

  public async Task<IEnumerable<BoardDto>> GetBoardsRoute(IMediator mediator) => await mediator.Send(new GetBoards.Query());
  public async Task<BoardDto> GetBoardRoute(IMediator mediator, string key) => await mediator.Send(new GetBoard.Query(key));
  public async Task<BoardDto> CreateBoardRoute(IMediator mediator, CreateBoard.Command command) => await mediator.Send(command);
  public async Task DeleteBoardRoute(IMediator mediator, Guid id) => await mediator.Send(new DeleteBoard.Command(id));
}