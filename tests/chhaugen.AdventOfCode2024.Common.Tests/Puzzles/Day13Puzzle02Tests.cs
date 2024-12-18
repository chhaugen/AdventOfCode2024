﻿using chhaugen.AdventOfCode2024.Common.Puzzles;
using chhaugen.AdventOfCode2024.Resources;
using Xunit.Abstractions;

namespace chhaugen.AdventOfCode2024.Common.Tests.Puzzles;

public class Day13Puzzle02Tests
{

    private readonly ITestOutputHelper _output;
    private readonly PuzzleInputs _inputs;

    public Day13Puzzle02Tests(ITestOutputHelper output)
    {
        _output = output;
        _inputs = new();
    }


    //[Fact]
    //public async Task SolveAsync_UsingExampleInput()
    //{
    //    // Arrange
    //    Day13Puzzle02 puzzle = new(_output.WriteLine);
    //    string input = await _inputs.GetInputAsync(day: 13, "example.txt");

    //    string correctOutput = 480.ToString();
    //    // Act
    //    string output = await puzzle.SolveAsync(input);
    //    // Assert
    //    //Assert.Equal(correctOutput, output);
    //}

    [Fact]
    public async Task SolveAsync_UsingRealInput()
    {
        // Arrange
        Day13Puzzle02 puzzle = new();
        string input = await _inputs.GetInputAsync(day: 13, "input.txt");
        string correctOutput = 83551068361379.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }
}
