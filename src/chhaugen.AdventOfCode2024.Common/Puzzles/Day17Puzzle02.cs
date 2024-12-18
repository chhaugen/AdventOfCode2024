using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static chhaugen.AdventOfCode2024.Common.Puzzles.Day17Puzzle01;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day17Puzzle02 : Puzzle
{
    public Day17Puzzle02(Action<string>? progressOutput) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {

        Computer computer = new();

        computer.LoadProgram(input);
        var expectedOutput = computer.Program;
        Computer foundComputer = computer.Clone();
        Parallel.For(0, int.MaxValue, (i, pls) =>
        {
            var tempComputer = computer.Clone();
            tempComputer.RegisterA = i;
            tempComputer.RunUntilHalt();
            if (Enumerable.SequenceEqual(expectedOutput, tempComputer.Output))
            {
                foundComputer = tempComputer;
                pls.Break();
            }
        });

        return Task.FromResult(foundComputer.RegisterA.ToString());
    }
}
