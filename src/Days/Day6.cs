using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day6 : IDay
    {
        public void PlayPartOne(string text)
        {
            int answer = 0;

            string[] groups = text.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            answer  = groups.Select(YesByAnyone).Sum();

            Console.WriteLine($"Day6:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            int answer = 0;

            string[] groups = text.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            answer  = groups.Select(YesByEveryone).Sum();

            Console.WriteLine($"Day6:PartTwo: {answer}");
        }

        private int YesByAnyone(string groupAnswers)
        {
            var answers = new HashSet<char>();

            foreach (var answer in groupAnswers)
            {
                if (answer == '\n') continue;

                if (!answers.Contains(answer))
                {
                    answers.Add(answer);
                }
            }
            return answers.Count;
        }

        private int YesByEveryone(string groupAnswers)
        {
            var answers = new Dictionary<char, int>();
            var persons = 1;

            foreach (var answer in groupAnswers.TrimEnd())
            {
                if (answer == '\n')
                {
                    persons++;
                    continue;
                }

                if (answers.TryGetValue(answer, out int times))
                {
                    answers[answer] = ++times;
                }
                else
                {
                    answers.Add(answer, 1);
                }
            }

            var a = answers.Count(val => val.Value >= persons);
            return a;
        }
    }
}