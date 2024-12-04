using Microsoft.Extensions.Logging;

namespace AdventOfCode2024.Extentions;
public static class DictionaryExtentions
{
    public static Dictionary<string, IPuzzle> AddPuzzle<T>(this Dictionary<string, IPuzzle> dictionary, ILogger logger, DirectoryInfo resourceDirectory) where T : IPuzzle
    {
        var typeName = typeof(T).Name;
        var puzzleResourceDirectory = resourceDirectory
        .GetDirectories(typeName)
        .First();

        T? puzzle = (T?)Activator.CreateInstance(typeof(T), logger, puzzleResourceDirectory)
            ?? throw new InvalidOperationException($"Could not create instance of {typeof(T).Name}");

        dictionary.Add(typeName, puzzle);
        return dictionary;
    }
}
