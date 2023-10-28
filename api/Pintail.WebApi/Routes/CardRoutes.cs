using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pintail.BusinessLogic.Cards;

namespace Pintail.WebApi.Routes;

// Routes to getCards, CreateCard, DeleteCard.
public class CardRoutes : IRouteBundle
{
  public void RegisterRoutes(WebApplication app)
  {
    var group = app.MapGroup("/api/boards/{BoardId}/cards").WithTags("Cards");

    group.MapGet("", GetCardsRoute);
    group.MapPost("", CreateCardRoute);
    group.MapDelete("{CardId}", DeleteCardRoute);
    group.MapPut("{CardId}", UpdateCardRoute);
  }

  public async Task<IEnumerable<CardDto>> GetCardsRoute(IMediator mediator, Guid BoardId) => await mediator.Send(new GetCards.Query(BoardId));
  public async Task<CardDto> CreateCardRoute(IMediator mediator, CreateCard.Command command) => await mediator.Send(command);
  public async Task DeleteCardRoute(IMediator mediator, Guid BoardId, Guid CardId) => await mediator.Send(new DeleteCard.Command(BoardId, CardId));
  
  public async Task UpdateCardRoute(IMediator mediator, Guid BoardId, Guid CardId, UpdateCard.Command command) {
    command.BoardId = BoardId;
    command.CardId = CardId;
    await mediator.Send(command);
  }
}