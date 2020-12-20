using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day19 : IDay
    {
        public void PlayPartOne(string text)
        {
            int answer = 0;
            string[] parts = text.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            var rules = parts[0].Split('\n', StringSplitOptions.TrimEntries)
                            .Select(s => s.Split(": "))
                            .ToDictionary(r => r[0], r => r[1].Replace("\"", ""));


            var messages = parts[1].Split('\n', StringSplitOptions.RemoveEmptyEntries);

            var rulePattern = RuleToPattern("0", rules);
            answer = messages.Count(m => Matches(rulePattern, m));
            Console.WriteLine($"Day19:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            int answer = 0;
            string[] parts = text.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            var rules = parts[0].Split('\n', StringSplitOptions.TrimEntries)
                            .Select(s => s.Split(": "))
                            .ToDictionary(r => r[0], r => r[1].Replace("\"", ""));

            rules["8"] = "42+";
            rules["11"] = "42{n} 31{n}";

            var messages = parts[1].Split('\n', StringSplitOptions.RemoveEmptyEntries);

            var rulePattern = RuleToPattern("0", rules);

            for (int i = 1; i < 10; i++)
            {
                var pat = rulePattern.Replace("n", i.ToString());
                var c = messages.Count(m => Matches(pat, m));
                answer += c;
            }

            Console.WriteLine($"Day19:PartTwo: {answer}");
        }

        string RuleToPattern(string ruleIndex, Dictionary<string, string> rules)
        {
            var rule = rules[ruleIndex];
            Regex r = new Regex(@"\d{1,3}");
            Match m = r.Match(rule);
            while (m.Success)
            {
                var ruleToReplace = rules[m.Value];
                var replacement = ruleToReplace == "a" || ruleToReplace == "b" ? ruleToReplace : $"({ruleToReplace})";
                rule = rule.Remove(m.Index, m.Length).Insert(m.Index, replacement);
                m = r.Match(rule);
            }
            rule = rule.Replace(" ", "");
            return rule;
        }

        bool Matches(string rule, string message)
        {
            return Regex.IsMatch(message, $"^{rule}$");
        }
    }
}
