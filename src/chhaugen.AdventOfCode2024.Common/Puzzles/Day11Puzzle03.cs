
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Numerics;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day11Puzzle03 : Puzzle
{
    public Day11Puzzle03(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }
    public override Task<string> SolveAsync(string input)
        => SolveAsync(input, times: 1500);

    public Task<string> SolveAsync(string input, int times)
    {
        var initialStones = Day11Puzzle01.ParseInput(input).ToArray();

        BigInteger[] counts = new BigInteger[initialStones.Length];
        ConcurrentDictionary<ulong, BigInteger> stoneCache = [];
        Stopwatch sw = Stopwatch.StartNew();
        Parallel.For(0, initialStones.Length, stoneIndex =>
        {
            counts[stoneIndex] = GetBlinkStonesCountCached(initialStones[stoneIndex], times, stoneCache);
        });

        BigInteger count = 0;
        foreach (var countStone in counts)
        {
            count += countStone;
        }
        sw.Stop();
        _progressOutput($"Time used: {sw.ElapsedMilliseconds}ms");

        return Task.FromResult(count.ToString());
    }

    internal BigInteger GetBlinkStonesCount(BigInteger stone, int times, IDictionary<ulong, BigInteger> cache)
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

    public BigInteger GetBlinkStonesCountCached(BigInteger stone, int times, IDictionary<ulong, BigInteger> cache)
    {
        ulong hashcode = MakeLong(stone.GetHashCode(), times.GetHashCode());
        if (cache.TryGetValue(hashcode, out BigInteger stoneCachedCount))
        {
            return stoneCachedCount;
        }
        else
        {
            BigInteger calculatedStoneValue = GetBlinkStonesCount(stone, times, cache);
            cache[hashcode] = calculatedStoneValue;
            return calculatedStoneValue;
        }
    }
    public static int CountDigits(BigInteger num)
    {
        if (num == 0)
            return 1;
        return (int)BigInteger.Log10(num) + 1;
    }
    //public static long MakeLong(int left, int right)
    //{
    //    //implicit conversion of left to a long
    //    long res = left;

    //    //shift the bits creating an empty space on the right
    //    // ex: 0x0000CFFF becomes 0xCFFF0000
    //    res <<= 32;

    //    //combine the bits on the right with the previous value
    //    // ex: 0xCFFF0000 | 0x0000ABCD becomes 0xCFFFABCD
    //    res |= (uint)right; //uint first to prevent loss of signed bit

    //    //return the combined result
    //    return res;
    //}

    public static unsafe ulong MakeLong(int low, int high)
    {
        ulong retVal;
        int* ptr = (int*)&retVal;
        *ptr++ = low;
        *ptr = high;
        return retVal;
    }
}
