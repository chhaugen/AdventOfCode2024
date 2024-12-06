﻿using AdventOfCode2024.Extentions;
using Microsoft.Extensions.Logging;

namespace AdventOfCode2024.Puzzles;
public class Day05Puzzle01 : Puzzle
{
    public Day05Puzzle01(ILogger logger, DirectoryInfo puzzleResourceDirectory) : base(logger, puzzleResourceDirectory)
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

        int sum = 0;
        foreach (var update in input.Updates)
        {
            if(!IsUpdateFollowingRules(update, input.Rules))
                continue;

            var middleIndex = update.Count / 2;
            sum += update[middleIndex];
        }

        return sum.ToString();
    }

    public static Input ParseInput(string input)
    {
        var parts = input.Split("\n\n", count: 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var rules = ParseRules(parts[0]);
        var updates = ParseUpdates(parts[1]);
        return new Input { Rules = rules, Updates = updates };
    }

    public static List<Rule> ParseRules(string rules)
        => rules.Split('\n')
            .Select(x => x.Split('|', StringSplitOptions.RemoveEmptyEntries))
            .Select(x => x.Select(int.Parse).ToArray())
            .Select(x => new Rule(x[0], x[1]))
            .ToList();

    public static List<List<int>> ParseUpdates(string updates)
        => updates
        .Split("\n", StringSplitOptions.RemoveEmptyEntries)
        .Select(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries))
        .Select(x => x.Select(int.Parse).ToList())
        .ToList();

    public static bool IsUpdateFollowingRules(List<int> updates, List<Rule> rules)
    {
        bool isFollowingRules = true;
        foreach (var rule in rules)
        {
            var pageBeforeIndex = updates.IndexOf(rule.PageBefore);
            if (pageBeforeIndex == -1)
                continue;

            var pageAfterIndex = updates.IndexOf(rule.PageAfter);
            if (pageAfterIndex == -1)
                continue;

            if (pageBeforeIndex >= pageAfterIndex)
            {
                isFollowingRules = false;
                break;
            }
        }
        return isFollowingRules;
    }

    public class Input
    {
        public List<Rule> Rules { get; set; } = [];
        public List<List<int>> Updates { get; set; } = [];
    }

    public record Rule(int PageBefore,  int PageAfter);
}