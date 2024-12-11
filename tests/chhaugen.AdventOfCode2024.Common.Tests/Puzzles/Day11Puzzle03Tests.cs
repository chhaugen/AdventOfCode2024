using chhaugen.AdventOfCode2024.Common.Puzzles;
using chhaugen.AdventOfCode2024.Resources;
using System.Numerics;

namespace chhaugen.AdventOfCode2024.Common.Tests.Puzzles;

public class Day11Puzzle03Tests
{
    private readonly PuzzleInputs _inputs = new();

    [Theory]
    [InlineData(25, "55312")]
    [InlineData(75, "65601038650482")]
    [InlineData(150, "2705183445934430257146293156")]
    [InlineData(200, "3228697720950807773236428359413636851")]
    [InlineData(500, "9332778333171329647192501576620127875703052322448004098317987815134809182249715590744339095")]
    public async Task SolveAsync_UsingExampleInput(int times, string count)
    {
        // Arrange
        Day11Puzzle03 puzzle = new();
        string input = await _inputs.GetInputAsync(day: 11, "example.txt");
        // Act
        string output = await puzzle.SolveAsync(input, times);
        // Assert
        Assert.Equal(count, output);
    }

    [Theory]
    [InlineData(25, "222461")]
    [InlineData(75, "264350935776416")]
    [InlineData(150, "10900606074554562631028267812")]
    [InlineData(200, "13010119344002949133898595086994668979")]
    [InlineData(500, "37606667031737277464887525086188379202237469282192933802518986560424519998378572700789498897")]
    //[InlineData(1000, "220582363471151968280170102882767382933850413179724668923921439702820670848595211677467884355272289538638202439741039235942169752525022851406779817242302208765379391808020477618944857")]
    public async Task SolveAsync_UsingRealInput(int times, string count)
    {
        // Arrange
        Day11Puzzle03 puzzle = new();
        string input = await _inputs.GetInputAsync(day: 11, "input.txt");
        // Act
        string output = await puzzle.SolveAsync(input, times);
        // Assert
        Assert.Equal(count, output);
    }
}
