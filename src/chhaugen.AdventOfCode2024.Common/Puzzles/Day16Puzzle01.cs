using chhaugen.AdventOfCode2024.Common.Extentions;
using chhaugen.AdventOfCode2024.Common.Structures;
using System.Threading.Tasks.Dataflow;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day16Puzzle01 : Puzzle
{
    public Day16Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {
        Map2D<char> map = Map2D.ParseInput(input);

        Point2D start = map
            .AsPointEnumerable()
            .First(x => map[x] == 'S');

        Point2D end = map
            .AsPointEnumerable()
            .First(x => map[x] == 'E');

        Reindeer reindeer = new(start, CardinalDirection.East);

        var graf = CreateReindeerPathGraf(reindeer, map);

            var endLeafs = graf
                .GetLeafs()
            .Where(x => map[x.Value.Position] == 'E')
                .ToList();

            var endLeafCount = endLeafs.Count();

            var paths = endLeafs
                .Select(x => x.GetAncestors().Select(x => x.Value).ToList())
                .ToList();

            paths.ForEach(x => x.Reverse());

            var scores = paths
                .Select(GetScore)
                .ToList();
            var minScore = scores.Min();

        return Task.FromResult(minScore.ToString());
    }

    public static int GetScore(IEnumerable<Reindeer> reindeers)
    {
        int score = 0;
        Reindeer? previous = null;
        foreach (var reindeer in reindeers)
        {
            if (previous == null)
            {
                previous = reindeer;
                continue;
            }

            // Turn
            if (previous.Value.Direction != reindeer.Direction)
                score += 1000;

            // Walk forward
            score++;
            
            previous = reindeer;
        }
        return score;
    }

    public  Node<Reindeer> CreateReindeerPathGraf(Reindeer start, Map2D<char> map)
    {
        List<Point2D> breadcrumbs = [];
        Node<Reindeer> root = ScanPathsRecursivly(start, map, breadcrumbs)
            ?? throw new InvalidOperationException("Somehow the root node is null");
        return root;
    }

    public Node<Reindeer>? ScanPathsRecursivly(Reindeer reindeer, Map2D<char> map, List<Point2D> breadcrumbs)
    {
        Node<Reindeer> parent = new(reindeer);
        var reindeersToCheck = GetReindeersToCheck(reindeer);
        foreach (var reindeerToCheck in reindeersToCheck)
        {
            if (map[reindeerToCheck.Position] == '.' && !breadcrumbs.Contains(reindeerToCheck.Position))
            {
                breadcrumbs.Add(reindeerToCheck.Position);
                var newChild = ScanPathsRecursivly(reindeerToCheck, map, [.. breadcrumbs]);
                if (newChild is null)
                    continue;
                newChild.Parent = parent;
                _progressOutput(PrintNode(newChild, map));
                parent.Children.Add(newChild);
            }
            else if (map[reindeerToCheck.Position] == 'E')
                parent.Children.Add(new(reindeerToCheck, parent));
        }
        if (parent.IsLeaf)
            return null;
        return parent;
    }

    public static IEnumerable<Reindeer> GetReindeersToCheck(Reindeer reindeer)
    {
        CardinalDirection right = reindeer.Direction.TurnClockwise();
        Point2D pointRight = reindeer.Position.GetPointInDirection(right);
        yield return new(pointRight, right);

        Point2D pointInFront = reindeer.Position.GetPointInDirection(reindeer.Direction);
        yield return new(pointInFront, reindeer.Direction);

        CardinalDirection left = reindeer.Direction.TurnAntiClockwise();
        Point2D pointLeft = reindeer.Position.GetPointInDirection(left);
        yield return new(pointLeft, left);
    }

    public static string PrintNode(Node<Reindeer> node, Map2D<char> map)
    {
        var mapCopy = map.Clone();

        foreach (var child in node.GetAllChildren())
        {
            if (map[child.Value.Position] is '.')
                mapCopy[child.Value.Position] = child.Value.Direction.ToArrow();
        }
        mapCopy[node.Value.Position] = 'X';
        foreach(var parent in node.GetAncestors().Skip(1))
        {
            mapCopy[parent.Value.Position] = parent.Value.Direction.ToArrow();
        }
        return mapCopy.PrintMap();
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
}
