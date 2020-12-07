using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day7 : IDay
    {
        const string MyBag = "shiny gold";

        private Dictionary<string, Dictionary<string, int>> _containers;

        public void PlayPartOne(string text)
        {
            int answer = 0;

            string[] rules = text.Split("\n", StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries);

            var smallBagList = new HashSet<string>() { MyBag };
            var bigBagList = new HashSet<string>();

            var generalBaglist = new HashSet<string>();

            while (smallBagList.Count > 0)
            {
                foreach (var smallBag in smallBagList)
                {
                    foreach (var rule in rules)
                    {
                        if (CanContain(rule, smallBag))
                        {
                            bigBagList.Add(ContainerBag(rule));
                        }
                    }
                    // Add all to general to avoid duplicates
                    foreach (var b in bigBagList)
                    {
                        generalBaglist.Add(b);
                    }
                }

                smallBagList = new HashSet<string>(bigBagList);
                bigBagList = new HashSet<string>();
            }

            answer = generalBaglist.Count;

            Console.WriteLine($"Day7:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            int answer = 0;
            string[] rules = text.Split("\n", StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries);
            _containers = ParseRules(rules);

            _containers.TryGetValue(MyBag, out var myBagContainers);
            answer = GetContained(myBagContainers);

            Console.WriteLine($"Day7:PartTwo: {answer}");
        }

        private int GetContained(Dictionary<string, int> container)
        {
            int count = 0;

            if (container.Count == 0)
                return count;
            else
            {
                foreach (var bag in container)
                {
                    count += bag.Value;
                    _containers.TryGetValue(bag.Key, out var containees);
                    count += bag.Value * GetContained(containees);
                }
                return count;
            }
        }

        private Dictionary<string, Dictionary<string, int>> ParseRules(string[] rules)
        {
            var containers = new Dictionary<string, Dictionary<string, int>>();

            foreach (var rule in rules)
            {
                var container = ContainerBag(rule);
                var containees = GetContainees(rule);
                containers.Add(container, containees);
            }
            return containers;
        }

        private Dictionary<string, int> GetContainees(string rule)
        {
            var containees = new Dictionary<string, int>();

            if (Regex.IsMatch(rule, @"contain no other bags\.$"))
            {
                return containees;
            }
            var matches = Regex.Matches(rule, @"(contain\s|,\s)(\d)\s(\w+\s\w+)(\sbags?)");
            foreach (Match match in matches)
            {
                var bag = match.Groups[3].Value;
                var times = match.Groups[2].Value;
                containees.Add(bag, Int32.Parse(times));
            }
            return containees;
        }

        private bool CanContain(string rule, string bag)
        {
            return Regex.IsMatch(rule, "(?<!^)"+bag);
        }

        private string ContainerBag(string rule)
        {
            var match = Regex.Match(rule, @"(^\w+\s\w+) bags contain.");
            return  match.Groups[1].Value;
        }
    }
}
