using Pintail.Domain.Core;

namespace Pintail.Domain.Aggregates;

public class TextMultiline(string Body) : ValueObject
{
  public string Value => Body;
  protected override IEnumerable<IComparable> GetEqualityComponents()
  {
    yield return Body;
  }
}