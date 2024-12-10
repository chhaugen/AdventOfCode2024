namespace chhaugen.AdventOfCode2024.Common.Extentions;
public static class DictionaryExtentions
{
    public static Dictionary<string, IPuzzle> AddPuzzle<T>(this Dictionary<string, IPuzzle> dictionary, Action<string> progressOutput) where T : IPuzzle
    {
        var typeName = typeof(T).Name;

        T? puzzle = (T?)Activator.CreateInstance(typeof(T), progressOutput)
            ?? throw new InvalidOperationException($"Could not create instance of {typeof(T).Name}");

        dictionary.Add(typeName, puzzle);
        return dictionary;
    }

    public static Dictionary<string, Puzzle> AddAllPuzzles(this Dictionary<string, Puzzle> dictionary, Action<string> progressOutput)
    {
        var type = typeof(Puzzle);
        var assembly = type.Assembly;
        var @namespace = $"{type.Namespace}.Puzzles";

        var puzzleClasses = assembly
            .GetExportedTypes()
            .Where(x => x?.Namespace == @namespace)
            .Where(x => !x.FullName?.Contains('+') ?? false);

        foreach (var puzzleClass in puzzleClasses)
        {
            object? instance = Activator.CreateInstance(type: puzzleClass, progressOutput);
            if (instance is Puzzle puzzle)
                dictionary.Add(puzzleClass.Name, puzzle);
        }
        return dictionary;
    }
}
