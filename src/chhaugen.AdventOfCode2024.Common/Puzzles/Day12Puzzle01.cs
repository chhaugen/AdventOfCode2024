using chhaugen.AdventOfCode2024.Common.Structures;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day12Puzzle01 : Puzzle
{
    public Day12Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {
        Map2D<char> map = Map2D<char>.ParseInput(input, x => x);
        var flowers = map.GetUniqueValues();
        List<List<Point2D<char>>> blobs = []; 
        foreach (var flower in flowers)
        {
            var flowerPoints = map
                .AsPointEnumerable()
                .Where(x => x.Value == flower)
                .ToList();

            while (flowerPoints.Count > 0)
            {
                var startPoint = flowerPoints[0];
                flowerPoints.RemoveAt(0);

                List<Point2D<char>> blobPoints = GetItselfAndNeigboursRecursive(startPoint, flowerPoints).ToList();
                blobs.Add(blobPoints);
            }
        }

        var blobNumbers = blobs
            .Select(x => new { Edges = x.Sum(x => x.GetEdgeCount()), Area = x.Count })
            .ToList();
        var sum = blobNumbers.Select(x => x.Edges * x.Area).Sum();

        return Task.FromResult(sum.ToString());
    }

    public static IEnumerable<Point2D<T>> GetItselfAndNeigboursRecursive<T>(Point2D<T> point, List<Point2D<T>> popList)
    {
        yield return point;
        var value = point.Value;
        foreach(var direction in Enum.GetValues<CardinalDirection>())
        {
            var possibleNeigbour = point.GetPointInDirection(direction);
            if (!possibleNeigbour.IsOnMap)
                continue;
            var popListResult = popList.FirstOrDefault(x => x.Equals(possibleNeigbour));
            if (popListResult != null)
            {
                popList.Remove(popListResult);
                foreach (var subNeighboar in GetItselfAndNeigboursRecursive(possibleNeigbour, popList))
                    yield return subNeighboar;
            }
        }
    }

    public static void AddEdges(Map2D<char> map)
    {
        map.ForEachPoint(p =>
        {
            if (p.Value == default)
            {
                foreach (var direction in Enum.GetValues<CardinalDirection>())
                {
                    var directionPoint = p.GetPointInDirection(direction);
                    if (directionPoint.IsOnMap)
                    {
                        if (directionPoint.Value != default)
                        {
                            var directionInt = (int)direction;
                        }
                    }
                }
            }
        });
    }

}
