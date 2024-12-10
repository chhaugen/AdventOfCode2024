namespace chhaugen.AdventOfCode2024.Common.Extentions;
public static class EnumerableExtentions
{
    public static bool AllIncreasing(this IEnumerable<int> values)
    {
        bool isInIncreasingOrder = true;
        int? previousValue = null;
        foreach (var value in values)
        {
            if (!previousValue.HasValue)
            {
                previousValue = value;
                continue;
            }

            if (previousValue <= value)
            {
                isInIncreasingOrder = false;
                break;
            }
            previousValue = value;
        }
        return isInIncreasingOrder;
    }

    public static bool AllDecreasing(this IEnumerable<int> values)
    {
        bool isInDecreasingOrder = true;
        int? previousValue = null;
        foreach (var value in values)
        {
            if (!previousValue.HasValue)
            {
                previousValue = value;
                continue;
            }

            if (previousValue >= value)
            {
                isInDecreasingOrder = false;
                break;
            }
            previousValue = value;
        }
        return isInDecreasingOrder;
    }

    public static bool IsAdjacentValueDifferenceConstraintUpheld(this IEnumerable<int> values, int minDifference = 0, int maxDifference = int.MaxValue)
    {
        bool adjacentValuesConstraintUpheld = true;
        int? previousValue = null;
        foreach (var value in values)
        {
            if (!previousValue.HasValue)
            {
                previousValue = value;
                continue;
            }

            int difference = Math.Abs(value - previousValue.Value);

            if (minDifference > difference || difference > maxDifference)
            {
                adjacentValuesConstraintUpheld = false;
                break;
            }
            previousValue = value;
        }

        return adjacentValuesConstraintUpheld;
    }
}
