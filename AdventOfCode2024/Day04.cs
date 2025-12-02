using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2024
{
    public class Day04
    {
        private const string Day = "Day04";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";
        private const string XMAS = "XMAS";

        public static void Run()
        {
            Program.WriteTitle("--- Day 4: Ceres Search ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var grid = BuildGrid();
            var total = 0;
            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[i].Count; j++)
                {
                    if (grid[i][j] == 'X')
                    {
                        total += CountXmasInstances(grid, i, j);
                    }
                }
            }
            Program.WriteOutput("Total Instances: " + total);
        }

        private static int CountXmasInstances(List<List<char>> grid, int r, int c)
        {
            var matches = 0;
            var sb = new StringBuilder();
            //W
            if (c - 3 >= 0)
            {
                sb.Append(grid[r][c]);
                sb.Append(grid[r][c-1]);
                sb.Append(grid[r][c-2]);
                sb.Append(grid[r][c-3]);
                if (sb.ToString() == XMAS)
                {
                    matches++;
                }
                sb.Clear();
            }
            //E
            if (c + 3 < grid[r].Count)
            {
                sb.Append(grid[r][c]);
                sb.Append(grid[r][c+1]);
                sb.Append(grid[r][c+2]);
                sb.Append(grid[r][c+3]);
                if (sb.ToString() == XMAS)
                {
                    matches++;
                }
                sb.Clear();
            }
            //N
            if (r - 3 >= 0)
            {
                sb.Append(grid[r][c]);
                sb.Append(grid[r-1][c]);
                sb.Append(grid[r-2][c]);
                sb.Append(grid[r-3][c]);
                if (sb.ToString() == XMAS)
                {
                    matches++;
                }
                sb.Clear();
            }
            //S
            if (r + 3 < grid.Count)
            {
                sb.Append(grid[r][c]);
                sb.Append(grid[r+1][c]);
                sb.Append(grid[r+2][c]);
                sb.Append(grid[r+3][c]);
                if (sb.ToString() == XMAS)
                {
                    matches++;
                }
                sb.Clear();
            }
            //NE
            if (r - 3 >= 0 && c + 3 < grid[r].Count)
            {
                sb.Append(grid[r][c]);
                sb.Append(grid[r-1][c+1]);
                sb.Append(grid[r-2][c+2]);
                sb.Append(grid[r-3][c+3]);
                if (sb.ToString() == XMAS)
                {
                    matches++;
                }
                sb.Clear();
            }
            //SE
            if (r + 3 < grid.Count && c + 3 < grid[r].Count)
            {
                sb.Append(grid[r][c]);
                sb.Append(grid[r+1][c+1]);
                sb.Append(grid[r+2][c+2]);
                sb.Append(grid[r+3][c+3]);
                if (sb.ToString() == XMAS)
                {
                    matches++;
                }
                sb.Clear();
            }
            //NW
            if (r - 3 >= 0 && c - 3 >= 0)
            {
                sb.Append(grid[r][c]);
                sb.Append(grid[r-1][c-1]);
                sb.Append(grid[r-2][c-2]);
                sb.Append(grid[r-3][c-3]);
                if (sb.ToString() == XMAS)
                {
                    matches++;
                }
                sb.Clear();
            }
            //SW
            if (r + 3 < grid.Count && c - 3 >= 0)
            {
                sb.Append(grid[r][c]);
                sb.Append(grid[r + 1][c - 1]);
                sb.Append(grid[r + 2][c - 2]);
                sb.Append(grid[r + 3][c - 3]);
                if (sb.ToString() == XMAS)
                {
                    matches++;
                }
                sb.Clear();
            }
            return matches;
        }

        private static List<List<char>> BuildGrid()
        {
            using (var reader = Program.GetReader(FileLocation))
            {
                var grid = new List<List<char>>();
                var currentLine = reader.ReadLine();
                while (currentLine != null)
                {
                    grid.Add(currentLine.ToList());
                    currentLine = reader.ReadLine();
                }

                return grid;
            }
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var grid = BuildGrid();
            var total = 0;
            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[i].Count; j++)
                {
                    if (grid[i][j] == 'A')
                    {
                        total += MasInAnXInstances(grid, i, j);
                    }
                }
            }
            Program.WriteOutput("Total Instances: " + total);
        }

        private static int MasInAnXInstances(List<List<char>> grid, int r, int c)
        {
            var matches = 0;
            if (r - 1 >= 0 &&
                r + 1 < grid.Count &&
                c - 1 >= 0 &&
                c + 1 < grid[r].Count)
            {
                var sb = new StringBuilder();
                sb.Append(grid[r - 1][c - 1]);
                sb.Append(grid[r + 1][c + 1]);
                var current = sb.ToString();
                sb.Clear();
                if (current.Contains('M') &&
                    current.Contains('S'))
                {
                    sb.Append(grid[r + 1][c - 1]);
                    sb.Append(grid[r - 1][c + 1]);
                    current = sb.ToString();
                    matches = current.Contains('M') &&
                              current.Contains('S')
                        ? 1 : 0;
                }
            }

            return matches;
        }
    }
}