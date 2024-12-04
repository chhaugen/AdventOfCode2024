using AdventOfCode2024.Extentions;
using Microsoft.Extensions.Logging;

namespace AdventOfCode2024.Puzzles;

public class Day01Puzzle01 : Puzzle
{
    public Day01Puzzle01(ILogger logger, DirectoryInfo puzzleResourceDirectory) : base(logger, puzzleResourceDirectory)
    {
    }

    public override async Task<string> SolveAsync()
    {
        var inputText = _puzzleResourceDirectory.GetFiles("input.txt").First();
        var input = await inputText.ReadAllTextAsync();
        (List<int> firstColumn, List<int> secondColumn) = ParseInput(input);
        firstColumn.Sort();
        secondColumn.Sort();
        _logger.Log(LogLevel.Debug, "{column} has a count of {count}", nameof(firstColumn), firstColumn.Count);
        _logger.Log(LogLevel.Debug, "{column} has a count of {count}", nameof(secondColumn), secondColumn.Count);
        if (firstColumn.Count != secondColumn.Count)
            throw new InvalidOperationException("The two columns are not the same length!");

        List<int> differences = [];
        for (int i = 0; i < firstColumn.Count; i++)
        {
            int difference = Math.Abs(firstColumn[i] - secondColumn[i]);
            differences.Add(difference);
        }

        int differenceSum = differences.Sum();
        return differenceSum.ToString();
    }

    public static (List<int>, List<int>) ParseInput(string input)
    {
        List<int> first = [];
        List<int> second = [];
        string[] lines = input.Split('\n'); 
        foreach (var line in lines)
        {
            string[] valuesStrings = line.Split(' ', options: StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (valuesStrings.Length == 0)
                continue;

            if (!int.TryParse(valuesStrings[0], out int firstValue))
                throw new InvalidOperationException($"First value {valuesStrings[0]} could not be parsed.");

            if (!int.TryParse(valuesStrings[1], out int secondValue))
                throw new InvalidOperationException($"Second value {valuesStrings[1]} could not be parsed.");

            first.Add(firstValue);
            second.Add(secondValue);
        }
        return (first, second);
    }
}
