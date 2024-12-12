namespace chhaugen.AdventOfCode2024.Common.Structures;
public class Point<T> : IEquatable<Point<T>>
{
    private readonly T[,] _map;
    private readonly int _xLength;
    private readonly int _yLength;
    public Point(T[,] map, int x, int y)
    {
        _map = map;
        _xLength = _map.GetLength(0);
        _yLength = _map.GetLength(1);
        (X, Y) = (x, y);
    }
    public int X { get; }
    public int Y { get; }

    public Point<T> West
        => new(_map, X - 1, Y);

    public Point<T> NorthWest
        => new(_map, X - 1, Y - 1);

    public Point<T> North
        => new(_map,X, Y - 1);

    public Point<T> NorthEast
        => new(_map, X + 1, Y - 1);

    public Point<T> East
        => new(_map, X + 1, Y);

    public Point<T> SouthEast
        => new(_map, X + 1, Y + 1);

    public Point<T> South
        => new(_map, X, Y + 1);

    public Point<T> SouthWest
        => new(_map, X - 1, Y + 1);
    public bool ExistsOnMap
        => 0 <= X && X < _xLength && 0 <= Y && Y < _yLength;

    public Point<T> GetPointInCardinalDirection(CardinalDirection direction)
        => direction switch
        {
            CardinalDirection.West => West,
            CardinalDirection.North => North,
            CardinalDirection.East => East,
            CardinalDirection.South => South,
            _ => throw new NotImplementedException(),
        };

    public T Value
    {
        get => _map[X, Y];
        set => _map[X, Y] = value;
    }

    public int GetEdgeCount()
    {
        int count = 0;
        T pointValue = Value;
        foreach(var direction in Enum.GetValues<CardinalDirection>())
        {
            var directionPoint = GetPointInCardinalDirection(direction);
            if (!directionPoint.ExistsOnMap || !(directionPoint.Value?.Equals(pointValue) ?? false))
                count++;
        }
        return count;
    }

    public IEnumerable<CardinalDirection> GetEdgeDirections()
    {
        T pointValue = Value;
        foreach (var direction in Enum.GetValues<CardinalDirection>())
        {
            var directionPoint = GetPointInCardinalDirection(direction);
            if (!directionPoint.ExistsOnMap || !(directionPoint.Value?.Equals(pointValue) ?? false))
                yield return direction;
        }
    }

    public bool Equals(Point<T>? other)
        => other?.X == X && other?.Y == Y;

    public override bool Equals(object? obj)
    {
        if (obj is Point<T> other)
            return Equals(other);
        else
            throw new InvalidOperationException("Cannot compare non point objects");
    }

    public static bool operator ==(Point<T>? left, Point<T>? right)
    {
        return left?.Equals(right) ?? left is null && right is null;
    }

    public static bool operator !=(Point<T>? left, Point<T>? right)
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
