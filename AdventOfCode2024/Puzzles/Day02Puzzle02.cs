using AdventOfCode2024.Extentions;
using Microsoft.Extensions.Logging;

namespace AdventOfCode2024.Puzzles;
public class Day02Puzzle02 : Puzzle
{
    public Day02Puzzle02(ILogger logger, DirectoryInfo puzzleResourceDirectory) : base(logger, puzzleResourceDirectory)
    {
    }

    public override async Task<string> SolveAsync()
    {
        var inputText = _puzzleResourceDirectory.GetFiles("input.txt").First();
        var input = await inputText.ReadAllTextAsync();
        var reports = Day02Puzzle01.ParseReports(input);

        var problemDamperReportSets = reports.
            Select(ProblemDampnerTestSetGenerator);

        var safeReports = problemDamperReportSets
            .Where(x => x
                .Any(y => (y.AllIncreasing() || y.AllDecreasing()) && y.IsAdjacentValueDifferenceConstraintUpheld(minDifference: 1, maxDifference: 3)))
            .ToList();

        return safeReports.Count.ToString();
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
