using Pintail.Domain.Core;

namespace Pintail.Domain.ValueObjects;

public class TextLine : ValueObject
{
  private readonly string Body;

  public string Value => Body;
  public TextLine(string Body) {
    this.Body = Body.Replace("\n", "").Replace("\r", "");
  }

  protected override IEnumerable<IComparable> GetEqualityComponents()
  {
    yield return Body;
  }
}