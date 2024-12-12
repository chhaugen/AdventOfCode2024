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
        Map<char> map = ParseInput(input);

        var flowers = map.GetUniqueValues();
        List<List<Point<char>>> blobs = [];
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

                List<Point<char>> blobPoints = GetItselfAndNeigboursRecursive(startPoint, flowerPoints).ToList();
                blobs.Add(blobPoints);
            }
        }

        List<(List<Point<char>> blob, List<List<Point<char>>> edgeGroups)> wow = [];

        foreach (var blob in blobs)
        {
            var what = Enum
                .GetValues<CardinalDirection>()
                .Select(d => blob
                    .Where(p => p
                        .GetEdgeDirections()
                        .Contains(d))
                    .GroupBy(x => d.GetAxis() == Axis.X ? x.Y : x.X)
                    .ToList())
                .ToList();
            List<List<Point<char>>> edgeGroupsOuter = [];
            foreach (var directionGroup in what)
            {
                var popList = directionGroup.SelectMany(x => x).ToList();
                List<List<Point<char>>> edgeGroupsInner = [];
                while (popList.Count > 0)
                {
                    var startPoint = popList[0];
                    popList.RemoveAt(0);
                    var list = GetItselfAndNeigboursRecursive(startPoint, popList).ToList();
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

    //public static IEnumerable<Point<T>> RecursivlyFindPointsOnSameSide<T>(Point<T> point, int axis, List<List<Point<char>>> edgeGroupsInner)
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
    //        if (!possibleNeigbour.ExistsOnMap)
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
