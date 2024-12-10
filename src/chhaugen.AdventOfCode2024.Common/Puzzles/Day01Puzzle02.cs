using System.Text.Json;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day01Puzzle02 : Puzzle
{
    public Day01Puzzle02(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {
        (List<int> leftColumn, List<int> rightColumn) = Day01Puzzle01.ParseInput(input);

        var groupings = rightColumn
            .Where(leftColumn.Contains)
            .Select(x => leftColumn.Count(y => y == x) * x)
            .ToList();
        var groupingsSerialized = JsonSerializer.Serialize(groupings, options: new JsonSerializerOptions() { WriteIndented = true });
        _progressOutput(groupingsSerialized);

        var similarityScore = groupings.Sum();
        return Task.FromResult(similarityScore.ToString());
    }
}
