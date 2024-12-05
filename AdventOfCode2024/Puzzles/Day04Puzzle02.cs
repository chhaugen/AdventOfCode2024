using AdventOfCode2024.Extentions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AdventOfCode2024.Puzzles;
public class Day04Puzzle02 : Puzzle
{
    public Day04Puzzle02(ILogger logger, DirectoryInfo puzzleResourceDirectory) : base(logger, puzzleResourceDirectory)
    {
    }

    public override async Task<string> SolveAsync()
    {
        var inputText = _puzzleResourceDirectory.GetFiles("input.txt").First();
        var input = await inputText.ReadAllTextAsync();

        //Example text = 18 times
        //input = "MMMSXXMASM\nMSAMXMSMSA\nAMXSXMAAMM\nMSAMASMSMX\nXMASAMXAMM\nXXAMMXXAMA\nSMSMSASXSS\nSAXAMASAAA\nMAMMMXMMMM\nMXMXAXMASX";

        char[,] inputArray = Day04Puzzle01.ParseInput(input);
        //_logger.LogDebug(Print2DArray(inputArray));

        char?[,] firstMask = {
            { 'M' , null, 'S'  },
            { null, 'A' , null },
            { 'M' , null, 'S'  }
        };
        //_logger.LogDebug(Print2DArray(firstMask));

        var maskRotatedOnce = RotateMatrixCounterClockwise(firstMask);
        //_logger.LogDebug(Print2DArray(maskRotatedOnce));
        var maskRotatedTwice = RotateMatrixCounterClockwise(maskRotatedOnce);
        //_logger.LogDebug(Print2DArray(maskRotatedTwice));
        var maskRotatedTrise = RotateMatrixCounterClockwise(maskRotatedTwice);
        //_logger.LogDebug(Print2DArray(maskRotatedTrise));

        List<char?[,]> masks = [firstMask, maskRotatedOnce, maskRotatedTwice, maskRotatedTrise];

        int count = 0;

        foreach (var mask in masks)
        {
            count += CountMaskIn2DArray(inputArray, mask);
        }

        return count.ToString();
    }

    // https://stackoverflow.com/a/18035050/11345730
    public static T[,] RotateMatrixCounterClockwise<T>(T[,] oldMatrix)
    {
        T[,] newMatrix = new T[oldMatrix.GetLength(1), oldMatrix.GetLength(0)];
        int newColumn, newRow = 0;
        for (int oldColumn = oldMatrix.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
        {
            newColumn = 0;
            for (int oldRow = 0; oldRow < oldMatrix.GetLength(0); oldRow++)
            {
                newMatrix[newRow, newColumn] = oldMatrix[oldRow, oldColumn];
                newColumn++;
            }
            newRow++;
        }
        return newMatrix;
    }

    public static string Print2DArray<T>(T?[,] twoDArray) where T : struct
    {
        int xWidth = twoDArray.GetLength(0);
        int yHeight = twoDArray.GetLength(0);
        StringBuilder sb = new();
        for (int y = 0; y < yHeight; y++)
        {
            for (int x = 0; x < xWidth; x++)
            {
                T? value = twoDArray[x, y];
                if (value.HasValue)
                    sb.Append(value.Value);
                else
                    sb.Append(' ');
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public static string Print2DArray<T>(T[,] twoDArray)
    {
        int xWidth = twoDArray.GetLength(0);
        int yHeight = twoDArray.GetLength(0);
        StringBuilder sb = new();
        for (int y = 0; y < yHeight; y++)
        {
            for (int x = 0; x < xWidth; x++)
            {
                T value = twoDArray[x, y];
                if (value != null)
                    sb.Append(value);
                else
                    sb.Append(' ');
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public static int CountMaskIn2DArray<T>(T[,] twoDArray, T?[,] mask) where T : struct, IEquatable<T>
    {
        int twoDArrayXWidth = twoDArray.GetLength(0);
        int twoDArrayYHeight = twoDArray.GetLength(1);
        int maskXWidth = mask.GetLength(0);
        int maskYHeight = mask.GetLength(0);

        int stopXLength = twoDArrayXWidth - maskXWidth;
        int stopYLength = twoDArrayYHeight - maskYHeight;

        int count = 0;

        for (int x = 0; x <= stopXLength; x++)
        {
            for (int y = 0; y <= stopYLength; y++)
            {
                bool success = true;
                for (int xMask = 0; xMask < maskXWidth; xMask++)
                {
                    for (int yMask = 0; yMask < maskYHeight; yMask++)
                    {
                        T? maskChar = mask[xMask, yMask];
                        if (maskChar == null)
                            continue;

                        T? arrayChar = twoDArray[x +  xMask, y + yMask];
                        if (arrayChar == null)
                            continue;

                        if (!maskChar.Equals(arrayChar))
                        {
                            success = false;
                            goto breakInner;
                        }
                    }
                }
                breakInner:
                if (success)
                    count++;
            }
        }
        return count;
    }
}
