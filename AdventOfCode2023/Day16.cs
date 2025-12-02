using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode2023
{
    public static class Day16
    {
        private const char Vert = '|';
        private const char Horiz = '-';
        private const char MirrorRight = '\\';
        private const char MirrorLeft = '/';
        private const char Empty = '.';

        private const int Up = 0;
        private const int Down = 1;
        private const int Left = 2;
        private const int Right = 3;


        private const string FileName = "Day16.txt";
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        private static void Problem1()
        {
            Console.WriteLine("Day16 P1");
            var map = BuildMap();
            var total = TraverseMap(map, 0, 0, Right);
            Console.WriteLine($"Total energized: {total}");
        }

        private static int TraverseMap(char[][] map, int x, int y, int dir)
        {
            var visited = new HashSet<Point>();
            var queue = new Queue<Point>();
            var start = new Point(x, y, dir);
            queue.Enqueue(start);
            visited.Add(start);
            while (queue.Count != 0)
            {
                var currentPoint = queue.Dequeue();
                TraverseMap(map, visited, queue, currentPoint);
            }

            var countMap = new char[map.Length, map[0].Length];
            foreach (var p in visited)
            {
                countMap[p.X,p.Y] = '#';
            }

            var count = 0;
            foreach (var c in countMap)
            {
                count +=  c == '#' ? 1 : 0;
            }
            return count;
        }

        private static void TraverseMap(char[][] map, HashSet<Point> visited, Queue<Point> queue, Point point)
        {
            var newPoints = Split(map, point);
            foreach (var p in newPoints)
            {
                if (!visited.Contains(p))
                {
                    visited.Add(p);
                    queue.Enqueue(p);
                }
            }
        }

        private static List<Point> Split(char[][] map, Point point)
        {
            var list = new List<Point>();
            switch (map[point.X][point.Y])
            {
                case Vert:
                    if (point.Dir == Up || point.Dir == Down)
                    {
                        list.Add(Move(map, point));
                    }
                    else
                    {
                        list.Add(Move(map, point, Up));
                        list.Add(Move(map, point, Down));
                    }
                    break;
                case Horiz:
                    if (point.Dir == Left || point.Dir == Right)
                    {
                        list.Add(Move(map, point));
                    }
                    else
                    {
                        list.Add(Move(map, point, Left));
                        list.Add(Move(map, point, Right));
                    }
                    break;
                case MirrorLeft:
                    switch (point.Dir)
                    {
                        case Up:
                            list.Add(Move(map, point, Right));
                            break;
                        case Down:
                            list.Add(Move(map, point, Left));
                            break;
                        case Left:
                            list.Add(Move(map, point, Down));
                            break;
                        case Right:
                            list.Add(Move(map, point, Up));
                            break;
                    }
                    break;
                case MirrorRight:
                    switch (point.Dir)
                    {
                        case Up:
                            list.Add(Move(map, point, Left));
                            break;
                        case Down:
                            list.Add(Move(map, point, Right));
                            break;
                        case Left:
                            list.Add(Move(map, point, Up));
                            break;
                        case Right:
                            list.Add(Move(map, point, Down));
                            break;
                    }
                    break;
                default:
                    list.Add(Move(map, point));
                    break;
            }

            return list.Where(p => p != null).ToList();
        }

        private static Point Move(char[][] map, Point point, int dir = -1)
        {
            var direction = dir < 0 ? point.Dir : dir;
            switch (direction)
            {
                case Up:
                    if (point.X > 0)
                    {
                        return new Point(point.X - 1, point.Y, direction);
                    }
                    break;
                case Down:
                    if (point.X < map.Length - 1)
                    {
                        return new Point(point.X + 1, point.Y, direction);
                    }
                    break;
                case Left:
                    if (point.Y > 0)
                    {
                        return new Point(point.X, point.Y - 1, direction);
                    }
                    break;
                case Right:
                    if (point.Y < map[point.X].Length - 1)
                    {
                        return new Point(point.X, point.Y + 1, direction);
                    }
                    break;
                default:
                    throw new NotSupportedException();
            }

            return null;
        }


        private static char[][] BuildMap()
        {
            var input = File.ReadAllLines(FileName);
            return input.Select(s => s.ToCharArray()).ToArray();
        }

        private static void Problem2()
        {
            Console.WriteLine("Day16 P2");
            var map = BuildMap();
            var max = 0;
            for (int i = 0; i < map.Length; i++)
            {
                var right = TraverseMap(map, i, 0, Right);
                var left = TraverseMap(map, i, map[i].Length - 1, Left);
                max = Math.Max(max, Math.Max(right, left));
            }

            for (int i = 0; i < map[0].Length; i++)
            {
                var down = TraverseMap(map, 0, i, Down);
                var up = TraverseMap(map, map.Length - 1, i, Up);
                max = Math.Max(max, Math.Max(down, up));
            }
            Console.WriteLine($"Total energized: {max}");
        }

        internal class Point : IEquatable<Point>
        {
            public int X;
            public int Y;
            public int Dir;

            public Point(int x, int y, int dir)
            {
                X = x;
                Y = y;
                Dir = dir;
            }

            public bool Equals(Point other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return X == other.X && Y == other.Y && Dir == other.Dir;
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
                    var hashCode = X;
                    hashCode = (hashCode * 397) ^ Y;
                    hashCode = (hashCode * 397) ^ Dir;
                    return hashCode;
                }
            }

            public override string ToString()
            {
                return $"({X},{Y}), {Dir}";
            }
        }

    }
}