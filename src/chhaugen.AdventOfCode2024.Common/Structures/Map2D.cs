namespace chhaugen.AdventOfCode2024.Common.Structures;
public abstract class Map2D
{
    public static Map2D<TResult> ParseInput<TResult>(string input, Func<char, TResult> converter)
    {
        var data = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        int yLength = data.Length;
        int xLength = data[0].Length;
        TResult[,] map = new TResult[xLength, yLength];

        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLength; x++)
            {
                map[x, y] = converter(data[y][x]);
            }
        }

        return new(map);
    }

    public static Map2D<char> ParseInput(string input)
    {
        var data = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        int yLength = data.Length;
        int xLength = data[0].Length;
        char[,] map = new char[xLength, yLength];

        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLength; x++)
            {
                map[x, y] = data[y][x];
            }
        }

        return new(map);
    }
}
