using Pintail.Domain.Core;
using Pintail.Domain.ValueObjects;

namespace Pintail.Domain.Aggregates;

public class Card: BaseEntity<Guid> {
  public Guid BoardId {get;set;}
  public Position Position {get;set;}
  public TextMultiline Body {get;set;}

  public Card(Guid boardId, Position position, TextMultiline body) {
    BoardId = boardId;
    Position = position;
    Body = body;
  }
}