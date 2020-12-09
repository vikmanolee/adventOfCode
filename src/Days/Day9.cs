using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day9 : IDay
    {
        long PartOneAnswer = 0L;

        public void PlayPartOne(string text)
        {
            long answer = 0;
            long[] numbers = text.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(Int64.Parse).ToArray();
            int preamble = 25;

            for (int i = preamble; i < numbers.Length; i++)
            {
                // check that is sum of prev
                var valid = false;
                for (int j = i - preamble; j < i; j++)
                {
                    for (int k = j + 1; k < i; k++)
                    {
                        if (j == k) continue;

                        if (numbers[j] + numbers[k] == numbers[i])
                        {
                            valid = true;
                        }
                    }
                }

                if (!valid)
                {
                    answer = numbers[i];
                    break;
                }
            }

            PartOneAnswer = answer;
            Console.WriteLine($"Day9:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            long answer = 0;
            long[] numbers = text.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(Int64.Parse).ToArray();
            long targetSum = PartOneAnswer;

            for (int multip = 2; multip <= numbers.Length; multip++)
            {
                for (int start = 0; start < numbers.Length - multip; start++)
                {
                    var sum = 0L;
                    var smallest = long.MaxValue;
                    var largest = 0L;
                    for (int j = start; j < start + multip; j++)
                    {
                        sum += numbers[j];
                        if (numbers[j] < smallest) smallest = numbers[j];
                        if (numbers[j] > largest) largest = numbers[j];
                    }
                    if (sum == targetSum)
                    {
                        answer = smallest + largest;
                        break;
                    }
                }
            }

            Console.WriteLine($"Day9:PartTwo: {answer}");
        }
    }
}
