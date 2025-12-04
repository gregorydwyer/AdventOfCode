using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024
{
    public class Day04
    {
        private const string Day = "Day04";
        private const char Roll = '@';
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";

        public static void Run()
        {
            Program.WriteTitle("--- Day 4: Printing Department ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var grid = BuildGrid();
            var count = 0;
            for (int r = 0; r < grid.Count; r++)
            {
                for (int c = 0; c < grid[r].Count; c++)
                {
                    if (grid[r][c] == Roll
                        && CountAdjacentRolls(r, c, grid) < 4)
                    {
                        count++;
                    }
                }
            }

            Program.WriteOutput("Movable rolls: " + count);
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var grid = BuildGrid();
            var count = 0;
            var rollsRemoved = false;
            do
            {
                rollsRemoved = false;
                for (int r = 0; r < grid.Count; r++)
                {
                    for (int c = 0; c < grid[r].Count; c++)
                    {
                        if (grid[r][c] == Roll
                            && CountAdjacentRolls(r, c, grid) < 4)
                        {
                            grid[r][c] = '.';
                            rollsRemoved = true;
                            count++;
                        }
                    }
                }
            } while (rollsRemoved);
            Program.WriteOutput("Rolls Removed: " + count);
        }

        private static List<List<char>> BuildGrid()
        {
            using(var sr = Program.GetReader(FileLocation))
            {
                var grid = new List<List<char>>();
                var line = sr.ReadLine();
                do
                {
                    grid.Add(line.ToCharArray().ToList());
                    line = sr.ReadLine();
                } while (!string.IsNullOrEmpty(line));

                return grid;
            }
        }

        private static int CountAdjacentRolls(int row, int col, List<List<char>> grid)
        {
            var count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    try
                    {
                        if (i == 0 && j == 0)
                        {
                            continue;
                        }
                        if (grid[row + i][col + j] == Roll)
                        {
                            count++;
                        }
                    }
                    catch{}
                }
            }

            return count;
        }

    }
}