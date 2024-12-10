namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day04Puzzle01 : Puzzle
{
    public Day04Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {
        var searchString = "XMAS";

        //Example text = 18 times
        //input = "MMMSXXMASM\nMSAMXMSMSA\nAMXSXMAAMM\nMSAMASMSMX\nXMASAMXAMM\nXXAMMXXAMA\nSMSMSASXSS\nSAXAMASAAA\nMAMMMXMMMM\nMXMXAXMASX";

        char[,] inputArray = ParseInput(input);

        List<Node> xNodes = [];
        var xWidth = inputArray.GetLength(0);
        var yHeight = inputArray.GetLength(1);
        for (int x = 0; x < xWidth; x++)
        {
            for (int y = 0; y < yHeight; y++)
            {
                char value = inputArray[x, y];
                if (value == searchString[0])
                {
                    xNodes.Add(new(x, y, 0));
                }
            }
        }

        SearchRecursively(xNodes, inputArray, searchString);
        //_progressOutput(JsonSerializer.Serialize(xNodes, options: new() { WriteIndented = true }));

        int leafCount = CountLeafs(xNodes, searchString.Length);

        return Task.FromResult(leafCount.ToString());
    }


    public static char[,] ParseInput(string input)
    {
        var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        char[,] array = new char[lines[0].Length, lines.Length];
        for (int x = 0; x < lines[0].Length; x++)
        {
            for (int y = 0; y < lines.Length; y++)
            {
                array[x, y] = lines[y][x];
            }
        }
        return array;
    }

    public static void SearchRecursively(List<Node> nodes, char[,] array, string searchString)
    {
        foreach (Node node in nodes)
        {
            var scanResult = node.ScanForNeighbours(array, searchString);
            if (scanResult.Count > 0)
                SearchRecursively(scanResult, array, searchString);
        }
    }

    public static int CountLeafs(List<Node> nodes, int searchStringLength)
    {
        int count = 0;
        int lastIndex = searchStringLength - 1;
        foreach (var node in nodes)
        {
            if (node.SearchStringIndex == lastIndex)
            {
                count++;
                continue;
            }

            if (node.Neighbours.Count > 0)
            {
                count += CountLeafs(node.Neighbours, searchStringLength);
            }

        }
        return count;
    }

    public class Node
    {
        public Node(int x, int y, int searchStringIndex, Direction? direction = null)
        {
            X = x;
            Y = y;
            SearchStringIndex = searchStringIndex;
            Direction = direction;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public Direction? Direction { get; set; }

        public int SearchStringIndex { get; set; } = 0;

        public List<Node> Neighbours { get; private set; } = [];

        public Dictionary<Direction, Point> GetSurroundingCoordinates()
            => new()
            {
                {Day04Puzzle01.Direction.UpLeft   , new (X-1, Y+1)},
                {Day04Puzzle01.Direction.Up       , new (X  , Y+1)},
                {Day04Puzzle01.Direction.UpRight  , new (X+1, Y+1)},
                {Day04Puzzle01.Direction.Left     , new (X-1, Y  )},
                {Day04Puzzle01.Direction.Right    , new (X+1, Y  )},
                {Day04Puzzle01.Direction.DownLeft , new (X-1, Y-1)},
                {Day04Puzzle01.Direction.Down     , new (X  , Y-1)},
                {Day04Puzzle01.Direction.DownRight, new (X+1, Y-1)},
            };

        public List<Node> ScanForNeighbours(char[,] array, string searchString)
        {
            List<Node> result = [];
            int nextSearchStringIndex = SearchStringIndex + 1;

            if (!(searchString.Length > nextSearchStringIndex))
                return Neighbours = result;

            var coordsToSearch = GetSurroundingCoordinates();
            var xWidth = array.GetLength(0);
            var yHeight = array.GetLength(1);

            if (Direction.HasValue)
            {
                Point directionPoint = coordsToSearch[Direction.Value];
                coordsToSearch = new() { { Direction.Value, directionPoint } };
            }

            foreach (var coord in coordsToSearch)
            {
                if (!(0 <= coord.Value.X && coord.Value.X < xWidth))
                    continue;

                if (!(0 <= coord.Value.Y && coord.Value.Y < yHeight))
                    continue;

                char value = array[coord.Value.X, coord.Value.Y];
                char matchingValue = searchString[nextSearchStringIndex];
                if (value == matchingValue)
                {
                    result.Add(new(coord.Value.X, coord.Value.Y, nextSearchStringIndex, coord.Key));
                }
            }
            return Neighbours = result;
        }
    }

    public struct Point
    {
        public Point(int x, int y)
        {
            X = x; Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
    }


    public enum Direction
    {
        UpLeft,
        Up,
        UpRight,
        Left,
        Right,
        DownLeft,
        Down,
        DownRight,
    }
}
