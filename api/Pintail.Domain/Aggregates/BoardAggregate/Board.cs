using System.Collections.Frozen;
using Pintail.Domain.Core;
using Pintail.Domain.ValueObjects;

namespace Pintail.Domain.Aggregates.BoardAggregate;

public class Board: BaseEntity<Guid>, IAggregateRoot {
  public ShortGuid ShortGuid { get; set; }

  private List<Card> _cards { get; set; } = [];
  public IEnumerable<Card> Cards => _cards.AsEnumerable();

  private List<Label> _labels { get; set; } = [];
  public IEnumerable<Label> Labels => _labels.AsEnumerable();

  public Board() {
    Id = Guid.NewGuid();
    ShortGuid = new ShortGuid(Id);
  }

  public Board(IEnumerable<Card> cards, IEnumerable<Label> labels): this() {

    foreach (var card in cards) {
      AddCard(card);
    }

    foreach (var label in labels) {
      AddLabel(label);
    }
  }

  public void AddCard(Card card) {
    _cards.Add(card);
  }

  public void RemoveCard(Card card) {
    _cards.Remove(card);
  }

  public void AddLabel(Label label) {
    _labels.Add(label);
  }

  public void RemoveLabel(Label label) {
    _labels.Remove(label);
  }
}