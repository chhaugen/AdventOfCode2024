using static chhaugen.AdventOfCode2024.Common.Puzzles.Day01Puzzle01;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day01Puzzle02 : Puzzle
{
    public Day01Puzzle02(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {
        LocationLists locationLists = LocationLists.ParseInput(input);

        return Task.FromResult(locationLists.GetSimilarityScore().ToString());
    }
}
