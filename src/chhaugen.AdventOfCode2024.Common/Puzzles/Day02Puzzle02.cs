using chhaugen.AdventOfCode2024.Common.Extentions;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day02Puzzle02 : Puzzle
{
    public Day02Puzzle02(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {
        var reports = Day02Puzzle01.ParseReports(input);

        var problemDamperReportSets = reports.
            Select(ProblemDampnerTestSetGenerator);

        var safeReports = problemDamperReportSets
            .Where(x => x
                .Any(y => (y.AllIncreasing() || y.AllDecreasing()) && y.IsAdjacentValueDifferenceConstraintUpheld(minDifference: 1, maxDifference: 3)))
            .ToList();

        return Task.FromResult(safeReports.Count.ToString());
    }

    public static List<List<int>> ProblemDampnerTestSetGenerator(int[] report)
    {
        List<List<int>> variants = [];
        variants.Add(report.ToList());

        for (int i = 0; i < report.Length; i++)
        {
            var variant = report.ToList();
            variant.RemoveAt(i);
            variants.Add(variant);
        }
        return variants;
    }
}
