using Pintail.Domain.Core;

namespace Pintail.Domain.Aggregates;

public class TextMultiline(string Body) : ValueObject
{
  protected override IEnumerable<IComparable> GetEqualityComponents()
  {
    yield return Body;
  }
}