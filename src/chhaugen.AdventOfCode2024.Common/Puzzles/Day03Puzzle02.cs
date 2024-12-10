using System.Text.Json;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day03Puzzle02 : Puzzle
{
    public Day03Puzzle02(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {

        // Example String = 48
        //input = """xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))""";

        var enableString = GetEnabledDontString(input);
        _progressOutput(JsonSerializer.Serialize(enableString, options: new() { WriteIndented = true }));


        string numbers = "1234567890";

        string[] splitOnMulParenthesis = enableString
            .Split("mul(", options: StringSplitOptions.RemoveEmptyEntries)
            .Where(x => numbers.Contains(x.First()))
            .Select(x => new string(x.Take(8).ToArray()))
            .ToArray();
        //_progressOutput(JsonSerializer.Serialize(splitOnMulParenthesis, options: new() { WriteIndented = true }));

        var containsClosingPanrenthesis = splitOnMulParenthesis.Where(x => x.Contains(')')).ToList();

        var splitOnClosingParenthesis = containsClosingPanrenthesis.Select(x => x.Split(')')[0]).ToList();
        //_progressOutput(JsonSerializer.Serialize(splitOnClosingParenthesis, options: new() { WriteIndented = true }));

        var splitOnComma = splitOnClosingParenthesis.Select(x => x.Split(",")).ToList();
        //_progressOutput(JsonSerializer.Serialize(splitOnComma, options: new() { WriteIndented = true }));

        var keepOnlyMax3Chars = splitOnComma
            .Select(x => x
                .Select(y => new string(y
                    .Take(3)
                    .ToArray()))
                .ToArray())
            .ToList();
        //_progressOutput(JsonSerializer.Serialize(keepOnlyMax3Chars, options: new() { WriteIndented = true }));

        var filterOnNumbers = keepOnlyMax3Chars
            .Select(x => x
                .Select(y => new string(y
                    .Where(numbers.Contains)
                    .ToArray()))
                .Where(y => !string.IsNullOrWhiteSpace(y))
                .ToArray())
            .ToList();

        var parseInt = filterOnNumbers
            .Select(x => x
                .Select(y => int
                    .Parse(y))
                .ToArray())
            .ToList();
        //_progressOutput(JsonSerializer.Serialize(parseInt, options: new() { WriteIndented = true }));

        var multiplicationSum = parseInt.Where(x => x.Length == 2).Sum(x => x[0] * x[1]);

        return Task.FromResult(multiplicationSum.ToString());
    }

    public static string GetEnabledDontString(string input)
    {
        var returnString = string.Empty;
        var dontSplit = input.Split("don't()", count: 2);
        returnString += dontSplit[0];
        if (dontSplit[1].Contains("do()"))
        {
            returnString += GetEnabledDoString(dontSplit[1]);
        }
        return returnString;
    }

    public static string GetEnabledDoString(string input)
    {
        var returnString = string.Empty;
        var doSplit = input.Split("do()", count: 2);
        if (doSplit[1].Contains("don't()"))
        {
            returnString += GetEnabledDontString(doSplit[1]);
        }
        else
        {
            returnString += doSplit[1];
        }
        return returnString;

    }
}
