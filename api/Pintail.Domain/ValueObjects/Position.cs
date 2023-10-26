using System.Numerics;
using Pintail.Domain.Core;

namespace Pintail.Domain.ValueObjects;

public class Position(Vector2 Vector): ValueObject {
  public override string ToString()
  {
    return Vector.ToString();
  }

  protected override IEnumerable<IComparable> GetEqualityComponents()
  {
    yield return Vector.X;
    yield return Vector.Y;
  }
}