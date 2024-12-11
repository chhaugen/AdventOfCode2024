using static chhaugen.AdventOfCode2024.Common.Puzzles.Day09Puzzle01;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day09Puzzle02 : Puzzle
{
    public Day09Puzzle02(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override async Task<string> SolveAsync(string inputString)
    {

        Disk disk = Disk.ParseInput(inputString);
        //_progressOutput(disk.PrintDiskLayout());
        disk.Defragment();
        //_progressOutput(disk.PrintDiskLayout());

        var checksum = disk.CalculateChecksum();

        return checksum.ToString();
    }
}
