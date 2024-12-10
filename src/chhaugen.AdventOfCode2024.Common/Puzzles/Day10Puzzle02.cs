using static chhaugen.AdventOfCode2024.Common.Puzzles.Day10Puzzle01;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day10Puzzle02 : Puzzle
{
    public Day10Puzzle02(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override async Task<string> SolveAsync(string inputString)
    {
        TrialMap map = TrialMap.ParseInput(inputString);

        var startingNodes = map.GetPossibleStaringNodes();

        var scores = startingNodes
            .Select(x => x
                .GetLeafs()
                .Where(y => y.Point.Value == 9)
                .ToList())
            .ToList();

        var totalScore = scores.Sum(x => x.Count);

        return totalScore.ToString();
    }
}
