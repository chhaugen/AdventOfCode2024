using chhaugen.AdventOfCode2024.Common.Structures;
using static chhaugen.AdventOfCode2024.Common.Puzzles.Day14Puzzle01;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day14Puzzle02 : Puzzle
{
    public Day14Puzzle02(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
        => SolveAsync(input, xMax: 101, yMax: 103);

    public Task<string> SolveAsync(string input, long xMax, long yMax)
    {
        Robot[] robots = ParseInput(input);

        for (int i = 0; i < 7672; i++)
        {
            for (int j = 0; j < robots.Length; j++)
            {
                robots[j] = robots[j].ElapseOneSecond(xMax, yMax);
            }
            var entropy = RobotsInLine(robots).Max(x => x.Value);
            if (entropy > 30)
            {
                _progressOutput((i+1).ToString());
                _progressOutput(entropy.ToString());
                _progressOutput(PrintRobots(robots, xMax, yMax));
            }
        }

        return Task.FromResult(string.Empty);
    }

    public static int DiagonalScores(Robot[] robots)
        => robots
        .Select(x => DoesRobotHaveDaigonalNeighbours(x, robots))
        .Where(x => x)
        .Count();

    public static bool DoesRobotHaveDaigonalNeighbours(Robot robot, Robot[] robots)
    {
        bool northEastExists = robots.Any(x => x.Position == robot.Position.NorthEast);
        bool southWestExists = robots.Any(x => x.Position == robot.Position.SouthWest);

        if (northEastExists && southWestExists)
            return true;

        bool northWestExists = robots.Any(x => x.Position == robot.Position.NorthWest);
        bool southEastExists = robots.Any(x => x.Position == robot.Position.SouthEast);

        return northEastExists && southWestExists;
    }

    public Dictionary<long, int> RobotsInLine(Robot[] robots)
        => robots
        .GroupBy(x => x.Position.Y)
        .ToDictionary(x => x.Key, x => x.Count());

}
