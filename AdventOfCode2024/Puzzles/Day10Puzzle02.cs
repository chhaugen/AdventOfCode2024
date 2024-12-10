using AdventOfCode2024.Extentions;
using Microsoft.Extensions.Logging;
using static AdventOfCode2024.Puzzles.Day10Puzzle01;

namespace AdventOfCode2024.Puzzles;
public class Day10Puzzle02 : Puzzle
{
    public Day10Puzzle02(ILogger logger, DirectoryInfo puzzleResourceDirectory) : base(logger, puzzleResourceDirectory)
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

        var scores = startingNodes
            .Select(x => x
                .GetLeafs()
                .Where(y => y.Point.Value == 9)
                .ToList())
            .ToList();

        var totalScore = scores.Sum(x => x.Count);

        return totalScore.ToString();
    }
}
