using chhaugen.AdventOfCode2024.Common.Extentions;
using System.Text.Json;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day02Puzzle01 : Puzzle
{
    public Day02Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {
        var reports = ParseReports(input);

        var reportsSerialized = JsonSerializer.Serialize(reports, options: new() { WriteIndented = true });
        _progressOutput(reportsSerialized);

        var safeReports = reports.Where(x => (x.AllIncreasing() || x.AllDecreasing()) && x.IsAdjacentValueDifferenceConstraintUpheld(minDifference: 1, maxDifference: 3));

        return Task.FromResult(safeReports.Count().ToString());
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
