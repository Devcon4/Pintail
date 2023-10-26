using Pintail.Domain.Core;

namespace Pintail.Domain.Aggregates;

public class TextLine : ValueObject
{
  private readonly string Body;

  public TextLine(string Body) {
    this.Body = Body.Replace("\n", "").Replace("\r", "");
  }

  protected override IEnumerable<IComparable> GetEqualityComponents()
  {
    yield return Body;
  }
}