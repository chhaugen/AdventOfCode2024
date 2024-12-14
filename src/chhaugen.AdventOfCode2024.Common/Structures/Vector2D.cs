using System.Numerics;

namespace chhaugen.AdventOfCode2024.Common.Structures;
public readonly struct Vector2D
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
}
