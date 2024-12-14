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

    public Vector2D MultiplyWith(Matrix2x2<long> matrix)
    {
        long x = matrix.A * X + matrix.B * Y;
        long y = matrix.C * X + matrix.D * Y;
        return new(x, y);
    }
    public Vector2D MultiplyWith(Matrix2x2<double> matrix)
    {
        long x = Convert.ToInt64(matrix.A * X + matrix.B * Y) +1;
        long y = Convert.ToInt64(matrix.C * X + matrix.D * Y) +1;
        return new(x, y);
    }
}
