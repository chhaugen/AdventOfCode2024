using chhaugen.AdventOfCode2024.Common.Structures;

namespace chhaugen.AdventOfCode2024.Common.Tests.Structures;
public class Line2DTests
{

    [Fact]
    public void Constructor_OnePoint()
    {
        // Arrange
        Point2D point = new(10, 5);
        Line2D expected = new(-1,2,0);

        // Act
        Line2D line2D = new(point);

        // Assert
        Assert.Equal(expected, line2D);
    }

    [Fact]
    public void Constructor_TwoPoint()
    {
        // Arrange
        Point2D point1 = new(11, 13);
        Point2D point2 = new(5, 7);
        Line2D expected = new(-6, 6, -12);

        // Act
        Line2D line2D = new(point1, point2);

        // Assert
        Assert.Equal(expected, line2D);
    }

    [Fact]
    public void GetDistanceTo()
    {
        // Arrange
        Line2D line2D = new(a: -2, b: 1, c: -2);
        Point2D point = new(11, 11);
        var expected = 5.8138;
        // Act
        var distance = line2D.GetDistanceTo(point);

        // Assert
        Assert.Equal(expected, distance, 0.1);
    }
}
