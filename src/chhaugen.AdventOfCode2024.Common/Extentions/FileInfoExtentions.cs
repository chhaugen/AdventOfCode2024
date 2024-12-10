namespace chhaugen.AdventOfCode2024.Common.Extentions;

public static class FileInfoExtentions
{
    public static async Task<string> ReadAllTextAsync(this FileInfo file)
    {
        using StreamReader streamReader = file.OpenText();
        return await streamReader.ReadToEndAsync();
    }
    public static string ReadAllText(this FileInfo file)
    {
        using StreamReader streamReader = file.OpenText();
        return streamReader.ReadToEnd();
    }
}
