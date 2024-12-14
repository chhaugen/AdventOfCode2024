using chhaugen.AdventOfCode2024.Common.Puzzles;
using chhaugen.AdventOfCode2024.Resources;
using Xunit.Abstractions;

namespace chhaugen.AdventOfCode2024.Common.Tests.Puzzles;
public class Day14Puzzle01Tests
{

    private readonly ITestOutputHelper _output;
    private readonly PuzzleInputs _inputs;

    public Day14Puzzle01Tests(ITestOutputHelper output)
    {
        _output = output;
        _inputs = new();
    }


    [Fact]
    public async Task SolveAsync_UsingExampleInput()
    {
        // Arrange
        Day14Puzzle01 puzzle = new(_output.WriteLine);
        string input = await _inputs.GetInputAsync(day: 14, "example.txt");
        string correctOutput = 12.ToString();
        // Act
        string output = await puzzle.SolveAsync(input, xMax: 11, yMax: 7);
        // Assert
        Assert.Equal(correctOutput, output);
    }

    [Fact]
    public async Task SolveAsync_UsingRealInput()
    {
        // Arrange
        Day14Puzzle01 puzzle = new();
        string input = await _inputs.GetInputAsync(day: 14, "input.txt");
        string correctOutput = 230686500.ToString();
        // Act
        string output = await puzzle.SolveAsync(input, xMax: 101, yMax: 103);
        // Assert
        Assert.Equal(correctOutput, output);
    }
}
