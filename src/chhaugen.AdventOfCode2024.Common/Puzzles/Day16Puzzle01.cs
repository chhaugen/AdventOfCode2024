using chhaugen.AdventOfCode2024.Common.Extentions;
using chhaugen.AdventOfCode2024.Common.Structures;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day16Puzzle01 : Puzzle
{
    public Day16Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    // thx :) https://andrewlock.net/implementing-dijkstras-algorithm-for-finding-the-shortest-path-between-two-nodes-using-priorityqueue-in-dotnet-9/
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
        intersections.ForEach(i => i.Edges = GetEdges(i.Point, map, intersections).ToArray());


        var drawMap = map.Clone();
        intersections.ForEach(x => drawMap[x.Point] = 'X');
        _progressOutput(drawMap.PrintMap());

        var route = CalculateShortestPath([.. intersections], startIntersection, endIntersection)
            ?? throw new InvalidOperationException("Could not find route!");

        foreach (var routePart in route)
        {
            _progressOutput($"{routePart.Item1.Point}: {routePart.Item2}");
        }

        //var sum = route.Max(x => x.Item2 -2) + (1000 * (route.Count - 3));
        var sum = route.Max(x => x.Item2);

        var drawMap2 = map.Clone();
        route.ForEach(x => drawMap2[x.Item1.Point] = 'X');
        _progressOutput(drawMap2.PrintMap());

        return Task.FromResult(sum.ToString());
    }

    public static List<(Intersection, int)>? CalculateShortestPath(Intersection[] intersections, Intersection startNode, Intersection endNode)
    {
        // Initialize all the distances to max, and the "previous" intersection to null
        var distances = intersections
            .Select((intersection, i) => (intersection, details: (Previous: (Intersection?)null, Distance: int.MaxValue)))
            .ToDictionary(x => x.intersection, x => x.details);

        // priority queue for tracking shortest distance from the start node to each other node
        var queue = new PriorityQueue<Intersection, int>();

        // initialize the start node at a distance of 0
        distances[startNode] = (null, 0);

        // add the start node to the queue for processing
        queue.Enqueue(startNode, 0);

        // as long as we have a node to process, keep looping
        while (queue.Count > 0)
        {
            // remove the node with the current smallest distance from the start node
            var current = queue.Dequeue();

            // if this is the node we want, then we're finished
            // as we must already have the shortest route!
            if (current == endNode)
            {
                // build the route by tracking back through previous
                return BuildRoute(distances, endNode);
            }

            // add the node to the "visited" list
            var currentNodeDistance = distances[current].Distance;

            foreach (var edge in current.Edges)
            {
                // get the current shortest distance to the connected node
                int distance = distances[edge.ConnectedTo].Distance;
                // calculate the new cumulative distance to the edge
                int newDistance = currentNodeDistance + edge.Distance;

                // if the new distance is shorter, then it represents a new 
                // shortest-path to the connected edge
                if (newDistance < distance)
                {
                    // update the shortest distance to the connection
                    // and record the "current" node as the shortest
                    // route to get there 
                    distances[edge.ConnectedTo] = (current, newDistance);

                    // if the node is already in the queue, first remove it
                    queue.Remove(edge.ConnectedTo, out _, out _);
                    // now add the node with the new distance
                    queue.Enqueue(edge.ConnectedTo, newDistance);
                }
            }
        }

        // if we don't have anything left, then we've processed everything,
        // but didn't find the node we want
        return null;
    }

    public static List<(Intersection, int)> BuildRoute(Dictionary<Intersection, (Intersection? previous, int Distance)> distances, Intersection endNode)
    {
        var route = new List<(Intersection, int)>();
        Intersection? prev = endNode;

        // Keep examining the previous version until we
        // get back to the start node
        while (prev is not null)
        {
            var current = prev;
            (prev, var distance) = distances[current];
            route.Add((current, distance));
        }

        // reverse the route
        route.Reverse();
        return route;
    }

    public static List<Edge> GetEdges(Point2D intersection, Map2D<char> map, List<Intersection> allIntersections)
    {
        List<Edge> edgesFound = [];
        List<(CardinalDirection Direction, Point2D Point, int distance)> stillSearching = Enum.GetValues<CardinalDirection>()
            .Select(x => (x, intersection, 0))
            .ToList();

        while (stillSearching.Count > 0)
        {
            List<(CardinalDirection Direction, Point2D Point, int distance)> toBeSearched = [];
            foreach ((var searchDirection, var searchPoint, var searchDistance) in stillSearching)
            {
                Point2D pointInFront = searchPoint.GetPointInDirection(searchDirection);
                Intersection? connected = allIntersections.FirstOrDefault(x => x.Point == searchPoint);
                if (map[pointInFront] is '#')
                {
                    if (connected is not null && searchPoint != intersection)
                        edgesFound.Add(new(connected, searchDistance + 1000));
                }
                else if (connected is not null && searchPoint != intersection)
                {
                    edgesFound.Add(new(connected, searchDistance + 1000));
                    toBeSearched.Add((searchDirection, pointInFront, searchDistance + 1));
                }
                else
                {
                    toBeSearched.Add((searchDirection, pointInFront, searchDistance + 1));
                }
            }
            stillSearching = toBeSearched;
        }
        return edgesFound;
    }

    public static IEnumerable<Point2D> GetIntersectionPoints(Map2D<char> map)
    {
        foreach (var point in map.AsPointEnumerable())
        {
            if (!(1 <= point.X && point.X < map.Width - 1 && 1 <= point.Y && point.Y < map.Height - 1))
                continue;

            if (map[point] is '#')
                continue;

            foreach (var direction in Enum.GetValues<CardinalDirection>())
            {
                Point2D pointInfront = point.GetPointInDirection(direction);
                Point2D pointClockwise = point.GetPointInDirection(direction.TurnClockwise());
                if (map[pointInfront] is not '#' && map[pointClockwise] is not '#')
                    yield return point;
            }
        }
    }


    public record Intersection(Point2D Point)
    {
        public Edge[] Edges { get; set; } = [];
    }

    public record Edge(Intersection ConnectedTo, int Distance);

}
