using Ardalis.GuardClauses;
using MediatR;
using Pintail.Domain.Aggregates.BoardAggregate;
using Pintail.Domain.Core;

namespace Pintail.BusinessLogic.Cards;

// Request to delete a card from a board.
public record DeleteCard {
  public record Command(Guid BoardId, Guid CardId): IRequest { }

  public class Handler(IRepository<Board> boardRepo): IRequestHandler<Command> {
    public async Task Handle(Command request, CancellationToken cancellationToken) {
      var board = await boardRepo.FirstOrDefaultAsync(new GetFullBoardSpec(request.BoardId), cancellationToken);

      if (board == null) {
        throw new NotFoundException(request.BoardId.ToString(), "Board not found with matching id");
      }

      var card = board.Cards.FirstOrDefault(c => c.Id == request.CardId);

      if (card == null) {
        throw new NotFoundException(request.CardId.ToString(), "Card not found with matching id");
      }

      board.RemoveCard(card);
      await boardRepo.UpdateAsync(board, cancellationToken);
    }
  }
}
