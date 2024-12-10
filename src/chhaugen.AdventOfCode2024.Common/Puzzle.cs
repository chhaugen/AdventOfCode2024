namespace chhaugen.AdventOfCode2024.Common;

public abstract class Puzzle : IPuzzle
{
    protected readonly Action<string> _progressOutput;
    public Puzzle(Action<string>? progressOutput = null)
    {
        _progressOutput = progressOutput ?? new Action<string>((_) => { });
    }

    public abstract Task<string> SolveAsync(string input);
}
