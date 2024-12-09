using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Extentions;
public static class ArrayExtentions
{
    public static T[,] To2DArray<T>(this T[][] values)
    {
        long yLemgth = values.LongLength;
        long xLength = values[0].LongLength;

        T[,] returnValue = new T[xLength, yLemgth];

        for (int y = 0;  y < yLemgth; y++)
        {
            var inner = values[y];
            for (int x = 0; x < xLength; x++)
            {
                returnValue[x, y] = inner[x];
            }
        }
        return returnValue;
    }
}
