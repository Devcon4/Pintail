using Pintail.Domain.Aggregates.BoardAggregate;

namespace Pintail.BusinessLogic.Cards;

public record CardDto {
  public Guid Id {get;set;}
  public Guid BoardId {get;set;}
  public float X {get;set;}
  public float Y {get;set;}
  public string Body {get;set;}

  public CardDto(Card card) {
    Id = card.Id;
    BoardId = card.BoardId;
    X = card.Position.X;
    Y = card.Position.Y;
    Body = card.Body.Value;
  }
}