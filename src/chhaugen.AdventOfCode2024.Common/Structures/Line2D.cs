using System.Collections.Generic;

namespace chhaugen.AdventOfCode2024.Common.Structures;
public readonly struct Line2D : IEquatable<Line2D>
{
    public Line2D(double a, double b, double c)
    {
        A = a;
        B = b;
        C = c;
    }

    public Line2D(Point2D second)
    {
        A = second.Y;
        B = -second.X;
        C = 0;
    }

    public Line2D(Point2D first, Point2D second)
    {
        A = second.Y - first.Y;
        B = first.X - second.X;
        C = first.Y * (second.X - first.X) - (second.Y - first.Y) * first.X;
    }

    // x * A + y * B + C = 0
    public double A { get; }
    public double B { get; }
    public double C { get; }

    public double GetYValue(double x)
        => (-(A * x) - C) / B;

    public double GetXValue(double y)
        => (-(B * y) - C) / A;

    public double GetDistanceTo(Point2D point)
        => Math.Abs((A * point.X) + (B * point.Y) + C) / Math.Sqrt((A * A) + (B * B));

    public override string ToString()
        => $"{A}x {(double.IsPositive(B) ? '+' : '-')} {Math.Abs(B)}y {(double.IsPositive(C) ? '+' : '-')} {Math.Abs(C)} = 0";

    public bool Equals(Line2D other)
        => C == other.C && A / other.A == B / other.B;

    public override bool Equals(object? obj)
        => obj is Line2D other && Equals(other);

    public static bool operator ==(Line2D left, Line2D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Line2D left, Line2D right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
        => HashCode.Combine(A, B, C);
}
