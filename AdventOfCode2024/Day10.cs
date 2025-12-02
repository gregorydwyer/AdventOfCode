using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024
{
    public class Day10
    {
        private const string Day = "Day10";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";

        public static void Run()
        {
            Program.WriteTitle("--- Day 10: Hoof It ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var map = BuildMap();
            var totalScore = 0;
            for (int r = 0; r < map.Count; r++)
            {
                for (int c = 0; c < map[r].Count; c++)
                {
                    if (map[r][c] != 0)
                    {
                        continue;
                    }

                    totalScore += ScoreTrailhead(r, c, map, false);
                }
            }
            Program.WriteOutput("Total peaks score: "  + totalScore);
        }

        private static int ScoreTrailhead(int row, int col, List<List<int>> map, bool scoreByTrailCount)
        {
            var queue = new Queue<(int r, int c)>();
            var peaks = new HashSet<(int, int)>();
            var trails = 0;
            queue.Enqueue((row,col));
            while (queue.Any())
            {
                var point = queue.Dequeue();
                var r = point.r;
                var c = point.c;
                var value = map[r][c];

                if (value == 9)
                {
                    peaks.Add(point);
                    trails++;
                    continue;
                }
                if (r > 0)
                {
                    if (map[r - 1][c] == value + 1)
                    {
                        queue.Enqueue((r-1, c));
                    }
                }
                if (r < map.Count - 1)
                {
                    if (map[r + 1][c] == value + 1)
                    {
                        queue.Enqueue((r + 1, c));
                    }
                }
                if (c > 0)
                {
                    if (map[r][c - 1] == value + 1)
                    {
                        queue.Enqueue((r,c - 1));
                    }
                }

                if (c < map[r].Count - 1)
                {
                    if (map[r][c + 1] == value + 1)
                    {
                        queue.Enqueue((r, c + 1));
                    }
                }
            }

            return scoreByTrailCount ? trails : peaks.Count;
        }

        private static List<List<int>> BuildMap()
        {
            using (var reader = Program.GetReader(FileLocation))
            {
                var map = new List<List<int>>();
                var currentLine = reader.ReadLine();
                while (currentLine != null)
                {
                    map.Add(currentLine.Select(c => int.Parse(c.ToString())).ToList());

                    currentLine = reader.ReadLine();
                }

                return map;
            }
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var map = BuildMap();
            var totalPaths = 0;
            for (int r = 0; r < map.Count; r++)
            {
                for (int c = 0; c < map[r].Count; c++)
                {
                    if (map[r][c] != 0)
                    {
                        continue;
                    }

                    totalPaths += ScoreTrailhead(r, c, map, true);
                }
            }
            Program.WriteOutput("Total trail score: " + totalPaths);
        }
    }

}