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
        Map2D<MapObject> map = Map2D.ParseInput<MapObject>(input, x => x switch
        {
            '.' => new Floor(),
            '#' => new Wall(),
            'S' => new Start(),
            'E' => new End(),
            _ => throw new NotImplementedException(),
        });

        Point2D start = map
            .AsPointEnumerable()
            .First(x => map[x] is Start);

        Point2D end = map
            .AsPointEnumerable()
            .First(x => map[x] is End);

        Reindeer startReindeer = new(start, CardinalDirection.East);

        AddCostsToMap(startReindeer, map);
        _progressOutput(PrintMap(map));

        var eValue = (End)map[end];
        BridgeOddities(map);

        CardinalDirection[] endDirections = [CardinalDirection.East, CardinalDirection.South];
        List<int> bestPathPointsList = [];
        foreach (var endDirection in endDirections)
        {
            var node = RecursivlyScanFromEndToFindStartOnBudget(new(end, endDirection), eValue.Cost, start, map)
                ?? new Node<Reindeer>(default);
            var children = node.GetAllChildren().DistinctBy(x => x.Value.Position).ToList();
            //var allLeafs = node.GetLeafs().ToList();
            //var leafs = allLeafs
            //    .Where(x => map[x.Value.Position] is Start)
            //    .ToList();
            //var endLeafCount = leafs.Count;
            //var bestPathPoints = leafs.Select(x => x.GetAncestors().ToList()).SelectMany(x => x).DistinctBy(x => x.Value.Position).ToList();
            bestPathPointsList.Add(children.Count);
        }

        return Task.FromResult(bestPathPointsList.Max().ToString());
    }

    public void BridgeOddities(Map2D<MapObject> map)
    {
        CardinalDirection[] directions = Enum.GetValues<CardinalDirection>();
        foreach (var point in map.AsPointEnumerable().Where(p => map[p] is Floor))
        {
            List<Floor> neighbours = [];
            for (var i = 0; i < directions.Length; i++)
            {
                var possibleNeighbour = point.GetPointInDirection(directions[i]);
                if (map[possibleNeighbour] is Floor neighbour)
                    neighbours.Add(neighbour);
            }
            if (neighbours.Count == 2)
            {
                Floor? neighbourMin = neighbours.MinBy(x => x.Cost)!;
                Floor? neighbourMax = neighbours.MaxBy(x => x.Cost)!;
                int diffCost = neighbourMax.Cost - neighbourMin.Cost;
                if (diffCost == 2)
                {
                    int supposedValue = neighbourMin.Cost + 1;
                    if (((Floor)map[point]).Cost > supposedValue)
                        ((Floor)map[point]).Cost = supposedValue;
                }
                if (diffCost == 1002)
                {
                    int supposedValue = neighbourMin.Cost + 1001;
                    if (((Floor)map[point]).Cost > supposedValue)
                        ((Floor)map[point]).Cost = supposedValue;
                }
            }
            var drawMap = map.Clone();
            drawMap[point] = new Cross((Floor)drawMap[point]);
            //_progressOutput(PrintMap(drawMap));
        }
    }

    public Node<Reindeer>? RecursivlyScanFromEndToFindStartOnBudget(Reindeer reindeer, int budget, Point2D startPoint, Map2D<MapObject> map)
    {
        Node<Reindeer> parent = new(reindeer);
        Point2D pointInFront = reindeer.Position.GetPointInDirection(reindeer.Direction);
        int currentFloorCount = ((Floor)map[reindeer.Position]).Cost;
        if (pointInFront == startPoint)
            parent.Children.Add(new(new(pointInFront, reindeer.Direction), parent));
        else if (map[pointInFront] is Floor floorInFront)
        {
            var countDiff = currentFloorCount - floorInFront.Cost;
            if (countDiff == 1 || countDiff == 1001)
            {
                foreach ((int addCost, Reindeer reindeerToCheck) in GetReindeersToCheck(new(pointInFront, reindeer.Direction)))
                {
                    var newChild = RecursivlyScanFromEndToFindStartOnBudget(reindeerToCheck, budget - countDiff, startPoint, map);
                    if (newChild is not null)
                    {
                        newChild.Parent = parent;
                        parent.Children.Add(newChild);
                    }
                }
            }
            //else if (countDiff == 1001)
            //{
            //    var newChild = RecursivlyScanFromEndToFindStartOnBudget(new Reindeer(pointInFront, reindeer.Direction.TurnClockwise()), budget - countDiff, startPoint, map);
            //    newChild   ??= RecursivlyScanFromEndToFindStartOnBudget(new Reindeer(pointInFront, reindeer.Direction.TurnAntiClockwise()), budget - countDiff, startPoint, map);
            //    if (newChild is not null)
            //    {

            //        newChild.Parent = parent;
            //        parent.Children.Add(newChild);
            //    }
            //}
        }
        if (parent.IsLeaf)
            return null;
        else
        {
            var drawMap = map.Clone();
            foreach (var child in parent.GetAllChildren())
                drawMap[child.Value.Position] = new Cross((Floor)drawMap[child.Value.Position]);
            drawMap[pointInFront] = new Cross((Floor)drawMap[pointInFront]);
            _progressOutput(PrintMap(drawMap));
        }
        return parent;
    }

    public static IEnumerable<(int addCost, Reindeer reindeer)> GetReindeersToCheck(Reindeer reindeer)
    {
        CardinalDirection right = reindeer.Direction.TurnClockwise();
        Point2D pointRight = reindeer.Position;
        yield return (1001, new(pointRight, right));

        Point2D pointInFront = reindeer.Position;
        yield return (1, reindeer);

        CardinalDirection left = reindeer.Direction.TurnAntiClockwise();
        Point2D pointLeft = reindeer.Position;
        yield return (1001, new(pointLeft, left));
    }


}
