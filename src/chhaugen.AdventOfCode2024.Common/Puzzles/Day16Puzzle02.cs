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


        List<Intersection> intersections = GetIntersectionPoints(map)
            .Select(x => new Intersection(x))
            .ToList();
        Intersection? startIntersection = intersections.FirstOrDefault(x => x.Point == start);
        if (startIntersection is null)
        {
            startIntersection = new(start);
            intersections.Add(startIntersection);
        }
        Intersection? endIntersection = intersections.FirstOrDefault(x => x.Point == end);
        if (endIntersection is null)
        {
            endIntersection = new(end);
            intersections.Add(endIntersection);
        }
        intersections.ForEach(i => i.Edges = [.. GetEdges(i.Point, map, intersections)]);

        var route = CalculateShortestPath([.. intersections], startIntersection, endIntersection)
            ?? throw new InvalidOperationException("Could not find route!");

        var minRoute = route.Max(x => x.Item2);

        var node = RecursivlyBuildPathBackFromEnd(endIntersection, startIntersection, minRoute)
            ?? throw new InvalidOperationException("Could not construct node tree backwards");

        var leafs = node.GetLeafs().ToList();
        var routes = leafs
            .Select(x => x
                .GetAncestors()
                .Select(x => x.Value)
                .ToList())
            .ToList();

        var points = routes.Select(GetPointsOfPath).ToList();
        var distinctPoints = points.SelectMany(x => x).Distinct().ToList();

        foreach (var point in points)
        {
            var drawMap = map.Clone();
            foreach (var pointt in point)
            {
                drawMap[pointt] = 'X';
            }
            _progressOutput(drawMap.PrintMap());
        }


        return Task.FromResult(distinctPoints.Count.ToString());
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

    public static Node<Intersection>? RecursivlyBuildPathBackFromEnd(Intersection end, Intersection start, int maxDistance)
    {
        Node<Intersection> parent = new(end);
        foreach (var edge in end.Edges)
        {
            int newDistanceMax = maxDistance - edge.Distance;
            if (newDistanceMax < 0)
                continue;

            if (edge.ConnectedTo.Point == start.Point)
            {
                parent.Children.Add(new(start, parent));
            }
            else
            {
                var newChild = RecursivlyBuildPathBackFromEnd(edge.ConnectedTo, start, newDistanceMax);
                if (newChild != null)
                {
                    newChild.Parent = parent;
                    parent.Children.Add(newChild);
                }
            }
        }
        if (parent.IsLeaf)
            return null;
        return parent;
    }

}