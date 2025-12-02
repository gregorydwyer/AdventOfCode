using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace AdventOfCode2023
{
    public class Day18
    {
        private const string FileName = "Day18.txt";

        private const int Up = 0;
        private const int Down = 1;
        private const int Left = 2;
        private const int Right = 3;

        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        private static void Problem1()
        {
            Console.WriteLine("Day18 P1");
            var input = File.ReadAllLines(FileName);
            Point current = null;
            var edges = new List<Point>();
            var edgeCount = 0L;
            long final = 0L;
            foreach (var line in input)
            {
                var parse = Parse(line);
                if (current == null)
                {
                    current = new Point(0, 0, Color.Black);
                }
                var next = Move2(current, parse.Direction, (long)parse.Steps);
                final += current.X * next.Y - next.X * current.Y;
                current = next;
                edges.Add(current);
                edgeCount += parse.Steps;
            }

            final = Math.Abs(final / 2);
            final += (edgeCount / 2) + 1;
            Console.WriteLine($"Total: {final}");
            return;

            // This draws a nice picture.
            //var input = File.ReadAllLines(FileName);
            //var edges = new HashSet<Point>();
            //Point current = null;
            //foreach (var line in input)
            //{
            //    var parse = Parse(line);
            //    if(current == null)
            //    {
            //        current = new Point(0, 0, parse.Color);
            //        edges.Add(current);
            //    }

            //    for (int i = 0; i < parse.Steps; i++)
            //    {
            //        current = Move(current, parse.Direction, parse.Color);
            //        edges.Add(current);
            //    }
            //}

            //var xMin = (int)edges.Min(p => p.X);
            //var xMax = (int)edges.Max(p => p.X);
            //var xOffset = (int)Math.Abs(xMin);
            //var yMin = (int)edges.Min(p => p.Y);
            //var yMax = (int)edges.Max(p => p.Y);
            //var yOffset = (int)Math.Abs(yMin);

            //var bitmap = new Bitmap( yMax + yOffset + 1, xMax + xOffset + 1);
            //foreach (var point in edges)
            //{
            //    var imageY = point.X + xOffset;
            //    var imageX = point.Y + yOffset;
            //    bitmap.SetPixel( (int)imageX, (int)imageY, point.Color);
            //}
            //bitmap.Save("Day18Pt1.bmp");
            //Console.WriteLine("File Saved");
        }

        private static Point Move(Point p, int dir, Color color)
        {
            switch (dir)
            {
                case Up:
                    return new Point(p.X - 1, p.Y, color);
                case Down:
                    return new Point(p.X + 1, p.Y, color);
                case Left:
                    return new Point(p.X, p.Y - 1, color);
                case Right:
                    return new Point(p.X, p.Y + 1, color);
                default:
                    throw new NotSupportedException();
            }
        }

        private static Point Move2(Point p, int dir, long steps)
        {
            switch (dir)
            {
                case Up:
                    return new Point(p.X - steps, p.Y, Color.DarkSalmon);
                case Down:
                    return new Point(p.X + steps, p.Y, Color.DarkSalmon);
                case Left:
                    return new Point(p.X, p.Y - steps, Color.DarkSalmon);
                case Right:
                    return new Point(p.X, p.Y + steps, Color.DarkSalmon);
                default:
                    throw new NotSupportedException();
            }
        }

        private static (Color Color,int Direction, int Steps) Parse(string str)
        {
            var parts = str.Split(' ');
            var dir = -1;
            var steps = int.Parse(parts[1]);
            switch (parts[0])
            {
                case "U":
                    dir = Up;
                    break;
                case "D":
                    dir = Down;
                    break;
                case "L":
                    dir = Left;
                    break;
                case "R":
                    dir = Right;
                    break;
                default:
                    throw new NotSupportedException();
            }
            var color = System.Drawing.ColorTranslator.FromHtml(parts[2].Trim(new char[]{'(',')'}));
            return (color, dir, steps);
        }

        private static (int Direction, long Steps) Parse2(string str)
        {
            const int R = 0;
            const int D = 1;
            const int L = 2;
            const int U = 3;
            var parts = str.Split(' ');
            var dir = -1;
            var temp = parts[2].Substring(2, 5);
            var steps = Convert.ToInt64(parts[2].Substring(2, 5), 16);
            switch (int.Parse(parts[2][7].ToString()))
            {
                case U:
                    dir = Up;
                    break;
                case D:
                    dir = Down;
                    break;
                case L:
                    dir = Left;
                    break;
                case R:
                    dir = Right;
                    break;
                default:
                    throw new NotSupportedException();
            }

            return (dir, steps);
        }

        private static void Problem2()
        {
            Console.WriteLine("Day18 P2");
            var input = File.ReadAllLines(FileName);
            Point current = null;
            var edges = new List<Point>();
            var edgeCount = 0L;
            long final = 0L;
            foreach (var line in input)
            {
                var parse = Parse2(line);
                if (current == null)
                {
                    current = new Point(0, 0, Color.Black);
                }
                var next = Move2(current, parse.Direction,(long) parse.Steps);
                final += current.X * next.Y - next.X * current.Y;
                current = next;
                edges.Add(current);
                edgeCount += parse.Steps;
            }

            final = Math.Abs(final / 2);
            final += (edgeCount / 2) + 1;
            Console.WriteLine($"Total: {final}");
        }

        internal class Point : IEquatable<Point>
        {
            public long X;
            public long Y;
            public Color Color;

            public Point(long x, long y, Color color)
            {
                X = x;
                Y = y;
                Color = color;
            }

            public bool Equals(Point other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return X == other.X && Y == other.Y && Color.Equals(other.Color);
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
                    var hashCode = (int) X;
                    hashCode = (hashCode * 397) ^ (int) Y;
                    hashCode = (hashCode * 397) ^ Color.GetHashCode();
                    return hashCode;
                }
            }
        }
    }
}