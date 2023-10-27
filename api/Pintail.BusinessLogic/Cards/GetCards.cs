using System.Numerics;
using Ardalis.GuardClauses;
using Ardalis.Specification;
using MediatR;
using Pintail.BusinessLogic.Boards;
using Pintail.Domain.Aggregates.BoardAggregate;
using Pintail.Domain.Core;

namespace Pintail.BusinessLogic.Cards;

public class GetCards {
  public record Query(Guid BoardId): IRequest<IEnumerable<CardDto>> { }

  public class Handler(IRepository<Board> boardRepo): IRequestHandler<Query, IEnumerable<CardDto>> {
    public async Task<IEnumerable<CardDto>> Handle(Query request, CancellationToken cancellationToken) {
      var board = await boardRepo.FirstOrDefaultAsync(new GetBoardWithCardsSpec(request.BoardId), cancellationToken);

      if (board == null) {
        throw new NotFoundException(request.BoardId.ToString(), "Board not found with matching id");
      }

      return board.Cards.Select(c => new CardDto(c)).ToList();
    }
  }
}
