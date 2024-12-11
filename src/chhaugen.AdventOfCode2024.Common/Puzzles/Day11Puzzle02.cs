﻿using static chhaugen.AdventOfCode2024.Common.Puzzles.Day11Puzzle01;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day11Puzzle02 : Puzzle
{
    public Day11Puzzle02(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {

        var initialStones = ParseInput(input);

        var times = 75;
        Dictionary<(int times, long stone), long> stoneCache = [];
        var counts = initialStones.Select(x => GetBlinkStonesCount(x, times, stoneCache)).ToList();
        return Task.FromResult(counts.Select(x => (long)x).Sum().ToString());
    }

    public long GetBlinkStonesCount(long stone, int times, Dictionary<(int times, long stone), long> cache)
    {
        if (times == 0)
            return 1;

        long count = 0;
        if (stone == 0)
        {
            long newStoneOne = 1;
            count = GetBlinkStonesCountCached(newStoneOne, times - 1, cache);

        }

        if (count == 0)
        {
            var stoneLength = CountDigits(stone);
            if (stoneLength % 2 == 0)
            {
                var halfLength = stoneLength / 2;
                var firstStone = stone / (long)Math.Pow(10, halfLength);
                count += GetBlinkStonesCountCached(firstStone, times - 1, cache);

                var secondStone = stone % (long)Math.Pow(10, halfLength);
                count += GetBlinkStonesCountCached(secondStone, times - 1, cache);
            }
        }

        if (count == 0)
        {
            long newStone = stone * 2024;
            count = GetBlinkStonesCountCached(newStone, times - 1, cache);
        }

        if (times > 25)
        {
            _progressOutput($"[{times:D2}] CacheSize: {cache.Count:D4} , Count: {count}");
        }
        return count;
    }

    internal long GetBlinkStonesCountCached(long stone, int times, Dictionary<(int times, long stone), long> cache)
    {
        if (cache.TryGetValue((times, stone), out long stoneCachedCount))
            return stoneCachedCount;
        else
        {
            long calculatedStoneValue = GetBlinkStonesCount(stone, times, cache);
            cache[(times, stone)] = calculatedStoneValue;
            return calculatedStoneValue;
        }
    }

    public static int CountDigits(long num)
    {
        if (num == 0)
            return 1;
        return (int)Math.Log10(num) + 1;
    }
}
