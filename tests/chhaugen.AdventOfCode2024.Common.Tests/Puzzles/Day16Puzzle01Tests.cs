﻿using chhaugen.AdventOfCode2024.Common.Puzzles;
using chhaugen.AdventOfCode2024.Resources;
using Xunit.Abstractions;

namespace chhaugen.AdventOfCode2024.Common.Tests.Puzzles;
public class Day16Puzzle01Tests
{

    private readonly ITestOutputHelper _output;
    private readonly PuzzleInputs _inputs;

    public Day16Puzzle01Tests(ITestOutputHelper output)
    {
        _output = output;
        _inputs = new();
    }


    [Fact]
    public async Task SolveAsync_UsingExample1Input()
    {
        // Arrange
        Day16Puzzle01 puzzle = new(_output.WriteLine);
        string input = await _inputs.GetInputAsync(day: 16, "example1.txt");
        string correctOutput = 7036.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }

    [Fact]
    public async Task SolveAsync_UsingExample2Input()
    {
        // Arrange
        Day16Puzzle01 puzzle = new(_output.WriteLine);
        string input = await _inputs.GetInputAsync(day: 16, "example2.txt");
        string correctOutput = 11048.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }

    [Fact]
    public async Task SolveAsync_UsingRealInput()
    {
        // Arrange
        Day16Puzzle01 puzzle = new();
        string input = await _inputs.GetInputAsync(day: 16, "input.txt");
        string correctOutput = 83444.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }
}