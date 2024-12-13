using chhaugen.AdventOfCode2024.Common.Structures;
using System.Diagnostics;
using static chhaugen.AdventOfCode2024.Common.Puzzles.Day06Puzzle01;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day06Puzzle02 : Puzzle
{
    public Day06Puzzle02(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string inputString)
    {
        var map = Map2D.ParseInput(inputString);

        var initialGuard = FindGuard(map);
        List<Point2D> coveredPoints = GetPointsOfPlayedMap(map, initialGuard)
            .Where(x => x != initialGuard.Point)
            .ToList();

        int countLooped = 0;

        Stopwatch sw = Stopwatch.StartNew();

        IEnumerable<Map2D<char>> mapVariations = NewObstructionInPathVariations(map, coveredPoints);
        Parallel.ForEach(mapVariations, (newObstructionMap) =>
        {
            if (GuardWillLoop(newObstructionMap))
                Interlocked.Increment(ref countLooped);
        });
        //await Parallel.ForEachAsync(mapVariations, async (newObstructionMap, ct) =>
        //{
        //    await Task.Run(() =>
        //    {

        //        if (GuardWillLoop(newObstructionMap))
        //            Interlocked.Increment(ref countLooped);
        //    }, ct);
        //});
        //foreach (var newObstructionMap in mapVariations)
        //{
        //    //Console.SetCursorPosition(0, 0);
        //    //_progressOutput(PrintMap(newObstructionMap));
        //    //_progressOutput($"Current count: {countLooped}");
        //    if (GuardWillLoop(newObstructionMap))
        //        countLooped++;
        //}
        sw.Stop();

        _progressOutput($"Calculations took {sw.ElapsedMilliseconds}ms");

        return Task.FromResult(countLooped.ToString());
    }

    private static bool GuardWillLoop(Map2D<char> newObstructionMap)
    {
        Guard? guard = FindGuard(newObstructionMap);
        HashSet<Guard> previousGuardPositions = [];
        bool guardIsGone = false;
        bool guardLooped = false;
        while (!guardIsGone)
        {
            if (guard.HasValue)
            {
                if (previousGuardPositions.TryGetValue(guard.Value, out _))
                {
                    guardLooped = true;
                    break;
                }
                previousGuardPositions.Add(guard.Value);
                guard = guard.Value.PlayOneStep();
            }
            else
                guardIsGone = true;
            //Console.SetCursorPosition(0, 0);
            //_progressOutput(PrintMap(newObstructionMap));
        }
        return guardLooped;
    }

    private IEnumerable<Point2D> GetPointsOfPlayedMap(Map2D<char> map, Guard initialGuard)
    {
        Map2D<char> mapCopy = map.Clone();
        initialGuard = new(mapCopy, initialGuard.Point);
        PlayGuard(initialGuard, () => _progressOutput(mapCopy.PrintMap()));
        Thread.Sleep(1);

        var coveredPoints = mapCopy.AsPointEnumerable().Where(x => map[x] == 'X');
        return coveredPoints;
    }

    public static IEnumerable<Map2D<char>> NewObstructionInPathVariations(Map2D<char> map, IEnumerable<Point2D> coveredPoints)
    {
        foreach (var coveredPoint in coveredPoints)
        {
            if (map[coveredPoint] != 'X')
                continue;
            Map2D<char> mapCopy = map.Clone();
            mapCopy[coveredPoint] = '#';
            yield return mapCopy;
        }
    }
}
