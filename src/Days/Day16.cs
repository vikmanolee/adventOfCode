using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day16 : IDay
    {
        int[][] Rules;

        public void PlayPartOne(string text)
        {
            long answer = 0;
            var sections = text.Split("\n\n", StringSplitOptions.TrimEntries);

            Rules = ParseRules(sections[0]);

            answer = sections[2].Split(new char[] { ',', '\n' })
                        .Skip(1)
                        .Select(Int32.Parse)
                        .Where(IsNotValid).Sum();

            Console.WriteLine($"Day16:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            long answer = 0;
            var sections = text.Split("\n\n", StringSplitOptions.TrimEntries);

            // Parse
            Rules = ParseRules(sections[0]);
            int[] myTicket = sections[1].Substring(13).Split(',').Select(Int32.Parse).ToArray();
            var tickets = sections[2].Split('\n')
                            .Skip(1)
                            .Select(t => t.Split(',').Select(Int32.Parse).ToArray())
                            .Where(IsValid);

            // Setup
            var posibilities = GetAllPosibilities(myTicket.Length);

            // Narrow down
            foreach (var ticket in tickets)
            {
                for (int ruleId = 1; ruleId < Rules.Length; ruleId += 2)
                {
                    posibilities.GetValueOrDefault(ruleId)
                        .IntersectWith(GetPlausiblePositionsForRule(ticket, ruleId));
                }
            }

            // Refine
            var correctTicket = new int[myTicket.Length];
            var found = new HashSet<int>();
            while (posibilities.Count != 0)
            {
                foreach (KeyValuePair<int, HashSet<int>> entry in posibilities)
                {
                    int ruleId = entry.Key;
                    HashSet<int> possiblePositions = entry.Value;

                    possiblePositions.ExceptWith(found);
                    if (possiblePositions.Count == 1)
                    {
                        int correctPosition = (ruleId - 1) / 2;
                        int currentPosition = possiblePositions.First();

                        correctTicket[correctPosition] = myTicket[currentPosition];
                        found.Add(currentPosition);
                        posibilities.Remove(ruleId);
                    }
                }
            }

            // Answer
            answer = correctTicket.Take(6).Aggregate(1L, (total, next) => total * next);
            Console.WriteLine($"Day16:PartTwo: {answer}");
        }

        int[][] ParseRules(string writenRules) => Regex.Matches(writenRules, @"\d{0,3}-\d{0,3}")
                                                    .Select(m => m.Value)
                                                    .Select(m => m.Split('-').Select(Int32.Parse).ToArray())
                                                    .ToArray();

        bool InRange(int num, int[] range) => num >= range[0] && num <= range[1];

        bool IsValid(int num) => Rules.Any(range => InRange(num, range));

        bool IsValid(int[] ticket) => ticket.All(value => IsValid(value));

        bool IsNotValid(int num) => !IsValid(num);

        bool IsValidForRule(int value, int ruleNum) => (InRange(value, Rules[ruleNum - 1]) || InRange(value, Rules[ruleNum]));

        Dictionary<int, HashSet<int>> GetAllPosibilities(int fieldNum)
        {
            var p = new Dictionary<int, HashSet<int>>();

            for (int ruleId = 1; ruleId < Rules.Length; ruleId += 2)
                p.Add(ruleId, Enumerable.Range(0, fieldNum).ToHashSet());

            return p;
        }

        HashSet<int> GetPlausiblePositionsForRule(int[] ticket, int ruleId)
        {
            var set = new HashSet<int>();
            for (int position = 0; position < ticket.Length; position++)
            {
                if (IsValidForRule(ticket[position], ruleId))
                {
                    set.Add(position);
                }
            }
            return set;
        }
    }
}