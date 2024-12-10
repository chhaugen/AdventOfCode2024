using AdventOfCode2024.Extentions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AdventOfCode2024.Puzzles;
public class Day10Puzzle01 : Puzzle
{
    public Day10Puzzle01(ILogger logger, DirectoryInfo puzzleResourceDirectory) : base(logger, puzzleResourceDirectory)
    {
    }

    public override async Task<string> SolveAsync()
    {
        var inputFile = _puzzleResourceDirectory.GetFiles("input.txt").First();
        var inputString = await inputFile.ReadAllTextAsync();

        // Example
        //var exampleFile = _puzzleResourceDirectory.GetFiles("example.txt").First();
        //inputString = await exampleFile.ReadAllTextAsync();

        TrialMap map = TrialMap.ParseInput(inputString);

        var startingNodes = map.GetPossibleStaringNodes();

        //var flattenedTrials = TrialNode
        //    .FlattenTrials(startingNodes)
        //    .Where(x => x.Select(x => x.Point.Value).First() == 9);

        //var nineCount = map.Count(number: 9);

        //Console.WriteLine(TrialNode.PrintFlattendTrials(flattenedTrials));

        var scores = startingNodes
            .Select(x => x
                .GetLeafs()
                .Where(y => y.Point.Value == 9)
                .DistinctBy(x => (x.Point.X, x.Point.Y))
                .ToList())
            .ToList();

        var totalScore = scores.Sum(x => x.Count);

        return totalScore.ToString();
    }

    public class TrialMap
    {
        private readonly ushort[,] _map;
        private readonly int _xLength;
        private readonly int _yLength;

        private TrialMap(ushort[,] map)
        {
            _map = map;
            _xLength = map.GetLength(0);
            _yLength = map.GetLength(1);
        }

        public List<TrialNode> GetPossibleStaringNodes()
        {
            List<TrialNode> points = [];
            for (int x = 0; x < _xLength; x++)
            {
                for (int y = 0; y < _yLength; y++)
                {
                    ushort value = _map[x, y];
                    if (value == 0)
                        points.Add(new TrialNode(new(this, x, y)));
                }
            }
            return points;
        }

        public ushort this[int x, int y]
        {
            get => _map[x, y];
            set => _map[x, y] = value;
        }

        public ushort this[MapPoint point]
        {
            get => _map[point.X, point.Y];
            set => _map[point.X, point.Y] = value;
        }

        public bool IsPointOnMap(int x, int y)
            => 0 <= x && x < _xLength && 0 <= y && y < _yLength;

        public bool IsPointOnMap(MapPoint point)
            => IsPointOnMap(point.X, point.Y);

        public int Count(ushort number)
        {
            int count = 0;
            for (int x = 0; x < _xLength; x++)
            {
                for (int y = 0; y < _yLength; y++)
                {
                    if (number == _map[x, y])
                        count++;
                }
            }
            return count;
        }

        public static TrialMap ParseInput(string input)
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
                    ReadOnlySpan<char> charSpan = line.AsSpan(x,1);
                    map[x, y] = ushort.Parse(charSpan);
                }
            }
            return new TrialMap(map);
        }
    }

    public readonly struct MapPoint
    {
        private readonly TrialMap _map;

        public MapPoint(TrialMap map, int x, int y)
        {
            _map = map;
            (X, Y) = (x, y);
        }

        public int X { get;}

        public int Y { get; }

        public MapPoint West
            => new(_map, X - 1, Y);
        public MapPoint North
            => new(_map, X, Y - 1);
        public MapPoint East
            => new(_map, X + 1, Y);
        public MapPoint South
            => new(_map, X, Y + 1);

        public ushort Value
            => _map[this];

        public bool IsOnMap
            => _map.IsPointOnMap(this);

        public IEnumerable<MapPoint> ScanForPosibleTrialContinuation()
        {
            var currentValue = Value;

            {
                var west = West;
                if (west.IsOnMap && west.Value == currentValue + 1)
                    yield return west;
            }

            {
                var north = North;
                if (north.IsOnMap && north.Value == currentValue + 1)
                    yield return north;
            }

            {
                var east = East;
                if (east.IsOnMap && east.Value == currentValue + 1)
                    yield return east;
            }

            {
                var south = South;
                if (south.IsOnMap && south.Value == currentValue + 1)
                    yield return south;
            }
        }

        public override string ToString()
            => $"({X}, {Y})";
    }

    public class TrialNode
    {
        public TrialNode(MapPoint mapPoint, TrialNode? parrent = null)
        {
            Point = mapPoint;
            Parrent = parrent;
        }

        public TrialNode? Parrent { get; }

        public MapPoint Point { get; }

        public IEnumerable<TrialNode> Children => Point
            .ScanForPosibleTrialContinuation()
            .Select(x => new TrialNode(x, this));

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

        public static string PrintFlattendTrials(IEnumerable<IEnumerable<TrialNode>> trialNodes)
        {
            StringBuilder sb = new();
            sb.Append('[');
            foreach (var trialNode in trialNodes)
            {
                sb.Append('[');
                foreach (var leaf in trialNode)
                {
                    sb.Append($"{leaf.Point.Value}: {leaf.Point}, ");
                }
                sb.Append(']');
            }
            sb.Append(']');
            return sb.ToString();
        }
    }
}
