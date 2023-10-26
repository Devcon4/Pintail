using Ardalis.GuardClauses;
using MediatR;
using Pintail.Domain.Aggregates.Board;
using Pintail.Domain.Core;

namespace Pintail.BusinessLogic.Boards;

public class GetBoard {
  public record Query: IRequest<BoardDto> {
    public string Key {get;set;}

    public Query(string key) {
      Key = Guard.Against.NullOrWhiteSpace(key, nameof(key));
    }
  }

  public class Handler(IRepository<Board> boardRepo): IRequestHandler<Query, BoardDto> {
    public async Task<BoardDto> Handle(Query request, CancellationToken cancellationToken) {

      var board = await boardRepo.FirstOrDefaultAsync(new GetBoardByKeySpec(request.Key), cancellationToken);

      if (board == null) {
        throw new NotFoundException(request.Key, "Board not found with matching key");
      }

      return new BoardDto(board);
    }
  }
}
