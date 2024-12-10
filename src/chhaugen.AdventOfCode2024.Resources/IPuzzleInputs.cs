namespace chhaugen.AdventOfCode2024.Resources;
public interface IPuzzleInputs
{
    public Task<string> GetInputAsync(int day, string filename);
    public List<string> GetFilenames(int day);
}
