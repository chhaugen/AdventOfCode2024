using chhaugen.AdventOfCode2024.Common.Structures;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day15Puzzle02 : Puzzle
{
    public Day15Puzzle02(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {

        (Map2D<IMapObject> map, List<IMove> moves) = ParseInputs(input);
        _progressOutput(map.PrintMap(x => x.ToString()));

        Point2D robotPoint = map.AsPointEnumerable().First(x => map[x] is Robot);

        foreach (IMove move in moves)
        {
            robotPoint = DoMove(robotPoint, move, map);
            //_progressOutput(map.PrintMap(x => x.ToString()));
        }
        _progressOutput(map.PrintMap(x => x.ToString()));
        var gpsList = map
            .AsPointEnumerable()
            .Where(x => map[x] is LeftBox)
            .Select(x => (x.Y * 100) + x.X)
            //.Select(x => (x.Y <= (map.Height / 2) ? (x.Y * 100) : ((map.Height -1 - x.Y) * 100)) + (x.X <= (map.Width / 2) ? x.X : (map.Height -1 - x.X)))
            .ToList();

        var sum = gpsList.Sum();

        return Task.FromResult(sum.ToString());
    }

    public static Point2D DoMove(Point2D robotPoint, IMove move, Map2D<IMapObject> map)
    {
        List<Action<Map2D<IMapObject>>> shuffleMoves = [];
        Point2D pointInfront = robotPoint.GetPointInDirection(move.Direction);
        if (!ScanMove(pointInfront, move, map, shuffleMoves))
            return robotPoint;
        shuffleMoves.Add(x => (x[pointInfront], x[robotPoint]) = (x[robotPoint], x[pointInfront]));

        foreach (var shuffleMove in shuffleMoves)
        {
            shuffleMove(map);
        }
        return pointInfront;
    }

    public static bool ScanMove(Point2D objectPoint, IMove move, Map2D<IMapObject> map, List<Action<Map2D<IMapObject>>> shuffleMoves)
    { 
        if (map[objectPoint] is Floor)
            return true;

        if (map[objectPoint] is Wall)
            return false;

        if (map[objectPoint] is RightBox)
        {
            return ScanMoveBox(objectPoint.West, objectPoint, move, map, shuffleMoves);
        }

        if (map[objectPoint] is LeftBox)
        {
            return ScanMoveBox(objectPoint, objectPoint.East, move, map, shuffleMoves);
        }

        return false;

    }

    public static bool ScanMoveBox(Point2D boxLeft, Point2D boxRight, IMove move, Map2D<IMapObject> map, List<Action<Map2D<IMapObject>>> shuffleMoves)
    {
        switch (move.Direction)
        {
            case CardinalDirection.West:
                Point2D pointInfrontWest = boxLeft.West;
                if (ScanMove(pointInfrontWest, move, map, shuffleMoves))
                {
                    shuffleMoves.Add(x => (x[pointInfrontWest], x[boxLeft], x[boxRight]) = (x[boxLeft], x[boxRight], x[pointInfrontWest]));
                    return true;
                }
                return false;
            case CardinalDirection.East:
                Point2D pointInfrontEast = boxRight.East;
                if (ScanMove(pointInfrontEast, move, map, shuffleMoves))
                {
                    shuffleMoves.Add(x => (x[boxLeft], x[boxRight], x[pointInfrontEast]) = (x[pointInfrontEast], x[boxLeft], x[boxRight]));
                    return true;
                }
                return false;
            case CardinalDirection.North:
            case CardinalDirection.South:
                Point2D leftInfront = boxLeft.GetPointInDirection(move.Direction);
                Point2D rightInfront = boxRight.GetPointInDirection(move.Direction);
                bool moveSucess = false;
                if (map[leftInfront] is LeftBox && map[rightInfront] is RightBox)
                    moveSucess = ScanMoveBox(leftInfront, rightInfront, move, map, shuffleMoves);
                else
                    moveSucess = ScanMove(leftInfront, move, map, shuffleMoves) && ScanMove(rightInfront, move, map, shuffleMoves);
                if (moveSucess)
                {
                    shuffleMoves.Add(x => (x[leftInfront], x[boxLeft]) = (x[boxLeft], x[leftInfront]));
                    shuffleMoves.Add(x => (x[rightInfront], x[boxRight]) = (x[boxRight], x[rightInfront]));
                    return true;
                }
                return false;
            default:
                return false;
        }

    }


    public static string ExpandMapString(string input)
        => input
        .Replace("#", "##")
        .Replace(".", "..")
        .Replace("O", "[]")
        .Replace("@", "@.");

    public static (Map2D<IMapObject>, List<IMove>) ParseInputs(string input)
    {
        var mapAndMovesStrings = input.Split("\n\n");
        var mapString = ExpandMapString(mapAndMovesStrings[0]);
        var mapStrings = mapString.Split('\n');
        var movesString = mapAndMovesStrings[1].Replace("\n", string.Empty);

        int yLength = mapStrings.Length;
        int xLength = mapStrings[0].Length;
        IMapObject[,] mapArray = new IMapObject[xLength, yLength];
        for (int y = 0; y < yLength; y++)
        {
            var mapLine = mapStrings[y];
            for (int x = 0; x < xLength; x++)
            {
                var mapChar = mapLine[x];
                mapArray[x, y] = mapChar switch
                {
                    '#' => new Wall(),
                    '.' => new Floor(),
                    '[' => new LeftBox(),
                    ']' => new RightBox(),
                    '@' => new Robot(),
                    _ => throw new NotImplementedException(),
                };
            }
        }

        List<IMove> moves = [];
        foreach (var move in movesString)
        {
            IMove moveObject = move switch
            {
                '<' => new MoveLeft(),
                '^' => new MoveUp(),
                '>' => new MoveRight(),
                'v' => new MoveDown(),
                _ => throw new NotImplementedException(),
            };
            moves.Add(moveObject);
        }

        return (new(mapArray),  moves);
    }

    public interface IMove
    {
        string ToString();
        CardinalDirection Direction { get; }
    }

    public readonly struct MoveUp : IMove
    {
        public CardinalDirection Direction => CardinalDirection.North;

        public override string ToString()
            => "^";
    }
    public readonly struct MoveDown : IMove
    {
        public CardinalDirection Direction => CardinalDirection.South;
        public override string ToString()
            => "v";
    }
    public readonly struct MoveLeft : IMove
    {
        public CardinalDirection Direction => CardinalDirection.West;
        public override string ToString()
            => "<";
    }
    public readonly struct MoveRight : IMove
    {
        public CardinalDirection Direction => CardinalDirection.East;
        public override string ToString()
            => ">";
    }

    public interface IMapObject
    {
        string ToString();
    }

    public readonly struct Floor : IMapObject
    {
        public override string ToString()
            => ".";
    }

    public readonly struct Wall : IMapObject
    {
        public override string ToString()
            => "#";
    }

    public readonly struct LeftBox : IMapObject
    {
        public override string ToString()
            => "[";
    }
    public readonly struct RightBox : IMapObject
    {
        public override string ToString()
            => "]";
    }

    public readonly struct Robot : IMapObject
    {
        public override string ToString()
            => "@";
    }
}
