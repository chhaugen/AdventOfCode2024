using chhaugen.AdventOfCode2024.Common.Extentions;
using System.Drawing;
using System.Text;

namespace chhaugen.AdventOfCode2024.Common.Structures;
public class Map2D<T> : Map2D, ICloneable
{
    private readonly T[,] _map;
    private readonly int _xLength;
    private readonly int _yLength;

    public Map2D(T[,] map)
    {
        _xLength = map.GetLength(0);
        _yLength = map.GetLength(1);
        _map = map;
    }

    public T this[int x, int y]
    {
        get => _map[x, y];
        set => _map[x, y] = value;
    }

    public T this[Point2D point]
    {
        get => _map[point.X, point.Y];
        set => _map[point.X, point.Y] = value;
    }

    public HashSet<T> GetUniqueValues()
    {
        HashSet<T> plantTypes = [];
        for (int x = 0; x < _xLength; x++)
        {
            for (int y = 0; y < _yLength; y++)
            {
                plantTypes.Add(_map[x, y]);
            }
        }
        return plantTypes;
    }

    public Map2D<T> GetMapOfValue(T type)
    {
        T[,] newMap = new T[_xLength, _yLength];

        for (int x = 0; x < _xLength; x++)
        {
            for (int y = 0; y < _yLength; y++)
            {
                T mapValue = _map[x, y];
                if (mapValue?.Equals(type) ?? false)
                    newMap[x, y] = mapValue;
            }
        }
        return new(newMap);
    }

    public Map2D<T> AddMargin()
    {
        T[,] newMap = new T[_xLength + 2, _yLength + 2];

        for (int x = 0; x < _xLength; x++)
        {
            for (int y = 0; y < _yLength; y++)
            {
                newMap[x + 1, y + 1] = _map[x, y];
            }
        }
        return new(newMap);
    }

    public bool HasPoint(Point2D point)
        => 0 <= point.X && point.X < _xLength && 0 <= point.Y && point.Y < _yLength;

    public int GetEdgeCount(Point2D point)
    {
        int count = 0;
        T pointValue = this[point];
        foreach (var direction in Enum.GetValues<CardinalDirection>())
        {
            var directionPoint = point.GetPointInDirection(direction);
            if (!HasPoint(directionPoint) || !(this[directionPoint]?.Equals(pointValue) ?? false))
                count++;
        }
        return count;
    }

    public IEnumerable<CardinalDirection> GetEdgeDirections(Point2D point)
    {
        T pointValue = this[point];
        foreach (var direction in Enum.GetValues<CardinalDirection>())
        {
            var directionPoint = point.GetPointInDirection(direction);
            if (!HasPoint(point) || !(this[directionPoint]?.Equals(pointValue) ?? false))
                yield return direction;
        }
    }

    public int CountOf(T value)
    {
        int count = 0;
        for (int x = 0; x < _xLength; x++)
        {
            for (int y = 0; y < _yLength; y++)
            {
                if (value?.Equals(_map[x, y]) ?? value is null && _map[x, y] is null)
                    count++;
            }
        }
        return count;
    }

    public void ForEachPoint(Action<Point2D> action)
    {
        for (int x = 0; x < _xLength; x++)
        {
            for (int y = 0; y < _yLength; y++)
            {
                action(new(x, y));
            }
        }
    }


    public IEnumerable<Point2D> AsPointEnumerable()
    {
        for (int x = 0; x < _xLength; x++)
        {
            for (int y = 0; y < _yLength; y++)
            {
                yield return new(x, y);
            }
        }
    }

    public IEnumerable<T> AsEnumerable()
    {
        for (int x = 0; x < _xLength; x++)
        {
            for (int y = 0; y < _yLength; y++)
            {
                yield return _map[x, y];
            }
        }
    }

    public IEnumerable<Point2D> GetSpiralInwardsPoints()
    {
        int xMin = 0;
        int yMin = 0;
        int xMax = _xLength - 1;
        int yMax = _yLength - 1;
        Point2D? currentPoint = null;
        CardinalDirection direction = CardinalDirection.South;
        for (int i = 0; i < _xLength * _yLength; i++)
        {
            if (currentPoint is null)
            {
                currentPoint = new(0, 0);
                yield return currentPoint.Value;
                continue;
            }
            var nextPoint = currentPoint.Value.GetPointInDirection(direction);
            if (nextPoint.X == xMin && nextPoint.Y == yMin)
            {
                xMin++;
                yMin++;
                xMax--;
                yMax--;
            }
            if (!(xMin <= nextPoint.X && nextPoint.X <= xMax && yMin <= nextPoint.Y && nextPoint.Y <= yMax))
            {
                direction = direction.TurnAntiClockwise();
                nextPoint = currentPoint.Value.GetPointInDirection(direction);
            }
            currentPoint = nextPoint;
            yield return nextPoint;
        }
    }

    public string PrintMap()
    {
        StringBuilder sb = new();
        for (int x = 0; x < _xLength; x++)
        {
            for (int y = 0; y < _yLength; y++)
            {
                sb.Append(_map[x, y]);
            }
            sb.Append(Environment.NewLine);
        }
        return sb.ToString();
    }

    public string PrintMap(Func<T, string> itemToStringConverter)
    {
        StringBuilder sb = new();
        for (int x = 0; x < _xLength; x++)
        {
            for (int y = 0; y < _yLength; y++)
            {
                sb.Append(itemToStringConverter(_map[x,y]));
            }
            sb.Append(Environment.NewLine);
        }
        return sb.ToString();
    }

    public Map2D<T> Clone()
    {
        T[,] clonedMap = (T[,])_map.Clone();
        return new(clonedMap);
    }

    object ICloneable.Clone()
        => Clone();
}
