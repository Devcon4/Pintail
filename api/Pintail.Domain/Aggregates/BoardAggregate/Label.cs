using Pintail.Domain.Core;
using Pintail.Domain.ValueObjects;

namespace Pintail.Domain.Aggregates.BoardAggregate;

public class Label: BaseEntity<Guid> {
  public Guid BoardId {get;set;}
  public Board Board {get;set;} = null!;
  public Position Position {get;set;}
  public TextLine Body {get;set;}

  // EF Core requires a parameterless constructor
  private Label() {}

  public Label(Board board, Position position, TextLine body) {
    Id = Guid.NewGuid();
    Board = board;
    Position = position;
    Body = body;
  }
}