using System.Text;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day08Puzzle01 : Puzzle
{
    public Day08Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string inputString)
    {
        Map map = Map.Parse(inputString);
        _progressOutput(map.PrintToString());

        foreach (var group in map.GetAntennaGroups())
        {
            foreach (var firstAntenae in group)
            {
                foreach (var secondAntenae in group)
                {
                    if (firstAntenae.Point == secondAntenae.Point)
                        continue;
                    //_progressOutput($"1. antenae {firstAntenae.Frequency} {firstAntenae.Point}");
                    //_progressOutput($"2. antenae {secondAntenae.Frequency} {secondAntenae.Point}");
                    Vector vector = new(firstAntenae.Point, secondAntenae.Point);
                    //_progressOutput($"Vector: {vector}");
                    //_progressOutput($"Vector Length: {(int)vector.AbsoluteLength}");
                    Point antinodePoint = secondAntenae.Point.Add(vector);
                    //_progressOutput($"Antinode {antinodePoint}");
                    if (map.HasPoint(antinodePoint))
                    {
                        Antinode antinode = new(antinodePoint);
                        map.SetItem(antinode);
                    }
                }
            }
        }
        _progressOutput(map.PrintToString());

        var antinodeCount = map.Count<Antinode>();

        return Task.FromResult(antinodeCount.ToString());
    }

    public class Map : ICloneable
    {
        public readonly MapItem?[,] _map;
        public readonly long _xLength;
        public readonly long _yLength;
        private Map(MapItem?[,] map)
        {
            _map = map;
            _xLength = _map.GetLongLength(0);
            _yLength = _map.GetLongLength(1);
        }
        public static Map Parse(string input)
        {
            var lines = input
                .Split('\n', StringSplitOptions.RemoveEmptyEntries);

            int yLength = lines.Length;
            int xLength = lines[0].Length;

            MapItem[,] map = new MapItem[xLength, yLength];

            for (int y = 0; y < yLength; y++)
            {
                var line = lines[y];
                for (int x = 0; x < xLength; x++)
                {
                    char @char = line[x];
                    if (@char == '.')
                        continue;
                    map[x, y] = new Antenna(new(x, y), line[x]);
                }
            }
            return new Map(map);
        }

        public MapItem? this[Point point]
        {
            get => _map[point.X, point.Y];
            set => _map[point.X, point.Y] = value;
        }

        public void SetItem(MapItem item)
            => this[item.Point] = item;

        public IEnumerable<Antenna> GetAntennas()
        {
            for (int x = 0; x < _yLength; x++)
            {
                for (int y = 0; y < _xLength; y++)
                {
                    var item = _map[x, y];
                    if (item is Antenna antenna)
                        yield return antenna;
                }
            }
        }

        public IEnumerable<IGrouping<char, Antenna>> GetAntennaGroups()
            => GetAntennas().GroupBy(x => x.Frequency);

        public string PrintToString()
        {
            StringBuilder sb = new();
            for (int y = 0; y < _yLength; y++)
            {
                for (int x = 0; x < _xLength; x++)
                {
                    var item = _map[x, y];
                    if (item is null)
                        sb.Append('.');
                    else if (item is Antenna antenna)
                        sb.Append(antenna.Frequency);
                    else if (item is Antinode)
                        sb.Append('#');
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public long Count<T>()
            where T : MapItem
        {
            long count = 0;
            for (int y = 0; y < _yLength; y++)
            {
                for (int x = 0; x < _xLength; x++)
                {
                    var item = _map[x, y];
                    if (item is T)
                        count++;
                }
            }
            return count;
        }

        public bool HasPoint(Point point)
            => 0 <= point.X && point.X < _xLength && 0 <= point.Y && point.Y < _yLength;

        public Map Clone()
            => new((MapItem[,])_map.Clone());

        object ICloneable.Clone()
            => Clone();
    }

    public readonly struct Point : IEquatable<Point>
    {
        public Point(int x, int y)
        {
            (X, Y) = (x, y);
        }
        public int X { get; }
        public int Y { get; }

        public double DistanceTo(Point other)
        {
            (int xLength, int yLength) = GetManhattanLegths(other);
            return Math.Sqrt(Math.Pow(xLength, 2) + Math.Pow(yLength, 2));
        }

        public int ManhattanDistanceTo(Point other)
        {
            (int xLength, int yLength) = GetManhattanLegths(other);
            return xLength + yLength;
        }

        public (int xLength, int yLength) GetManhattanLegths(Point other)
        {
            int xLength = Math.Abs(X - other.X);
            int yLength = Math.Abs(Y - other.Y);
            return (xLength, yLength);
        }

        public Vector AsVector()
            => new(this);

        public Point Add(Vector vector)
            => new(X + vector.X, Y + vector.Y);

        public Point Subtract(Vector vector)
            => new(X - vector.X, Y - vector.Y);

        public Vector VectorBetween(Point other)
            => new(this, other);

        public bool Equals(Point other)
            => other.X == X && other.Y == Y;

        public override bool Equals(object? obj)
        {
            if (obj is Point other)
                return Equals(other);
            else
                throw new InvalidOperationException("Cannot compare non point objects");
        }

        public static bool operator ==(Point left, Point right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override string ToString()
            => $"({X}, {Y})";
    }

    public readonly struct Vector
    {
        public Vector(Point point)
            => (X, Y) = (point.X, point.Y);

        public Vector(Point from, Point to)
            => (X, Y) = (to.X - from.X, to.Y - from.Y);

        public int X { get; }
        public int Y { get; }

        public double AbsoluteLength => Math.Sqrt(Math.Pow(Math.Abs(X), 2) + Math.Pow(Math.Abs(Y), 2));

        public override string ToString()
            => $"[{X}, {Y}]";
    }

    public abstract class MapItem
    {
        public MapItem(Point point)
        {
            Point = point;
        }

        public Point Point { get; }

        public double DistanceTo(MapItem other)
            => Point.DistanceTo(other.Point);

        public int ManhattanDistanceTo(MapItem other)
            => Point.ManhattanDistanceTo(other.Point);

    }

    public class Antenna : MapItem
    {
        public Antenna(Point point, char frequency) : base(point)
        {
            Frequency = frequency;
        }
        public char Frequency { get; set; }
    }

    public class Antinode : MapItem
    {
        public Antinode(Point point) : base(point)
        {
        }
    }
}
