using Ardalis.Specification;
using Pintail.Domain.Aggregates.BoardAggregate;

namespace Pintail.BusinessLogic.Boards;

// Get cards for a board guid spec.
public class GetBoardWithCardsSpec: Specification<Board> {
  public GetBoardWithCardsSpec(Guid boardId) {
    Query.Where(b => b.Id == boardId).Include(b => b.Cards);
  }
}
