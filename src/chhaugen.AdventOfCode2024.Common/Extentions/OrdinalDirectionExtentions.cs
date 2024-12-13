using chhaugen.AdventOfCode2024.Common.Structures;

namespace chhaugen.AdventOfCode2024.Common.Extentions;
public static class OrdinalDirectionExtentions
{
    public static OrdinalDirection TurnClockwise(this OrdinalDirection direction)
        => direction switch
        {
            OrdinalDirection.NorthEast => OrdinalDirection.SouthEast,
            OrdinalDirection.SouthEast => OrdinalDirection.SouthWest,
            OrdinalDirection.SouthWest => OrdinalDirection.NorthWest,
            OrdinalDirection.NorthWest => OrdinalDirection.NorthEast,
            _ => throw new NotImplementedException(),
        };

    public static OrdinalDirection TurnAntiClockwise(this OrdinalDirection direction)
        => direction switch
        {
            OrdinalDirection.NorthEast => OrdinalDirection.NorthWest,
            OrdinalDirection.SouthEast => OrdinalDirection.SouthWest,
            OrdinalDirection.SouthWest => OrdinalDirection.SouthEast,
            OrdinalDirection.NorthWest => OrdinalDirection.NorthEast,
            _ => throw new NotImplementedException(),
        };

    public static PrincipalWind ToPrincipalWind(this OrdinalDirection direction)
        => direction switch
        {
            OrdinalDirection.NorthEast => PrincipalWind.NorthEast,
            OrdinalDirection.SouthEast => PrincipalWind.SouthEast,
            OrdinalDirection.SouthWest => PrincipalWind.SouthWest,
            OrdinalDirection.NorthWest => PrincipalWind.NorthWest,
            _ => throw new NotImplementedException(),
        };
}
