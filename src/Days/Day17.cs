using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day17 : IDay
    {
        public void PlayPartOne(string text)
        {
            int answer = 0;
            int cycles = 6;
            char[][] diagram = text.Split('\n').Select(s => s.ToCharArray()).ToArray();
            var space = new Dictionary<(int, int, int), char>();
            for (int y = 0; y < diagram.Length; y++)
            {
                for (int x = 0; x < diagram[y].Length; x++)
                {
                    space.Add((x, y, 0), diagram[y][x]);
                }
            }

            for (int cycle = 1; cycle <= cycles; cycle++)
            {
                var newSpace = new Dictionary<(int, int, int), char>();

                for (int z = -cycle; z <= cycle; z++)
                {
                    for (int y = -cycle; y < diagram.Length + cycle; y++)
                    {
                        for (int x = -cycle; x < diagram[0].Length + cycle; x++)
                        {
                            var coord = (x, y, z);
                            var newState = GetNewState(coord, space);
                            newSpace.Add(coord, newState);
                        }
                    }
                }
                space = newSpace;
            }

            answer = space.Count(s => IsActive(s.Value));
            Console.WriteLine($"Day17:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            int answer = 0;
            int cycles = 6;
            char[][] diagram = text.Split('\n').Select(s => s.ToCharArray()).ToArray();
            var space = new Dictionary<(int, int, int, int), char>();
            for (int y = 0; y < diagram.Length; y++)
            {
                for (int x = 0; x < diagram[y].Length; x++)
                {
                    space.Add((x, y, 0, 0), diagram[y][x]);
                }
            }

            for (int cycle = 1; cycle <= cycles; cycle++)
            {
                var newSpace = new Dictionary<(int, int, int, int), char>();
                for (int w = -cycle; w <= cycle; w++)
                {
                    for (int z = -cycle; z <= cycle; z++)
                    {
                        for (int y = -cycle; y < diagram.Length + cycle; y++)
                        {
                            for (int x = -cycle; x < diagram[0].Length + cycle; x++)
                            {
                                var coord = (x, y, z, w);
                                var newState = GetNewState(coord, space);
                                newSpace.Add(coord, newState);
                            }
                        }
                    }
                }
                space = newSpace;
            }

            answer = space.Count(s => IsActive(s.Value));
            Console.WriteLine($"Day17:PartTwo: {answer}");
        }

        bool IsActive(char cube) => cube == '#';

        char GetNewState<T>(T coordinates, Dictionary<T, char> pocket)
        {
            var ns = GetNeighbours(coordinates);
            var nearbyActive = ns.Select(d => GetCurrentCubeState(d, pocket)).Count(IsActive);
            if (IsActive(GetCurrentCubeState(coordinates, pocket)))
            {
                return (nearbyActive == 2 || nearbyActive == 3) ? '#' : '.';
            }
            else
            {
                return (nearbyActive == 3) ? '#' : '.';
            }
        }

        char GetCurrentCubeState<T>(T coordinates, Dictionary<T, char> space)
        {
            if (space.TryGetValue(coordinates, out char state))
            {
                return state;
            }
            else
            {
                return '.';
            }
        }

        T[] GetNeighbours<T>(T coord)
        {
            if (coord is ValueTuple<int, int, int> m)
            {
                return (T[])(object)Neighbours(m);
            }

            if (coord is ValueTuple<int, int, int, int> m2)
            {
                return (T[])(object)Neighbours(m2);
            }

            return new T[] { };
        }

        (int, int, int)[] Neighbours((int xD, int yD, int zD) coord)
        {
            var n = new (int, int, int)[] { };

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        if (x == 0 && y == 0 && z == 0)
                            continue;

                        n = n.Append((coord.xD + x, coord.yD + y, coord.zD + z)).ToArray();
                    }
                }
            }
            return n;
        }

        (int, int, int, int)[] Neighbours((int xD, int yD, int zD, int wD) coord)
        {
            var n = new (int, int, int, int)[] { };

            for (int w = -1; w <= 1; w++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        for (int z = -1; z <= 1; z++)
                        {
                            if (x == 0 && y == 0 && z == 0 && w == 0)
                                continue;

                            n = n.Append((coord.xD + x, coord.yD + y, coord.zD + z, coord.wD + w)).ToArray();
                        }
                    }
                }
            }
            return n;
        }
    }
}