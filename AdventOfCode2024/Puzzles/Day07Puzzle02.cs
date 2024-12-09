using System.Diagnostics;
using AdventOfCode2024.Extentions;
using Microsoft.Extensions.Logging;
using static AdventOfCode2024.Puzzles.Day07Puzzle01;

namespace AdventOfCode2024.Puzzles;
public class Day07Puzzle02 : Puzzle
{
    public Day07Puzzle02(ILogger logger, DirectoryInfo puzzleResourceDirectory) : base(logger, puzzleResourceDirectory)
    {
    }

    public override async Task<string> SolveAsync()
    {
        var inputFile = _puzzleResourceDirectory.GetFiles("input.txt").First();
        var inputString = await inputFile.ReadAllTextAsync();


        // Example
        //var exampleFile = _puzzleResourceDirectory.GetFiles("example.txt").First();
        //inputString = await exampleFile.ReadAllTextAsync();

        var unfinishedEquations = ParseInput(inputString);

        BinaryOperator<long>[] operators =
        [
            new("+", (f, s) => f + s),
            new("*", (f, s) => f * s),
            //new("||", (f, s) => long.Parse(f.ToString() + s.ToString())),
            new("||", (f, s) => f * (long)Math.Pow(10, (int)Math.Log10(s) + 1) + s),
        ];

        long plausibleCalibrationsSum = 0;

        Stopwatch sw = Stopwatch.StartNew();

        await Parallel.ForEachAsync(unfinishedEquations, async (unfinishedEquation, ct) =>
        {
            await Task.Run(() =>
            {
                if (IsUnfinishedEquationPlausible(unfinishedEquation, operators))
                    Interlocked.Add(ref plausibleCalibrationsSum, unfinishedEquation.LeftSide);
            }, ct);
        });

        //foreach (var unfinishedEquation in unfinishedEquations)
        //{
        //    if (IsUnfinishedEquationPlausible(unfinishedEquation, operators))
        //        Interlocked.Add(ref plausibleCalibrationsSum, unfinishedEquation.LeftSide);
        //}

        sw.Stop();
        _logger.LogDebug("Calculations took {ElapsedMilliseconds}ms", sw.ElapsedMilliseconds);

        return plausibleCalibrationsSum.ToString();
    }

    private static bool IsUnfinishedEquationPlausible(UnfinishedEquation unfinishedEquation, BinaryOperator<long>[] operators)
    {
        long operatorLength = unfinishedEquation.RightSide.Length - 1;
        IEnumerable<BinaryOperator<long>[]> operatorSets = CartesianPower(operators, operatorLength);
        foreach (var operatorSet in operatorSets)
        {
            if (unfinishedEquation.IsEquationTrue(operatorSet))
            {
                return true;
            }
        }
        return false;
    }
}
