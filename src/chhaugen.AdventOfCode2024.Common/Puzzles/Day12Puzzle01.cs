using chhaugen.AdventOfCode2024.Common.Extentions;
using chhaugen.AdventOfCode2024.Common.Structures;
using System.Collections;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day12Puzzle01 : Puzzle
{
    public Day12Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
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

        var blobNumbers = blobs
            .Select(x => new { Edges = x.Sum(x => x.GetEdgeCount()), Area = x.Count })
            .ToList();
        var sum = blobNumbers.Select(x => x.Edges * x.Area).Sum();

        return Task.FromResult(sum.ToString());
    }

    public static IEnumerable<Point<T>> GetItselfAndNeigboursRecursive<T>(Point<T> point, List<Point<T>> popList)
    {
        yield return point;
        var value = point.Value;
        foreach(var direction in Enum.GetValues<CardinalDirection>())
        {
            var possibleNeigbour = point.GetPointInCardinalDirection(direction);
            if (!possibleNeigbour.ExistsOnMap)
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

    public class Map<T>
    {
        private readonly T[,] _map;
        private readonly int _xLength;
        private readonly int _yLength;

        public Map(T[,] map)
        {
            _xLength = map.GetLength(0);
            _yLength = map.GetLength(1);
            _map = map;
        }

        public Point<T> GetPoint(int x, int y)
            => new(_map, x, y);

        public HashSet<T> GetUniqueValues()
        {
            HashSet<T> plantTypes = [];
            for (int x = 0; x < _xLength; x++)
            {
                for (int y = 0; y < _yLength; y++)
                {
                    plantTypes.Add(_map[x, y]);
                }
            }
            return plantTypes;
        }

        public Map<T> GetMapOfValue(T type)
        {
            T[,] newMap = new T[_xLength, _yLength];

            for (int x = 0; x < _xLength; x++)
            {
                for (int y = 0; y < _yLength; y++)
                {
                    T mapValue = _map[x, y];
                    if (mapValue?.Equals(type) ?? false)
                        newMap[x, y] = mapValue;
                }
            }
            return new(newMap);
        }

        public IEnumerable<Map<T>> GetMapsOfDifferentTypes()
            => GetUniqueValues().Select(GetMapOfValue);

        public Map<T> AddMargin()
        {
            T[,] newMap = new T[_xLength +2, _yLength +2];

            for (int x = 0; x < _xLength; x++)
            {
                for (int y = 0; y < _yLength; y++)
                {
                    newMap[x +1, y +1] = _map[x, y];
                }
            }
            return new(newMap);
        }

        public void ForEachPoint(Action<Point<T>> action)
        {
            for (int x = 0; x < _xLength; x++)
            {
                for (int y = 0; y < _yLength; y++)
                {
                    action(new(_map, x, y));
                }
            }
        }

        public IEnumerable<Point<T>> GetSpiralInwardsPoints()
        {
            int xMin = 0;
            int yMin = 0;
            int xMax = _xLength - 1;
            int yMax = _yLength - 1;
            Point<T>? currentPoint = null;
            CardinalDirection direction = CardinalDirection.South;
            for (int i = 0; i < _xLength * _yLength; i++)
            {
                if (currentPoint is null)
                {
                    currentPoint = new(_map, 0, 0);
                    yield return currentPoint;
                    continue;
                }
                var nextPoint = currentPoint.GetPointInCardinalDirection(direction);
                if (nextPoint.X == xMin && nextPoint.Y == yMin)
                {
                    xMin++;
                    yMin++;
                    xMax--;
                    yMax--;
                }
                if (!(xMin <= nextPoint.X && nextPoint.X <= xMax && yMin <= nextPoint.Y && nextPoint.Y <= yMax))
                {
                    direction = direction.TurnAntiClockwise();
                    nextPoint = currentPoint.GetPointInCardinalDirection(direction);
                }
                currentPoint = nextPoint;
                yield return nextPoint;
            }
        }

        public IEnumerable<Point<T>> AsPointEnumerable()
        {
            for (int x = 0; x < _xLength; x++)
            {
                for (int y = 0; y < _yLength; y++)
                {
                    yield return new(_map, x, y);
                }
            }
        }

        public IEnumerable<T> AsEnumerable()
        {
            for (int x = 0; x < _xLength; x++)
            {
                for (int y = 0; y < _yLength; y++)
                {
                    yield return  _map[x, y];
                }
            }
        }

    }

    public static Map<char> ParseInput(string input)
    {
        var data = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        int yLength = data.Length;
        int xLength = data[0].Length;
        char[,] map = new char[xLength, yLength];
        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLength; x++)
            {
                map[x, y] = data[y][x];
            }
        }
        return new(map);
    }

    public static void AddEdges(Map<char> map)
    {
        map.ForEachPoint(p =>
        {
            if (p.Value == default)
            {
                foreach (var direction in Enum.GetValues<CardinalDirection>())
                {
                    var directionPoint = p.GetPointInCardinalDirection(direction);
                    if (directionPoint.ExistsOnMap)
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
