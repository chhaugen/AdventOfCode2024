using System.Text;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day09Puzzle01 : Puzzle
{
    public Day09Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override async Task<string> SolveAsync(string inputString)
    {
        Disk disk = Disk.ParseInput(inputString);
        _progressOutput(disk.PrintDiskLayout());
        disk.MoveDataToLeft();
        _progressOutput(disk.PrintDiskLayout());

        var checksum = disk.CalculateChecksum();

        return checksum.ToString();
    }


    public class Disk
    {
        private readonly int?[] _map;

        private Disk(int?[] map)
        {
            _map = map;
        }

        public static Disk ParseInput(string input)
        {
            var inputCode = input
                .Where(char.IsNumber)
                .Select(x => int.Parse(new([x])))
                .ToList();

            var sum = inputCode.Sum();

            int?[] map = new int?[sum];
            int mapIndex = 0;
            for (int i = 0; i < inputCode.Count; i += 2)
            {
                int id = i / 2;
                int takenDiskSpace = inputCode[i];
                int freeDiskSpace = i + 1 < inputCode.Count ? inputCode[i + 1] : 0;

                for (int t = 0; t < takenDiskSpace; t++)
                {
                    map[mapIndex++] = id;
                }

                mapIndex += freeDiskSpace;
            }

            return new Disk(map);
        }

        public string PrintDiskLayout()
        {
            StringBuilder sb = new();
            foreach (var block in _map)
            {
                if (block.HasValue)
                {
                    sb.Append('[');
                    sb.Append(block.Value);
                    sb.Append(']');
                }
                else
                    sb.Append('.');
            }
            return sb.ToString();
        }

        public void MoveDataToLeft()
        {
            for (int right = _map.Length - 1; right >= 0; right--)
            {
                int? rightValue = _map[right];
                if (!rightValue.HasValue)
                    continue;

                for (int left = 0; left < _map.Length; left++)
                {
                    if (right == left)
                        return;

                    int? leftValue = _map[left];
                    if (leftValue.HasValue)
                        continue;
                    _map[left] = _map[right];
                    _map[right] = null;
                    break;
                }
            }
        }

        public long CalculateChecksum()
        {
            long sum = 0;
            for (int i = 0; i < _map.Length; i++)
            {
                int? value = _map[i];
                if (value.HasValue)
                    sum += i * value.Value;
            }
            return sum;
        }

        public void Defragment()
        {
            for (int rightIndex = _map.Length - 1; rightIndex >= 0; rightIndex--)
            {
                // Skip null values on the right
                int? rightId = _map[rightIndex];
                if (!rightId.HasValue)
                    continue;

                // End of the right range
                int rightEndIndex = rightIndex;
                int? rightStartIndex = null;

                // find end of right range
                while (!rightStartIndex.HasValue)
                {
                    if (rightIndex - 1 < 0 || rightId != _map[rightIndex - 1])
                    {
                        rightStartIndex = rightIndex;
                    }
                    rightIndex--;
                }
                rightIndex++;
                // Use a span to slice the right range
                Range rightRange = new(rightStartIndex.Value, rightEndIndex + 1);
                Span<int?> rightSpan = _map.AsSpan(rightRange);


                for (int leftIndex = 0; leftIndex < _map.Length; leftIndex++)
                {
                    // The right and left shouldn't clash
                    if (rightStartIndex <= leftIndex)
                        break;

                    // Skip filled data on the left
                    int? leftId = _map[leftIndex];
                    if (leftId.HasValue)
                        continue;

                    // Start of the left range
                    int leftStartIndex = leftIndex;
                    int? leftEndIndex = null;

                    // Find the end of the continous free space
                    while (!leftEndIndex.HasValue)
                    {

                        if (leftIndex + 1 > _map.Length - 1 || _map[leftIndex + 1].HasValue)
                        {
                            leftEndIndex = leftIndex;
                        }
                        leftIndex++;
                    }
                    leftIndex--;

                    // Represent as a span
                    Range leftRange = new(leftStartIndex, leftEndIndex.Value + 1);
                    Span<int?> leftSpan = _map.AsSpan(leftRange);

                    // Only move when there is enough space on the left
                    if (leftSpan.Length < rightSpan.Length)
                        continue;

                    // Move data from right span to left span
                    for (int copyIndex = 0; copyIndex < rightSpan.Length; copyIndex++)
                    {
                        leftSpan[copyIndex] = rightSpan[copyIndex];
                        rightSpan[copyIndex] = null;
                    }
                    break;
                }
            }
        }
    }
}
