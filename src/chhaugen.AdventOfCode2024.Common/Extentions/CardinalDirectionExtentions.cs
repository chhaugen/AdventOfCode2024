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
}
