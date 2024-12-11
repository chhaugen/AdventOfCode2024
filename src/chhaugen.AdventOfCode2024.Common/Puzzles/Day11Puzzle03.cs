
using System.Numerics;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day11Puzzle03 : Puzzle
{
    public Day11Puzzle03(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }
    public override Task<string> SolveAsync(string input)
        => this.SolveAsync(input, times: 150);

    public Task<string> SolveAsync(string input, int times)
    {
        var initialStones = Day11Puzzle01.ParseInput(input);

        Dictionary<StoneTimes, BigInteger> stoneCache = [];
        var counts = initialStones.Select(x => GetBlinkStonesCount(x, times, stoneCache)).ToList();

        BigInteger count = 0;
        foreach (BigInteger stoneCount in counts)
        {
            count += stoneCount;
        }

        return Task.FromResult(count.ToString());
    }

    public BigInteger GetBlinkStonesCount(BigInteger stone, int times, Dictionary<StoneTimes, BigInteger> cache)
    {
        if (times == 0)
            return 1;

        if (stone == 0)
        {
            BigInteger newStoneOne = 1;
            return GetBlinkStonesCountCached(newStoneOne, times - 1, cache);

        }

        var stoneLength = CountDigits(stone);
        if (stoneLength % 2 == 0)
        {
            var halfLength = stoneLength / 2;
            var firstStone = stone / (BigInteger)Math.Pow(10, halfLength);
            var count = GetBlinkStonesCountCached(firstStone, times - 1, cache);

            var secondStone = stone % (BigInteger)Math.Pow(10, halfLength);
            count += GetBlinkStonesCountCached(secondStone, times - 1, cache);
            return count;
        }

        BigInteger newStone = stone * 2024;
        return GetBlinkStonesCountCached(newStone, times - 1, cache);
    }

    internal BigInteger GetBlinkStonesCountCached(BigInteger stone, int times, Dictionary<StoneTimes, BigInteger> cache)
    {
        StoneTimes stoneTimes = new(stone, times);
        if (cache.TryGetValue(stoneTimes, out BigInteger stoneCachedCount))
        {
            return stoneCachedCount;
        }
        else
        {
            BigInteger calculatedStoneValue = GetBlinkStonesCount(stone, times, cache);
            cache[stoneTimes] = calculatedStoneValue;
            return calculatedStoneValue;
        }
    }
    public static int CountDigits(BigInteger num)
    {
        if (num == 0)
            return 1;
        return (int)BigInteger.Log10(num) + 1;
    }

    public readonly struct StoneTimes
    {
        public StoneTimes(BigInteger stone, int times)
        {
            Stone = stone;
            Times = times;
        }
        public BigInteger Stone { get; }
        public int Times { get; }
    }
}
