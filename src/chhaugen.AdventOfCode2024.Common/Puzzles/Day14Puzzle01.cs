using chhaugen.AdventOfCode2024.Common.Structures;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day14Puzzle01 : Puzzle
{
    public Day14Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
        => SolveAsync(input, xMax: 101, yMax: 103);
        

    public Task<string> SolveAsync(string input, long xMax, long yMax)
    {
        Robot[] robots = ParseInput(input);
        _progressOutput(PrintRobots(robots, xMax, yMax));

        for (int i = 0; i < 100; i++)
        {
            for (int j =  0; j < robots.Length; j++)
            {
                robots[j] = robots[j].ElapseOneSecond(xMax, yMax);
            }
            _progressOutput(PrintRobots(robots, xMax, yMax));
        }

        long xMiddle = (xMax / 2);
        long yMiddle = (yMax / 2);
        List<Robot> coutingRobots = [];
        foreach(Robot robot in robots)
        {
            if (robot.Position.X == xMiddle)
                continue;
            if (robot.Position.Y == yMiddle)
                continue;
            coutingRobots.Add(robot);
        }
        _progressOutput(PrintRobots(coutingRobots, xMax, yMax));

        List<Robot> firstQuadrant  = [];
        List<Robot> secondQuadrant = [];
        List<Robot> thirdQuadrant  = [];
        List<Robot> forthQuadrant  = [];
        foreach (Robot robot in coutingRobots)
        {
            if (0 <= robot.Position.X && robot.Position.X <= xMax / 2 &&
                0 <= robot.Position.Y && robot.Position.Y <= yMax / 2)
                firstQuadrant.Add(robot);

            if (xMax / 2 <  robot.Position.X && robot.Position.X <= xMax     &&
                0        <= robot.Position.Y && robot.Position.Y <= yMax / 2 )
                secondQuadrant.Add(robot);

            if (xMax / 2 < robot.Position.X && robot.Position.X <= xMax &&
                yMax / 2 < robot.Position.Y && robot.Position.Y <= yMax )
                thirdQuadrant.Add(robot);

            if (0        <= robot.Position.X && robot.Position.X <  xMax / 2 &&
                yMax / 2 <  robot.Position.Y && robot.Position.Y <= yMax     )
                forthQuadrant.Add(robot);
        }
        long safetyFactor = firstQuadrant.Count * secondQuadrant.Count * thirdQuadrant.Count * forthQuadrant.Count;

        return Task.FromResult(safetyFactor.ToString());
    }

    public static Robot[] ParseInput(string input)
    {
        string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Robot[] robots = new Robot[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            var parts = lines[i].Split(' ');
            var positionParts = parts[0].Replace("p=", string.Empty).Split(',').Select(long.Parse).ToArray();
            var velocityParts = parts[1].Replace("v=", string.Empty).Split(',').Select(long.Parse).ToArray();
            robots[i] = new Robot(new(positionParts[0], positionParts[1]), new(velocityParts[0], velocityParts[1]));
        }
        return robots;
    }

    public static string PrintRobots(IEnumerable<Robot> robots, long xMax, long yMax)
    {
        char[,] counts = new char[xMax, yMax];
        for (long x = 0; x < counts.GetLongLength(0); x++)
            for (long y = 0; y < counts.GetLongLength(1); y++)
            {
                counts[x, y] = '.';
            }
        foreach(Robot robot in robots)
        {
            if(counts[robot.Position.X, robot.Position.Y] == '.')
            {
                counts[robot.Position.X, robot.Position.Y] = 1.ToString()[0];
            }
            else
            {
                counts[robot.Position.X, robot.Position.Y]++;
            }
        }
        Map2D<char> map = new(counts);
        return map.PrintMap();
    }

    public readonly struct Robot
    {
        public Robot(Point2D position, Vector2D velocity)
        {
            Position = position;
            Velocity = velocity;
        }

        public Point2D Position { get; }
        public Vector2D Velocity { get; }

        public Robot ElapseOneSecond(long xMax, long yMax)
        {
            Point2D newPosition = Position.Add(Velocity);
            newPosition = new((newPosition.X + xMax) % xMax, (newPosition.Y + yMax) % yMax);
            return new(newPosition, Velocity);
        }

        public override string ToString()
            => $"p={Position.X},{Position.Y} v={Velocity.X},{Velocity.Y}";
    } 
}
