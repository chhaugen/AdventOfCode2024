using AdventOfCode2024.Extentions;
using Microsoft.Extensions.Logging;
using static AdventOfCode2024.Puzzles.Day09Puzzle01;

namespace AdventOfCode2024.Puzzles;
public class Day09Puzzle02 : Puzzle
{
    public Day09Puzzle02(ILogger logger, DirectoryInfo puzzleResourceDirectory) : base(logger, puzzleResourceDirectory)
    {
    }

    public override async Task<string> SolveAsync()
    {
        var inputFile = _puzzleResourceDirectory.GetFiles("input.txt").First();
        var inputString = await inputFile.ReadAllTextAsync();

        // Example
        //var exampleFile = _puzzleResourceDirectory.GetFiles("example.txt").First();
        //inputString = await exampleFile.ReadAllTextAsync();

        Disk disk = Disk.ParseInput(inputString);
        //Console.WriteLine(disk.PrintDiskLayout());
        disk.Defragment();
        //Console.WriteLine(disk.PrintDiskLayout());

        var checksum = disk.CalculateChecksum();

        return checksum.ToString();
    }
}
