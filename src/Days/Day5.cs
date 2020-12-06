using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day5 : IDay
    {
        const int TotalRows = 128;
        const int TotalColumns = 8;

        public void PlayPartOne(string text)
        {
            int answer = 0;

            string[] boardingPasses = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            answer = boardingPasses.Select(GetSeatID).Max();

            Console.WriteLine($"Day5:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            int answer = 0;

            string[] boardingPasses = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            var seatList = boardingPasses.Select(GetSeatID);
            var allPossibleSeats = GetAllPossibleSeatIDs();
            foreach (var seatId in allPossibleSeats)
            {
                if (!seatList.Contains(seatId) && seatList.Contains(seatId+1) && seatList.Contains(seatId-1))
                {
                    answer = seatId;
                }
            }

            Console.WriteLine($"Day5:PartTwo: {answer}");
        }

        private IEnumerable<int> GetAllPossibleSeatIDs()
        {
            List<int> listAll = new List<int>();
            for (int r = 0; r < TotalRows; r++)
            {
                for (int c = 0; c < TotalColumns; c++)
                {
                    listAll.Add(CalculateSeatID(r,c));
                }
            }
            return listAll;
        }

        private int GetSeatID(string boardingPass)
        {
            (string rowSpec, string columnSpec) = GetSpecs(boardingPass);
            int row = CalculateRow(rowSpec);
            int column = CalculateColumn(columnSpec);
            return CalculateSeatID(row, column);
        }

        private (string, string) GetSpecs(string seat)
        {
            return (seat.Substring(0, 7), seat.Substring(7));
        }
        private int CalculateRow(string rowSpec)
        {
            int start = 0;
            int range = TotalRows;
            foreach (var letter in rowSpec)
            {
                if (letter == 'B') start += range/2;
                range = range/2;
            }
            return start;
        }

        private int CalculateColumn(string columnSpec)
        {
            int start = 0;
            int range = TotalColumns;
            foreach (var letter in columnSpec)
            {
                if (letter == 'R') start += range/2;
                range = range/2;
            }
            return start;
        }

        private int CalculateSeatID(int row, int column) => row * 8 + column;
    }
}