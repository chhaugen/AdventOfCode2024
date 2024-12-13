using chhaugen.AdventOfCode2024.Common.Structures;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day12Puzzle01 : Puzzle
{
    public Day12Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {
        Map2D<char> map = Map2D.ParseInput(input, x => x);
        var flowers = map.GetUniqueValues();
        List<List<Point2D>> blobs = []; 
        foreach (var flower in flowers)
        {
            var flowerPoints = map
                .AsPointEnumerable()
                .Where(x => map[x] == flower)
                .ToList();

            while (flowerPoints.Count > 0)
            {
                var startPoint = flowerPoints[0];
                flowerPoints.RemoveAt(0);

                List<Point2D> blobPoints = GetItselfAndNeigboursRecursive(map, startPoint, flowerPoints).ToList();
                blobs.Add(blobPoints);
            }
        }

        var blobNumbers = blobs
            .Select(x => new { Edges = x.Sum(x => map.GetEdgeCount(x)), Area = x.Count })
            .ToList();
        var sum = blobNumbers.Select(x => x.Edges * x.Area).Sum();

        return Task.FromResult(sum.ToString());
    }

    public static IEnumerable<Point2D> GetItselfAndNeigboursRecursive<T>(Map2D<T> map, Point2D point, List<Point2D> popList)
    {
        yield return point;
        var value = map[point];
        foreach(var direction in Enum.GetValues<CardinalDirection>())
        {
            var possibleNeigbour = point.GetPointInDirection(direction);
            if (!map.HasPoint(possibleNeigbour))
                continue;
            Point2D defaultPoint = new(-1, -1);
            var popListResult = popList.FirstOrDefault(x => x.Equals(possibleNeigbour), defaultPoint);
            if (popListResult != defaultPoint)
            {
                popList.Remove(popListResult);
                foreach (var subNeighboar in GetItselfAndNeigboursRecursive(map, possibleNeigbour, popList))
                    yield return subNeighboar;
            }
        }
    }

    public static void AddEdges(Map2D<char> map)
    {
        map.ForEachPoint(p =>
        {
            if (map[p] == default)
            {
                foreach (var direction in Enum.GetValues<CardinalDirection>())
                {
                    var directionPoint = p.GetPointInDirection(direction);
                    if (map.HasPoint(directionPoint))
                    {
                        if (map[directionPoint] != default)
                        {
                            var directionInt = (int)direction;
                        }
                    }
                }
            }
        });
    }

}
