using AdventOfCode2024.Extentions;
using AdventOfCode2024.Puzzles;
using Microsoft.Extensions.Logging;
using System.Reflection;
using static System.Console;

namespace AdventOfCode2024;

internal class Program
{
    public const string RESOURCES_DIRECTORY = "Resources";
    static async Task Main(string[] args)
    {
        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
            .AddFilter("Microsoft", LogLevel.Warning)
            .AddFilter("System", LogLevel.Warning)
            .AddFilter("AdventOfCode2024", LogLevel.Debug)
            .AddConsole();
        });

        ILogger logger = loggerFactory.CreateLogger<Program>();

        Dictionary<string, IPuzzle> puzzles = [];
        puzzles.AddPuzzle<Day01Puzzle01>(logger, ResourceDirectoryInfo);
        puzzles.AddPuzzle<Day01Puzzle02>(logger, ResourceDirectoryInfo);
        puzzles.AddPuzzle<Day02Puzzle01>(logger, ResourceDirectoryInfo);
        puzzles.AddPuzzle<Day02Puzzle02>(logger, ResourceDirectoryInfo);
        puzzles.AddPuzzle<Day03Puzzle01>(logger, ResourceDirectoryInfo);
        puzzles.AddPuzzle<Day03Puzzle02>(logger, ResourceDirectoryInfo);
        puzzles.AddPuzzle<Day04Puzzle01>(logger, ResourceDirectoryInfo);
        puzzles.AddPuzzle<Day04Puzzle02>(logger, ResourceDirectoryInfo);
        puzzles.AddPuzzle<Day05Puzzle01>(logger, ResourceDirectoryInfo);

        WriteLine("Which puzzle do you want to solve?");
        int puzzleI = 0;
        foreach (var puzzle in puzzles)
        {
            WriteLine($"[{puzzleI}] {puzzle.Key}");
            puzzleI++;
        }
        WriteLine();
        Write("Enter the name or number of the puzzle: ");
        string? userInput = ReadLine();
        KeyValuePair<string, IPuzzle>? puzzleKeyValue = null;
        if(int.TryParse(userInput, out int puzzleNumber))
        {
            puzzleKeyValue = puzzles.ElementAt(puzzleNumber);
        }
        else if (!string.IsNullOrEmpty(userInput) && puzzles.TryGetValue(userInput, out IPuzzle? userInputPuzzle))
        {
            puzzleKeyValue = new(userInput, userInputPuzzle);
        }
        else
        {
            throw new InvalidOperationException($"Puzzle {userInput} not found");
        }

        var puzzleResult = await puzzleKeyValue.Value.Value.SolveAsync();

        WriteLine($"Puzzle {puzzleKeyValue.Value.Key} resulted in: {puzzleResult}");

    }

    public static string ResourceDirectoryPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, RESOURCES_DIRECTORY);

    public static DirectoryInfo ResourceDirectoryInfo = new(ResourceDirectoryPath);
}
