using chhaugen.AdventOfCode2024.Common.Extentions;
using chhaugen.AdventOfCode2024.Common.Structures;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day16Puzzle01 : Puzzle
{
    public Day16Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {
        Map2D<MapObject> map = Map2D.ParseInput<MapObject>(input, x => x switch
        {
            '.' => new Floor(),
            '#' => new Wall(),
            'S' => new Start(),
            'E' => new End(),
            _ => throw new NotImplementedException(),
        });

        Point2D start = map
            .AsPointEnumerable()
            .First(x => map[x] is Start);

        Point2D end = map
            .AsPointEnumerable()
            .First(x => map[x] is End);

        Reindeer startReindeer = new(start, CardinalDirection.East);

        AddCostsToMap(startReindeer, map);
        _progressOutput(PrintMap(map));

        var eValue = (End)map[end];

        return Task.FromResult(eValue.Cost.ToString());
    }

    private static string PrintMap(Map2D<MapObject> map)
    {
        int maxNum = map
            .AsEnumerable()
            .Where(x => x is Floor)
            .Cast<Floor>()
            .Select(x => x.Cost)
            .Max();
        int numWidth = maxNum.ToString().Length;
        string toStringCode = $"D{numWidth}";

        string wallString = new(Enumerable.Range(0, numWidth + 2).Select(_ => '#').ToArray());

        return map.PrintMap(x => x switch
        {
            Wall wall => wallString,
            Floor floor => $"({floor.Cost.ToString(toStringCode)})",
            _ => throw new NotImplementedException(),
        });
    }

    public static void AddCostsToMap(Reindeer start, Map2D<MapObject> map)
    {
        List<Reindeer> currentReindeers = [start];
        while (currentReindeers.Count > 0)
        {
            List<Reindeer> newReindeers = [];
            foreach (var currentReindeer in currentReindeers)
            {
                int currentCost = ((Floor)map[currentReindeer.Position]).Cost;
                foreach ((int addCost, var reindeerToCheck) in GetReindeersToCheck(currentReindeer))
                {
                    if (map[reindeerToCheck.Position] is Floor newFloor)
                    {
                        int newCost = currentCost + addCost;
                        if (newFloor.Cost == -1 || newCost < newFloor.Cost)
                        {
                            newFloor.Cost = newCost;
                            newReindeers.RemoveAll(x => x.Position == reindeerToCheck.Position);
                            newReindeers.Add(reindeerToCheck);
                            //_progressOutput(map.PrintMap());
                        }
                    }
                }
            }
            currentReindeers = newReindeers;
        }
    }

    public static IEnumerable<(int addCost, Reindeer reindeer)> GetReindeersToCheck(Reindeer reindeer)
    {
        CardinalDirection right = reindeer.Direction.TurnClockwise();
        Point2D pointRight = reindeer.Position.GetPointInDirection(right);
        yield return (1001, new(pointRight, right));

        Point2D pointInFront = reindeer.Position.GetPointInDirection(reindeer.Direction);
        yield return (1, new(pointInFront, reindeer.Direction));

        CardinalDirection left = reindeer.Direction.TurnAntiClockwise();
        Point2D pointLeft = reindeer.Position.GetPointInDirection(left);
        yield return (1001, new(pointLeft, left));
    }

    public readonly struct Reindeer : IEquatable<Reindeer>
    {
        public Reindeer(Point2D position, CardinalDirection direction)
        {
            Position = position;
            Direction = direction;
        }

        public Point2D Position { get; }
        public CardinalDirection Direction { get; }

        public Reindeer TurnClockwise()
            => new(Position, Direction.TurnClockwise());
        public Reindeer TurnAntiClockwise()
            => new(Position, Direction.TurnAntiClockwise());

        public override string ToString()
            => $"{Position} {Enum.GetName(Direction)}";

        public override int GetHashCode()
            => HashCode.Combine(Position, Direction);

        public bool Equals(Reindeer other)
            => Position == other.Position && Direction == other.Direction;

        public override bool Equals(object? obj)
            => obj is Reindeer other && Equals(other);

        public static bool operator ==(Reindeer left, Reindeer right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Reindeer left, Reindeer right)
        {
            return !(left == right);
        }
    }

    public abstract class MapObject {
    }

    public class Floor : MapObject
    {
        public Floor()
        {
            Cost = -1;
        }
        
        public int Cost { get; set; }
        public override string ToString()
            => ".";
    }

    public class Wall : MapObject
    {
        public override string ToString()
            => "#";
    }

    public class Start : Floor
    {
        public Start()
        {
            Cost = 0;
        }

        public override string ToString()
            => "S";
    }

    public class End : Floor
    {
        public override string ToString()
            => "E";
    }



}
