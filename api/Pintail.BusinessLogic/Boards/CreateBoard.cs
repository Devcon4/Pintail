using Ardalis.GuardClauses;
using MediatR;
using Pintail.Domain.Aggregates.BoardAggregate;
using Pintail.Domain.Core;
using Pintail.Domain.ValueObjects;

namespace Pintail.BusinessLogic.Boards;

// request to create a new board
public class CreateBoard {
  public record Command: IRequest<BoardDto> {
    public string Title {get;set;}

    public Command(string title) {
      Title = Guard.Against.NullOrWhiteSpace(title, nameof(title));
    }
  }

  public class Handler(IRepository<Board> boardRepo): IRequestHandler<Command, BoardDto> {
    public async Task<BoardDto> Handle(Command request, CancellationToken cancellationToken) {
      var board = new Board(new TextLine(request.Title));
      await boardRepo.AddAsync(board, cancellationToken);
      return new BoardDto(board);
    }
  }
}