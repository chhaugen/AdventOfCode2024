using System.Text.Json;
using static chhaugen.AdventOfCode2024.Common.Puzzles.Day11Puzzle01;

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
        Dictionary<StoneTimes, long> stoneCache = [];
        Dictionary<StoneTimes, int>? cacheHitsCount = null;
#if DEBUG
        cacheHitsCount = [];
#endif

        var counts = initialStones.Select(x => GetBlinkStonesCount(x, times, stoneCache, cacheHitsCount)).ToList();

        var count = counts.Sum().ToString();

        _progressOutput($"CacheSize: {stoneCache.Count:D4}, Count: {count}");

#if DEBUG
        SaveCacheResults(stoneCache, cacheHitsCount);
#endif
        return Task.FromResult(count.ToString());
    }

    internal void SaveCacheResults(Dictionary<StoneTimes, long> stoneCache, Dictionary<StoneTimes, int> cacheHitsCount)
    {
        using FileStream outFile = File.Create($"{nameof(Day11Puzzle02)}Cache.json");
        var jsonList = stoneCache
            .Join(cacheHitsCount, x => x.Key, x => x.Key, (x, y) => new { x.Key.Times, x.Key.Stone, Count = x.Value, Hit = y.Value })
            .OrderByDescending(x => x.Count)
            .ThenByDescending(x => x.Hit)
            .ThenByDescending(x => x.Times)
            .ToList();
        JsonSerializer.Serialize(outFile, jsonList, options: new() { WriteIndented = true });
    }

    public long GetBlinkStonesCount(long stone, int times, Dictionary<StoneTimes, long> cache, Dictionary<StoneTimes, int>? cacheHitsResults = null)
    {
        if (times == 0)
            return 1;

        long count = 0;
        if (stone == 0)
        {
            long newStoneOne = 1;
            count = GetBlinkStonesCountCached(newStoneOne, times - 1, cache, cacheHitsResults);

        }

        if (count == 0)
        {
            var stoneLength = CountDigits(stone);
            if (stoneLength % 2 == 0)
            {
                var halfLength = stoneLength / 2;
                var firstStone = stone / (long)Math.Pow(10, halfLength);
                count += GetBlinkStonesCountCached(firstStone, times - 1, cache, cacheHitsResults);

                var secondStone = stone % (long)Math.Pow(10, halfLength);
                count += GetBlinkStonesCountCached(secondStone, times - 1, cache, cacheHitsResults);
            }
        }

        if (count == 0)
        {
            long newStone = stone * 2024;
            count = GetBlinkStonesCountCached(newStone, times - 1, cache, cacheHitsResults);
        }

        if (times > 25)
        {
            _progressOutput($"[{times:D2}] CacheSize: {cache.Count:D4}, Count: {count}");
        }
        return count;
    }

    internal long GetBlinkStonesCountCached(long stone, int times, Dictionary<StoneTimes, long> cache, Dictionary<StoneTimes, int>? cacheHitsResults = null)
    {
        StoneTimes stoneTimes = new(stone, times);
        if (cache.TryGetValue(stoneTimes, out long stoneCachedCount))
        {
            RecordCacheHit(stoneTimes, cacheHitsResults);

            return stoneCachedCount;
        }
        else
        {
            long calculatedStoneValue = GetBlinkStonesCount(stone, times, cache, cacheHitsResults);
            cache[stoneTimes] = calculatedStoneValue;
            return calculatedStoneValue;
        }
    }

    private static void RecordCacheHit(StoneTimes stoneTimes, Dictionary<StoneTimes, int>? cacheHitsResults)
    {
        if (cacheHitsResults is not null)
        {
            if (cacheHitsResults.TryGetValue(stoneTimes, out int lookUpCount))
                cacheHitsResults[stoneTimes] = ++lookUpCount;
            else
                cacheHitsResults[stoneTimes] = 1;
        }
    }

    public static int CountDigits(long num)
    {
        if (num == 0)
            return 1;
        return (int)Math.Log10(num) + 1;
    }

    public readonly struct StoneTimes
    {
        public StoneTimes(long stone, int times)
        {
            Stone = stone;
            Times = times;
        }
        public long Stone { get; }
        public int Times { get; }
    }
}
