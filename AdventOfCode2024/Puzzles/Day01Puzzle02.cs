using AdventOfCode2024.Extentions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AdventOfCode2024.Puzzles;
public class Day01Puzzle02 : Puzzle
{
    public Day01Puzzle02(ILogger logger, DirectoryInfo puzzleResourceDirectory) : base(logger, puzzleResourceDirectory)
    {
    }

    public override async Task<string> SolveAsync()
    {
        var inputText = _puzzleResourceDirectory.GetFiles("input.txt").First();
        var input = await inputText.ReadAllTextAsync();
        (List<int> leftColumn, List<int> rightColumn) = Day01Puzzle01.ParseInput(input);

        var groupings = rightColumn
            .Where(leftColumn.Contains)
            .GroupBy(x => x)
            .ToList();
        var groupingsSerialized = JsonSerializer.Serialize(groupings, options: new JsonSerializerOptions() { WriteIndented = true });
        _logger.LogDebug(groupingsSerialized);

        var similarityScore = groupings.Sum(x => x.Count() * x.Key);
        return similarityScore.ToString();
    }
}
