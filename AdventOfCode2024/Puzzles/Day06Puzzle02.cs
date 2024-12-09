using System.Diagnostics;
using AdventOfCode2024.Extentions;
using Microsoft.Extensions.Logging;
using static AdventOfCode2024.Puzzles.Day06Puzzle01;

namespace AdventOfCode2024.Puzzles;
public class Day06Puzzle02 : Puzzle
{
    public Day06Puzzle02(ILogger logger, DirectoryInfo puzzleResourceDirectory) : base(logger, puzzleResourceDirectory)
    {
    }

    public override async Task<string> SolveAsync()
    {
        var inputFile = _puzzleResourceDirectory.GetFiles("input.txt").First();
        var inputString = await inputFile.ReadAllTextAsync();


        // Example
        //var exampleFile = _puzzleResourceDirectory.GetFiles("example.txt").First();
        //inputString = await exampleFile.ReadAllTextAsync();

        var map = ParseInput(inputString);

        var initialGuard = FindGuard(map);
        List<Point> coveredPoints = GetPointsOfPlayedMap(map, initialGuard)
            .Where(x => x != initialGuard.Point)
            .ToList();

        int countLooped = 0;

        Stopwatch sw = Stopwatch.StartNew();

        IEnumerable<MapFeature[,]> mapVariations = NewObstructionInPathVariations(map, coveredPoints);
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
        //    //Console.WriteLine(PrintMap(newObstructionMap));
        //    //Console.WriteLine($"Current count: {countLooped}");
        //    if (GuardWillLoop(newObstructionMap))
        //        countLooped++;
        //}
        sw.Stop();

        _logger.LogDebug("Calculations took {ElapsedMilliseconds}ms", sw.ElapsedMilliseconds);

        return countLooped.ToString();
    }

    private static bool GuardWillLoop(MapFeature[,] newObstructionMap)
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
            //Console.WriteLine(PrintMap(newObstructionMap));
        }
        return guardLooped;
    }

    private static List<Point> GetPointsOfPlayedMap(MapFeature[,] map, Guard initialGuard)
    {
        MapFeature[,] mapCopy = (MapFeature[,])map.Clone();
        PlayGuard(new(mapCopy, initialGuard.Point));

        var coveredPoints = GetPoints(mapCopy, MapFeature.Covered);
        return coveredPoints;
    }

    public static IEnumerable<MapFeature[,]> NewObstructionInPathVariations(MapFeature[,] map, IEnumerable<Point> coveredPoints)
    {
        foreach(var coveredPoint in coveredPoints)
        {
            if (coveredPoint.GetFeature(map) == MapFeature.Covered)
                continue;
            MapFeature[,] mapCopy = (MapFeature[,])map.Clone();
            coveredPoint.SetFeature(mapCopy, MapFeature.Obstruction);
            yield return mapCopy;
        }
    }

    public static List<Point> GetPoints(MapFeature[,] map, MapFeature mapFeature)
    {
        int xCount = map.GetLength(0);
        int yCount = map.GetLength(1);
        List<Point> points = [];
        for (int y = 0; y < yCount; y++)
        {
            for (int x = 0; x < xCount; x++)
            {
                MapFeature feature = map[x, y];
                if (feature == mapFeature)
                    points.Add(new Point(x, y));
            }
        }
        return points;
    }
}
