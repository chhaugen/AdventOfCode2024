using AdventOfCode2024.Extentions;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text.Json;

namespace AdventOfCode2024.Puzzles;
public class Day03Puzzle01 : Puzzle
{
    public Day03Puzzle01(ILogger logger, DirectoryInfo puzzleResourceDirectory) : base(logger, puzzleResourceDirectory)
    {
    }

    public override async Task<string> SolveAsync()
    {
        var inputText = _puzzleResourceDirectory.GetFiles("input.txt").First();
        var input = await inputText.ReadAllTextAsync();

        // Test string = 161
        //input = """xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))""";

        string numbers = "1234567890";

        string[] splitOnMulParenthesis = input
            .Split("mul(", options: StringSplitOptions.RemoveEmptyEntries)
            .Where(x => numbers.Contains(x.First()))
            .Select(x => new string(x.Take(8).ToArray()))
            .ToArray();
        _logger.LogDebug(JsonSerializer.Serialize(splitOnMulParenthesis, options: new() { WriteIndented = true }));

        var containsClosingPanrenthesis = splitOnMulParenthesis.Where(x => x.Contains(')')).ToList();

        var splitOnClosingParenthesis = containsClosingPanrenthesis.Select(x => x.Split(')')[0]).ToList();
        _logger.LogDebug(JsonSerializer.Serialize(splitOnClosingParenthesis, options: new() { WriteIndented = true }));

        var splitOnComma = splitOnClosingParenthesis.Select(x => x.Split(",")).ToList();
        _logger.LogDebug(JsonSerializer.Serialize(splitOnComma, options: new() { WriteIndented = true }));

        var keepOnlyMax3Chars = splitOnComma
            .Select(x => x
                .Select(y => new string(y
                    .Take(3)
                    .ToArray()))
                .ToArray())
            .ToList();
        _logger.LogDebug(JsonSerializer.Serialize(keepOnlyMax3Chars, options: new() { WriteIndented = true }));

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
        _logger.LogDebug(JsonSerializer.Serialize(parseInt, options: new() { WriteIndented = true }));

        var multiplicationSum = parseInt.Where(x => x.Length == 2).Sum(x => x[0] * x[1]);

        return multiplicationSum.ToString();
    }
}
