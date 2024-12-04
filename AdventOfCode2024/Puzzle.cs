using Microsoft.Extensions.Logging;

namespace AdventOfCode2024;

public abstract class Puzzle : IPuzzle
{
    protected readonly ILogger _logger;
    protected readonly DirectoryInfo _puzzleResourceDirectory;
    public Puzzle(ILogger logger, DirectoryInfo puzzleResourceDirectory)
    {
        _logger = logger;
        _puzzleResourceDirectory = puzzleResourceDirectory;
    }

    public abstract Task<string> SolveAsync();
}
