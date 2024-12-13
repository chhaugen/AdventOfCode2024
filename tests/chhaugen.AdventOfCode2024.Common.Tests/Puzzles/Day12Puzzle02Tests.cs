using chhaugen.AdventOfCode2024.Common.Puzzles;
using chhaugen.AdventOfCode2024.Common.Structures;
using chhaugen.AdventOfCode2024.Resources;
using System.Drawing;
using Xunit.Abstractions;

namespace chhaugen.AdventOfCode2024.Common.Tests.Puzzles;

public class Day12Puzzle02Tests
{
    private readonly ITestOutputHelper _output;
    private readonly PuzzleInputs _inputs = new();

    public Day12Puzzle02Tests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task SolveAsync_UsingExample1Input()
    {
        // Arrange
        Day12Puzzle02 puzzle = new(_output.WriteLine);
        string input = await _inputs.GetInputAsync(day: 12, "example1.txt");
        string correctOutput = 80.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }

    [Fact]
    public async Task SolveAsync_UsingExample2Input()
    {
        // Arrange
        Day12Puzzle02 puzzle = new(_output.WriteLine);
        string input = await _inputs.GetInputAsync(day: 12, "example2.txt");
        string correctOutput = 1206.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }

    //[Fact]
    //public async Task SolveAsync_UsingRealInput()
    //{
    //    // Arrange
    //    Day12Puzzle01 puzzle = new(_output.WriteLine);
    //    string input = await _inputs.GetInputAsync(day: 12, "input.txt");
    //    string correctOutput = 2166959.ToString();
    //    // Act
    //    string output = await puzzle.SolveAsync(input);
    //    // Assert
    //    Assert.Equal(correctOutput, output);
    //}

    [Fact]
    public void GetSpiralInwardsPoints_UsingExampleInput()
    {
        // Arrange
        string input = string.Join('\n', Enumerable.Range(0, 4).Select(x => "QQQQ"));
        Map2D<char> map = Map2D.ParseInput(input);
        List<Point2D> expected = [
            new(0,0),
            new(0,1),
            new(0,2),
            new(0,3),
            new(1,3),
            new(2,3),
            new(3,3),
            new(3,2),
            new(3,1),
            new(3,0),
            new(2,0),
            new(1,0),
            new(1,1),
            new(1,2),
            new(2,2),
            new(2,1),
            ];

        // Act
        var result = map
            .GetSpiralInwardsPoints()
            .Take(expected.Count)
            .ToList();

        // Assert
        Assert.Equal(expected, result);
    }
}
