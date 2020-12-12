using System;
using System.Linq;

namespace AdventOfCode
{
    public class Day12 : IDay
    {
        public static char[] Compass = new char[] { 'E', 'N', 'W', 'S' };

        public void PlayPartOne(string text)
        {
            int answer = 0;
            var instructions = text.Split("\n", StringSplitOptions.RemoveEmptyEntries)
                                    .Select(s => (s.First(), Int16.Parse(s.Substring(1))));

            int x = 0;
            int y = 0;
            char facing = 'E';
            foreach (var instruction in instructions)
            {
                switch (instruction.Item1)
                {
                    case 'N':
                        y += instruction.Item2;
                        break;
                    case 'S':
                        y -= instruction.Item2;
                        break;
                    case 'E':
                        x += instruction.Item2;
                        break;
                    case 'W':
                        x -= instruction.Item2;
                        break;

                    case 'L':
                    case 'R':
                        facing = TurnShip(instruction.Item1, instruction.Item2, facing);
                        break;

                    case 'F':
                        x = ForwardShipX(x, facing, instruction.Item2);
                        y = ForwardShipY(y, facing, instruction.Item2);
                        break;
                    default:
                        break;
                }
            }
            answer = ManhattanDistance(x, y);

            Console.WriteLine($"Day12:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            int answer = 0;
            var instructions = text.Split("\n", StringSplitOptions.RemoveEmptyEntries)
                                    .Select(s => (s.First(), Int16.Parse(s.Substring(1))));

            int shipX = 0, shipY = 0;
            int waypointX = 10, waypointY = 1;

            foreach (var instruction in instructions)
            {
                switch (instruction.Item1)
                {
                    case 'N':
                        waypointY += instruction.Item2;
                        break;
                    case 'S':
                        waypointY -= instruction.Item2;
                        break;
                    case 'E':
                        waypointX += instruction.Item2;
                        break;
                    case 'W':
                        waypointX -= instruction.Item2;
                        break;

                    case 'L':
                    case 'R':
                        (waypointX, waypointY) = TurnWaypoint(shipX, shipY, waypointX, waypointY, instruction.Item1, instruction.Item2);
                        break;

                    case 'F':
                        (shipX, shipY) = ForwardShip(shipX, shipY, waypointX, waypointY, instruction.Item2);
                        break;
                    default:
                        break;
                }
            }
            answer = ManhattanDistance(shipX, shipY);

            Console.WriteLine($"Day12:PartTwo: {answer}"); // 286
        }

        int ManhattanDistance(int x, int y) => ManhattanDistance(0, 0, x, y);

        int ManhattanDistance(int startX, int startY, int endX, int endY)
            => Math.Abs(endX - startX) + Math.Abs(endY - startY);

        char TurnShip(char rotation, short degrees, char current)
        {
            var move = degrees / 90;
            var currentIndex = Array.IndexOf(Compass, current);
            var nextIndex = (rotation == 'L') ? mod(currentIndex + move, 4) : mod(currentIndex - move, 4);
            return Compass[nextIndex];
        }

        int ForwardShipX(int x, char direction, short points)
        {
            switch (direction)
            {
                case 'E':
                    return x + points;
                case 'W':
                    return x - points;
                default:
                    return x;
            }
        }

        int ForwardShipY(int y, char direction, short points)
        {
            switch (direction)
            {
                case 'N':
                    return y + points;
                case 'S':
                    return y - points;
                default:
                    return y;
            }
        }

        (int wX, int wY) TurnWaypoint(int shipX, int shipY, int waypointX, int waypointY, char direction, short degrees)
        {
            switch ((direction, degrees))
            {
                case ('L', 90):
                case ('R', 270):
                    return (-waypointY, waypointX);
                case ('L', 180):
                case ('R', 180):
                    return (-waypointX, -waypointY);
                case ('L', 270):
                case ('R', 90):
                    return (waypointY, -waypointX);
                default:
                    return (waypointX, waypointY);
            }
        }

        (int sX, int sY) ForwardShip(int shipX, int shipY, int waypointX, int waypointY, int units)
        {
            var x = shipX + waypointX * units;
            var y = shipY + waypointY * units;
            return (x, y);
        }

        int mod(int x, int m) => (x % m + m) % m;
    }
}
