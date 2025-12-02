using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Xml.XPath;

namespace AdventOfCode2023
{
    public class Day10
    {
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        public static void Problem1()
        {
            Console.WriteLine("Day10 P1");
            var map = BuildMap(out var start);
            var length = LoopLength(map, start);
            Console.WriteLine($"Maximum Distance: {length/2}");
        }

        private static int LoopLength(List<List<char>> map, Point start)
        {
            var current = new Point(start.X, start.Y);
            var length = 0;
            var previous = new Point(-1, -1);
            do
            {
                switch (map[current.X][current.Y])
                {
                    case '|':
                        var vert = previous.X < current.X ? 1 : -1;
                            previous = current;
                        current = current.Move(vert, 0);
                        break;
                    case '-':
                        var hor = previous.Y < current.Y ? 1 : -1;
                        previous = current;
                        current = current.Move(0, hor);
                        break;
                    case 'L':
                        if (previous.X < current.X)
                        {
                            previous = current;
                            current = current.Move(0, 1);
                        }
                        else
                        {
                            previous = current;
                            current = current.Move(-1, 0);
                        }
                        break;
                    case 'J':
                        if (previous.X < current.X)
                        {
                            previous = current;
                            current = current.Move(0, -1);
                        }
                        else
                        {
                            previous = current;
                            current = current.Move(-1, 0);
                        }
                        break;
                    case '7':
                        if (previous.X > current.X)
                        {
                            previous = current;
                            current = current.Move(0, -1);
                        }
                        else
                        {
                            previous = current;
                            current = current.Move(1, 0);
                        }
                        break;
                    case 'F':
                        if (previous.X > current.X)
                        {
                            previous = current;
                            current = current.Move(0, 1);
                        }
                        else
                        {
                            previous = current;
                            current = current.Move(1, 0);
                        }
                        break;
                    case 'S':
                        previous = current;
                        if ("F7|".Contains(map[current.X - 1][current.Y]))
                        {
                            current = current.Move(-1, 0);

                        }
                        else if ("LJ|".Contains(map[current.X + 1][current.Y]))
                        {
                            current = current.Move(1, 0);

                        }
                        else if ("LF-".Contains(map[current.X][current.Y - 1]))
                        {
                            current = current.Move( 0, -1);

                        }
                        else if ("J-7".Contains(map[current.X][current.Y + 1]))
                        {
                            current = current.Move(0, 1);

                        }
                        break;
                    default:
                        throw new NotSupportedException();
                }

                length++;
            } while (current.X != start.X || current.Y != start.Y);

            return length;
        }

        public static void Problem2()
        {
            Console.WriteLine("Day10 P2");
            var map = BuildMap(out var start);
            MakeImage(map, start);
        }

        private static void MakeImage(List<List<char>> map, Point start)
        {
            var current = new Point(start.X, start.Y);
            var previous = new Point(-1, -1);
            var points = new Dictionary<Point, char>();
            do
            {
                points.Add(new Point(current.X, current.Y), map[current.X][current.Y]);

                switch (map[current.X][current.Y])
                {
                    case '|':
                        var vert = previous.X < current.X ? 1 : -1;
                        previous = current;
                        current = current.Move(vert, 0);
                        break;
                    case '-':
                        var hor = previous.Y < current.Y ? 1 : -1;
                        previous = current;
                        current = current.Move(0, hor);
                        break;
                    case 'L':
                        if (previous.X < current.X)
                        {
                            previous = current;
                            current = current.Move(0, 1);
                        }
                        else
                        {
                            previous = current;
                            current = current.Move(-1, 0);
                        }
                        break;
                    case 'J':
                        if (previous.X < current.X)
                        {
                            previous = current;
                            current = current.Move(0, -1);
                        }
                        else
                        {
                            previous = current;
                            current = current.Move(-1, 0);
                        }
                        break;
                    case '7':
                        if (previous.X > current.X)
                        {
                            previous = current;
                            current = current.Move(0, -1);
                        }
                        else
                        {
                            previous = current;
                            current = current.Move(1, 0);
                        }
                        break;
                    case 'F':
                        if (previous.X > current.X)
                        {
                            previous = current;
                            current = current.Move(0, 1);
                        }
                        else
                        {
                            previous = current;
                            current = current.Move(1, 0);
                        }
                        break;
                    case 'S':
                        previous = current;
                        if ("F7|".Contains(map[current.X - 1][current.Y]))
                        {
                            current = current.Move(-1, 0);

                        }
                        else if ("LJ|".Contains(map[current.X + 1][current.Y]))
                        {
                            current = current.Move(1, 0);

                        }
                        else if ("LF-".Contains(map[current.X][current.Y - 1]))
                        {
                            current = current.Move(0, -1);

                        }
                        else if ("J-7".Contains(map[current.X][current.Y + 1]))
                        {
                            current = current.Move(0, 1);

                        }
                        break;
                    default:
                        throw new NotSupportedException();
                }
            } while (current.X != start.X || current.Y != start.Y);

            var bitmapx3 = new Bitmap(map[0].Count * 3, map.Count * 3);
            var bitmap = new Bitmap(map[0].Count, map.Count);
            foreach (var point in points)
            {
                DrawPipe(bitmapx3, point.Key, point.Value);
                bitmap.SetPixel(point.Key.X, point.Key.Y, Color.CadetBlue);
            }
            bitmapx3.Save("mapX3.bmp");
            bitmap.Save("map.bmp");
        }

