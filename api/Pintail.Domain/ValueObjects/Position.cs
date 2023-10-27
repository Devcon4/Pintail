using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Pintail.Domain.Core;

namespace Pintail.Domain.ValueObjects;

public class Position(Vector2 Vector): ValueObject {

  public float X => Vector.X;
  public float Y => Vector.Y;

  public Position(float x, float y): this(new Vector2(x, y)) {}

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