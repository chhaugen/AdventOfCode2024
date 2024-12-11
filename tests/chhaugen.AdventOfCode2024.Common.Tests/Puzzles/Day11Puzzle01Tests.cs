using chhaugen.AdventOfCode2024.Common.Puzzles;
using chhaugen.AdventOfCode2024.Resources;

namespace chhaugen.AdventOfCode2024.Common.Tests.Puzzles;

public class Day11Puzzle01Tests
{
    private readonly PuzzleInputs _inputs = new();

    [Fact]
    public async Task SolveAsync_UsingExampleInput()
    {
        // Arrange
        Day11Puzzle01 puzzle = new();
        string input = await _inputs.GetInputAsync(day: 11, "example.txt");
        string correctOutput = 55312.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }

    [Fact]
    public async Task SolveAsync_UsingRealInput()
    {
        // Arrange
        Day11Puzzle01 puzzle = new();
        string input = await _inputs.GetInputAsync(day: 11, "input.txt");
        string correctOutput = 222461.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }
}
