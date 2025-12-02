using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2023
{
    public static class Day17
    {
        private const int Up = 0;
        private const int Down = 1;
        private const int Left = 2;
        private const int Right = 3;


        private const string FileName = "Day17.txt";
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        private static void Problem1()
        {
            Console.WriteLine("Day17 P1");
            var map = BuildMap();
            var total = BuildValueMap(map);
            Console.WriteLine($"Total loss: {total}");
        }

        private static long BuildValueMap(Point[,] map, bool part2 = false)
        {
            var start = map[0,0];
            var temp1 = map.Length - 1;
            var end = map[map.GetUpperBound(0),map.GetUpperBound(0)];
            start.MinCost = 0;
            var list = new List<Point>();
            var dict = new Dictionary<Point, long>();
            dict.Add(start, 0);
            var startingPoints = Move(map, start, dict, new[] {Down, Right}, part2);
            foreach (var point in startingPoints)
            {
                list.Add(point);
            }
            while (list.Count > 0)
            {
                var current = list.First();
                list.RemoveAt(0);
                var newPoints = UpdatePoints(map, dict, current, part2);
                foreach (var point in newPoints)
                {
                    if (list.Contains(point))
                    {
                        if (list.First(p => p.Equals(point)).MinCost > point.MinCost)
                        {
                            list.Remove(point);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    list.Add(point);
                }
                list.Sort();
            }

            var temp = dict.Where(kvp => kvp.Key.X == map.GetUpperBound(0) && kvp.Key.Y == map.GetUpperBound(0)).ToList();
            var final =  temp.Min(p => p.Value);
            return final;
        }

        private static List<Point> UpdatePoints(Point[,] map, Dictionary<Point, long> dict, Point current, bool part2)
        {
            var newPoints = new List<Point>();
            if (current.Dir == Down || current.Dir == Up)
            {
                newPoints = Move(map, current, dict, new[] {Left, Right}, part2);
            }
            else
            {
                newPoints = Move(map, current, dict, new[] { Up, Down }, part2);
            }

            return newPoints;
        }

        private static List<Point> Move(Point[,] map, Point current, Dictionary<Point, long> dict, int[] dirs, bool part2)
        {
            var newPoints = new List<Point>();
            var steps = part2 ? 10 : 3;
            foreach (var dir in dirs)
            {
                var min = current.MinCost;
                switch (dir)
                {
                    case Up:
                        for (int i = 1; i <= steps; i++)
                        {
                            if (current.X - i >= 0)
                            {
                                var point = new Point(map[current.X - i, current.Y], Up, i);
                                min += point.Value;
                                point.MinCost = min;
                                if (!dict.ContainsKey(point))
                                {
                                    newPoints.Add(point);
                                    dict.Add(point, min);
                                }
                                else if(dict[point] > point.MinCost)
                                {

                                    dict[point] =  point.MinCost;
                                    newPoints.Add(point);
                                }
                            }
                        }

                        break;
                    case Down:
                        for (int i = 1; i <= steps; i++)
                        {
                            if (current.X + i <= map.GetUpperBound(0))
                            {
                                var point = new Point(map[current.X + i, current.Y], Down, i);
                                min += point.Value;
                                point.MinCost = min;
                                if (!dict.ContainsKey(point))
                                {
                                    newPoints.Add(point);
                                    dict.Add(point, point.MinCost);
                                }
                                else if (dict[point] > point.MinCost)
                                {

                                    dict[point] = point.MinCost;
                                    newPoints.Add(point);
                                }
                            }
                        }

                        break;
                    case Left:
                        for (int i = 1; i <= steps; i++)
                        {
                            if (current.Y - i >= 0)
                            {
                                var point = new Point(map[current.X, current.Y - i], Left, i);
                                min += point.Value;
                                point.MinCost = min;
                                if (!dict.ContainsKey(point))
                                {
                                    newPoints.Add(point);
                                    dict.Add(point, point.MinCost);
                                }
                                else if (dict[point] > point.MinCost)
                                {

                                    dict[point] = point.MinCost;
                                    newPoints.Add(point);
                                }
                            }
                        }

                        break;
                    case Right:
                        for (int i = 1; i <= steps; i++)
                        {
                            if (current.Y + i <= map.GetUpperBound(0))
                            {
                                var point = new Point(map[current.X, current.Y + i], Left, i);
                                min += point.Value;
                                point.MinCost = min;
                                if (!dict.ContainsKey(point))
                                {
                                    newPoints.Add(point);
                                    dict.Add(point, point.MinCost);
                                }
                                else if (dict[point] > point.MinCost)
                                {

                                    dict[point] = point.MinCost;
                                    newPoints.Add(point);
                                }
                            }
                        }

                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            if (part2)
            {
                return newPoints.Where(p => p.StepsInDir >= 4).ToList();
            }

            return newPoints;
        }


        private static Point[,] BuildMap()
        {
            var input = File.ReadAllLines(FileName);
            var map = new Point[input.Length,input[0].Length];
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    map[i,j] = new Point(i,j,int.Parse(input[i][j].ToString()));
                }
            }

            return map;
        }

        private static void Problem2()
        {
            Console.WriteLine("Day17 P2");
            var map = BuildMap();
            var total = BuildValueMap(map, true);
            Console.WriteLine($"Total loss: {total}");
        }


        internal class Point : IEquatable<Point>, IComparable<Point>
        {
            public int X;
            public int Y;
            public int Value;
            public long MinCost = long.MaxValue;
            public int StepsInDir;
            public int Dir;

            public Point(int x, int y, int value)
            {
                X = x;
                Y = y;
                Value = value;
            }

            public Point(Point p, int dir, int steps = 1)
            {
                X = p.X;
                Y = p.Y;
                Value = p.Value;
                Dir = dir;
                StepsInDir = steps;
            }

            public override string ToString()
            {
                return $"({X},{Y}), {MinCost}";
            }

            public bool Equals(Point other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return X == other.X && Y == other.Y && StepsInDir == other.StepsInDir && Dir == other.Dir;
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
                    hashCode = (hashCode * 397) ^ StepsInDir;
                    hashCode = (hashCode * 397) ^ Dir;
                    return hashCode;
                }
            }

            public int CompareTo(Point other)
            {
                if (ReferenceEquals(this, other)) return 0;
                if (ReferenceEquals(null, other)) return 1;
                return MinCost.CompareTo(other.MinCost);
            }
        }

    }
}