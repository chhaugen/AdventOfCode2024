using chhaugen.AdventOfCode2024.Common;
using chhaugen.AdventOfCode2024.Common.Extentions;
using chhaugen.AdventOfCode2024.Resources;
using static System.Console;

namespace chhaugen.AdventOfCode2024.Console;

internal class Program
{
    static async Task Main()
    {

        Action<string> progressOutput = WriteLine;

        Dictionary<string, Puzzle> puzzles = [];
        puzzles.AddAllPuzzles(progressOutput);

        WriteLine("Which puzzle do you want to solve?");
        int puzzleI = 0;
        foreach (var puzzle in puzzles)
        {
            WriteLine($"[{puzzleI:D2}] {puzzle.Key}");
            puzzleI++;
        }
        WriteLine();
        Write("Enter the name or number of the puzzle: ");
        string? userInput = ReadLine();
        KeyValuePair<string, Puzzle>? puzzleKeyValue = null;
        if (int.TryParse(userInput, out int puzzleNumber))
        {
            puzzleKeyValue = puzzles.ElementAt(puzzleNumber);
        }
        else if (!string.IsNullOrEmpty(userInput) && puzzles.TryGetValue(userInput, out Puzzle? userInputPuzzle))
        {
            puzzleKeyValue = new(userInput, userInputPuzzle);
        }
        else if (string.IsNullOrWhiteSpace(userInput))
        {
            puzzleKeyValue = puzzles.Last();
        }
        else
        {
            throw new InvalidOperationException($"Puzzle {userInput} not found");
        }

        PuzzleInputs puzzleInputs = new();
        var day = puzzleKeyValue
            .Value
            .Key
            .Split("Puzzle")
            .Select(x => x.Where(char.IsNumber).ToArray())
            .Select(x => int.Parse(x.AsSpan()))
            .First();
        var filenames = puzzleInputs.GetFilenames(day);

        for (int i = 0; i < filenames.Count; i++)
        {
            WriteLine($"[{i:D2}] {filenames[i]}");
        }
        WriteLine();
        WriteLine("Which input do you wanna use?");
        string? userInputFilename = ReadLine();
        string filename;
        if (int.TryParse(userInputFilename, out int filenameIndex))
            filename = filenames[filenameIndex];
        else if (!string.IsNullOrEmpty(userInputFilename))
            filename = userInputFilename;
        else
            filename = filenames.First(x => x.Contains("input"));
        var input = await puzzleInputs.GetInputAsync(day, filename);

        var puzzleResult = await puzzleKeyValue.Value.Value.SolveAsync(input);

        WriteLine($"Puzzle {puzzleKeyValue.Value.Key} resulted in: {puzzleResult}");

    }
}
