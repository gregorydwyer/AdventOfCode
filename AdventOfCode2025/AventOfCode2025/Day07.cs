using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024
{
    public class Day07
    {
        private const string Day = "Day07";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";

        public static void Run()
        {
            Program.WriteTitle("--- Day 7: Laboratories ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var map = BuildMap();
            var count = 0;
            for (int r = 0; r < map.Count - 1; r++)
            {
                for (int c = 0; c < map[r].Count; c++)
                {
                    if (map[r][c] == 'S')
                    {
                        map[r + 1][c] = '|';
                    }

                    if (map[r][c] == '^'
                        && map[r - 1][c] == '|')
                    {
                        count++;
                        map[r][c - 1] = '|';
                        map[r][c + 1] = '|';
                        map[r + 1][c - 1] = '|';
                        map[r + 1][c + 1] = '|';
                    }

                    if (map[r][c] == '|'
                        && map[r + 1][c] == '.')
                    {
                        map[r + 1][c] = '|';
                    }
                }
            }

            Program.WriteOutput("Beam Split Count: " + count);
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var map = BuildMap();
            var count = 0L;
            var counts = Enumerable.Repeat(1L, map[0].Count).ToArray();
            for (int r = map.Count - 1; r >= 0; r--)
            {
                for (int c = 0; c < map[r].Count; c++)
                {
                    if (map[r][c] == '^')
                    {
                        counts[c] = counts[c - 1] + counts[c + 1];
                    }

                    if (map[r][c] == 'S')
                    {
                        count = counts[c];
                    }
                }
            }

            Program.WriteOutput("Unique Paths: " + count);

        }

        private static List<List<char>> BuildMap()
        {
            var map = new List<List<char>>();
            using (var sr = Program.GetReader(FileLocation))
            {
                var line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    map.Add(line.ToCharArray().ToList());
                    line = sr.ReadLine();
                }
            }

            return map;
        }
    }
}