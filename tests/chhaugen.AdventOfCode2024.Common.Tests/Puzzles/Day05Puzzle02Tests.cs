using chhaugen.AdventOfCode2024.Common.Puzzles;
using chhaugen.AdventOfCode2024.Resources;
using static chhaugen.AdventOfCode2024.Common.Puzzles.Day05Puzzle02;

namespace chhaugen.AdventOfCode2024.Common.Tests.Puzzles;

public class Day05Puzzle02Tests
{
    private readonly PuzzleInputs _inputs = new();

    [Fact]
    public async Task SolveAsync_UsingExampleInput()
    {
        // Arrange
        Day05Puzzle02 puzzle = new();
        string input = await _inputs.GetInputAsync(day: 05, "example.txt");
        string correctOutput = 123.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }

    [Fact]
    public async Task SolveAsync_UsingRealInput()
    {
        // Arrange
        Day05Puzzle02 puzzle = new();
        string input = await _inputs.GetInputAsync(day: 05, "input.txt");
        string correctOutput = 4971.ToString();
        // Act
        string output = await puzzle.SolveAsync(input);
        // Assert
        Assert.Equal(correctOutput, output);
    }

    [Theory]
    [InlineData("75,97,47,61,53", "97,75,47,61,53")]
    [InlineData("61,13,29", "61,29,13")]
    [InlineData("97,13,75,29,47", "97,75,47,29,13")]
    public void RuleComparer_UsingExampleSort(string sortInput, string expectedOrder)
    {
        // Arrange
        string ruleText = @"47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13";
        var rules = Day05Puzzle01.ParseRules(ruleText);
        RuleComparer ruleComparer = new(rules);

        List<int> input = sortInput.Split(',').Select(int.Parse).ToList();
        List<int> expected = expectedOrder.Split(',').Select(int.Parse).ToList();

        // Act
        input.Sort(ruleComparer);

        // Assert
        
        Assert.Equal(expected, input);
    }
}
