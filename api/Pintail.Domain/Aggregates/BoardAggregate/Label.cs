using Pintail.Domain.Core;
using Pintail.Domain.ValueObjects;

namespace Pintail.Domain.Aggregates;

public class Label: BaseEntity<Guid> {
  public Guid BoardId {get;set;}
  public Position Position {get;set;}
  public TextLine Body {get;set;}

  public Label(Guid boardId, Position position, TextLine body) {
    BoardId = boardId;
    Position = position;
    Body = body;
  }
}