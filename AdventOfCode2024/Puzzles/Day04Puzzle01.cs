using AdventOfCode2024.Extentions;
using Microsoft.Extensions.Logging;

namespace AdventOfCode2024.Puzzles;
public class Day04Puzzle01 : Puzzle
{
    public Day04Puzzle01(ILogger logger, DirectoryInfo puzzleResourceDirectory) : base(logger, puzzleResourceDirectory)
    {
    }

    public override async Task<string> SolveAsync()
    {
        var inputText = _puzzleResourceDirectory.GetFiles("input.txt").First();
        var input = await inputText.ReadAllTextAsync();

        throw new NotImplementedException();
    }
}
