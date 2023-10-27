using Ardalis.Specification;
using Pintail.Domain.Aggregates.BoardAggregate;

namespace Pintail.BusinessLogic.Cards;

// GetFullBoardSpec
public class GetFullBoardSpec: Specification<Board> {
  public GetFullBoardSpec(Guid boardId) {
    Query.Where(b => b.Id == boardId)
      .Include(b => b.Cards)
      .Include(b => b.Labels);
  }
}