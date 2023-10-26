using Ardalis.Specification;
using MediatR;
using Pintail.Domain.Aggregates.Board;
using Pintail.Domain.Core;

namespace Pintail.BusinessLogic.Boards;

// mediator query/handler to get all boards

public class GetBoards {
  public record Query: IRequest<IEnumerable<BoardDto>> { }

  public class Handler(IRepository<Board> boardRepo): IRequestHandler<Query, IEnumerable<BoardDto>> {
    public async Task<IEnumerable<BoardDto>> Handle(Query request, CancellationToken cancellationToken) {
      var boards = await boardRepo.ListAsync(cancellationToken);

      return boards.Select(b => new BoardDto(b)).ToList();
    }
  }
}
