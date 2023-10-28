using Ardalis.GuardClauses;
using MediatR;
using Pintail.Domain.Aggregates.BoardAggregate;
using Pintail.Domain.Core;

namespace Pintail.BusinessLogic.Boards;

// request to delete a board by id
public class DeleteBoard {
  public record Command: IRequest {
    public Guid Id {get;set;}

    public Command(Guid id) {
      Id = Guard.Against.Default(id, nameof(id));
    }
  }

  public class Handler(IRepository<Board> boardRepo): IRequestHandler<Command> {
    public async Task Handle(Command request, CancellationToken cancellationToken) {
      var board = await boardRepo.GetByIdAsync(request.Id, cancellationToken);

      if (board == null) {
        throw new NotFoundException("Board not found with Id", request.Id.ToString());
      }
      await boardRepo.DeleteAsync(board, cancellationToken);
    }
  }
}