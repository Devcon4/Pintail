using System.Collections.Frozen;
using Pintail.Domain.Core;

namespace Pintail.Domain.Aggregates.Board;

public class Board: BaseEntity<Guid>, IAggregateRoot {
  public Guid ShortGuid { get; set; }

  private HashSet<Card> _cards { get; set; } = [];
  public FrozenSet<Card> Cards => _cards.ToFrozenSet();

  private HashSet<Label> _labels { get; set; } = [];
  public FrozenSet<Label> Labels => _labels.ToFrozenSet();

  void AddCard(Card card) {
    _cards.Add(card);
  }

  void RemoveCard(Card card) {
    _cards.Remove(card);
  }

  void AddLabel(Label label) {
    _labels.Add(label);
  }

  void RemoveLabel(Label label) {
    _labels.Remove(label);
  }
}