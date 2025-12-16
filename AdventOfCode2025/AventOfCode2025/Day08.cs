using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024
{
    public class Day08
    {
        private const string Day = "Day08";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";

        public static void Run()
        {
            Program.WriteTitle("--- Day 8: Playground ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var points = BuildPoints();
            var circuits = MakeConnections(points, 1000, out var _);
            var ordered = circuits.OrderByDescending(set => set.Count).ToList();
            var total = 1L;
            for (int i = 0; i < 3; i++)
            {
                total *= ordered[i].Count;
            }
            Program.WriteOutput("Product of 3 largest circuits: " + total);
        }

        private static List<HashSet<Point3D>> MakeConnections(List<Point3D> points, int maxConnections, out (Point3D, Point3D) final)
        {
            var distances = PairsSortedByDistance(points);
            final = (new Point3D(0,0,0), new Point3D(0, 0, 0));
            var circuits = new List<HashSet<Point3D>>();
            var used = new HashSet<Point3D>();
            var index = 0;
            for (int count = 0; count < maxConnections; count++)
            {
                var (a, b)  = distances[index];
                if (used.Contains(a))
                {
                    var hashA = circuits.First(set => set.Contains(a));
                    if (hashA.Contains(b))
                    {
                        index++;
                        continue;
                    }

                    if (used.Contains(b))
                    {
                        var hashB = circuits.First(set => set.Contains(b));
                        foreach (var point in hashB)
                        {
                            hashA.Add(point);
                        }

                        circuits.Remove(hashB);
                    }
                    else
                    {
                        hashA.Add(b);
                        used.Add(b);
                    }
                }
                else if (used.Contains(b))
                {
                    var hashB = circuits.First(set => set.Contains(b));
                    hashB.Add(a);
                    used.Add(a);
                }
                else
                {
                    circuits.Add(new HashSet<Point3D>() { a, b });
                    used.Add(a);
                    used.Add(b);
                }

                if (used.Count == points.Count && circuits.Count == 1)
                {
                    final = (a, b);
                    return circuits;
                }
                index++;
            }

            return circuits;
        }

        private static List<(Point3D, Point3D)> PairsSortedByDistance(List<Point3D> points)
        {
            var distances = new List<(double Dist, (Point3D, Point3D) Points)>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    distances.Add((points[i].Distance(points[j]), (points[i], points[j])));
                }
            }

            return distances.OrderBy(item => item.Dist).Select(item => item.Points).ToList();
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var points = BuildPoints();
            var circuits = MakeConnections(points, int.MaxValue, out var final);
            var product = (long) final.Item1.X * final.Item2.X;
            Program.WriteOutput("Distance from wall: " + product);
        }

        public static List<Point3D> BuildPoints()
        {
            var list = new List<Point3D>();
            using (var sr = Program.GetReader(FileLocation))
            {
                var line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var nums = line.Split(',').Select(num => int.Parse(num)).ToArray();
                    list.Add(new Point3D(nums[0],nums[1],nums[2]));
                    line = sr.ReadLine();
                }
            }

            return list;
        }

    }

    public class Point3D : IEquatable<Point3D>
    {
        public int X, Y, Z;

        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double Distance(Point3D other)
        {
            var x = Math.Pow(X - other.X, 2);
            var y = Math.Pow(Y - other.Y, 2);
            var z = Math.Pow(Z - other.Z, 2);
            return Math.Sqrt(x + y + z);
        }

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }

        public bool Equals(Point3D other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Point3D)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X;
                hashCode = (hashCode * 397) ^ Y;
                hashCode = (hashCode * 397) ^ Z;
                return hashCode;
            }
        }
    }
}