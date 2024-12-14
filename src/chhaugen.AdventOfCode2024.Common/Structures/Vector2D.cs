using System.Numerics;

namespace chhaugen.AdventOfCode2024.Common.Structures;
public readonly struct Vector2D : IEquatable<Vector2D>
{
    
    public Vector2D(long x, long y)
    {
        (X, Y) = (x, y);
    }

    public Vector2D(Point2D point)
    {
        (X, Y) = point;
    }
    public Vector2D(Point2D fromPoint, Point2D toPoint)
    {
        X = toPoint.X - fromPoint.X;
        Y = toPoint.Y - fromPoint.Y;
    }

    public long X { get; }
    public long Y { get; }


    public double Length => Math.Sqrt(((double)X * X) + (Y * (double)Y));
    public long ManhattanLength => X + Y;

    public Vector2D Scale(long scale)
        => new(X * scale, Y * scale);

    public override string ToString()
        => $"[{X}, {Y}]";

    public bool Equals(Vector2D other)
        => X == other.X && Y == other.Y;

    public override bool Equals(object? obj)
        => obj is Vector2D other && Equals(other);

    public static bool operator ==(Vector2D left, Vector2D right)
        => left.Equals(right);

    public static bool operator !=(Vector2D left, Vector2D right)
        => !(left == right);

    public override int GetHashCode()
        => HashCode.Combine(X, Y);
}
