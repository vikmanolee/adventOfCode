using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day14 : IDay
    {
        public void PlayPartOne(string text)
        {
            long answer = 0;
            var instructions = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            var memory = new long[] {};
            Array.Resize(ref memory, 99999);
            var mask = new char[] {};

            foreach (var instruction in instructions)
            {
                if (IsMask(instruction))
                {
                    mask = GetReverseMask(instruction);
                }
                else
                {
                    (int address, long value) = GetMemory(instruction);
                    long writeValue = GetValueToWrite(value, mask);
                    memory[address] = writeValue;
                }
            }

            answer = memory.Sum();
            Console.WriteLine($"Day14:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            long answer = 0;
            var instructions = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            var memory = new Dictionary<string, long>();
            var mask = new char[] {};

            foreach (var instruction in instructions)
            {
                if (IsMask(instruction))
                {
                    mask = GetMask(instruction);
                }
                else
                {
                    (int address, long value) = GetMemory(instruction);
                    var addreses = GetAddressesToWrite(address, mask);
                    foreach (var key in addreses)
                    {
                        if (memory.TryAdd(key, value) == false)
                        {
                            memory[key] = value;
                        }
                    }
                }
            }

            answer = memory.Sum(s => s.Value);
            Console.WriteLine($"Day14:PartTwo: {answer}");
        }

        bool IsMask(string instruction) => instruction.StartsWith("mask");

        char[] GetMask(string instruction) => instruction.Substring(7).ToArray();
        char[] GetReverseMask(string instruction) => instruction.Substring(7).Reverse().ToArray();

        (int address, long value) GetMemory(string instruction)
        {
            var m = Regex.Match(instruction, @"mem\[(\d+)\]\s\=\s(\d+)");

            var ad = m.Groups[1].Value;
            var va = m.Groups[2].Value;

            if (!Int32.TryParse(va, out int vaint)) Console.WriteLine(instruction);

            return (Int32.Parse(ad), Int64.Parse(va));
        }

        long GetValueToWrite(long value, char[] mask)
        {
            char[] binary = Convert.ToString(value, 2).PadLeft(36, '0').Reverse().ToArray();
            for (int index = 0; index < mask.Length; index++)
            {
                switch (mask[index])
                {
                    case '0':
                        binary[index] = '0';
                        break;
                    case '1':
                        binary[index] = '1';
                        break;
                    default:
                        continue;
                }
            }

            return binary.Select(PowerTwo).Sum();
        }

        long PowerTwo(char b, int index) => b == '1' ? Convert.ToInt64(Math.Pow(2, index)) : 0;

        List<string> GetAddressesToWrite(int address, char[] mask)
        {
            char[] binary = Convert.ToString(address, 2).PadLeft(36, '0').ToArray();
            var Xindexes = new List<int>();

            for (int index = 0; index < mask.Length; index++)
            {
                switch (mask[index])
                {
                    case '1':
                        binary[index] = '1';
                        break;
                    case 'X':
                        binary[index] = 'X';
                        Xindexes.Add(index);
                        break;
                    default:
                        continue;
                }
            }

            var combinations = GeneratePermutations(Xindexes.Count);

            var addressList = new List<string>();
            foreach (var comb in combinations)
            {
                char[] adr = (char[])binary.Clone();
                for (int i = 0; i < Xindexes.Count; i++)
                {
                    adr[Xindexes[i]] = comb[i];
                }
                addressList.Add(new string(adr));
            }

            return addressList;
        }

        List<char[]> GeneratePermutations(int length)
        {
            var perms = new List<char[]> { Enumerable.Repeat('0', length).ToArray() };
            for (int i = 0; i < length; i++)
            {
                var newPerms = new List<char[]>();
                foreach (var item in perms)
                {
                    var newItem = (char[])item.Clone();
                    newItem[i] = '1';
                    newPerms.Add(newItem);
                }
                perms.AddRange(newPerms);
            }
            return perms;
        }
    }
}
