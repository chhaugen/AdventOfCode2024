using chhaugen.AdventOfCode2024.Common.Extentions;
using chhaugen.AdventOfCode2024.Common.Structures;
using static chhaugen.AdventOfCode2024.Common.Puzzles.Day16Puzzle01;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day16Puzzle02 : Puzzle
{
    public Day16Puzzle02(Action<string>? progressOutput) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {
        Map2D<char> map = Map2D.ParseInput(input);

        Point2D start = map
            .AsPointEnumerable()
            .First(x => map[x] is 'S');

        Point2D end = map
            .AsPointEnumerable()
            .First(x => map[x] is 'E');


        List<Intersection> intersections = TraverseIntersections(new(start), map);
        Intersection startIntersection = intersections.First(x => x.Point == start);
        Intersection endIntersection = intersections.First(x => x.Point == end);

        var endNode = GetAllShortestPathsDijkstras(startIntersection, endIntersection, intersections);

        var leafs = endNode
            .GetLeafs()
            .ToList();

        var paths = leafs
            .Select(x => x
                .GetAncestors()
                .Select(x => x.Value)
                .ToList())
            .ToList();

        var pathPoints = paths.Select(x => GetPointsOfPath(x.Select(y => y.Intersection).ToList()).ToList()).ToList();

        var drawMap = map.Clone();
        for (int p = 0; p < pathPoints.Count; p++)
        {
            var path = pathPoints[p];
            for (int i = 0; i < path.Count; i++)
            {
                drawMap[path[i]] = (char)('@' + p);
            }
            _progressOutput(drawMap.PrintMap());
        }

        _progressOutput(paths.Count.ToString());

        HashSet<Point2D> allPoints = [];

        foreach (var path in pathPoints)
        {
            foreach (var point in path)
            {
                allPoints.Add(point);
            }
        }

        return Task.FromResult(allPoints.Count.ToString());
    }

    public static IEnumerable<Point2D> GetPointsOfPath(List<Intersection> intersections)
    {
        Intersection previousIntersection = intersections[0];
        foreach (var intersection in intersections.Skip(1))
        {
            if (previousIntersection.Point.X == intersection.Point.X)
            {
                if (previousIntersection.Point.Y < intersection.Point.Y)
                {
                    for (long y = previousIntersection.Point.Y; y <= intersection.Point.Y; y++)
                    {
                        yield return new(previousIntersection.Point.X, y);
                    }
                }
                else
                {
                    for (long y = previousIntersection.Point.Y; y >= intersection.Point.Y; y--)
                    {
                        yield return new(previousIntersection.Point.X, y);
                    }
                }
            }
            else
            {
                if (previousIntersection.Point.X < intersection.Point.X)
                {
                    for (long x = previousIntersection.Point.X; x <= intersection.Point.X; x++)
                    {
                        yield return new(x, previousIntersection.Point.Y);
                    }
                }
                else
                {
                    for (long x = previousIntersection.Point.X; x >= intersection.Point.X; x--)
                    {
                        yield return new(x, previousIntersection.Point.Y);
                    }
                }
            }
            previousIntersection = intersection;
        }
    }
}