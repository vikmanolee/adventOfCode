using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day11 : IDay
    {
        int Rows;
        int Cols;
        char[][] Layout;

        public void PlayPartOne(string text)
        {
            int answer = 0;
            InitPlay(text);

            char[][] floorPlan = ClonePlan(Layout);
            bool peopleMoved;

            do
            {
                var newFloorPlan = ClonePlan(floorPlan);
                peopleMoved = false;

                for (int row = 0; row < Rows; row++)
                {
                    for (int col = 0; col < Cols; col++)
                    {
                        var position = floorPlan[row][col];
                        int occupiedAroundIt;
                        switch (position)
                        {
                            case 'L':
                                occupiedAroundIt = GetAdjacentPositions(row, col)
                                                    .Select(p => floorPlan[p.Row][p.Col])
                                                    .Count(IsOccupied);
                                if (occupiedAroundIt == 0)
                                {
                                    newFloorPlan[row][col] = '#';
                                    peopleMoved = true;
                                }
                                break;
                            case '#':
                                occupiedAroundIt = GetAdjacentPositions(row, col)
                                                    .Select(p => floorPlan[p.Row][p.Col])
                                                    .Count(IsOccupied);
                                if (occupiedAroundIt >= 4)
                                {
                                    newFloorPlan[row][col] = 'L';
                                    peopleMoved = true;
                                }
                                break;
                            default:
                                continue;
                        }
                    }
                }
                floorPlan = ClonePlan(newFloorPlan);
            }
            while (peopleMoved);

            answer = CountOccupiedForPlan(floorPlan);
            Console.WriteLine($"Day11:PartOne: {answer}");
        }

        public void PlayPartTwo(string text)
        {
            int answer = 0;
            InitPlay(text);

            char[][] currentFloorPlan = ClonePlan(Layout);
            bool peopleMoved;

            do
            {
                var newFloorPlan = ClonePlan(currentFloorPlan);
                peopleMoved = false;

                for (int row = 0; row < Rows; row++)
                {
                    for (int col = 0; col < Cols; col++)
                    {
                        var position = currentFloorPlan[row][col];
                        int occupiedAroundIt;
                        switch (position)
                        {
                            case 'L':
                                occupiedAroundIt = GetVisiblePositions(row, col)
                                                    .Select(p => currentFloorPlan[p.Row][p.Col])
                                                    .Count(IsOccupied);
                                if (occupiedAroundIt == 0)
                                {
                                    newFloorPlan[row][col] = '#';
                                    peopleMoved = true;
                                }
                                break;
                            case '#':
                                occupiedAroundIt = GetVisiblePositions(row, col)
                                                    .Select(p => currentFloorPlan[p.Row][p.Col])
                                                    .Count(IsOccupied);
                                if (occupiedAroundIt >= 5)
                                {
                                    newFloorPlan[row][col] = 'L';
                                    peopleMoved = true;
                                }
                                break;
                            default:
                                continue;
                        }
                    }
                }
                currentFloorPlan = ClonePlan(newFloorPlan);
            }
            while (peopleMoved);

            answer = CountOccupiedForPlan(currentFloorPlan);
            Console.WriteLine($"Day11:PartTwo: {answer}");
        }

        private void InitPlay(string text)
        {
            Layout = text
                        .Split("\n", StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.ToCharArray())
                        .ToArray();
            Rows = Layout.Length;
            Cols = Layout[0].Length;
        }

        private Position[] GetVisiblePositions(int row, int col)
        {
            var positions = Enumerable.Empty<Position>();

            // diagonal Up Left
            for (int r = row-1, c = col-1; r >=0 && c >=0; r--, c--)
            {
                if (IsSeat(Layout[r][c]))
                {
                    positions = positions.Append(new Position(r,c));
                    break;
                }
            }

            // diagonal Up Right
            for (int r = row-1, c = col+1; r >=0 && c < Cols; r--, c++)
            {
                if (IsSeat(Layout[r][c]))
                {
                    positions = positions.Append(new Position(r,c));
                    break;
                }
            }

            // diagonal Down Left
            for (int r = row+1, c = col-1; r < Rows && c >=0; r++, c--)
            {
                if (IsSeat(Layout[r][c]))
                {
                    positions = positions.Append(new Position(r,c));
                    break;
                }
            }

            // diagonal Down Right
            for (int r = row+1, c = col+1; r < Rows && c < Cols; r++, c++)
            {
                if (IsSeat(Layout[r][c]))
                {
                    positions = positions.Append(new Position(r,c));
                    break;
                }
            }

            // Up
            for (int r = row-1; r >=0; r--)
            {
                if (IsSeat(Layout[r][col]))
                {
                    positions = positions.Append(new Position(r,col));
                    break;
                }
            }

            // Down
            for (int r = row+1; r < Rows; r++)
            {
                if (IsSeat(Layout[r][col]))
                {
                    positions = positions.Append(new Position(r,col));
                    break;
                }
            }

            // Left
            for (int c = col-1; c >=0; c--)
            {
                if (IsSeat(Layout[row][c]))
                {
                    positions = positions.Append(new Position(row,c));
                    break;
                }
            }

            // Right
            for (int c = col+1; c < Cols; c++)
            {
                if (IsSeat(Layout[row][c]))
                {
                    positions = positions.Append(new Position(row,c));
                    break;
                }
            }

            return positions.ToArray();
        }

        private Position[] GetAdjacentPositions(int row, int col)
        {
            return new Position[] {
                new Position(row -1, col -1),
                new Position(row -1, col),
                new Position(row -1, col +1),
                new Position(row, col -1),
                new Position(row, col +1),
                new Position(row +1, col -1),
                new Position(row +1, col),
                new Position(row +1, col +1),
            }
            .Where(p => p.Row >= 0 && p.Col >= 0 && p.Row < Rows && p.Col < Cols).ToArray();
        }

        private int CountOccupiedForPlan(char[][] plan)
        {
            var seatCount = 0;
            foreach (var row in plan)
            {
                foreach (var position in row)
                {
                    if (IsOccupied(position))
                        seatCount++;
                }
            }
            return seatCount;
        }

        private bool IsSeat(char position)
        {
            return position.Equals('#') || position.Equals('L');
        }

        private bool IsOccupied(char seat)
        {
            return seat.Equals('#');
        }

        private char[][] ClonePlan(char[][] plan)
        {
            return plan.Select(c => (char[])c.Clone()).ToArray();
        }

        record Position
        {
            public int Row { get; init; }
            public int Col { get; init; }

            public Position(int row, int col)
            {
                Row = row;
                Col = col;
            }
        }
    }
}
