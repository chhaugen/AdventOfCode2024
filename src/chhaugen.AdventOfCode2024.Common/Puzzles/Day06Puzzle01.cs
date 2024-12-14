using chhaugen.AdventOfCode2024.Common.Extentions;
using chhaugen.AdventOfCode2024.Common.Structures;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day06Puzzle01 : Puzzle
{
    public Day06Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string inputString)
    {
        var map = Map2D.ParseInput(inputString);
        _progressOutput(map.PrintMap());

        var guard = FindGuard(map);
        PlayGuard(guard, () => { });

        long countCovered = map.CountOf('X');

        _progressOutput(map.PrintMap());

        return Task.FromResult(countCovered.ToString());
    }

    public static void PlayGuard(Guard guard, Action action)
    {
        bool guardIsGone = false;
        Guard? internalGuard = guard;
        while (!guardIsGone)
        {
            if (internalGuard.HasValue)
                internalGuard = internalGuard.Value.PlayOneStep();
            else
                guardIsGone = true;
            action();
        }
    }

    public static CardinalDirection GuardCharToDirection(char guard)
        => guard switch
        {
            '^' => CardinalDirection.North,
            '>' => CardinalDirection.East,
            'v' => CardinalDirection.South,
            '<' => CardinalDirection.West,
            _ => throw new NotImplementedException(),
        };

    public static char DirectionToGuardChar(CardinalDirection direction)
        => direction switch
        {
            CardinalDirection.North => '^',
            CardinalDirection.East  => '>',
            CardinalDirection.South => 'v',
            CardinalDirection.West  => '<',
            _ => throw new NotImplementedException(),
        };

    public static Guard FindGuard(Map2D<char> map)
        => new(map, map
            .AsPointEnumerable()
            .First(x => map[x] is '<' or '^' or '>' or 'v'));



    public readonly struct Guard : IEquatable<Guard>
    {
        private readonly Map2D<char> _map;

        public Guard(Map2D<char> map, Point2D point)
        {
            _map = map;
            Point = point;
        }

        public Point2D Point { get; }

        public CardinalDirection Direction => GuardCharToDirection(_map[Point]);

        public Guard Turn90Degrees()
        {
            var newDirection = Direction.TurnClockwise();
            _map[Point] = DirectionToGuardChar(newDirection);
            return this;
        }

        public Guard MoveForward()
        {
            Point2D newPoint = Point.GetPointInDirection(Direction);
            _map[newPoint] = _map[Point];
            _map[Point] = 'X';
            return new(_map, newPoint);
        }

        public Guard? PlayOneStep()
        {
            var pointInFront = Point.GetPointInDirection(Direction);
            // Guard left the map
            if (!_map.HasPoint(pointInFront))
            {
                _map[Point] = 'X';
                return null;
            }

            if (_map[pointInFront] == '#')
                return Turn90Degrees();

            return MoveForward();
        }

        public bool Equals(Guard other)
        {
            return Direction == other.Direction && Point == other.Point;
        }

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
            return HashCode.Combine(Direction.GetHashCode(), Point.GetHashCode());
        }
    }

    #region Don't think about it
    //public static Point2D? MoveGuard(Point2D guardPoint, MapFeature[,] map)
    //{
    //    MapFeature guard = guardPoint.GetFeature(map);
    //    switch (guard)
    //    {
    //        case MapFeature.GuardLookingWest:
    //            Point2D infrontWest = guardPoint.West;
    //            if (!IsCoordinateOnMap(infrontWest, map))
    //            {
    //                guardPoint.SetFeature(map, MapFeature.Covered);
    //                return null;
    //            }
    //            MapFeature infrontWestFeature = infrontWest.GetFeature(map);
    //            if(infrontWestFeature == MapFeature.Obstruction)
    //            {
    //                Point2D north = guardPoint.North;
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
    //            Point2D infrontNorth = guardPoint.North;
    //            if (!IsCoordinateOnMap(infrontNorth, map))
    //            {
    //                guardPoint.SetFeature(map,MapFeature.Covered);
    //                return null;
    //            }
    //            MapFeature infrontNorthFeature = infrontNorth.GetFeature(map);
    //            if (infrontNorthFeature == MapFeature.Obstruction)
    //            {
    //                Point2D east = guardPoint.East;
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
    //            Point2D infrontEast = guardPoint.East;
    //            if (!IsCoordinateOnMap(infrontEast, map))
    //            {
    //                guardPoint.SetFeature(map, MapFeature.Covered);
    //                return null;
    //            }
    //            MapFeature infrontEastFeature = infrontEast.GetFeature(map);
    //            if (infrontEastFeature == MapFeature.Obstruction)
    //            {
    //                Point2D south = guardPoint.South;
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
    //            Point2D infrontSouth = guardPoint.South;
    //            if (!IsCoordinateOnMap(infrontSouth, map))
    //            {
    //                guardPoint.SetFeature(map, MapFeature.Covered);
    //                return null;
    //            }
    //            MapFeature infrontSouthFeature = infrontSouth.GetFeature(map);
    //            if (infrontSouthFeature == MapFeature.Obstruction)
    //            {
    //                Point2D west = guardPoint.West;
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
