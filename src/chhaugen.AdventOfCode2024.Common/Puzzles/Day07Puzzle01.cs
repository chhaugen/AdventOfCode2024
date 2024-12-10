using System.Text;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day07Puzzle01 : Puzzle
{
    public Day07Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string inputString)
    {

        var unfinishedEquations = ParseInput(inputString);

        BinaryOperator<long>[] operators =
        [
            new("+", (f, s) => f + s),
            new("*", (f, s) => f * s),
        ];

        long plausibleCalibrationsSum = 0;

        foreach (var unfinishedEquation in unfinishedEquations)
        {
            long operatorLength = unfinishedEquation.RightSide.Length - 1;
            IEnumerable<BinaryOperator<long>[]> operatorSets = CartesianPower(operators, operatorLength);
            foreach (var operatorSet in operatorSets)
            {
                if (unfinishedEquation.IsEquationTrue(operatorSet))
                {
                    plausibleCalibrationsSum += unfinishedEquation.LeftSide;
                    break;
                }
            }
        }

        return Task.FromResult(plausibleCalibrationsSum.ToString());
    }

    public static UnfinishedEquation[] ParseInput(string input)
        => input
        .Split('\n', StringSplitOptions.RemoveEmptyEntries)
        .Select(x => x.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        .Select(x => new { Left = long.Parse(x[0]), Right = x[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray() })
        .Select(x => new UnfinishedEquation(x.Left, x.Right))
        .ToArray();

    // thx ChatGPT https://chatgpt.com/share/6754ec41-d4c8-800e-903e-7a4154dc3c14
    public static IEnumerable<T[]> CartesianPower<T>(T[] set, long length)
    {
        if (length < 1)
            throw new ArgumentException("The length of the tuples must be at least 1.", nameof(length));

        int totalCombinations = (int)Math.Pow(set.Length, length);
        T[] result = new T[length];

        for (int i = 0; i < totalCombinations; i++)
        {
            int temp = i;
            for (long j = 0; j < length; j++)
            {
                result[j] = set[temp % set.Length];
                temp /= set.Length;
            }
            yield return (T[])result.Clone();
        }
    }

    public readonly struct UnfinishedEquation
    {
        public UnfinishedEquation(long leftSide, long[] rightSide)
        {
            LeftSide = leftSide;
            RightSide = rightSide;
        }

        public long LeftSide { get; }

        public long[] RightSide { get; }

        public bool IsEquationTrue(BinaryOperator<long>[] rightSideOperators)
        {
            if (rightSideOperators.Length != RightSide.Length - 1)
                throw new ArgumentException("The length of the operator set must be 1 less than the right side.", nameof(rightSideOperators));
            long rightSideSum = RightSide[0];

            for (long i = 0; i < rightSideOperators.Length; i++)
            {
                var @operator = rightSideOperators[i];
                var secondNumber = RightSide[i + 1];
                rightSideSum = @operator.Function(rightSideSum, secondNumber);
            }
            return LeftSide == rightSideSum;
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append(LeftSide);
            sb.Append(':');
            foreach (long x in RightSide)
            {
                sb.Append(' ');
                sb.Append(x);
            }
            return sb.ToString();
        }
    }

    public readonly struct BinaryOperator<T>
    {
        public BinaryOperator(string symbol, Func<T, T, T> function)
        {
            Symbol = symbol;
            Function = function;
        }

        public string Symbol { get; }
        public Func<T, T, T> Function { get; }

        public override string ToString()
            => Symbol;
    }

}
