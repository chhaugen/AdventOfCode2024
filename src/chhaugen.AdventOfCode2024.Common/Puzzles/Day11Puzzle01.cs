using System.Numerics;
using System.Text;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day11Puzzle01 : Puzzle
{
    public Day11Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {

        var stones = ParseInput(input);
        for (int i = 0; i < 25; i++)
        {
            stones = BlinkStones(stones);
        }

        return Task.FromResult(stones.Count().ToString());
    }

    public static IEnumerable<long> ParseInput(string input)
    {
        var stoneStrings = input.Split(' ');
        return stoneStrings.Select(long.Parse);
    }

    public static IEnumerable<long> BlinkStones(IEnumerable<long> stones)
    {
        foreach (var stone in stones)
        {
            if (stone == 0)
            {
                yield return 1;
                continue;
            }

            var stoneString = stone.ToString();
            if (stoneString.Length % 2 == 0)
            {
                var halfLength = stoneString.Length / 2;
                var firstStoneSpan = stoneString.AsSpan(..halfLength);
                var firstStone = long.Parse(firstStoneSpan);
                yield return firstStone;
                var secondStoneSpan = stoneString.AsSpan(halfLength..);
                var secondStone = long.Parse(secondStoneSpan);
                yield return secondStone;
                continue;
            }

            yield return stone * 2024;
        }
    }

    public static string PrintStones(IEnumerable<long> stones)
    {
        StringBuilder sb = new();
        foreach (var stone in stones)
        {
            sb.Append(stone);
            sb.Append(' ');
        }
        return sb.ToString();
    }
}
