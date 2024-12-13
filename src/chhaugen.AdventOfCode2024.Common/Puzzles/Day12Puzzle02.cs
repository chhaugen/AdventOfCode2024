namespace chhaugen.AdventOfCode2024.Common.Puzzles;

using chhaugen.AdventOfCode2024.Common.Extentions;
using chhaugen.AdventOfCode2024.Common.Structures;
using static chhaugen.AdventOfCode2024.Common.Puzzles.Day12Puzzle01;
public class Day12Puzzle02 : Puzzle
{
    public Day12Puzzle02(Action<string>? progressOutput = null) : base(progressOutput)
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

        List<(List<Point2D> blob, List<List<Point2D>> edgeGroups)> wow = [];

        foreach (var blob in blobs)
        {
            var what = Enum
                .GetValues<CardinalDirection>()
                .Select(d => blob
                    .Where(p => map
                        .GetEdgeDirections(p)
                        .Contains(d))
                    .GroupBy(x => d.ToAxis() == Axis2D.X ? x.Y : x.X)
                    .ToList())
                .ToList();
            List<List<Point2D>> edgeGroupsOuter = [];
            foreach (var directionGroup in what)
            {
                var popList = directionGroup.SelectMany(x => x).ToList();
                List<List<Point2D>> edgeGroupsInner = [];
                while (popList.Count > 0)
                {
                    var startPoint = popList[0];
                    popList.RemoveAt(0);
                    var list = GetItselfAndNeigboursRecursive(map, startPoint, popList).ToList();
                    edgeGroupsInner.Add(list);
                }
                edgeGroupsOuter.AddRange(edgeGroupsInner);
            }
            wow.Add((blob, edgeGroupsOuter));
        }

        var temp1 = wow.Select(x => new { Area = x.blob.Count, Sides = x.edgeGroups.Count });
        var result = temp1.Select(x => x.Sides * x.Area).Sum();

        return Task.FromResult(result.ToString() ?? string.Empty);
    }

    //public static IEnumerable<Point2D<T>> RecursivlyFindPointsOnSameSide<T>(Point2D<T> point, int axis, List<List<Point2D<char>>> edgeGroupsInner)
    //{
    //    yield return point;
    //    var value = point.Value;
    //    List<CardinalDirection> possibleNeigbours = axis switch
    //    {
    //        0 => [CardinalDirection.North, CardinalDirection.South],
    //        1 => [CardinalDirection.West, CardinalDirection.East],
    //        _ => throw new NotImplementedException()
    //    };
    //    foreach (var direction in possibleNeigbours)
    //    {
    //        var possibleNeigbour = point.
    //        if (!possibleNeigbour.IsOnMap)
    //            return;

    //        var popListResult = popList.FirstOrDefault(x => x.Equals(possibleNeigbour));
    //        if (popListResult != null)
    //        {
    //            popList.Remove(popListResult);
    //            foreach (var subNeighboar in GetItselfAndNeigboursRecursive(possibleNeigbour, popList))
    //                yield return subNeighboar;
    //        }
    //    }
    //}
}
