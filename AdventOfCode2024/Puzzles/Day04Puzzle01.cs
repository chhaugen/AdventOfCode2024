using AdventOfCode2024.Extentions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;
using Microsoft.Extensions.Logging;
using System;

namespace AdventOfCode2024.Puzzles;
public class Day04Puzzle01 : Puzzle
{
    public Day04Puzzle01(ILogger logger, DirectoryInfo puzzleResourceDirectory) : base(logger, puzzleResourceDirectory)
    {
    }

    public override async Task<string> SolveAsync()
    {
        var inputText = _puzzleResourceDirectory.GetFiles("input.txt").First();
        var input = await inputText.ReadAllTextAsync();

        //Example text = 18 times
        input = "MMMSXXMASM\nMSAMXMSMSA\nAMXSXMAAMM\nMSAMASMSMX\nXMASAMXAMM\nXXAMMXXAMA\nSMSMSASXSS\nSAXAMASAAA\nMAMMMXMMMM\nMXMXAXMASX";

        char[,] inputArray = ParseInput(input);

        List<Matrix<double>> matrices = [
            Matrix2D.Rotation(Angle.FromDegrees(0 * 45)),
            Matrix2D.Rotation(Angle.FromDegrees(1 * 45)),
            Matrix2D.Rotation(Angle.FromDegrees(2 * 45)),
            Matrix2D.Rotation(Angle.FromDegrees(3 * 45)).Inverse(),
            Matrix2D.Rotation(Angle.FromDegrees(4 * 45)).Inverse(),
            Matrix2D.Rotation(Angle.FromDegrees(5 * 45)).Inverse(),
            Matrix2D.Rotation(Angle.FromDegrees(7 * 45)).Inverse(),
            (Matrix2D.Rotation(Angle.FromDegrees(0 * 45)) * Matrix2D.Create(-1,0,0,-1)).Inverse(),
            (Matrix2D.Rotation(Angle.FromDegrees(1 * 45)) * Matrix2D.Create(-1,0,0,-1)).Inverse(),
            (Matrix2D.Rotation(Angle.FromDegrees(2 * 45)) * Matrix2D.Create(-1,0,0,-1)).Inverse(),
            (Matrix2D.Rotation(Angle.FromDegrees(3 * 45)) * Matrix2D.Create(-1,0,0,-1)).Inverse(),
            (Matrix2D.Rotation(Angle.FromDegrees(4 * 45)) * Matrix2D.Create(-1,0,0,-1)).Inverse(),
            (Matrix2D.Rotation(Angle.FromDegrees(5 * 45)) * Matrix2D.Create(-1,0,0,-1)).Inverse(),
            (Matrix2D.Rotation(Angle.FromDegrees(7 * 45)) * Matrix2D.Create(-1,0,0,-1)).Inverse(),
            ];

        //_logger.LogDebug(JsonSerializer.Serialize(matrices, options: new() { WriteIndented = true }));

        int xWidth = inputArray.GetLength(0);
        int yHeight = inputArray.GetLength(1);
        int stepDiagonal = xWidth * yHeight;

        foreach (var matrix in matrices)
        {
            for (var y = -stepDiagonal / 2;  y < stepDiagonal / 2; y++)
            {
                for (var x = -stepDiagonal / 2; x < stepDiagonal / 2; x++)
                {
                    Point2D point = new Point2D(x, y);
                    Point2D transformedPoint = point.TransformBy(matrix);
                    int pointX = (int)Math.Round(transformedPoint.X);
                    int pointY = (int)Math.Round(transformedPoint.Y);
                    if (0 <= pointX && pointX < xWidth && 0 <= pointY && pointY < yHeight)
                    {
                        char charAtPoint = inputArray[pointX, pointY];
                        Console.Write(charAtPoint);
                    }
                    else
                    {
                        Console.Write('*');
                    }
                }
                Console.WriteLine();
            }
        }

        return string.Empty;
    }


    public static char[,] ParseInput(string input)
    {
        var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        char[,] array = new char[lines[0].Length, lines.Length];
        for (int x = 0; x < lines[0].Length; x++)
        {
            for (int y = 0; y < lines.Length; y++)
            {
                array[x,y] = lines[y][x];
            }
        }
        return array;
    }
    
}
