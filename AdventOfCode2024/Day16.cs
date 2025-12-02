using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode2024
{
    public class Day16
    {
        private const string Day = "Day16";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";
        private static int Width, Height;
        private static Location Start;
        private static Location End;
        private static Graph<Location> Map;

        public static void Run()
        {
            Program.WriteTitle("--- Day 16: Reindeer Maze ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            Map = BuildMap();
            var visited = new HashSet<Location>();
            var fringe = new List<Location>();

            fringe.Add(Start);
            var isAtEnd = false;
            while(fringe.Count > 0)
            {
                var current = fringe[0];
                visited.Add(current);
                fringe.RemoveAt(0);

                var x = current.X;
                var y = current.Y;
                // N
                if (!Map[x, y - 1].IsWall && !visited.Contains(Map[x,y - 1]))
                {
                    isAtEnd = isAtEnd || UpdateCost(current, Map[x, y - 1], Direction.N, fringe);
                }
                // E
                if (!Map[x + 1, y].IsWall && !visited.Contains(Map[x + 1, y]))
                {
                    isAtEnd = isAtEnd || UpdateCost(current, Map[x + 1, y], Direction.E, fringe);
                }
                // S
                if (!Map[x, y + 1].IsWall && !visited.Contains(Map[x, y + 1]))
                {
                    isAtEnd = isAtEnd || UpdateCost(current, Map[x, y + 1], Direction.S, fringe);
                }
                // W
                if (!Map[x - 1, y].IsWall && !visited.Contains(Map[x - 1, y]))
                {
                    isAtEnd = isAtEnd || UpdateCost(current, Map[x - 1, y], Direction.W, fringe);
                }

                fringe.Sort();
            }
            Program.WriteOutput("Cheapest Path: " + End.Cost);

        }

        private static bool UpdateCost(Location current, Location next, Direction dir, List<Location> fringe)
        {
            var newCost = GetCost(current, dir);

            if (newCost < next.Cost)
            {
                next.Dir = dir;
                next.Cost = newCost;
            }
            fringe.Add(next);

            return next.X == End.X && next.Y == End.Y;
        }

        private static long GetCost(Location c, Direction d)
        {
            return c.Cost + GetSingleMoveCost(c.Dir, d);
        }

        private static long GetSingleMoveCost(Direction c, Direction n)
        {
            return 1 + TurnsToDirection(c, n) * 1000;
        }

        private static int TurnsToDirection(Direction start, Direction end)
        {
            if (start == end)
            {
                return 0;
            }
            if(Math.Abs(start - end) == 2)
            {
                return 2;
            }

            return 1;
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var visited = new HashSet<Location>();
            var fringe = new List<Location>();

            fringe.Add(End);
            Location prev = End;
            while (fringe.Count > 0)
            {
                var current = fringe[0];
                visited.Add(current);
                fringe.RemoveAt(0);

                var x = current.X;
                var y = current.Y;

                // N
                if (!Map[x, y - 1].IsWall && !visited.Contains(Map[x, y - 1]))
                {
                    if(current.Cost == GetCost(Map[x, y - 1], Direction.S) ||
                        (prev.Dir == Direction.S && prev.Cost - 2 == Map[x, y - 1].Cost))
                    {
                        fringe.Add(Map[x, y - 1]);
                    }
                }
                // E
                if (!Map[x + 1, y].IsWall && !visited.Contains(Map[x + 1, y]))
                {
                    if (current.Cost == GetCost(Map[x + 1, y], Direction.W) ||
                        (prev.Dir == Direction.W && prev.Cost - 2 == Map[x + 1, y].Cost))
                    {
                        fringe.Add(Map[x + 1, y]);
                    }
                }
                // S
                if (!Map[x, y + 1].IsWall && !visited.Contains(Map[x, y + 1]))
                {
                    if (current.Cost == GetCost(Map[x, y + 1], Direction.N) ||
                        (prev.Dir == Direction.N && prev.Cost - 2 == Map[x, y + 1].Cost))
                    {
                        fringe.Add(Map[x, y + 1]);
                    }
                }
                // W
                if (!Map[x - 1, y].IsWall && !visited.Contains(Map[x - 1, y]))
                {
                    if (current.Cost == GetCost(Map[x - 1, y], Direction.E) ||
                        (prev.Dir == Direction.E && prev.Cost - 2 == Map[x - 1, y].Cost))
                    {
                        fringe.Add(Map[x - 1, y]);
                    }
                }

                prev = current;
                fringe.Sort();
            }

            Program.WriteOutput("Seat Options: " + visited.Count);

        }

        private static Graph<Location> BuildMap()
        {
            using (var reader = Program.GetReader(FileLocation))
            {
                var map = new Graph<Location>();
                var currentLine = reader.ReadLine();
                Width = currentLine.Length;
                var y = -1;
                while (currentLine != null)
                {
                    y++;
                    for (var x = 0; x < currentLine.Length; x++)
                    {
                        var c = currentLine[x];
                        map[x, y] = new Location(x, y);
                        if (c == '#')
                        {
                            map[x, y].IsWall = true;
                        }
                        else if (c == 'S')
                        {
                            Start = map[x, y];
                            Start.Cost = 0;
                            Start.Dir = Direction.E;
                        }
                        else if (c == 'E')
                        {
                            End = map[x, y];
                        }
                    }

                    currentLine = reader.ReadLine();
                }
                Height = y + 1;
                return map;
            }
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Graph<T>
    {
        private readonly Dictionary<(int,int), T> Map = new Dictionary<(int, int), T>();
        public int Height,Width;


        public T this[int x, int y]
        {
            get
            {
                if (Map.TryGetValue((x, y), out var loc))
                {
                    return loc;
                }

                throw new ArgumentOutOfRangeException($"No value available for ({x},{y}).");
            }
            set
            {
                if (Map.ContainsKey((x, y)))
                {
                    Map[(x, y)] = value;
                }
                else
                {
                    Map.Add((x,y), value);
                }
            }
        }

        public bool Contains(int x, int y)
        {
            return Map.ContainsKey((x, y));
        }

        public Dictionary<(int X,int Y),T> GetSource()
        {
            return Map;
        }

    }

    public class Location : Point, IComparable<Location>
    {
        public bool IsWall;
        public long Cost = long.MaxValue;

        public Location(int x, int y, bool isWall = false) : base(x, y)
        {
            IsWall = isWall;
        }

        public int CompareTo(Location other)
        {
            if (IsWall && other.IsWall)
            {
                return 0;
            }
            if (IsWall)
            {
                return 1;
            }
            if (Cost == other.Cost)
            {
                return 0;
            }
            return Cost < other.Cost ? -1 : 1;
        }
    }
}