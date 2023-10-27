using Ardalis.GuardClauses;
using Pintail.Domain.Core;

namespace Pintail.Domain.ValueObjects;

public class ShortGuid: ValueObject {
  public Guid ParentGuid { get; set; }
  public string Key { get; set; }

  // EF Core requires a parameterless constructor
  private ShortGuid() { }

  public ShortGuid(Guid parentGuid) {
    ParentGuid = parentGuid;
    // Not garanteed to be unique, Could be improved.
    Key = parentGuid.ToString("n").Substring(0, 8);
  }

  public ShortGuid(string key) {
    Guard.Against.NullOrWhiteSpace(key, nameof(key));
    Guard.Against.InvalidInput(key, nameof(key), k => key.Length == 8);
    Key = key;
  }

  protected override IEnumerable<IComparable> GetEqualityComponents()
  {
    yield return Key;
  }
}