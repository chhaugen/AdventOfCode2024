﻿using chhaugen.AdventOfCode2024.Common.Puzzles;
using chhaugen.AdventOfCode2024.Resources;

namespace chhaugen.AdventOfCode2024.Common.Tests.Puzzles;

public class Day05Puzzle01Tests
{
    private readonly PuzzleInputs _inputs = new();

    [Fact]
    public async Task SolveAsync_UsingExampleInput()
    {
        // Arrange
        Day05Puzzle01 puzzle = new();
        string input = await _inputs.GetInputAsync(day: 05, "example.txt");
        string correctOutput = 143.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }

    [Fact]
    public async Task SolveAsync_UsingRealInput()
    {
        // Arrange
        Day05Puzzle01 puzzle = new();
        string input = await _inputs.GetInputAsync(day: 05, "input.txt");
        string correctOutput = 5087.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }
}
