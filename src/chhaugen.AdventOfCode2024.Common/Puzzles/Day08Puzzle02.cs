using static chhaugen.AdventOfCode2024.Common.Puzzles.Day08Puzzle01;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day08Puzzle02 : Puzzle
{
    public Day08Puzzle02(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string inputString)
    {
        Map map = Map.Parse(inputString);
        _progressOutput(map.PrintToString());

        foreach (var group in map.GetAntennaGroups())
        {
            foreach (var firstAntenae in group)
            {
                foreach (var secondAntenae in group)
                {
                    if (firstAntenae.Point == secondAntenae.Point)
                        continue;
                    Vector vector = new(firstAntenae.Point, secondAntenae.Point);
                    Point antinodePoint = secondAntenae.Point;
                    bool stillOnMap = true;
                    do
                    {
                        antinodePoint = antinodePoint.Add(vector);
                        if (map.HasPoint(antinodePoint))
                        {
                            Antinode antinode = new(antinodePoint);
                            map.SetItem(antinode);
                        }
                        else
                            stillOnMap = false;
                    } while (stillOnMap);
                }
            }
        }
        _progressOutput(map.PrintToString());

        var antinodeCount = map.Count<Antinode>();
        var antenaeCount = map.Count<Antenna>();

        return Task.FromResult((antinodeCount + antenaeCount).ToString());
    }
}
