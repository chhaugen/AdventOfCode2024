using System.Text;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day06Puzzle01 : Puzzle
{
    public Day06Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string inputString)
    {
        var map = ParseInput(inputString);
        _progressOutput(PrintMap(map));

        var guard = FindGuard(map);
        PlayGuard(guard);

        int countCovered = Count(map, MapFeature.Covered);

        _progressOutput(PrintMap(map));

        return Task.FromResult(countCovered.ToString());
    }

    public static void PlayGuard(Guard guard)
    {
        bool guardIsGone = false;
        Guard? internalGuard = guard.Clone();
        while (!guardIsGone)
        {
            if (internalGuard.HasValue)
                internalGuard = internalGuard.Value.PlayOneStep();
            else
                guardIsGone = true;
        }
    }

    public static MapFeature[,] ParseInput(string input)
    {
        var lines = input
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Select(CharToMapFeature).ToList())
            .ToList();

        int yCount = lines.Count;
        int xCount = lines[0].Count;
        MapFeature[,] map = new MapFeature[xCount, yCount];
        for (int y = 0; y < yCount; y++)
        {
            for (int x = 0; x < xCount; x++)
            {
                map[x, y] = lines[y][x];
            }
        }
        return map;
    }

    public static Guard FindGuard(MapFeature[,] map)
    {
        int xCount = map.GetLength(0);
        int yCount = map.GetLength(1);
        for (int y = 0; y < yCount; y++)
        {
            for (int x = 0; x < xCount; x++)
            {
                MapFeature feature = map[x, y];
                switch (feature)
                {
                    case MapFeature.GuardLookingWest:
                    case MapFeature.GuardLookingNorth:
                    case MapFeature.GuardLookingEast:
                    case MapFeature.GuardLookingSouth:
                        return new(map, new(x, y));
                }
            }
        }
        throw new InvalidOperationException("Guard not found");
    }

    public static MapFeature CharToMapFeature(char @char)
        => @char switch
        {
            '.' => MapFeature.Empty,
            '#' => MapFeature.Obstruction,
            '<' => MapFeature.GuardLookingWest,
            '^' => MapFeature.GuardLookingNorth,
            '>' => MapFeature.GuardLookingWest,
            'v' => MapFeature.GuardLookingSouth,
            _ => throw new InvalidOperationException($"This char appeared: {@char}")
        };

    public static bool IsPointOnMap(Point point, MapFeature[,] map)
    {
        int xCount = map.GetLength(0);
        int yCount = map.GetLength(1);
        bool withinX = 0 <= point.X && point.X < xCount;
        bool withinY = 0 <= point.Y && point.Y < yCount;
        return withinX && withinY;
    }

    public static int Count(MapFeature[,] map, MapFeature mapFeature)
    {
        int xCount = map.GetLength(0);
        int yCount = map.GetLength(1);
        int count = 0;
        for (int y = 0; y < yCount; y++)
        {
            for (int x = 0; x < xCount; x++)
            {
                MapFeature feature = map[x, y];
                if (feature == mapFeature)
                    count++;
            }
        }
        return count;
    }

    public static string PrintMap(MapFeature[,] map)
    {
        StringBuilder sb = new();
        int xCount = map.GetLength(0);
        int yCount = map.GetLength(1);
        for (int y = 0; y < yCount; y++)
        {
            for (int x = 0; x < xCount; x++)
            {
                MapFeature feature = map[x, y];
                char @char = feature switch
                {
                    MapFeature.Empty => '.',
                    MapFeature.Obstruction => '#',
                    MapFeature.GuardLookingWest => '<',
                    MapFeature.GuardLookingNorth => '^',
                    MapFeature.GuardLookingEast => '>',
                    MapFeature.GuardLookingSouth => 'v',
                    MapFeature.Covered => 'X',
                    _ => '?'
                };
                sb.Append(@char);
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public enum MapFeature
    {
        Empty, // .
        Obstruction, // #
        GuardLookingWest, // <
        GuardLookingNorth, // ^
        GuardLookingEast, // >
        GuardLookingSouth, // v
        Covered, // X
    }

    public readonly struct Point : IEquatable<Point>
    {
        public Point(int x, int y)
        {
            (X, Y) = (x, y);
        }
        public int X { get; }
        public int Y { get; }

        public Point West
            => new(X - 1, Y);
        public Point North
            => new(X, Y - 1);
        public Point East
            => new(X + 1, Y);
        public Point South
            => new(X, Y + 1);

        public bool Equals(Point other)
            => other.X == X && other.Y == Y;

        public MapFeature GetFeature(MapFeature[,] map)
            => map[X, Y];
        public void SetFeature(MapFeature[,] map, MapFeature newFeature)
            => map[X, Y] = newFeature;

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
    }

    public readonly struct Guard : ICloneable, IEquatable<Guard>
    {
        private readonly MapFeature[,] _map;

        public Guard(MapFeature[,] map, Point point)
        {
            switch (point.GetFeature(map))
            {
                case MapFeature.GuardLookingWest:
                case MapFeature.GuardLookingNorth:
                case MapFeature.GuardLookingEast:
                case MapFeature.GuardLookingSouth:
                    break;
                default:
                    throw new InvalidOperationException("MapFeature not a guard!");
            }
            _map = map;
            Point = point;
        }

        public MapFeature Feature => Point.GetFeature(_map);

        public Point Point { get; }

        public Point PointInfront => Feature switch
        {
            MapFeature.GuardLookingWest => Point.West,
            MapFeature.GuardLookingNorth => Point.North,
            MapFeature.GuardLookingEast => Point.East,
            MapFeature.GuardLookingSouth => Point.South,
            _ => throw new InvalidOperationException("MapFeature not a guard!"),
        };

        public MapFeature FeatureInFront => PointInfront.GetFeature(_map);

        public Guard Turn90Degrees()
        {
            switch (Feature)
            {
                case MapFeature.GuardLookingWest:
                    Point.SetFeature(_map, MapFeature.GuardLookingNorth);
                    break;
                case MapFeature.GuardLookingNorth:
                    Point.SetFeature(_map, MapFeature.GuardLookingEast);
                    break;
                case MapFeature.GuardLookingEast:
                    Point.SetFeature(_map, MapFeature.GuardLookingSouth);
                    break;
                case MapFeature.GuardLookingSouth:
                    Point.SetFeature(_map, MapFeature.GuardLookingWest);
                    break;
                default:
                    throw new InvalidOperationException("MapFeature not a guard!");
            }
            return this;
        }

        public Guard MoveForward()
        {
            Point newPoint;
            switch (Feature)
            {
                case MapFeature.GuardLookingWest:
                    newPoint = Point.West;
                    newPoint.SetFeature(_map, MapFeature.GuardLookingWest);
                    break;
                case MapFeature.GuardLookingNorth:
                    newPoint = Point.North;
                    newPoint.SetFeature(_map, MapFeature.GuardLookingNorth);
                    break;
                case MapFeature.GuardLookingEast:
                    newPoint = Point.East;
                    newPoint.SetFeature(_map, MapFeature.GuardLookingEast);
                    break;
                case MapFeature.GuardLookingSouth:
                    newPoint = Point.South;
                    newPoint.SetFeature(_map, MapFeature.GuardLookingSouth);
                    break;
                default:
                    throw new InvalidOperationException("MapFeature not a guard!");
            }
            Point.SetFeature(_map, MapFeature.Covered);
            return new(_map, newPoint);
        }

        public Guard? PlayOneStep()
        {
            // Guard left the map
            if (!IsPointOnMap(PointInfront, _map))
            {
                Point.SetFeature(_map, MapFeature.Covered);
                return null;
            }

            if (FeatureInFront == MapFeature.Obstruction)
                return Turn90Degrees();

            return MoveForward();
        }

        public Guard Clone()
            => new(_map, Point);

        object ICloneable.Clone()
            => Clone();

        public bool Equals(Guard other)
            => Point == other.Point && Feature == other.Feature;

        public override bool Equals(object? obj)
            => obj is Guard guard && Equals(guard);

        public static bool operator ==(Guard left, Guard right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Guard left, Guard right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return Point.GetHashCode() ^ Feature.GetHashCode();
        }
    }

    #region Don't think about it
    //public static Point? MoveGuard(Point guardPoint, MapFeature[,] map)
    //{
    //    MapFeature guard = guardPoint.GetFeature(map);
    //    switch (guard)
    //    {
    //        case MapFeature.GuardLookingWest:
    //            Point infrontWest = guardPoint.West;
    //            if (!IsCoordinateOnMap(infrontWest, map))
    //            {
    //                guardPoint.SetFeature(map, MapFeature.Covered);
    //                return null;
    //            }
    //            MapFeature infrontWestFeature = infrontWest.GetFeature(map);
    //            if(infrontWestFeature == MapFeature.Obstruction)
    //            {
    //                Point north = guardPoint.North;
    //                if (!IsCoordinateOnMap(north, map))
    //                {
    //                    guardPoint.SetFeature(map, MapFeature.Covered);
    //                    return null;
    //                }
    //                MapFeature northFeature = north.GetFeature(map);
    //                if (northFeature == MapFeature.Obstruction)
    //                {
    //                    guardPoint.SetFeature(map, MapFeature.GuardLookingNorth);
    //                    MoveGuard(guardPoint, map);
    //                }
    //                guardPoint.SetFeature(map, MapFeature.Covered);
    //                north.SetFeature(map, MapFeature.GuardLookingNorth);
    //                return north;
    //            }
    //            guardPoint.SetFeature(map, MapFeature.Covered);
    //            infrontWest.SetFeature(map, MapFeature.GuardLookingWest);
    //            return infrontWest;

    //        case MapFeature.GuardLookingNorth:
    //            Point infrontNorth = guardPoint.North;
    //            if (!IsCoordinateOnMap(infrontNorth, map))
    //            {
    //                guardPoint.SetFeature(map,MapFeature.Covered);
    //                return null;
    //            }
    //            MapFeature infrontNorthFeature = infrontNorth.GetFeature(map);
    //            if (infrontNorthFeature == MapFeature.Obstruction)
    //            {
    //                Point east = guardPoint.East;
    //                if (!IsCoordinateOnMap(east, map))
    //                {
    //                    guardPoint.SetFeature(map, MapFeature.Covered);
    //                    return null;
    //                }
    //                MapFeature eastFeature = east.GetFeature(map);
    //                if (eastFeature == MapFeature.Obstruction)
    //                {
    //                    guardPoint.SetFeature(map, MapFeature.GuardLookingEast);
    //                    MoveGuard(guardPoint, map);
    //                }
    //                guardPoint.SetFeature(map, MapFeature.Covered);
    //                east.SetFeature(map , MapFeature.GuardLookingEast);
    //                return east;
    //            }
    //            guardPoint.SetFeature(map, MapFeature.Covered);
    //            infrontNorth.SetFeature(map, MapFeature.GuardLookingNorth);
    //            return infrontNorth;

    //        case MapFeature.GuardLookingEast:
    //            Point infrontEast = guardPoint.East;
    //            if (!IsCoordinateOnMap(infrontEast, map))
    //            {
    //                guardPoint.SetFeature(map, MapFeature.Covered);
    //                return null;
    //            }
    //            MapFeature infrontEastFeature = infrontEast.GetFeature(map);
    //            if (infrontEastFeature == MapFeature.Obstruction)
    //            {
    //                Point south = guardPoint.South;
    //                if (!IsCoordinateOnMap(south, map))
    //                {
    //                    guardPoint.SetFeature(map,MapFeature.Covered);
    //                    return null;
    //                }
    //                MapFeature southFeature = south.GetFeature(map);
    //                if (southFeature == MapFeature.Obstruction)
    //                {
    //                    guardPoint.SetFeature(map, MapFeature.GuardLookingSouth);
    //                    MoveGuard(guardPoint, map);
    //                }
    //                guardPoint.SetFeature(map, MapFeature.Covered);
    //                south.SetFeature(map, MapFeature.GuardLookingSouth);
    //                return south;
    //            }
    //            guardPoint.SetFeature(map, MapFeature.Covered);
    //            infrontEast.SetFeature(map, MapFeature.GuardLookingEast);
    //            return infrontEast;

    //        case MapFeature.GuardLookingSouth:
    //            Point infrontSouth = guardPoint.South;
    //            if (!IsCoordinateOnMap(infrontSouth, map))
    //            {
    //                guardPoint.SetFeature(map, MapFeature.Covered);
    //                return null;
    //            }
    //            MapFeature infrontSouthFeature = infrontSouth.GetFeature(map);
    //            if (infrontSouthFeature == MapFeature.Obstruction)
    //            {
    //                Point west = guardPoint.West;
    //                if (!IsCoordinateOnMap(west, map))
    //                {
    //                    guardPoint.SetFeature(map,MapFeature.Covered);
    //                    return null;
    //                }
    //                MapFeature westFeature = west.GetFeature(map);
    //                if (westFeature == MapFeature.Obstruction)
    //                {
    //                    guardPoint.SetFeature(map, MapFeature.GuardLookingWest);
    //                    MoveGuard(guardPoint, map);
    //                }
    //                guardPoint.SetFeature(map, MapFeature.Covered);
    //                west.SetFeature(map, MapFeature.GuardLookingWest);
    //                return west;
    //            }
    //            guardPoint.SetFeature(map, MapFeature.Covered);
    //            infrontSouth.SetFeature(map, MapFeature.GuardLookingSouth);
    //            return infrontSouth;
    //    }
    //    throw new InvalidOperationException("This is not a guard");
    //}
    #endregion
}
