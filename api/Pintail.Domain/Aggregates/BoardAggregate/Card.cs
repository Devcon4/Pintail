using Pintail.Domain.Aggregates.BoardAggregate;
using Pintail.Domain.Core;
using Pintail.Domain.ValueObjects;

namespace Pintail.Domain.Aggregates.BoardAggregate;

public class Card: BaseEntity<Guid> {
  public Guid BoardId {get;set;}
  public Board Board {get;set;} = null!;
  public Position Position {get;set;}
  public TextMultiline Body {get;set;}

  // EF Core requires a parameterless constructor
  private Card() {}

  public Card(Board board, Position position, TextMultiline body) {
    Id = Guid.NewGuid();
    Board = board;
    Position = position;
    Body = body;
  }
}