using System;
using System.Linq;

namespace AdventOfCode
{
    public class Day3 : IDay
    {
        public void PlayPartOne(string text)
        {
            int answer;
            string[] lines = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            answer = SlopeBumps(lines, 3, 1);

            Console.WriteLine($"Day3:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            int answer;
            string[] lines = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            answer = new (int, int)[]
            {
                (1,1),
                (3,1),
                (5,1),
                (7,1),
                (1,2)
            }
            .Select((slope) => SlopeBumps(lines, slope.Item1, slope.Item2))
            .Aggregate(1, (x,y) => x * y);

            Console.WriteLine($"Day3:PartTwo: {answer}");
        }

        private int SlopeBumps(string[] map, int right, int down)
        {
            int treeNumber = 0;
            int x = 0, y = 0;
            int maxY = map.Length;
            int maxX = map[0].Length;

            while (y < maxY)
            {
                if (IsTree(map[y][x]))
                    treeNumber++;

                (x, y) = Move(x, y, right, down, maxX);
            }
            return treeNumber;
        }

        private (int, int) Move(int xPosition, int yPosition, int horizontally, int vertically, int maxX)
        {
            return ((xPosition + horizontally)%maxX, yPosition + vertically);
        }

        private bool IsTree(char spot) => spot == '#';
    }
}