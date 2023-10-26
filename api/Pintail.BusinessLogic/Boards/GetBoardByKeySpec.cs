using Ardalis.Specification;
using Pintail.Domain.Aggregates.Board;

namespace Pintail.BusinessLogic.Boards;

public class GetBoardByKeySpec: Specification<Board> {
  public GetBoardByKeySpec(string key) {
    Query.Where(b => b.ShortGuid.Key == key);
  }
}
