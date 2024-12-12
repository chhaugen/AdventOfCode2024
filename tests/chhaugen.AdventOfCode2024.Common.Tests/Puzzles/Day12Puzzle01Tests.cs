using chhaugen.AdventOfCode2024.Common.Puzzles;
using chhaugen.AdventOfCode2024.Common.Structures;
using chhaugen.AdventOfCode2024.Resources;
using System.Drawing;
using Xunit.Abstractions;

namespace chhaugen.AdventOfCode2024.Common.Tests.Puzzles;

public class Day12Puzzle01Tests
{
    private readonly ITestOutputHelper _output;
    private readonly PuzzleInputs _inputs = new();

    public Day12Puzzle01Tests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task SolveAsync_UsingExample1Input()
    {
        // Arrange
        Day12Puzzle01 puzzle = new(_output.WriteLine);
        string input = await _inputs.GetInputAsync(day: 12, "example1.txt");
        string correctOutput = 140.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }

    [Fact]
    public async Task SolveAsync_UsingExample2Input()
    {
        // Arrange
        Day12Puzzle01 puzzle = new(_output.WriteLine);
        string input = await _inputs.GetInputAsync(day: 12, "example2.txt");
        string correctOutput = 1930.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }

    [Fact]
    public async Task SolveAsync_UsingRealInput()
    {
        // Arrange
        Day12Puzzle01 puzzle = new(_output.WriteLine);
        string input = await _inputs.GetInputAsync(day: 12, "input.txt");
        string correctOutput = 1457298.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }

    //[Fact]
    //public void GetSpiralInwardsPoints_UsingExampleInput()
    //{
    //    // Arrange
    //    string input = string.Join('\n', Enumerable.Range(0, 4).Select(x => "QQQQ"));
    //    Day12Puzzle01.Map<char> map = Day12Puzzle01.ParseInput(input);
    //    List<Point<char>> expected = [
    //        map.GetPoint(0,0),
    //        map.GetPoint(0,1),
    //        map.GetPoint(0,2),
    //        map.GetPoint(0,3),
    //        map.GetPoint(1,3),
    //        map.GetPoint(2,3),
    //        map.GetPoint(3,3),
    //        map.GetPoint(3,2),
    //        map.GetPoint(3,1),
    //        map.GetPoint(3,0),
    //        map.GetPoint(2,0),
    //        map.GetPoint(1,0),
    //        map.GetPoint(1,1),
    //        map.GetPoint(1,2),
    //        map.GetPoint(2,2),
    //        map.GetPoint(2,1),
    //        ];

    //    // Act
    //    var result = map
    //        .GetSpiralInwardsPoints()
    //        .Take(expected.Count)
    //        .ToList();

    //    // Assert
    //    Assert.Equal(expected, result);
    //}
}
