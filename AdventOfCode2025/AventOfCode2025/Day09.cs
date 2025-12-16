using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AdventOfCode2024
{
    public class Day09
    {
        private const string Day = "Day09";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";
        private static List<KeyValuePair<long, (Point P1, Point P2)>> Rects = new List<KeyValuePair<long, (Point P1, Point P2)>>();
        private static Point BottomRight = new Point(0,0);
        

        public static void Run()
        {
            Program.WriteTitle("--- Day 9: Movie Theater ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var red = GetRedPoints();
            var biggest = 0L;
            for (int i = 0; i < red.Count - 1; i++)
            {
                for (int j = i + 1; j < red.Count; j++)
                {
                    var p1 = red[i];
                    var p2 = red[j];
                    var width = Math.Abs(red[i].X - red[j].X) + 1;
                    var height = Math.Abs(red[i].Y - red[j].Y) + 1;
                    var area = width * height;
                    var kvp = new KeyValuePair<long, (Point,Point)>(area, (p1, p2));
                    Rects.Add(kvp);
                    if (area > biggest)
                    {
                        biggest = area;
                    }
                }
            }
            Program.WriteOutput("Biggest rectangle area: " + biggest);
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            Program.WriteOutput("Long Running. Do not run with all others.");
            return;
            var red = GetRedPoints();
            var green = GetGreenPoints(red);
            var perimeter = red.ToHashSet();
            perimeter.UnionWith(green);
            var newPerim = ExpandPerim(perimeter);
            var rects = Rects.OrderByDescending(kvp => kvp.Key);
            var count = 0L;
            foreach (var rect in rects)
            {
                var p1 = rect.Value.P1;
                var p2 = rect.Value.P2;
                var x = new List<long>() { p1.X, p2.X };
                var y = new List<long>() { p1.Y, p2.Y };
                x.Sort();
                y.Sort();
                var rectPerim = new HashSet<Point>();
                for (long i = x[0]; i < x[1]; i++)
                {
                    rectPerim.Add(new Point(i, y[0]));
                    rectPerim.Add(new Point(i, y[1]));
                }

                for (long i = y[0]; i < y[1]; i++)
                {
                    rectPerim.Add(new Point(x[0], i));
                    rectPerim.Add(new Point(x[1], i));
                }
                rectPerim.IntersectWith(newPerim);
                if (!rectPerim.Any())
                {
                    var width = Math.Abs(p1.X - p2.X) + 1;
                    var height = Math.Abs(p1.Y - p2.Y) + 1;
                    var area = width * height;
                    Program.WriteOutput("Biggest Contained Rectangle Area: " + area);
                    break;
                }
            }

        }

        private static HashSet<Point> ExpandPerim(HashSet<Point> perim)
        {
            var newPerim = new HashSet<Point>();
            var start = new Point(BottomRight.X + 1, BottomRight.Y + 1);
            newPerim.Add(start);
            var block = new Block(new Point(start.X - 1, start.Y));
            do
            {
                var center = block.Center();
                if (newPerim.Contains(center))
                {
                    throw new Exception();
                }
                newPerim.Add(center);

                block.Move(perim);

            } while (!block.Center().Equals(start));

            return newPerim;
        }


        private static List<Point> GetRedPoints()
        {
            var list = new List<Point>();
            using (var sr = Program.GetReader(FileLocation))
            {
                var line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var split = line.Split(',').Select(item => long.Parse(item)).ToArray();
                    list.Add(new Point(split[0], split[1]));
                    if (split[0] > BottomRight.X
                        || (split[0] == BottomRight.X && split[1] > BottomRight.Y))
                    {
                        BottomRight = new Point(split[0], split[1]);
                    }
                    line = sr.ReadLine();
                }
            }

            return list;
        }

        private static List<Point> GetGreenPoints(List<Point> red)
        {
            var green = new List<Point>();
            for (int i = 0; i < red.Count; i++)
            {
                var p1 = red[i];
                var p2 = red[(i + 1)%red.Count];
                if (p1.X == p2.X)
                {
                    var min = Math.Min(p1.Y, p2.Y);
                    var max = Math.Max(p1.Y, p2.Y);
                    for (long j  = min; j < max; j++)
                    {
                        green.Add(new Point(p1.X, j));
                    }
                }
                else
                {
                    var min = Math.Min(p1.X, p2.X);
                    var max = Math.Max(p1.X, p2.X);
                    for (long j = min; j < max; j++)
                    {
                        green.Add(new Point(j, p1.Y));
                    }
                }
            }

            return green;
        }
    }

    public class Point : IEquatable<Point>
    {
        public long X, Y;

        public Point(long x, long y)
        {
            X = x;
            Y = y;
        }


        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public bool Equals(Point other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Point)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }
    }

    public class Block
    {
        private List<Point> Points;
        private Point _center => Points[4];
        private int Dir = Left;
        private const int Up = 0;
        private const int Right = 1;
        private const int Down = 2;
        private const int Left = 3;

        public Block(Point p)
        {
            Points = new List<Point>();
            Points.Add(new Point(p.X - 1, p.Y - 1));
            Points.Add(new Point(p.X, p.Y - 1));
            Points.Add(new Point(p.X + 1, p.Y - 1));
            Points.Add(new Point(p.X - 1, p.Y));
            Points.Add(new Point(p.X, p.Y));
            Points.Add(new Point(p.X + 1, p.Y));
            Points.Add(new Point(p.X - 1, p.Y + 1));
            Points.Add(new Point(p.X, p.Y + 1));
            Points.Add(new Point(p.X + 1, p.Y +1));
        }

        public Point Center()
        {
            return new Point(Points[4].X, Points[4].Y);
        }

        public void Move(HashSet<Point> perim)
        {
            switch (Dir)
            {
                case Up:
                    if (!perim.Contains(Points[1]) && (perim.Contains(Points[2]) || perim.Contains(Points[5])))
                    {
                        MoveUp();
                        break;
                    }

                    if (perim.Contains(Points[1]))
                    {
                        Dir = Left;
                        MoveLeft();
                    }
                    else
                    {
                        Dir = Right;
                        MoveRight();
                    }
                    break;
                case Right:
                    if (!perim.Contains(Points[5]) && (perim.Contains(Points[8]) || perim.Contains(Points[7])))
                    {
                        MoveRight();
                        break;
                    }

                    if (perim.Contains(Points[5]))
                    {
                        Dir = Up;
                        MoveUp();
                    }
                    else
                    {
                        Dir = Down;
                        MoveDown();
                    }
                    break;
                case Down:
                    if (!perim.Contains(Points[7]) && (perim.Contains(Points[6]) || perim.Contains(Points[3])))
                    {
                        MoveDown();
                        break;
                    }

                    if (perim.Contains(Points[7]))
                    {
                        Dir = Right;
                        MoveRight();
                    }
                    else
                    {
                        Dir = Left;
                        MoveLeft();
                    }
                    break;
                case Left:
                    if (!perim.Contains(Points[3]) && (perim.Contains(Points[0]) || perim.Contains(Points[1])))
                    {
                        MoveLeft();
                        break;
                    }

                    if (perim.Contains(Points[3]))
                    {
                        Dir = Down;
                        MoveDown();
                    }
                    else
                    {
                        Dir = Up;
                        MoveUp();
                    }
                    break;
                default:
                    throw new Exception();
            }
        }

        private void MoveLeft()
        {
            Points.ForEach(p => p.X--);
        }
        private void MoveRight()
        {
            Points.ForEach(p => p.X++);
        }

        private void MoveUp()
        {
            Points.ForEach(p => p.Y--);
        }

        private void MoveDown()
        {
            Points.ForEach(p => p.Y++);
        }
    }
}