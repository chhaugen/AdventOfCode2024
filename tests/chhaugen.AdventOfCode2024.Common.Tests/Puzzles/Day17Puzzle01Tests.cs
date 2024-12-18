using chhaugen.AdventOfCode2024.Common.Puzzles;
using chhaugen.AdventOfCode2024.Resources;
using Xunit.Abstractions;

namespace chhaugen.AdventOfCode2024.Common.Tests.Puzzles;
public class Day17Puzzle01Tests
{

    private readonly ITestOutputHelper _output;
    private readonly PuzzleInputs _inputs;

    public Day17Puzzle01Tests(ITestOutputHelper output)
    {
        _output = output;
        _inputs = new();
    }


    [Fact]
    public async Task SolveAsync_UsingExample1Input()
    {
        // Arrange
        Day17Puzzle01 puzzle = new(_output.WriteLine);
        string input = await _inputs.GetInputAsync(day: 17, "example1.txt");
        string correctOutput = "4,6,3,5,6,3,5,2,1,0";
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }

    //[Fact]
    //public async Task SolveAsync_UsingRealInput()
    //{
    //    // Arrange
    //    Day17Puzzle01 puzzle = new(_output.WriteLine);
    //    string input = await _inputs.GetInputAsync(day: 17, "input.txt");
    //    string correctOutput = 83444.ToString();
    //    // Act
    //    string output = await puzzle.SolveAsync(input);
    //    // Assert
    //    Assert.Equal(correctOutput, output);
    //}

    [Fact]
    public void Computer_OnlineTest1()
    {
        // Arrange
        int registerBExpected = 1;
        Day17Puzzle01.Computer computer = new()
        {
            RegisterC = 9,
            Program = [2, 6]
        };
        // Act
        computer.RunUntilHalt();

        // Assert
        Assert.Equal(registerBExpected, computer.RegisterB);
    }

    [Fact]
    public void Computer_OnlineTest2()
    {
        // Arrange
        List<int> expectedOutput = [0, 1, 2];
        Day17Puzzle01.Computer computer = new()
        {
            RegisterA = 10,
            Program = [5, 0, 5, 1, 5, 4]
        };
        // Act
        computer.RunUntilHalt();

        // Assert
        Assert.Equal(expectedOutput, computer.Output);
    }

    [Fact]
    public void Computer_OnlineTest3()
    {
        // Arrange
        List<int> expectedOutput = [4, 2, 5, 6, 7, 7, 7, 7, 3, 1, 0];
        int registerAExpected = 0;
        Day17Puzzle01.Computer computer = new()
        {
            RegisterA = 2024,
            Program = [0, 1, 5, 4, 3, 0]
        };
        // Act
        computer.RunUntilHalt();

        // Assert
        Assert.Equal(expectedOutput, computer.Output);
        Assert.Equal(registerAExpected, computer.RegisterA);
    }

    [Fact]
    public void Computer_OnlineTest4()
    {
        // Arrange
        int registerBExpected = 26;
        Day17Puzzle01.Computer computer = new()
        {
            RegisterB = 29,
            Program = [1, 7]
        };
        // Act
        computer.RunUntilHalt();

        // Assert
        Assert.Equal(registerBExpected, computer.RegisterB);
    }

    [Fact]
    public void Computer_OnlineTest5()
    {
        // Arrange
        int registerBExpected = 44354;
        Day17Puzzle01.Computer computer = new()
        {
            RegisterB = 2024,
            RegisterC = 43690,
            Program = [4, 0]
        };
        // Act
        computer.RunUntilHalt();

        // Assert
        Assert.Equal(registerBExpected, computer.RegisterB);
    }
}
