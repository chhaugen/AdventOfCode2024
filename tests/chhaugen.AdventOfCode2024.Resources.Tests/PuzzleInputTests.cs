using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chhaugen.AdventOfCode2024.Resources.Tests;

public class PuzzleInputTests
{
    [Fact]
    public async Task GetInputAsync_GetDay01()
    {
        // Arrange
        PuzzleInputs inputs = new();

        // Act
        var input = await inputs.GetInputAsync(day: 01, "input.txt");

        // Assert
        Assert.NotNull(input);
    }

    [Fact]
    public void GetFilenames()
    {
        // Arrange
        PuzzleInputs inputs = new();

        // Act
        var filenames = inputs.GetFilenames(01);

        // Assert
        Assert.NotEmpty(filenames);
    }
}
