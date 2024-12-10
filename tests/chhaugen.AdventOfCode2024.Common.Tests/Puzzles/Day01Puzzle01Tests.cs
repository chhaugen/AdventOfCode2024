using chhaugen.AdventOfCode2024.Common.Puzzles;
using chhaugen.AdventOfCode2024.Resources;

namespace chhaugen.AdventOfCode2024.Common.Tests.Puzzles;

public class Day01Puzzle01Tests
{
    private readonly PuzzleInputs _inputs;
    public Day01Puzzle01Tests()
    {
        _inputs = new();
    }

    [Fact]
    public async Task SolveAsync_UsingExampleInput()
    {
        // Arrange
        Day01Puzzle01 puzzle = new();
        string input = await _inputs.GetInputAsync(day: 01, "example.txt");
        string correctOutput = 11.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }

    [Fact]
    public async Task SolveAsync_UsingInput()
    {
        // Arrange
        Day01Puzzle01 puzzle = new();
        string input = await _inputs.GetInputAsync(day: 01, "input.txt");
        string correctOutput = 2166959.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }
}
