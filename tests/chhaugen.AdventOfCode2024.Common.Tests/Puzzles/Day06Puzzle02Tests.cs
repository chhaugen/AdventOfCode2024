using chhaugen.AdventOfCode2024.Common.Puzzles;
using chhaugen.AdventOfCode2024.Resources;

namespace chhaugen.AdventOfCode2024.Common.Tests.Puzzles;
public class Day06Puzzle02Tests
{
    private readonly PuzzleInputs _inputs = new();

    [Fact]
    public async Task SolveAsync_UsingExampleInput()
    {
        // Arrange
        Day06Puzzle02 puzzle = new();
        string input = await _inputs.GetInputAsync(day: 06, "example.txt");
        string correctOutput = 6.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }

    [Fact]
    public async Task SolveAsync_UsingRealInput()
    {
        // Arrange
        Day06Puzzle02 puzzle = new();
        string input = await _inputs.GetInputAsync(day: 06, "input.txt");
        string correctOutput = 1976.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }
}
