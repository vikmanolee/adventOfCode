using System;
using System.Linq;

namespace AdventOfCode
{
    public static class InputParsing
    {
        public static int[] ToIntegerArray(string input)
        {
            return input
                .Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(Int32.Parse)
                .ToArray();
        }
        
        public static long[] ToLongArray(string input)
        {
            return input
                .Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(Int64.Parse)
                .ToArray();
        }
    }
}