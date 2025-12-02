using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace AdventOfCode2022
{
    public static class Day14
    {
        //public static void Run()
        //{
        //    Problem1();
        //    Problem2();
        //    Console.ReadKey();
        //}

        //public static void Problem1()
        //{
        //    Console.WriteLine("D14 P1");
        //    var locs = new HashSet<Point>();
        //    var sandStart = new Point(500, 0);
        //    var maxDepth = 0;
        //    using (var stream = new StreamReader(new FileStream("Day14.txt", FileMode.Open, FileAccess.Read)))
        //    {
        //        var line = stream.ReadLine();
        //        while (line != null)
        //        {
        //            var nodes = line.Split(' ').Where(st => !string.IsNullOrEmpty(st) && !st.Equals("->")).ToList();
        //            var node1 = new int[2];
        //            var node2 = new int[2];

        //            for (int i = 0; i < nodes.Count - 1; i++)
        //            {
        //                node1 = nodes[i].Split(',').Select(int.Parse).ToArray();
        //                node2 = nodes[i+1].Split(',').Select(int.Parse).ToArray();
        //                var p1 = new Point(node1[0], node1[1]);
        //                var p2 = new Point(node2[0], node2[1]);
        //                maxDepth = Math.Max(maxDepth, Math.Max(p1.Y, p2.Y));
        //                for (int x = Math.Min(p1.X, p2.X); x <= Math.Max(p1.X, p2.X); x++)
        //                {
        //                    for(int y = Math.Min(p1.Y, p2.Y); y <= Math.Max(p1.Y, p2.Y); y++)
        //                    {
        //                        locs.Add(new Point(x,y));
        //                    }
        //                }


        //            }

        //            line = stream.ReadLine();
        //        }
        //    }

        //    var isDone = false;
        //    var sandCount = 0;

        //    while (!isDone)
        //    {
        //        sandCount++;
        //        var sand = new Point(sandStart.X, sandStart.Y);
        //        var stopped = false;
        //        while(!stopped)
        //        {
        //            if (!locs.Contains(sand.Down()))
        //            {
        //                sand = sand.Down();
        //                if (sand.Y > maxDepth)
        //                {
        //                    stopped = true;
        //                    isDone = true;
        //                }
        //            }
        //            else if (!locs.Contains(sand.Left()))
        //            {
        //                sand = sand.Left();
        //            }
        //            else if (!locs.Contains(sand.Right()))
        //            {
        //                sand = sand.Right();
        //            }
        //            else
        //            {
        //                locs.Add(sand);
        //                stopped = true;
        //            }
        //        }
        //    }

        //    Console.WriteLine("Sand count: " + (sandCount - 1));
        //}

        //public static void Problem2()
        //{
        //    Console.WriteLine("D14 P2");
        //    var locs = new HashSet<Point>();
        //    var sandStart = new Point(500, 0);
        //    var maxDepth = 0;
        //    using (var stream = new StreamReader(new FileStream("Day14.txt", FileMode.Open, FileAccess.Read)))
        //    {
        //        var line = stream.ReadLine();
        //        while (line != null)
        //        {
        //            var nodes = line.Split(' ').Where(st => !string.IsNullOrEmpty(st) && !st.Equals("->")).ToList();
        //            var node1 = new int[2];
        //            var node2 = new int[2];

        //            for (int i = 0; i < nodes.Count - 1; i++)
        //            {
        //                node1 = nodes[i].Split(',').Select(int.Parse).ToArray();
        //                node2 = nodes[i + 1].Split(',').Select(int.Parse).ToArray();
        //                var p1 = new Point(node1[0], node1[1]);
        //                var p2 = new Point(node2[0], node2[1]);
        //                maxDepth = Math.Max(maxDepth, Math.Max(p1.Y, p2.Y));
        //                for (int x = Math.Min(p1.X, p2.X); x <= Math.Max(p1.X, p2.X); x++)
        //                {
        //                    for (int y = Math.Min(p1.Y, p2.Y); y <= Math.Max(p1.Y, p2.Y); y++)
        //                    {
        //                        locs.Add(new Point(x, y));
        //                    }
        //                }


        //            }

        //            line = stream.ReadLine();
        //        }
        //    }

        //    var isDone = false;
        //    var sandCount = 0;
        //    var floor = maxDepth + 1;

        //    while (!isDone)
        //    {
        //        sandCount++;
        //        var sand = new Point(sandStart.X, sandStart.Y);
        //        var stopped = false;
        //        while (!stopped)
        //        {
        //            if (!locs.Contains(sand.Down()))
        //            {
        //                sand = sand.Down();
        //            }
        //            else if (!locs.Contains(sand.Left()))
        //            {
        //                sand = sand.Left();
        //            }
        //            else if (!locs.Contains(sand.Right()))
        //            {
        //                sand = sand.Right();
        //            }
        //            else
        //            {
        //                locs.Add(sand);
        //                stopped = true;
        //            }

        //            if (sand.Y == floor)
        //            {
        //                locs.Add(sand);
        //                stopped = true;
        //            }
        //            if(sand.Equals(sandStart))
        //            {
        //                locs.Add(sand);
        //                stopped = true;
        //                isDone = true;
        //            }
        //        }
        //    }

        //    Console.WriteLine("Sand count: " + (sandCount));
        //}
    }

    public class Point : IEquatable<Point>
    {
        public int X, Y;

        public Point(int x , int y)
        {
            X = x;
            Y = y;
        }

        public Point(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        public Point Down()
        {
            return new Point(X, Y + 1);
        }

        public Point Left()
        {
            return new Point(X - 1, Y + 1);
        }

        public Point Right()
        {
            return new Point(X + 1, Y + 1);
        }

        public void MoveLeft()
        {
            X--;
        }
        public void MoveRight()
        {
            X++;
        }
        public void MoveDown()
        {
            Y--;
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
                return X.GetHashCode() ^ Y.GetHashCode();
            }
        }
    }
}