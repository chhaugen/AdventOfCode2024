using chhaugen.AdventOfCode2024.Common.Structures;

namespace chhaugen.AdventOfCode2024.Common.Extentions;
public static class PrincipalWindExtentions
{
    public static PrincipalWind TurnClockwise(this PrincipalWind direction)
        => direction switch
        {
            PrincipalWind.North     => PrincipalWind.NorthEast,
            PrincipalWind.NorthEast => PrincipalWind.East     ,
            PrincipalWind.East      => PrincipalWind.SouthEast,
            PrincipalWind.SouthEast => PrincipalWind.South    ,
            PrincipalWind.South     => PrincipalWind.SouthWest,
            PrincipalWind.SouthWest => PrincipalWind.West     ,
            PrincipalWind.West      => PrincipalWind.NorthWest,
            PrincipalWind.NorthWest => PrincipalWind.North    ,
            _ => throw new NotImplementedException(),
        };

    public static PrincipalWind TurnAntiClockwise(this PrincipalWind direction)
        => direction switch
        {
            PrincipalWind.North     => PrincipalWind.NorthWest,
            PrincipalWind.NorthEast => PrincipalWind.North,
            PrincipalWind.East      => PrincipalWind.NorthEast,
            PrincipalWind.SouthEast => PrincipalWind.East,
            PrincipalWind.South     => PrincipalWind.SouthEast,
            PrincipalWind.SouthWest => PrincipalWind.South,
            PrincipalWind.West      => PrincipalWind.SouthWest,
            PrincipalWind.NorthWest => PrincipalWind.West,
            _ => throw new NotImplementedException(),
        };
}
