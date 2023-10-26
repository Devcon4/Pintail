using Ardalis.GuardClauses;
using Pintail.Domain.Core;

namespace Pintail.Domain.Aggregates.SiteAggregate;

public class Site : BaseEntity<Guid>, IAggregateRoot {
  public string Name { get; private set; }
  public Uri Url { get; private set; }
  public int Ordinal { get; private set; }

  public Site(Guid id, string name, Uri url, int ordinal) {
    Id = Guard.Against.Default(id);
    Name = Guard.Against.NullOrWhiteSpace(name).Trim();
    Url = Guard.Against.Null(url, nameof(url));
    Ordinal = Guard.Against.Negative(ordinal, nameof(ordinal));
  }

  public void UpdateName(string name) {
    Name = Guard.Against.NullOrWhiteSpace(name).Trim();
  }

  public void UpdateOrdinal(int ordinal) {
    Ordinal = Guard.Against.Negative(ordinal, nameof(ordinal));
  }
}