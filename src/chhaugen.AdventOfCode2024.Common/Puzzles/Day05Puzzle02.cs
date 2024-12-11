using static chhaugen.AdventOfCode2024.Common.Puzzles.Day05Puzzle01;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day05Puzzle02 : Puzzle
{
    public Day05Puzzle02(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string inputString)
    {
        Input input = ParseInput(inputString);


        List<List<int>> listOfInvalidUpdates = [];

        foreach (var update in input.Updates)
        {
            if (!IsUpdateFollowingRules(update, input.Rules))
            {
                listOfInvalidUpdates.Add(update);
            }
        }

        int sum = 0;
        foreach (var update in listOfInvalidUpdates)
        {
            update.Sort(new RuleComparer(input.Rules));
            var middleIndex = update.Count / 2;
            sum += update[middleIndex];
        }

        return Task.FromResult(sum.ToString());
    }

    public class RuleComparer : IComparer<int>
    {
        public List<Rule> Rules { get; }

        public RuleComparer(List<Rule> rules)
        {
            Rules = rules;
        }

        public int Compare(int x, int y)
        {
            if (x == y)
                return 0;

            var firstRuleLookup = Rules.FirstOrDefault(r => r.PageBefore == x && r.PageAfter == y);
            if (firstRuleLookup != default)
                return -1;
            else
                return 1;
        }
    }

    #region Don't think about it
    //public static List<int> Sort2(List<int> update, List<Rule> rules)
    //{
    //    List<Rule> spentRules = [];
    //    foreach (var rule in rules)
    //    {
    //        var pageBeforeIndex = update.IndexOf(rule.PageBefore);
    //        if (pageBeforeIndex == -1)
    //            continue;

    //        var pageAfterIndex = update.IndexOf(rule.PageAfter);
    //        if (pageAfterIndex == -1)
    //            continue;

    //        spentRules.Add(rule);

    //        if (pageBeforeIndex < pageAfterIndex)
    //        {
    //            continue;
    //        }

    //        List<int> possibleIndexes = Enumerable.Range(start: 0, update.Count).ToList();

    //        foreach (var pageBeforeNewIndex in possibleIndexes)
    //        {
    //            bool @break = false;
    //            foreach (var pageAfterNewIndex in possibleIndexes)
    //            {
    //                var newUpdate = update.ToList();
    //                newUpdate.Remove(rule.PageAfter);
    //                newUpdate.Insert(pageAfterNewIndex, rule.PageAfter);
    //                newUpdate.Remove(rule.PageBefore);
    //                newUpdate.Insert(pageBeforeNewIndex, rule.PageBefore);
    //                if (IsUpdateFollowingRules(newUpdate, spentRules))
    //                {
    //                    update = newUpdate;
    //                    @break = true;
    //                    break;
    //                }
    //            }
    //            if (@break)
    //                break;
    //        }
    //    }
    //    return update;

    //}


    //public static List<int> Sort(List<int> update, List<Rule> rules)
    //{
    //    var permutation = new Permutations<int>(update).AsParallel();

    //    var best = new List<int>();
    //    ParallelOptions parallelOptions = new()
    //    {
    //        MaxDegreeOfParallelism = -1,
    //    };
    //    Parallel.ForEach(source: permutation, parallelOptions: parallelOptions, body: (s, pls) =>
    //    {
    //        List<int> perm = [.. s];
    //        if (IsUpdateFollowingRules(perm, rules))
    //        {
    //            pls.Break();
    //            best = perm;
    //        }
    //    });

    //    if (best.Count == 0)
    //        throw new InvalidOperationException("could not sort");

    //    return best;
    //}
    #endregion
}
