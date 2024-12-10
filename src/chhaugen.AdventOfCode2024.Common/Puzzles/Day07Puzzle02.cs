using System.Diagnostics;
using static chhaugen.AdventOfCode2024.Common.Puzzles.Day07Puzzle01;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day07Puzzle02 : Puzzle
{
    public Day07Puzzle02(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override async Task<string> SolveAsync(string inputString)
    {

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
        _progressOutput($"Calculations took {sw.ElapsedMilliseconds}ms");

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
