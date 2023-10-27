using Ardalis.GuardClauses;
using MediatR;
using Pintail.Domain.Aggregates;
using Pintail.Domain.Aggregates.BoardAggregate;
using Pintail.Domain.Core;
using Pintail.Domain.ValueObjects;

namespace Pintail.BusinessLogic.Cards;

public record CreateCard {
  public record Command(Guid BoardId, float X, float Y, string Body): IRequest<CardDto> { }

  public class Handler(IRepository<Board> boardRepo): IRequestHandler<Command, CardDto> {
    public async Task<CardDto> Handle(Command request, CancellationToken cancellationToken) {
      var board = await boardRepo.FirstOrDefaultAsync(new GetFullBoardSpec(request.BoardId), cancellationToken);

      if (board == null) {
        throw new NotFoundException(request.BoardId.ToString(), "Board not found with matching id");
      }

      var card = new Card(board, new Position(request.X, request.Y), new TextMultiline(request.Body));
      board.AddCard(card);
      await boardRepo.UpdateAsync(board, cancellationToken);

      return new CardDto(card);
    }
  }
}