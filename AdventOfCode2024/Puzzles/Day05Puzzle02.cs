﻿using AdventOfCode2024.Extentions;
using Microsoft.Extensions.Logging;
using static AdventOfCode2024.Puzzles.Day05Puzzle01;

namespace AdventOfCode2024.Puzzles;
public class Day05Puzzle02 : Puzzle
{
    public Day05Puzzle02(ILogger logger, DirectoryInfo puzzleResourceDirectory) : base(logger, puzzleResourceDirectory)
    {
    }

    public override async Task<string> SolveAsync()
    {
        var inputFile = _puzzleResourceDirectory.GetFiles("input.txt").First();
        var inputString = await inputFile.ReadAllTextAsync();

        // Example
        //var exampleFile = _puzzleResourceDirectory.GetFiles("example.txt").First();
        //inputString = await exampleFile.ReadAllTextAsync();

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

        return sum.ToString();
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
            if (IsUpdateFollowingRules([x, y], Rules))
                return 1;
            else if (IsUpdateFollowingRules([y, x], Rules))
                return -1;
            else throw new InvalidOperationException("wtf");
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
