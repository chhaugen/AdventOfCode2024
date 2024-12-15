using chhaugen.AdventOfCode2024.Common.Structures;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day15Puzzle01 : Puzzle
{
    public Day15Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {

        (Map2D<IMapObject> map, List<IMove> moves) = ParseInputs(input);

        Point2D robotPoint = map.AsPointEnumerable().First(x => map[x] is Robot);

        foreach (IMove move in moves)
        {
            robotPoint = DoMove(robotPoint, move, map);
        }

        var gpsList = map
            .AsPointEnumerable()
            .Where(x => map[x] is Box)
            .Select(x => (x.Y * 100) + x.X)
            .ToList();

        var sum = gpsList.Sum();

        return Task.FromResult(sum.ToString());
    }

    public static Point2D DoMove(Point2D robotPoint, IMove move, Map2D<IMapObject> map)
    {
        if (TryMove(robotPoint, move, map))
        {
            return robotPoint.GetPointInDirection(move.Direction);
        }
        else
        {
            return robotPoint;
        }
    }

    public static bool TryMove(Point2D objectPoint, IMove move, Map2D<IMapObject> map)
    {
        Point2D pointInfront = objectPoint.GetPointInDirection(move.Direction);
        if (map[pointInfront] is Wall)
            return false;

        if (map[pointInfront] is Floor)
        {
            (map[pointInfront], map[objectPoint]) = (map[objectPoint], map[pointInfront]);
            return true;
        }

        if (map[pointInfront] is Box or Robot)
        {
            if (TryMove(pointInfront, move, map))
            {
                (map[pointInfront], map[objectPoint]) = (map[objectPoint], map[pointInfront]);
                return true;
            }
        }
        return false;

    }


    public static (Map2D<IMapObject>, List<IMove>) ParseInputs(string input)
    {
        var mapAndMovesStrings = input.Split("\n\n");
        var mapStrings = mapAndMovesStrings[0].Split('\n');
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
                    'O' => new Box(),
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

    public readonly struct Box : IMapObject
    {
        public override string ToString()
            => "O";
    }

    public readonly struct Robot : IMapObject
    {
        public override string ToString()
            => "@";
    }
}
