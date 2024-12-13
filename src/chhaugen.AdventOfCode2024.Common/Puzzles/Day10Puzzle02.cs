using chhaugen.AdventOfCode2024.Common.Structures;
using System.Globalization;
using static chhaugen.AdventOfCode2024.Common.Puzzles.Day10Puzzle01;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day10Puzzle02 : Puzzle
{
    public Day10Puzzle02(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string inputString)
    {
        Map2D<ushort> map = Map2D<ushort>.ParseInput(inputString, x => (ushort)CharUnicodeInfo.GetDigitValue(x));

        var startingNodes = GetPossibleStaringNodes(map);

        var scores = startingNodes
            .Select(x => x
                .GetLeafs()
                .Where(y => map[y.Point] == 9)
                .ToList())
            .ToList();

        var totalScore = scores.Sum(x => x.Count);

        return Task.FromResult(totalScore.ToString());
    }
}
