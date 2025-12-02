using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode2024
{
    public class Day08
    {
        private const string Day = "Day08";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";
        private static int MapWidth;
        private static int MapHeight;

        public static void Run()
        {
            Program.WriteTitle("--- Day 8: Resonant Collinearity ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var map = BuildMap();
            var totalNodes = new HashSet<Point>();
            foreach (var freq in map)
            {
                var antennae = freq.Value;
                for (int i = 0; i < antennae.Count - 1; i++)
                {
                    for (int j = i + 1; j < antennae.Count; j++)
                    {
                        var x1 = antennae[i].X - antennae[j].X;
                        var y1 = antennae[i].Y - antennae[j].Y;
                        var p1 = new Point(antennae[i].X + x1, antennae[i].Y + y1);
                        if (IsPointOnMap(p1))
                        {
                            totalNodes.Add(p1);
                        }

                        var x2 = antennae[j].X - antennae[i].X;
                        var y2 = antennae[j].Y - antennae[i].Y;
                        var p2 = new Point(antennae[j].X + x2, antennae[j].Y + y2);
                        if (IsPointOnMap(p2))
                        {
                            totalNodes.Add(p2);
                        }
                    }
                }
            }

            Program.WriteOutput("Total Antinodes: " + totalNodes.Count);
        }

        private static bool IsPointOnMap(Point p)
        {
            return p.X >= 0 &&
                   p.X <= MapWidth &&
                   p.Y >= 0 &&
                   p.Y <= MapHeight;
        }

        private static Dictionary<char, List<Point>> BuildMap()
        {
            using (var reader = Program.GetReader(FileLocation))
            {
                var map = new Dictionary<char, List<Point>>();
                var currentLine = reader.ReadLine();
                MapWidth = currentLine?.Length - 1 ?? 0;
                var row = 0;
                while (currentLine != null)
                {
                    for (int col = 0; col < currentLine.Length; col++)
                    {
                        var freq = currentLine[col];
                        if (freq == '.')
                        {
                            continue;
                        }
                        if(map.ContainsKey(freq))
                        {
                            map[freq].Add(new Point(row, col));
                        }
                        else
                        {
                            map.Add(currentLine[col], new List<Point>(){new Point(row, col)});
                        }
                    }
                    row++;
                    currentLine = reader.ReadLine();
                }

                MapHeight = row - 1;
                return map;
            }
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var map = BuildMap();
            var totalNodes = new HashSet<Point>();
            foreach (var freq in map)
            {
                var antennae = freq.Value;
                if (antennae.Count > 1)
                {
                    antennae.ForEach(ant => totalNodes.Add(ant));
                }
                for (int i = 0; i < antennae.Count - 1; i++)
                {
                    for (int j = i + 1; j < antennae.Count; j++)
                    {
                        var x1 = antennae[i].X - antennae[j].X;
                        var y1 = antennae[i].Y - antennae[j].Y;
                        var p1 = new Point(antennae[i].X + x1, antennae[i].Y + y1);
                        while (IsPointOnMap(p1))
                        {
                            totalNodes.Add(p1);
                            p1 = new Point(p1.X + x1, p1.Y + y1);
                        }

                        var x2 = antennae[j].X - antennae[i].X;
                        var y2 = antennae[j].Y - antennae[i].Y;
                        var p2 = new Point(antennae[j].X + x2, antennae[j].Y + y2);
                        while (IsPointOnMap(p2))
                        {
                            totalNodes.Add(p2);
                            p2 = new Point(p2.X + x2, p2.Y + y2);
                        }
                    }
                }
            }

            Program.WriteOutput("Total Antinodes: " + totalNodes.Count);
        }
    }
}