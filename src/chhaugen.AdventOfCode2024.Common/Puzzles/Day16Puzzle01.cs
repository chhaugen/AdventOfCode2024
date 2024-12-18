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


        Intersection startIntersection = new(start);
        List<Intersection> intersections = TraverseIntersections(startIntersection, map);
        Intersection endIntersection = intersections.Single(x => x.Point == end);

        //var trueStartEdges = falseStartIntersection.Edges
        //    .Where(x => x.ConnectedTo.Point.Y == falseStartIntersection.Point.Y)
        //    .ToList();

        //var falseStartEdges = falseStartIntersection.Edges
        //    .Where(x => x.ConnectedTo.Point.X == falseStartIntersection.Point.X)
        //    .ToList();

        //Intersection trueStartIntersection = new(falseStartIntersection.Point);
        //intersections.Add(trueStartIntersection);
        //trueStartIntersection.Edges = trueStartEdges;
        //trueStartIntersection.Edges.Add(new Edge(falseStartIntersection, 0));
        //falseStartIntersection.Edges = falseStartEdges;

        var endNode = GetAllShortestPathsDijkstras(startIntersection, endIntersection, intersections);

        return Task.FromResult(endNode.Value.Distance.ToString());
    }

    public static Node<(Intersection Intersection, int Distance)> GetAllShortestPathsDijkstras(Intersection start, Intersection end, List<Intersection> allIntersections)
    {
        List<Intersection> closed = [];
        Dictionary<Intersection, DijkstaEntry> entries = allIntersections.ToDictionary(x => x, x => new DijkstaEntry());
        entries[start].Distance = 0;

        List<Intersection> currentIntersections = [start];
        while (currentIntersections.Count > 0)
        {
            List<Intersection> nextIntersections = [];
            foreach (var currentIntersection in currentIntersections)
            {
                var currentEntry = entries[currentIntersection];
                foreach (var edge in currentIntersection.Edges.Where(x => !closed.Contains(x.ConnectedTo)))
                {
                    var edgeEntry = entries[edge.ConnectedTo];
                    int newDistance = currentEntry.Distance + edge.Distance + 1000;

                    if (newDistance == edgeEntry.Distance)
                    {
                        if(!edgeEntry.Parents.Contains(currentIntersection))
                            edgeEntry.Parents.Add(currentIntersection);
                    }
                    else if (newDistance < edgeEntry.Distance)
                    {
                        edgeEntry.Distance = newDistance;
                        edgeEntry.Parents.Clear();
                        edgeEntry.Parents.Add(currentIntersection);
                    }

                    if (!nextIntersections.Contains(edge.ConnectedTo))
                        nextIntersections.Add(edge.ConnectedTo);
                }
                closed.Add(currentIntersection);
            }
            currentIntersections = nextIntersections;
        }

        var endEntry = entries[end];
        Node<(Intersection Intersection, int Distance)> root = new((end, endEntry.Distance));
        List<Node<(Intersection Intersection, int Distance)>> currentReverseIntersections = [root];
        while (currentReverseIntersections.Count > 0)
        {
            List<Node<(Intersection Intersection, int Distance)>> nextReverseIntersections = [];
            foreach (var currentReverseIntersectionNode in currentReverseIntersections)
            {
                var reverseIntersection = currentReverseIntersectionNode.Value.Intersection;
                //var Distance = currentReverseIntersectionNode.Value.Distance;
                var entry = entries[reverseIntersection];
                foreach (var entryParent in entry.Parents)
                {
                    var entryParentEntry = entries[entryParent];
                    Node<(Intersection Intersection, int Distance)> newChild = new((entryParent, entryParentEntry.Distance), currentReverseIntersectionNode);
                    currentReverseIntersectionNode.Children.Add(newChild);
                    nextReverseIntersections.Add(newChild);
                }
            }
            currentReverseIntersections = nextReverseIntersections;
        }

        return root;
    }

    public static List<Intersection> TraverseIntersections(Intersection start, Map2D<char> map)
    {
        var directions = Enum.GetValues<CardinalDirection>();
        List<Intersection> allIntersections = [start];
        List<Intersection> currentlyScanning = [start];
        while (currentlyScanning.Count > 0)
        {
            List<Intersection> toBeScanned = [];
            foreach (var currentIntersection in currentlyScanning)
            {
                var currentPoint = currentIntersection.Point;
                foreach (var direction in directions)
                {
                    Point2D currentPosition = currentIntersection.Point;
                    int distance = 0;
                    bool hitWall = false;
                    do
                    {

                        distance++;
                        Point2D pointInFront = currentPosition.GetPointInDirection(direction);
                        Point2D twiceInFront = pointInFront.GetPointInDirection(direction);
                        Point2D clockwisePoint = pointInFront.GetPointInDirection(direction.TurnClockwise());
                        Point2D antiClockwisePoint = pointInFront.GetPointInDirection(direction.TurnAntiClockwise());

                        hitWall = map[pointInFront] is '#';
                        if (hitWall)
                            break;

                        bool pointIsIntersection = map[twiceInFront] is '#' || map[clockwisePoint] is not '#' || map[antiClockwisePoint] is not '#';

                        if (pointIsIntersection)
                        {
                            Intersection? hitIntersection = allIntersections.FirstOrDefault(x => x.Point == pointInFront);
                            if (hitIntersection is null)
                            {
                                hitIntersection = new(pointInFront);
                                allIntersections.Add(hitIntersection);
                                toBeScanned.Add(hitIntersection);
                            }
                            int localDistance = distance;
                            if (currentIntersection == start && direction == CardinalDirection.East)
                            {
                                localDistance -= 1000;
                            }
                            if (hitIntersection == start && direction == CardinalDirection.West)
                            {
                                localDistance -= 1000;
                            }
                            if (!currentIntersection.Edges.Any(x => x.ConnectedTo == hitIntersection))
                                currentIntersection.Edges.Add(new(hitIntersection, localDistance));
                        }

                        currentPosition = pointInFront;
                    } while (!hitWall);
                }
            }
            currentlyScanning = toBeScanned;
        }
        return allIntersections;
    }

    public class DijkstaEntry
    {
        public int Distance { get; set; } = int.MaxValue;
        public List<Intersection> Parents { get; set; } = [];
        public override string ToString()
            => $"{nameof(Distance)}: {Distance}, [{string.Join(',', Parents.Select(x => x.Point))}]";
    }

    public class Intersection : IEquatable<Intersection>
    {
        public Intersection(Point2D point)
        {
            Point = point;
        }

        public Point2D Point { get; }
        public List<Edge> Edges { get; set; } = [];

        public bool Equals(Intersection? other)
            => Point == other?.Point;

        public override string ToString()
            => $"{Point}, {nameof(Edges)}: {Edges.Count}";

        public override bool Equals(object? obj)
            => obj is Intersection other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(Point, Edges);
    }

    public readonly struct Edge
    {
        public Edge(Intersection connectedTo, int distance)
        {
            ConnectedTo = connectedTo;
            Distance = distance;
        }

        public Intersection ConnectedTo { get; }
        public int Distance { get; }
        public override string ToString()
            => $"{ConnectedTo.Point}, {nameof(Distance)}: {Distance}";
    }

}