        private static void DrawPipe(Bitmap bmp, Point point, char pipe)
        {
            var x = point.X * 3;
            var y = point.Y * 3;
            switch (pipe)
            {
                case '|':
                    bmp.SetPixel(x - 1, y, Color.CadetBlue);
                    bmp.SetPixel(x , y, Color.CadetBlue);
                    bmp.SetPixel(x + 1, y, Color.CadetBlue);
                    break;
                case '-':
                    bmp.SetPixel(x, y - 1, Color.CadetBlue);
                    bmp.SetPixel(x, y, Color.CadetBlue);
                    bmp.SetPixel(x, y + 1, Color.CadetBlue);
                    break;
                case 'L':
                    bmp.SetPixel(x -1 , y, Color.CadetBlue);
                    bmp.SetPixel(x, y, Color.CadetBlue);
                    bmp.SetPixel(x, y + 1, Color.CadetBlue);
                    break;
                case 'J':
                    bmp.SetPixel(x - 1, y, Color.CadetBlue);
                    bmp.SetPixel(x, y, Color.CadetBlue);
                    bmp.SetPixel(x, y - 1, Color.CadetBlue);
                    break;
                case '7':
                    bmp.SetPixel(x, y - 1, Color.CadetBlue);
                    bmp.SetPixel(x, y, Color.CadetBlue);
                    bmp.SetPixel(x + 1, y, Color.CadetBlue);
                    break;
                case 'F':
                    bmp.SetPixel(x, y + 1, Color.CadetBlue);
                    bmp.SetPixel(x, y, Color.CadetBlue);
                    bmp.SetPixel(x + 1, y, Color.CadetBlue);
                    break;
                case 'S':
                    bmp.SetPixel(x - 1 , y, Color.CadetBlue);
                    bmp.SetPixel(x, y, Color.CadetBlue);
                    bmp.SetPixel(x + 1, y, Color.CadetBlue);
                    bmp.SetPixel(x, y - 1, Color.CadetBlue);
                    bmp.SetPixel(x, y + 1, Color.CadetBlue);
                    break;
            }

        }

        private static List<List<char>> BuildMap(out Point start)
        {
            using (var stream = new StreamReader(new FileStream("Day10.txt", FileMode.Open, FileAccess.Read)))
            {
                start = new Point(-1, -1);
                var line = stream.ReadLine();
                var map = new List<List<char>>();
                while (line != null)
                {
                    map.Add(line.ToCharArray().ToList());
                    if (line.Contains("S"))
                    {
                        start = new Point(map.Count - 1, line.IndexOf("S"));
                    }
                    line = stream.ReadLine();
                }

                return map;
            }
        }

        private class Point
        {
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public Point Move(int x, int y)
            {
                return new Point(X + x, Y + y);
            }
        }
    }
}