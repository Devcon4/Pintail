using Pintail.Domain.Aggregates.Board;

namespace Pintail.BusinessLogic.Boards;

public record BoardDto {
  public Guid Id {get;set;}
  public string Key {get;set;}

  public BoardDto(Board board) {
    Id = board.Id;
    Key = board.ShortGuid.Key;
  }
}