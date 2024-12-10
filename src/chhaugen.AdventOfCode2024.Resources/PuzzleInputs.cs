using System.Reflection;

namespace chhaugen.AdventOfCode2024.Resources;

public class PuzzleInputs : IPuzzleInputs
{
    public PuzzleInputs()
    {
        ResourceNameDictionary = new(GetPuzzleInputResourceNames);
    }

    internal static string ResourcePrefix { get; } = $"{typeof(PuzzleInputs).Namespace}.PuzzleInputs";
    internal static Assembly ThisAssembly { get; } = typeof(PuzzleInputs).Assembly;

    internal Lazy<Dictionary<(int day, string filename), string>> ResourceNameDictionary { get; }

    public async Task<string> GetInputAsync(int day, string filename)
    {
        var resourceName = ResourceNameDictionary.Value[(day, filename)];
        using var resourceStream = ThisAssembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Could not get Manifest Resource Stream for {resourceName}");
        using var reader = new StreamReader(resourceStream);
        var input = await reader.ReadToEndAsync();
        return input;
    }

    public List<string> GetFilenames(int day)
        => ResourceNameDictionary
        .Value
        .Keys
        .Where(x => x.day == day)
        .Select(x => x.filename)
        .ToList();

    internal Dictionary<(int day, string filename), string> GetPuzzleInputResourceNames()
    {
        var puzzleInputsDictionary = ThisAssembly
            .GetManifestResourceNames()
            .Where(x => x.StartsWith(ResourcePrefix, StringComparison.InvariantCultureIgnoreCase))
            .ToDictionary(ResourceNameToKey, x => x);
        return puzzleInputsDictionary;
    }

    internal static (int day, string filename) ResourceNameToKey(string resourceName)
    {
        var leftResourceName = resourceName.Replace(ResourcePrefix, string.Empty);

        var parts = leftResourceName.Split('.', count: 2, options: StringSplitOptions.RemoveEmptyEntries);

        var dayNumberString = parts[0].Where(char.IsNumber).ToArray();

        if (!int.TryParse(dayNumberString.AsSpan(), out int day))
            throw new InvalidOperationException($"Could not parse day for resource {resourceName}");

        return (day, parts[1]);
    }
}
