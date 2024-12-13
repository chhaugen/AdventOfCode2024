using chhaugen.AdventOfCode2024.Common.Structures;
using System.Text;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day10Puzzle01 : Puzzle
{
    public Day10Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string inputString)
    {

        Map2D<ushort> map = ParseInput(inputString);

        var startingNodes = GetPossibleStaringNodes(map);

        //var flattenedTrials = TrialNode
        //    .FlattenTrials(startingNodes)
        //    .Where(x => x.Select(x => x.Point2D.Value).First() == 9);

        //var nineCount = map.Count(number: 9);

        //_progressOutput(TrialNode.PrintFlattendTrials(flattenedTrials));

        var scores = startingNodes
            .Select(x => x
                .GetLeafs()
                .Where(y => map[y.Point] == 9)
                .DistinctBy(x => x.Point)
                .ToList())
            .ToList();

        var totalScore = scores.Sum(x => x.Count);

        return Task.FromResult(totalScore.ToString());
    }
    
    public static IEnumerable<TrialNode> GetPossibleStaringNodes(Map2D<ushort> map)
        => map
        .AsPointEnumerable()
        .Where(x => map[x] == 0)
        .Select(x => new TrialNode(map, x));


    public static IEnumerable<Point2D> ScanForPosibleTrialContinuation(Point2D point, Map2D<ushort> map)
    {
        var currentValue = map[point];
        foreach (var direction in Enum.GetValues<CardinalDirection>())
        {
            var possibleNeighbor = point.GetPointInDirection(direction);
            if (map.HasPoint(possibleNeighbor) && map[possibleNeighbor] == currentValue +1)
                yield return possibleNeighbor;
        }
    }

    public static Map2D<ushort> ParseInput(string input)
    {
        var lines = input
            .Split('\n', StringSplitOptions.RemoveEmptyEntries);

        int yLength = lines.Length;
        int xLength = lines[0].Length;

        ushort[,] map = new ushort[xLength, yLength];

        for (int y = 0; y < yLength; y++)
        {
            var line = lines[y];
            for (int x = 0; x < xLength; x++)
            {
                ReadOnlySpan<char> charSpan = line.AsSpan(x, 1);
                map[x, y] = ushort.Parse(charSpan);
            }
        }
        return new(map);
    }


    public class TrialNode
    {
        private readonly Map2D<ushort> _map;
        public TrialNode(Map2D<ushort> map, Point2D mapPoint, TrialNode? parrent = null)
        {
            _map = map;
            Point = mapPoint;
            Parrent = parrent;
        }

        public TrialNode? Parrent { get; }

        public Point2D Point { get; }

        public IEnumerable<TrialNode> Children
            => ScanForPosibleTrialContinuation(Point, _map)
            .Select(x => new TrialNode(_map, x, this));

        public bool IsLeaf => !Children.Any();

        public IEnumerable<IEnumerable<TrialNode>> FlattenTrials()
            => GetLeafs().Select(x => x.GetAncestry());

        public IEnumerable<TrialNode> GetLeafs()
        {
            if (IsLeaf)
            {
                yield return this;
                yield break;
            }

            foreach (var child in Children)
            {
                var childLeaf = child.GetLeafs();
                foreach (var leaf in childLeaf)
                {
                    yield return leaf;
                }
            }
        }

        public IEnumerable<TrialNode> GetAncestry()
        {
            yield return this;

            if (Parrent is null)
            {
                yield break;
            }

            foreach (var ancestor in Parrent.GetAncestry())
                yield return ancestor;

        }

        public static IEnumerable<IEnumerable<TrialNode>> FlattenTrials(IEnumerable<TrialNode> trialNodes)
        {
            foreach (var trialNode in trialNodes)
            {
                foreach (var flattenedTrial in trialNode.FlattenTrials())
                    yield return flattenedTrial;
            }
        }

        public static string PrintFlattendTrials(IEnumerable<IEnumerable<TrialNode>> trialNodes, Map2D<char> map)
        {
            StringBuilder sb = new();
            sb.Append('[');
            foreach (var trialNode in trialNodes)
            {
                sb.Append('[');
                foreach (var leaf in trialNode)
                {
                    sb.Append($"{map[leaf.Point]}: {leaf.Point}, ");
                }
                sb.Append(']');
            }
            sb.Append(']');
            return sb.ToString();
        }
    }
}
