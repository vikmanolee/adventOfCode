using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day20 : IDay
    {
        public void PlayPartOne(string text)
        {
            long answer = 0;
            Dictionary<short, Dictionary<string, string>> tiles = ReadTiles(text); // 144 12*12, 10*10

            var cornerTiles = new List<short>();

            var tileNums = tiles.Keys.ToArray();



            foreach (var tileNum in tileNums)
            {
                var t = tiles[tileNum];
                int uniqueSides = 0;
                foreach (var tSide in t)
                {
                    var matched = MatchesAnySide(tSide.Value, tileNum, tiles);
                    var reverseSide = new string(tSide.Value.Reverse().ToArray());
                    var matchedReverse = MatchesAnySide(reverseSide, tileNum, tiles);
                    if (!matched && !matchedReverse)
                    {
                        uniqueSides++;
                    }
                }
                if (uniqueSides == 2)
                {
                    cornerTiles.Add(tileNum);
                }
            }

            answer = cornerTiles.Aggregate(1L, (total, next) => total * next);
            Console.WriteLine($"Day20:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            long answer = 0;

            Console.WriteLine($"Day20:PartTwo: {answer}");
        }

        bool MatchesAnySide(string tSide, short tileNum, Dictionary<short, Dictionary<string, string>> tiles)
        {
            var matched = false;
            foreach (var otherTile in tiles)
            {
                if (otherTile.Key == tileNum)
                    continue;

                foreach (KeyValuePair<string, string> side in otherTile.Value)
                {
                    if (side.Value.Equals(tSide))
                    {
                        matched = true;
                        break;
                    }
                }
                if (matched)
                {
                    break;
                }
            }
            return matched;
        }

        Dictionary<short, Dictionary<string, string>> ReadTiles(string text)
        {
            var tiles = text.Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                            .Select(t => t.Split(":\n"))
                            .ToDictionary(tile => Int16.Parse(tile[0].Substring(5, 4)), tile => TileEdges(tile[1].Split('\n')));
            return tiles;
        }

        Dictionary<string, string> TileEdges(string[] tile)
        {
            var edges = new Dictionary<string, string>();

            edges.Add("top", tile[0]);
            edges.Add("bottom", tile[^1]);
            edges.Add("left", new String(tile.Select(row => row[0]).ToArray()));
            edges.Add("right", new String(tile.Select(row => row[^1]).ToArray()));

            return edges;
        }

        Dictionary<string, string> Reverse(Dictionary<string, string> tile)
        {
            var edges = new Dictionary<string, string>();

            edges.Add("top", new string(tile["top"].Reverse().ToArray()));
            edges.Add("bottom", new string(tile["bottom"].Reverse().ToArray()));
            edges.Add("left", new string(tile["left"].Reverse().ToArray()));
            edges.Add("right", new string(tile["right"].Reverse().ToArray()));

            return edges;
        }

        Dictionary<string, string> FlipHorizontaly(Dictionary<string, string> tile)
        {
            var edges = new Dictionary<string, string>();

            return edges;
        }

        Dictionary<string, string> FlipVerticaly(Dictionary<string, string> tile)
        {
            var edges = new Dictionary<string, string>();

            return edges;
        }

        Dictionary<string, string> Rotate180(Dictionary<string, string> tile)
        {
            var edges = new Dictionary<string, string>();

            return edges;
        }


    }
}
