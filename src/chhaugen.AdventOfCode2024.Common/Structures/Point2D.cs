namespace chhaugen.AdventOfCode2024.Common.Structures;
public readonly struct Point2D : IEquatable<Point2D>
{
    public Point2D(long x, long y)
    {
        (X, Y) = (x, y);
    }
    public long X { get; }
    public long Y { get; }

    public void Deconstruct(out long x, out long y)
        => (x,y) = (X,Y);

    public Point2D West
        => new(X - 1, Y);

    public Point2D NorthWest
        => new(X - 1, Y - 1);

    public Point2D North
        => new(X, Y - 1);

    public Point2D NorthEast
        => new(X + 1, Y - 1);

    public Point2D East
        => new(X + 1, Y);

    public Point2D SouthEast
        => new(X + 1, Y + 1);

    public Point2D South
        => new(X, Y + 1);

    public Point2D SouthWest
        => new(X - 1, Y + 1);

    public Point2D Add(Vector2D vector)
        => new(X + vector.X, Y + vector.Y);

    public Vector2D AsVector()
        => new(this);

    public Point2D GetPointInDirection(CardinalDirection direction)
        => direction switch
        {
            CardinalDirection.West  => West,
            CardinalDirection.North => North,
            CardinalDirection.East  => East,
            CardinalDirection.South => South,
            _ => throw new NotImplementedException(),
        };

    public Point2D GetPointInDirection(OrdinalDirection direction)
        => direction switch
        {
            OrdinalDirection.NorthEast => NorthEast,
            OrdinalDirection.SouthEast => SouthEast,
            OrdinalDirection.SouthWest => SouthWest,
            OrdinalDirection.NorthWest => NorthWest,
            _ => throw new NotImplementedException(),
        };

    public Point2D GetPointInDirection(PrincipalWind direction)
        => direction switch
        {
            PrincipalWind.North     => North    ,
            PrincipalWind.NorthEast => NorthEast,
            PrincipalWind.East      => East     ,
            PrincipalWind.SouthEast => SouthEast,
            PrincipalWind.South     => South    ,
            PrincipalWind.SouthWest => SouthWest,
            PrincipalWind.West      => West     ,
            PrincipalWind.NorthWest => NorthWest,
            _ => throw new NotImplementedException(),
        };

    public bool Equals(Point2D other)
        => other.X == X && other.Y == Y;

    public override bool Equals(object? obj)
    {
        return obj is Point2D other && Equals(other);
    }

    public static bool operator ==(Point2D left, Point2D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Point2D left, Point2D right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
        => $"({X}, {Y})";
}
