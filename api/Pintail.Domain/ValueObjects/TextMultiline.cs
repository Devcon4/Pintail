using Pintail.Domain.Core;

namespace Pintail.Domain.ValueObjects;

public class TextMultiline(string Body) : ValueObject
{
  public string Value => Body;
  protected override IEnumerable<IComparable> GetEqualityComponents()
  {
    yield return Body;
  }
}