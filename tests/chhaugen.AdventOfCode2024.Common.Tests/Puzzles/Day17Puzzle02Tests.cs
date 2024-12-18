using chhaugen.AdventOfCode2024.Common.Puzzles;
using chhaugen.AdventOfCode2024.Resources;
using Xunit.Abstractions;

namespace chhaugen.AdventOfCode2024.Common.Tests.Puzzles;
public class Day17Puzzle02Tests
{

    private readonly ITestOutputHelper _output;
    private readonly PuzzleInputs _inputs;

    public Day17Puzzle02Tests(ITestOutputHelper output)
    {
        _output = output;
        _inputs = new();
    }


    [Fact]
    public async Task SolveAsync_UsingExample2Input()
    {
        // Arrange
        Day17Puzzle02 puzzle = new(_output.WriteLine);
        string input = await _inputs.GetInputAsync(day: 17, "example2.txt");
        input = input.Replace("2024", "117440");
        string correctOutput = "0,3,5,4,3,0";
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }

    //[Fact]
    //public async Task SolveAsync_UsingRealInput()
    //{
    //    // Arrange
    //    Day17Puzzle02 puzzle = new(_output.WriteLine);
    //    string input = await _inputs.GetInputAsync(day: 17, "input.txt");
    //    string correctOutput = 83444.ToString();
    //    // Act
    //    string output = await puzzle.SolveAsync(input);
    //    // Assert
    //    Assert.Equal(correctOutput, output);
    //}

}
