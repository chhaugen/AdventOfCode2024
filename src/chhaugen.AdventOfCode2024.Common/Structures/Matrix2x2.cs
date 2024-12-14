namespace chhaugen.AdventOfCode2024.Common.Structures;
public static class Matrix2x2
{
    public static Matrix2x2<double> GetInverse(this Matrix2x2<double> matrix)
    {
        double firstPart = ((matrix.A * matrix.D) - (matrix.B * matrix.C));
        return new(matrix.D / firstPart, -matrix.B / firstPart, -matrix.C / firstPart, matrix.A / firstPart);
    }
}
