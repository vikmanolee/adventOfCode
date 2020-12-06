using System;
using System.Linq;

namespace AdventOfCode
{
    public class Day1 : IDay
    {
        public void PlayPartOne(string text)
        {
            int answer = 0;

            string[] lines = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            int[] numbers = lines.Select(l => Int32.Parse(l)).ToArray();

            foreach (int number1 in numbers)
            {
                foreach (var number2 in numbers)
                {
                    if (number1 + number2 == 2020)
                    {
                        answer = number1 * number2;
                        break;
                    }
                }
            }

            Console.WriteLine($"Day1:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            int answer = 0;

            string[] lines = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            int[] numbers = lines.Select(l => Int32.Parse(l)).ToArray();

            foreach (int number1 in numbers)
            {
                foreach (var number2 in numbers)
                {
                    foreach (var number3 in numbers)
                    {
                        if (number1 + number2 + number3 == 2020)
                        {
                            answer = number1 * number2 * number3;
                            break;
                        }
                    }
                }
            }

            Console.WriteLine($"Day1:PartOne: {answer}");
        }
    }
}