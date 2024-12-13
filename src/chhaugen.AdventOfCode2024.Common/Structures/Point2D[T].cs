namespace chhaugen.AdventOfCode2024.Common.Structures;
public class Point2D<T> : IEquatable<Point2D<T>>
{
    private readonly T[,] _map;
    private readonly int _xLength;
    private readonly int _yLength;
    public Point2D(T[,] map, int x, int y)
    {
        _map = map;
        _xLength = _map.GetLength(0);
        _yLength = _map.GetLength(1);
        (X, Y) = (x, y);
    }

    public T Value
    {
        get => _map[X, Y];
        set => _map[X, Y] = value;
    }
    public int X { get; }
    public int Y { get; }

    public Point2D<T> West
        => new(_map, X - 1, Y);

    public Point2D<T> NorthWest
        => new(_map, X - 1, Y - 1);

    public Point2D<T> North
        => new(_map,X, Y - 1);

    public Point2D<T> NorthEast
        => new(_map, X + 1, Y - 1);

    public Point2D<T> East
        => new(_map, X + 1, Y);

    public Point2D<T> SouthEast
        => new(_map, X + 1, Y + 1);

    public Point2D<T> South
        => new(_map, X, Y + 1);

    public Point2D<T> SouthWest
        => new(_map, X - 1, Y + 1);
    public bool IsOnMap
        => 0 <= X && X < _xLength && 0 <= Y && Y < _yLength;

    public Point2D<T> GetPointInDirection(CardinalDirection direction)
        => direction switch
        {
            CardinalDirection.West  => West,
            CardinalDirection.North => North,
            CardinalDirection.East  => East,
            CardinalDirection.South => South,
            _ => throw new NotImplementedException(),
        };

    public Point2D<T> GetPointInDirection(OrdinalDirection direction)
        => direction switch
        {
            OrdinalDirection.NorthEast => NorthEast,
            OrdinalDirection.SouthEast => SouthEast,
            OrdinalDirection.SouthWest => SouthWest,
            OrdinalDirection.NorthWest => NorthWest,
            _ => throw new NotImplementedException(),
        };

    public Point2D<T> GetPointInDirection(PrincipalWind direction)
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

    public int GetEdgeCount()
    {
        int count = 0;
        T pointValue = Value;
        foreach(var direction in Enum.GetValues<CardinalDirection>())
        {
            var directionPoint = GetPointInDirection(direction);
            if (!directionPoint.IsOnMap || !(directionPoint.Value?.Equals(pointValue) ?? false))
                count++;
        }
        return count;
    }

    public IEnumerable<CardinalDirection> GetEdgeDirections()
    {
        T pointValue = Value;
        foreach (var direction in Enum.GetValues<CardinalDirection>())
        {
            var directionPoint = GetPointInDirection(direction);
            if (!directionPoint.IsOnMap || !(directionPoint.Value?.Equals(pointValue) ?? false))
                yield return direction;
        }
    }

    public bool Equals(Point2D<T>? other)
        => other?.X == X && other?.Y == Y;

    public override bool Equals(object? obj)
    {
        if (obj is Point2D<T> other)
            return Equals(other);
        else
            throw new InvalidOperationException("Cannot compare non point objects");
    }

    public static bool operator ==(Point2D<T>? left, Point2D<T>? right)
    {
        return left?.Equals(right) ?? left is null && right is null;
    }

    public static bool operator !=(Point2D<T>? left, Point2D<T>? right)
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
