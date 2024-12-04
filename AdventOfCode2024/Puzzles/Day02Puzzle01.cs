using AdventOfCode2024.Extentions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AdventOfCode2024.Puzzles;
public class Day02Puzzle01 : Puzzle
{
    public Day02Puzzle01(ILogger logger, DirectoryInfo puzzleResourceDirectory) : base(logger, puzzleResourceDirectory)
    {
    }

    public override async Task<string> SolveAsync()
    {
        var inputText = _puzzleResourceDirectory.GetFiles("input.txt").First();
        var input = await inputText.ReadAllTextAsync();
        var reports = ParseReports(input);

        var reportsSerialized = JsonSerializer.Serialize(reports, options: new JsonSerializerOptions() { WriteIndented = true });
        _logger.LogDebug(reportsSerialized);

        var safeReports = reports.Where(x => (x.AllIncreasing() || x.AllDecreasing()) && x.IsAdjacentValueDifferenceConstraintUpheld(minDifference: 1, maxDifference: 3));

        return safeReports.Count().ToString();
    }

    public static List<int[]> ParseReports(string input)
        => input
        .Split('\n', StringSplitOptions.RemoveEmptyEntries)
        .Select(x => x
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray())
        .ToList();
}
