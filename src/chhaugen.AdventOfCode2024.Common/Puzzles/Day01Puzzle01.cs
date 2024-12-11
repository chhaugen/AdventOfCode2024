namespace chhaugen.AdventOfCode2024.Common.Puzzles;

public class Day01Puzzle01 : Puzzle
{
    public Day01Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {
        LocationLists locationLists = LocationLists.ParseInput(input);

        var distance = locationLists.FindTotalDistance();

        return Task.FromResult(distance.ToString());
    }

    public class LocationLists
    {
        private readonly int[] _leftList;
        private readonly int[] _rightList;

        private LocationLists(int[] leftList, int[] rightList)
        {
            _leftList = leftList;
            _rightList = rightList;
        }

        public int FindTotalDistance()
        {
            var leftListSorted = _leftList.Order();
            var rightListSorted = _rightList.Order();

            var distance = leftListSorted
                .Zip(rightListSorted, (left, right) => Math.Abs(left-right))
                .Sum();
            return distance;
        }

        public int GetSimilarityScore()
        {
            return _rightList
            .Where(_leftList.Contains)
            .Select(x => _leftList.Count(y => y == x) * x)
            .Sum();
        }

        public static LocationLists ParseInput(string input)
        {
            string[] lines = input.Split('\n', options: StringSplitOptions.RemoveEmptyEntries);
            int[] left = new int[lines.Length];
            int[] right = new int[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                var valuesStrings = lines[i]
                    .Split(' ')
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();

                if (!int.TryParse(valuesStrings[0], out int leftValue))
                    throw new InvalidOperationException($"First value {valuesStrings[0]} could not be parsed.");

                if (!int.TryParse(valuesStrings[1], out int rightValue))
                    throw new InvalidOperationException($"Second value {valuesStrings[1]} could not be parsed.");

                left[i] = leftValue;
                right[i] = rightValue;
            }
            return new(left, right);
        }
    }
}
