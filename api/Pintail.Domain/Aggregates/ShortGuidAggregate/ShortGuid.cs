using Pintail.Domain.Core;

namespace Pintail.Domain.Aggregates;

public class ShortGuid: BaseEntity<Guid>, IAggregateRoot {
  public Guid ParentGuid { get; set; }
  public string Key { get; set; }

  public ShortGuid(Guid parentGuid) {
    ParentGuid = parentGuid;
    // Not garanteed to be unique, Could be improved.
    Key = parentGuid.ToString("n").Substring(0, 8);
  }
}