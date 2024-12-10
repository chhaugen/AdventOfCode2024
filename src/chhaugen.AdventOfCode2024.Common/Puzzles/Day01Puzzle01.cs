namespace chhaugen.AdventOfCode2024.Common.Puzzles;

public class Day01Puzzle01 : Puzzle
{
    public Day01Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {
        (List<int> firstColumn, List<int> secondColumn) = ParseInput(input);
        firstColumn.Sort();
        secondColumn.Sort();
        _progressOutput($"{firstColumn} has a count of {firstColumn.Count}");
        _progressOutput($"{secondColumn} has a count of {secondColumn.Count}");
        if (firstColumn.Count != secondColumn.Count)
            throw new InvalidOperationException("The two columns are not the same length!");

        List<int> differences = [];
        for (int i = 0; i < firstColumn.Count; i++)
        {
            int difference = Math.Abs(firstColumn[i] - secondColumn[i]);
            differences.Add(difference);
        }

        int differenceSum = differences.Sum();
        return Task.FromResult(differenceSum.ToString());
    }

    public static (List<int>, List<int>) ParseInput(string input)
    {
        List<int> first = [];
        List<int> second = [];
        string[] lines = input.Split('\n', options: StringSplitOptions.RemoveEmptyEntries);
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
