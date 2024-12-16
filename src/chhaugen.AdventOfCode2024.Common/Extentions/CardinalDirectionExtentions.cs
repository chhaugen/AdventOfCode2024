using chhaugen.AdventOfCode2024.Common.Structures;

namespace chhaugen.AdventOfCode2024.Common.Extentions;
public static class CardinalDirectionExtentions
{
    public static CardinalDirection TurnClockwise(this CardinalDirection direction)
        => direction switch
        {
            CardinalDirection.West  => CardinalDirection.North,
            CardinalDirection.North => CardinalDirection.East,
            CardinalDirection.East  => CardinalDirection.South,
            CardinalDirection.South => CardinalDirection.West,
            _ => throw new NotImplementedException(),
        };

    public static CardinalDirection TurnAntiClockwise(this CardinalDirection direction)
        => direction switch
        {
            CardinalDirection.West  => CardinalDirection.South,
            CardinalDirection.North => CardinalDirection.West ,
            CardinalDirection.East  => CardinalDirection.North,
            CardinalDirection.South => CardinalDirection.East,
            _ => throw new NotImplementedException(),
        };

    public static PrincipalWind ToPrincipalWind(this CardinalDirection direction)
        => direction switch
        {
            CardinalDirection.North => PrincipalWind.North,
            CardinalDirection.East  => PrincipalWind.East ,
            CardinalDirection.South => PrincipalWind.South,
            CardinalDirection.West  => PrincipalWind.West ,
            _ => throw new NotImplementedException(),
        };

    public static Axis2D ToAxis(this CardinalDirection direction)
        => direction switch
        {
            CardinalDirection.West => Axis2D.Y,
            CardinalDirection.North => Axis2D.X,
            CardinalDirection.East => Axis2D.Y,
            CardinalDirection.South => Axis2D.X,
            _ => throw new NotImplementedException(),
        };

    public static CardinalDirection ArrowToCardinalDirection(this char @char)
        => @char switch
        {
            '^' => CardinalDirection.North,
            '>' => CardinalDirection.East,
            'v' => CardinalDirection.South,
            '<' => CardinalDirection.West,
            _ => throw new NotImplementedException(),
        };

    public static char ToArrow(this CardinalDirection direction)
        => direction switch
        {
            CardinalDirection.North => '^',
            CardinalDirection.East => '>',
            CardinalDirection.South => 'v',
            CardinalDirection.West => '<',
            _ => throw new NotImplementedException(),
        };
}
