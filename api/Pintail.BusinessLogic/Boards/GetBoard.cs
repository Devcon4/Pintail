using Ardalis.GuardClauses;
using Ardalis.Specification;
using MediatR;
using Pintail.Domain.Aggregates;
using Pintail.Domain.Aggregates.BoardAggregate;
using Pintail.Domain.Core;
using Pintail.Domain.ValueObjects;

namespace Pintail.BusinessLogic.Boards;

public class GetBoard {
  public record Query: IRequest<BoardDto> {
    public string Key {get;set;}

    public Query(string key) {
      Guard.Against.NullOrWhiteSpace(key, nameof(key));
      Guard.Against.InvalidInput(key, nameof(key), k => key.Length == 8);
      Key = key;
    }
  }

  public class Handler(IRepository<Board> boardRepo): IRequestHandler<Query, BoardDto> {
    public async Task<BoardDto> Handle(Query request, CancellationToken cancellationToken) {

      // TODO: ShortGuid has issues translating to a SQL query. Filtering in memory for now.
      var boards = await boardRepo.ListAsync(cancellationToken);
      var board = boards.FirstOrDefault(b => b.ShortGuid.Key == request.Key);

      if (board == null) {
        throw new NotFoundException(request.Key, "Board not found with matching key");
      }

      return new BoardDto(board);
    }
  }
}
