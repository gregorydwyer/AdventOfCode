using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace AdventOfCode2024
{
    public class Day12
    {
        private const string Day = "Day12";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";
        private static int Height;
        private static int Width;
        private static long BasePrice = 0;
        private static long DiscountedPrice = 0;

        public static void Run()
        {
            Program.WriteTitle("--- Day 12: Garden Groups ---");
            DoCalculations();
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            Program.WriteOutput("Total price: " + BasePrice);
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            Program.WriteOutput("Discounted price: " + DiscountedPrice);

        }

        public static void DoCalculations()
        {
            var map = BuildMap();
            var visited = new HashSet<Plot>();
            var toVisit = new Stack<Plot>();
            for (int r = 0; r < Height; r++)
            {
                for (int c = 0; c < Width; c++)
                {
                    if (visited.Contains(map[r][c]))
                    {
                        continue;
                    }

                    var area = 0;
                    var sides = 0;
                    var perim = 0;
                    var verticals = new Dictionary<(int, Direction), List<int>>();
                    var horizontals = new Dictionary<(int, Direction), List<int>>();
                    toVisit.Push(map[r][c]);
                    while (toVisit.Count > 0)
                    {
                        var plot = toVisit.Pop();
                        if (visited.Contains(plot))
                        {
                            continue;
                        }

                        visited.Add(plot);
                        area++;

                        if (plot.FenceOnSide(Direction.W))
                        {
                            if (!verticals.ContainsKey((plot.X, Direction.W)))
                            {
                                verticals.Add((plot.X, Direction.W), new List<int>());
                            }
                            verticals[(plot.X, Direction.W)].Add(plot.Y);
                        }
                        if (plot.FenceOnSide(Direction.E))
                        {
                            if (!verticals.ContainsKey((plot.X + 1, Direction.E)))
                            {
                                verticals.Add((plot.X + 1, Direction.E), new List<int>());
                            }
                            verticals[(plot.X + 1, Direction.E)].Add(plot.Y);
                        }
                        if (plot.FenceOnSide(Direction.N))
                        {
                            if (!horizontals.ContainsKey((plot.Y, Direction.N)))
                            {
                                horizontals.Add((plot.Y, Direction.N), new List<int>());
                            }
                            horizontals[(plot.Y, Direction.N)].Add(plot.X);
                        }
                        if (plot.FenceOnSide(Direction.S))
                        {
                            if (!horizontals.ContainsKey((plot.Y + 1, Direction.S)))
                            {
                                horizontals.Add((plot.Y + 1, Direction.S), new List<int>());
                            }
                            horizontals[(plot.Y + 1, Direction.S)].Add(plot.X);
                        }

                        foreach (var touched in plot.Touching)
                        {
                            if (!visited.Contains(touched))
                            {
                                toVisit.Push(touched);
                            }
                        }

                        perim += plot.Perimeter;
                    }

                    foreach (var column in verticals)
                    {
                        var values = column.Value;
                        values.Sort();
                        sides++;
                        for (int i = 1; i < values.Count; i++)
                        {
                            var isConnected = values[i - 1] + 1 == values[i];
                            if (!isConnected)
                            {
                                sides++;
                            }
                        }
                    }
                    foreach (var row in horizontals)
                    {
                        var values = row.Value;
                        values.Sort();
                        sides++;
                        for (int i = 1; i < values.Count; i++)
                        {
                            var isConnected = values[i - 1] + 1 == values[i];
                            if (!isConnected)
                            {
                                sides++;
                            }
                        }
                    }

                    BasePrice += area * perim;
                    DiscountedPrice += area * sides;
                }
            }
        }

        public static List<List<Plot>> BuildMap()
        {
            using (var reader = Program.GetReader(FileLocation))
            {
                var map = new List<List<Plot>>();
                var currentLine = reader.ReadLine();
                Width = currentLine?.Length ?? -1;
                var row = -1;
                while (currentLine != null)
                {
                    row++;
                    map.Add(new List<Plot>());
                    for (int i = 0; i < currentLine.Length; i++)
                    {
                        map[row].Add(new Plot(currentLine[i],row, i));
                    }
                    currentLine = reader.ReadLine();
                }

                Height = map.Count;
                for (int r = 0; r < Height; r++)
                {
                    for (int c = 0; c < Width; c++)
                    {
                        var plot = map[r][c];
                        Plot check;
                        if (r > 0)
                        {
                            check = map[r - 1][c];
                            if (check.Plant == plot.Plant)
                            {
                                plot.Touches(check);
                                check.Touches(plot);
                            }
                        }

                        if (r < Height - 1)
                        {
                            check = map[r + 1][c];
                            if (check.Plant == plot.Plant)
                            {
                                plot.Touches(check);
                                check.Touches(plot);
                            }
                        }

                        if (c > 0)
                        {
                            check = map[r][c - 1];
                            if (check.Plant == plot.Plant)
                            {
                                plot.Touches(check);
                                check.Touches(plot);
                            }
                        }

                        if (c < Width - 1)
                        {
                            check = map[r][c + 1];
                            if (check.Plant == plot.Plant)
                            {
                                plot.Touches(check);
                                check.Touches(plot);
                            }
                        }

                    }
                }
                return map;
            }
        }

    }

    public class Plot : Point
    {
        public char Plant;
        public HashSet<Plot> Touching = new HashSet<Plot>();
        public int Perimeter => 4 - Touching.Count;

        public Plot(char plant, int x, int y): base(x,y)
        {
            Plant = plant;
        }

        public void Touches(Plot p)
        {
            Touching.Add(p);
        }

        public bool FenceOnSide(Direction d)
        {
            switch (d)
            {
                case Direction.N:
                    if (Touching.Contains(new Plot(Plant, X, Y - 1)))
                    {
                        return false;
                    }
                    break;
                case Direction.E:
                    if (Touching.Contains(new Plot(Plant, X + 1, Y)))
                    {
                        return false;
                    }
                    break;
                case Direction.S:
                    if (Touching.Contains(new Plot(Plant, X, Y + 1)))
                    {
                        return false;
                    }
                    break;
                case Direction.W:
                    if (Touching.Contains(new Plot(Plant, X - 1, Y)))
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }
    }
}