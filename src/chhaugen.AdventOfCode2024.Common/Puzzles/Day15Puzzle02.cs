using chhaugen.AdventOfCode2024.Common.Extentions;
using chhaugen.AdventOfCode2024.Common.Structures;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day15Puzzle02 : Puzzle
{
    public Day15Puzzle02(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {

        (Map2D<char> map, List<CardinalDirection> moves) = ParseInputs(input);
        _progressOutput(map.PrintMap());

        Point2D robot = map
            .AsPointEnumerable()
            .First(p => map[p] == '@');

        foreach (var move in moves)
        {
            robot = DoMove(robot, map, move);
        }
        _progressOutput(map.PrintMap());


        var gpsList = map
            .AsPointEnumerable()
            .Where(x => map[x] is '[')
            .Select(x => (x.Y * 100) + x.X)
            .ToList();

        var sum = gpsList.Sum();

        return Task.FromResult(sum.ToString());
    }

    public static Point2D DoMove(Point2D robot, Map2D<char> map, CardinalDirection move)
    {
        List<Shuffle> shuffles = [];
        Point2D inFrontOfRobot = robot.GetPointInDirection(move);
        if (!ScanShufflesRecursivly(inFrontOfRobot, move, map, shuffles))
            return robot;

        shuffles.Add(new(robot, m => (m[inFrontOfRobot], m[robot]) = (m[robot], m[inFrontOfRobot])));

        var distinctShuffles = shuffles
            .DistinctBy(x => x.Target)
            .ToList();

        foreach (var shuffle in distinctShuffles)
        {
            shuffle.Action(map);
        }
        return inFrontOfRobot; ;
    }

    public static bool ScanShufflesRecursivly(Point2D pointToScan, CardinalDirection move, Map2D<char> map, List<Shuffle> shuffles)
    {
        if (map[pointToScan] == '#')
            return false;

        if (map[pointToScan] == '.')
            return true;

        if (map[pointToScan] == '[')
        {
            if (move == CardinalDirection.East)
            {
                Point2D pointInFront = pointToScan.East.East;
                if (!ScanShufflesRecursivly(pointInFront, move, map, shuffles))
                    return false;

                shuffles.Add(new(pointToScan, m => (m[pointToScan], m[pointToScan.East], m[pointInFront]) = (m[pointInFront], m[pointToScan], m[pointToScan.East])));
                return true;
            }
            Point2D leftBox = pointToScan;
            Point2D rightBox = pointToScan.East;
            Point2D leftBoxInFront = leftBox.GetPointInDirection(move);
            Point2D rightBoxInfront = leftBoxInFront.East;
            return ScanWideBox(move, map, shuffles, leftBox, rightBox, leftBoxInFront, rightBoxInfront);
        }

        if (map[pointToScan] == ']')
        {
            if (move == CardinalDirection.West)
            {
                Point2D pointInFront = pointToScan.West.West;
                if (!ScanShufflesRecursivly(pointInFront, move, map, shuffles))
                    return false;

                shuffles.Add(new(pointToScan, m => (m[pointInFront], m[pointToScan.West], m[pointToScan]) = (m[pointToScan.West], m[pointToScan], m[pointInFront])));
                return true;
            }
            Point2D rightBox = pointToScan;
            Point2D leftBox = rightBox.West;
            Point2D rightBoxInfront = rightBox.GetPointInDirection(move);
            Point2D leftBoxInFront = rightBoxInfront.West;
            return ScanWideBox(move, map, shuffles, leftBox, rightBox, leftBoxInFront, rightBoxInfront);
        }

        return false;
    }

    private static bool ScanWideBox(CardinalDirection move, Map2D<char> map, List<Shuffle> shuffles, Point2D leftBox, Point2D rightBox, Point2D leftBoxInFront, Point2D rightBoxInfront)
    {
        if (map[leftBoxInFront] == '[' && map[rightBoxInfront] == ']')
        {
            if (!ScanShufflesRecursivly(leftBoxInFront, move, map, shuffles))
                return false;
        }
        else
        {
            if (!ScanShufflesRecursivly(leftBoxInFront, move, map, shuffles) || !ScanShufflesRecursivly(rightBoxInfront, move, map, shuffles))
                return false;
        }

        shuffles.Add(new(leftBox, m => (m[leftBox], m[leftBoxInFront]) = (m[leftBoxInFront], m[leftBox])));
        shuffles.Add(new(rightBox, m => (m[rightBox], m[rightBoxInfront]) = (m[rightBoxInfront], m[rightBox])));
        return true;
    }

    public static string ExpandMapString(string input)
        => input
        .Replace("#", "##")
        .Replace(".", "..")
        .Replace("O", "[]")
        .Replace("@", "@.");

    public static (Map2D<char>, List<CardinalDirection>) ParseInputs(string input)
    {
        var mapAndMovesStrings = input.Split("\n\n");
        var mapString = ExpandMapString(mapAndMovesStrings[0]);
        var movesString = mapAndMovesStrings[1].Replace("\n", string.Empty);

        Map2D<char> map = Map2D.ParseInput(mapString);

        List<CardinalDirection> moves = movesString
            .Select(CardinalDirectionExtentions.ArrowToCardinalDirection)
            .ToList();

        return (map, moves);
    }

    public readonly struct Shuffle
    {
        public Shuffle(Point2D target, Action<Map2D<char>> action)
        {
            Target = target;
            Action = action;
        }

        public Point2D Target { get; }
        public Action<Map2D<char>> Action { get; }
    }

}
