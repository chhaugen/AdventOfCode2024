using chhaugen.AdventOfCode2024.Common.Puzzles;
using chhaugen.AdventOfCode2024.Resources;
using Xunit.Abstractions;

namespace chhaugen.AdventOfCode2024.Common.Tests.Puzzles;
public class Day15Puzzle01Tests
{

    private readonly ITestOutputHelper _output;
    private readonly PuzzleInputs _inputs;

    public Day15Puzzle01Tests(ITestOutputHelper output)
    {
        _output = output;
        _inputs = new();
    }


    [Fact]
    public async Task SolveAsync_UsingExample1Input()
    {
        // Arrange
        Day15Puzzle01 puzzle = new(_output.WriteLine);
        string input = await _inputs.GetInputAsync(day: 15, "example1.txt");
        string correctOutput = 10092.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }

    [Fact]
    public async Task SolveAsync_UsingExample2Input()
    {
        // Arrange
        Day15Puzzle01 puzzle = new(_output.WriteLine);
        string input = await _inputs.GetInputAsync(day: 15, "example2.txt");
        string correctOutput = 2028.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }

    //[Fact]
    //public async Task SolveAsync_UsingRealInput()
    //{
    //    // Arrange
    //    Day15Puzzle01 puzzle = new();
    //    string input = await _inputs.GetInputAsync(day: 15, "input.txt");
    //    string correctOutput = 230686500.ToString();
    //    // Act
    //    string output = await puzzle.SolveAsync(input);
    //    // Assert
    //    Assert.Equal(correctOutput, output);
    //}
}
