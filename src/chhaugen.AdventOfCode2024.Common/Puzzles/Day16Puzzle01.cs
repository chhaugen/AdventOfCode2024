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
        Map2D<char> map = Map2D.ParseInput(input);

        var whiteSpaceCount = map.CountOf('.');

        Point2D start = map
            .AsPointEnumerable()
            .First(x => map[x] == 'S');

        Point2D end = map
            .AsPointEnumerable()
            .First(x => map[x] == 'E');

        Reindeer reindeer = new(start, CardinalDirection.East);

        RemoveBlindSpots(map);
        RemoveLoops(map);
        
        Node<Reindeer>? graf = null;

        var stackSize = 10000000;
        ThreadStart threadStart = () => graf = CreateReindeerPathGraf(reindeer, map);
        Thread grafThread = new(threadStart, stackSize);

        grafThread.Start();
        grafThread.Join();

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

    public void RemoveBlindSpots(Map2D<char> map)
    {
        List<Point2D> blindSpots = [];
        _progressOutput(map.PrintMap());
        do
        {
            blindSpots = map
                .AsPointEnumerable()
                .Where(p => map[p] == '.' && PathWaysCount(p, map) == 1)
                .ToList();

            blindSpots.ForEach(x => map[x] = '#');
            _progressOutput(map.PrintMap());

        } while (blindSpots.Count > 0);
    }

    public void RemoveLoops(Map2D<char> map)
    {   
        Point2D startPoint = default;
        do
        {
            startPoint = map
                .AsPointEnumerable()
                .FirstOrDefault(p => map[p] == '.' && PathWaysCount(p, map) == 2);

            var loopPoints = HugWallLeftBackToStart(startPoint, CardinalDirection.West, startPoint, map).ToList();
            List<(Point2D, int)> loopEntries = [];

            for (int i = 0; i < loopPoints.Count; i++)
            {
                var loopPoint = loopPoints[i];
                if (PathWaysCount(loopPoint, map) > 2)
                    loopEntries.Add((loopPoint, i));
            }

            if (loopEntries.Count == 2)
            {
                var loopEntry1 = loopEntries[0];
                var loopEntry2 = loopEntries[1];

                var length1 = Math.Abs(loopEntry1.Item2 - loopEntry2.Item2);
                var legngt2 = loopPoints.Count - 1 - length1;

                if (length1 > legngt2)
                {
                    Span<Point2D> span1 = loopPoints.
                }
            }


        } while (startPoint != default);
    }

    public IEnumerable<Point2D> HugWallLeftBackToStart(Point2D currentPoint, CardinalDirection currentDirection, Point2D start, Map2D<char> map)
    {
        Point2D pointInFront = currentPoint.GetPointInDirection(currentDirection);
        Point2D pointToLeft = currentPoint.GetPointInDirection(currentDirection.TurnAntiClockwise());
        if (pointInFront == start)
        {
        }
        else if (map[pointToLeft] == '.')
        {
            foreach (var p in HugWallLeftBackToStart(pointToLeft, currentDirection.TurnAntiClockwise(), start, map))
                yield return p;
            yield return currentPoint;
        }
        else if (map[pointInFront] == '.')
        {
            foreach (var p in HugWallLeftBackToStart(pointInFront, currentDirection, start, map))
                yield return p;
            yield return currentPoint;
        }
        else
        {
            foreach (var p in HugWallLeftBackToStart(currentPoint, currentDirection.TurnAntiClockwise(), start, map))
                yield return p;
        }
    }

    public static int PathWaysCount(Point2D point, Map2D<char> map)
    {
        int count = 4;
        foreach(CardinalDirection direction in Enum.GetValues<CardinalDirection>())
        {
            var pointInDirection = point.GetPointInDirection(direction);
            if (map[pointInDirection] == '#')
                count--;
        }
        return count;
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

    public Node<Reindeer> CreateReindeerPathGraf(Reindeer start, Map2D<char> map)
    {
        List<Point2D> breadcrumbs = [];
        Dictionary<Reindeer, Node<Reindeer>> memory = [];
        Node<Reindeer> root = ScanPathsRecursivly(start, map, breadcrumbs, memory)
            ?? throw new InvalidOperationException("Somehow the root node is null");
        return root;
    }

    public Node<Reindeer>? ScanPathsRecursivly(Reindeer reindeer, Map2D<char> map, List<Point2D> breadcrumbs, Dictionary<Reindeer, Node<Reindeer>> memory)
    {
        Node<Reindeer> parent = new(reindeer);
        var reindeersToCheck = GetReindeersToCheck(reindeer);
        foreach (var reindeerToCheck in reindeersToCheck)
        {
            if (map[reindeerToCheck.Position] == '.')
            {
                if (memory.TryGetValue(reindeerToCheck, out var previousNode))
                {
                    var newChild = previousNode.Clone(parent);
                    parent.Children.Add(newChild);
                }
                else if (!breadcrumbs.Contains(reindeerToCheck.Position))
                {
                    breadcrumbs.Add(reindeerToCheck.Position);
                    var newChild = ScanPathsRecursivly(reindeerToCheck, map, [.. breadcrumbs], memory);
                    if (newChild is null)
                        continue;
                    newChild.Parent = parent;
                    memory.Add(reindeerToCheck, newChild);
                    //_progressOutput(PrintNode(newChild, map));
                    parent.Children.Add(newChild);
                }
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
