using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using System.Xml.Schema;

namespace AdventOfCode2023
{
    public class Day11
    {

        private static HashSet<int> EmptyCols = new HashSet<int>();
        private static HashSet<int> EmptyRows = new HashSet<int>();
        private static List<Point> Stars = new List<Point>();

        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        public static void Problem1()
        {
            Console.WriteLine("Day11 P1");
            BuildStarMap();
            long finalTotal = 0;
            for (int i = 0; i < Stars.Count; i++)
            {
                for (int j = i + 1; j < Stars.Count; j++)
                {
                    finalTotal += Stars[i].DistanceTo(Stars[j], EmptyCols, EmptyRows);
                }
            }
            Console.WriteLine($"Total: {finalTotal}");
        }

        private static void BuildStarMap()
        {
            using (var stream = new StreamReader(new FileStream("Day11.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                var rowCount = 0;
                for (int i = 0; i < line.Length; i++)
                {
                    EmptyCols.Add(i);
                }
                while (line != null)
                {
                    if (!line.Contains("#"))
                    {
                        EmptyRows.Add(rowCount);
                    }
                    else
                    {
                        for (int i = 0; i < line.Length; i++)
                        {
                            var c = line[i];
                            if (c == '#')
                            {
                                Stars.Add(new Point(rowCount, i));
                                EmptyCols.Remove(i);
                            }
                        }
                    }

                    rowCount++;
                    line = stream.ReadLine();
                }
            }
        }

        public static void Problem2()
        {
            Console.WriteLine("Day11 P2");
            long finalTotal = 0;
            for (int i = 0; i < Stars.Count; i++)
            {
                for (int j = i + 1; j < Stars.Count; j++)
                {
                    finalTotal += Stars[i].DistanceTo(Stars[j], EmptyCols, EmptyRows, 1000000);
                }
            }
            Console.WriteLine($"Total: {finalTotal}");
        }

        private class Point : IEquatable<Point>
        {
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public long DistanceTo(Point p, HashSet<int> emptyCols, HashSet<int> emptyRows, long scale = 2)
            {
                long emptyCount = 0;
                foreach (var emptyCol in emptyCols)
                {
                    if (emptyCol < Math.Max(Y, p.Y)
                        && emptyCol > Math.Min(Y, p.Y))
                    {
                        emptyCount++;
                    }
                }
                foreach (var emptyRow in emptyRows)
                {
                    if (emptyRow < Math.Max(X, p.X)
                        && emptyRow > Math.Min(X, p.X))
                    {
                        emptyCount++;
                    }
                }


                return Math.Abs(X - p.X) + Math.Abs(Y - p.Y) + (emptyCount * scale) - emptyCount;
            }
            public bool Equals(Point other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return X == other.X && Y == other.Y;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Point) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (X * 397) ^ Y;
                }
            }
        }
    }
}
