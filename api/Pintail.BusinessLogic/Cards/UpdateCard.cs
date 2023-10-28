using Ardalis.GuardClauses;
using MediatR;
using Pintail.Domain.Aggregates.BoardAggregate;
using Pintail.Domain.Core;
using Pintail.Domain.ValueObjects;

namespace Pintail.BusinessLogic.Cards;

// Request to update a card on a board.
public record UpdateCard {
  public record Command: IRequest {

    public Command(string body, float x, float y) {
      Body = Guard.Against.NullOrWhiteSpace(body, nameof(body));
      X = Guard.Against.Default(x, nameof(x));
      Y = Guard.Against.Default(y, nameof(y));
    }

    public Guid BoardId { get; set; }
    public Guid CardId { get; set; }
    public string Body { get; }
    public float X { get; }
    public float Y { get; }
  }

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

      card.Body = new TextMultiline(request.Body);
      card.Position = new Position(request.X, request.Y);

      await boardRepo.UpdateAsync(board, cancellationToken);
    }
  }
}