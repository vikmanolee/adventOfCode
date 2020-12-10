using System;
using System.Linq;

namespace AdventOfCode
{
    public class Day10 : IDay
    {
        public void PlayPartOne(string text)
        {
            int answer = 0;
            int[] outputs = text.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();

            var sequence = outputs.OrderBy(o => o).Prepend(0).ToList();
            sequence = sequence.Append(sequence[^1] + 3).ToList();

            var oneDiffs = 0;
            var threeDiffs = 0;

            for (int i = 1; i < sequence.Count; i++)
            {
                if (sequence[i] - sequence[i - 1] == 3) threeDiffs++;
                if (sequence[i] - sequence[i - 1] == 1) oneDiffs++;
            }

            answer = oneDiffs * threeDiffs;

            Console.WriteLine($"Day10:PartOne: {answer}"); //35
        }

        public void PlayPartTwo(string text)
        {
            long answer = 0;
            int[] outputs = text.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();

            var sequence = outputs.OrderBy(o => o).Prepend(0).ToList();
            sequence = sequence.Append(sequence[^1] + 3).ToList();

            long allValidCombinations = 1;
            int contiguousSkippables = 0;
            for (int i = 1; i < sequence.Count - 1; i++)
            {
                if (sequence[i] - sequence[i - 1] < 3 && sequence[i + 1] - sequence[i] < 3)
                {
                    contiguousSkippables++;
                }
                else
                {
                    if (contiguousSkippables == 0)
                        continue;
                    else
                        allValidCombinations *= AllValidCombinationsForSequenceOf(contiguousSkippables);

                    contiguousSkippables = 0;
                }
            }
            answer = allValidCombinations;
            Console.WriteLine($"Day10:PartTwo: {answer}");
        }

        // Not correct for sequences greater than 4
        private long AllValidCombinationsForSequenceOf(int set)
        {
            long sum = 0L;
            int keepers = set;

            while (set - keepers < 3)
            {
                sum = sum + Combinations(set, keepers);
                keepers--;
            }
            return sum;
        }

        private static long Combinations(int n, int r) => Factorial(n) / (Factorial(r) * Factorial(n - r));

        private static long Factorial(int n) => DoFactorial(1, n);

        private static long DoFactorial(long prevResult, int n)
        {
            if (n <= 1) return prevResult;
            return DoFactorial(prevResult * n, n - 1);
        }
    }
}
