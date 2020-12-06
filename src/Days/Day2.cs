using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day2 : IDay
    {
        public void PlayPartOne(string text)
        {
            int answer = 0;

            string[] lines = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var parts = lines.Select(line => line.Split(new char[] {'-', ':', ' '}, StringSplitOptions.RemoveEmptyEntries));
            var checkedPolicy = parts.Select(CheckPolicy);
            answer = checkedPolicy.Count(p => p);

            Console.WriteLine($"Day2:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            int answer = 0;

            string[] lines = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var parts = lines.Select(line => line.Split(new char[] {'-', ':', ' '}, StringSplitOptions.RemoveEmptyEntries));
            var checkedPolicy = parts.Select(CheckPolicyRevised);
            answer = checkedPolicy.Count(p => p);

            Console.WriteLine($"Day2:PartOne: {answer}");
        }

        private bool CheckPolicyRevised(string[] part)
        {
            var position1 = Int32.Parse(part[0]);
            var position2 = Int32.Parse(part[1]);
            var checkChar = char.Parse(part[2]);
            var password = part[3];

            return (password[position1 - 1] == checkChar) ^ (password[position2 - 1] == checkChar);
        }

        private bool CheckPolicy(string[] part)
        {
            var minOccurencies = Int32.Parse(part[0]);
            var maxOcuurencies = Int32.Parse(part[1]);
            var checkChar = part[2];
            var password = part[3];

            var matches = Regex.Matches(password, checkChar).Count();

            return matches >= minOccurencies && matches <= maxOcuurencies;
        }
    }
}