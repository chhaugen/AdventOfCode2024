using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2024.Extentions;
using Microsoft.Extensions.Logging;
using static AdventOfCode2024.Puzzles.Day08Puzzle01;

namespace AdventOfCode2024.Puzzles;
public class Day08Puzzle02 : Puzzle
{
    public Day08Puzzle02(ILogger logger, DirectoryInfo puzzleResourceDirectory) : base(logger, puzzleResourceDirectory)
    {
    }

    public override async Task<string> SolveAsync()
    {
        var inputFile = _puzzleResourceDirectory.GetFiles("input.txt").First();
        var inputString = await inputFile.ReadAllTextAsync();


        // Example
        //var exampleFile = _puzzleResourceDirectory.GetFiles("example.txt").First();
        //inputString = await exampleFile.ReadAllTextAsync();

        Map map = Map.Parse(inputString);
        Console.WriteLine(map.PrintToString());

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
        Console.WriteLine(map.PrintToString());

        var antinodeCount = map.Count<Antinode>();
        var antenaeCount = map.Count<Antenna>();

        return (antinodeCount + antenaeCount).ToString();
    }
}
