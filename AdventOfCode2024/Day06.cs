using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024
{
    public class Day06
    {
        private const string Day = "Day06";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";
        private static Point Guard = new Point(0, 0);
        private const char Block = '#';
        private const char Visited = 'X';
        private const char GuardStart = '^';
        private static  Point StartingPoint;
        private static List<Point> OriginalGuardPath = new List<Point>();
        private static HashSet<Point> LoopLocations = new HashSet<Point>();
        private static HashSet<Point> Blocks = new HashSet<Point>();
        private static int MapWidth, MapHeight;

        public static void Run()
        {
            Program.WriteTitle("--- Day 6: Guard Gallivant ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var map = BuildMap();
            MapWidth = map[0].Length;
            MapHeight = map.Length;
            var uniquePoints = 0;
            StartingPoint = new Point(Guard);
            while (MapContainsPos(map))
            {
                if (SimpleMovePos(map))
                {
                    uniquePoints++;
                }
            }

            Program.WriteOutput("Unique Points: " + uniquePoints);
        }

        private static bool MapContainsPos(char[][] map)
        {
            return Guard.X >= 0 &&
                   Guard.X < map[0].Length &&
                   Guard.Y >= 0 &&
                   Guard.Y < map.Length;
        }

        private static bool SimpleMovePos(char[][] map)
        {
            OriginalGuardPath.Add(new Point(Guard));
            switch (Guard.Dir)
            {
                case Direction.N:
                    if (Guard.Y - 1 < 0)
                    {
                        Guard.Y--;
                        return false;
                    }
                    if (map[Guard.Y - 1][Guard.X] != Block)
                    {
                        Guard.Y--;
                        if (map[Guard.Y][Guard.X] != Visited)
                        {
                            map[Guard.Y][Guard.X] = Visited;
                            return true;
                        }

                        return false;
                    }
                    break;
                case Direction.E:
                    if (Guard.X + 1 >= map[0].Length)
                    {
                        Guard.X++;
                        return false;
                    }
                    if (map[Guard.Y][Guard.X + 1] != Block)
                    {
                        Guard.X++;
                        if (map[Guard.Y][Guard.X] != Visited)
                        {
                            map[Guard.Y][Guard.X] = Visited;
                            return true;
                        }

                        return false;
                    }
                    break;
                case Direction.S:
                    if (Guard.Y + 1 >= map.Length)
                    {
                        Guard.Y++;
                        return false;
                    }
                    if (map[Guard.Y + 1][Guard.X] != Block)
                    {
                        Guard.Y++;
                        if (map[Guard.Y][Guard.X] != Visited)
                        {
                            map[Guard.Y][Guard.X] = Visited;
                            return true;
                        }

                        return false;
                    }
                    break;
                case Direction.W:
                    if (Guard.X - 1 < 0)
                    {
                        Guard.X--;
                        return false;
                    }
                    if (map[Guard.Y][Guard.X - 1] != Block)
                    {
                        Guard.X--;
                        if (map[Guard.Y][Guard.X] != Visited)
                        {
                            map[Guard.Y][Guard.X] = Visited;
                            return true;
                        }

                        return false;
                    }
                    break;
            }

            Turn();
            return SimpleMovePos(map);
        }

        private static bool? LoopCheckMovePos(HashSet<Point> oldLocs, HashSet<Point> newLocs)
        {
            switch (Guard.Dir)
            {
                case Direction.N:
                    if (Guard.Y - 1 < 0)
                    {
                        return false;
                    }
                    if (!Blocks.Contains(new Point(Guard.X, Guard.Y - 1)))
                    {
                        Guard.Y--;
                        return null;
                    }
                    break;
                case Direction.E:
                    if (Guard.X + 1 >= MapWidth)
                    {
                        return false;
                    }
                    if (!Blocks.Contains(new Point(Guard.X + 1, Guard.Y)))
                    {
                        Guard.X++;
                        return null;
                    }
                    break;
                case Direction.S:
                    if (Guard.Y + 1 >= MapHeight)
                    {
                        return false;
                    }
                    if (!Blocks.Contains(new Point(Guard.X, Guard.Y + 1)))
                    {
                        Guard.Y++;
                        return null;
                    }
                    break;
                case Direction.W:
                    if (Guard.X - 1 < 0)
                    {
                        return false;
                    }
                    if (!Blocks.Contains(new Point(Guard.X - 1, Guard.Y)))
                    {
                        Guard.X--;
                        return null;
                    }
                    break;
            }

            Turn();
            if (oldLocs.Contains(Guard) || newLocs.Contains(Guard))
            {
                // we've been here before. must be a loop.
                return true;
            }
            newLocs.Add(Guard);
            return null;
        }

        private static void Turn()
        {
            Guard.Dir = NextDirection(Guard.Dir);
        }

        private static Direction NextDirection(Direction d)
        {
            return (Direction)(((int)d + 1) % 4);
        }

        private static char[][] BuildMap()
        {
            using (var reader = Program.GetReader(FileLocation))
            {
                var mapList = new List<string>();
                var currentLine = reader.ReadLine();
                while (currentLine != null)
                {
                    mapList.Add(currentLine);
                    var index = currentLine.IndexOf(GuardStart.ToString(), StringComparison.InvariantCulture);
                    if (index > 0)
                    {
                        Guard.X = index;
                        Guard.Y = mapList.Count - 1;
                    }

                    for (var i = 0; i < currentLine.Length; i++)
                    {
                        var loc = currentLine[i];
                        if (loc == Block)
                        {
                            Blocks.Add(new Point(i, mapList.Count - 1));
                        }
                    }

                    currentLine = reader.ReadLine();
                }

                return mapList.Select(str => str.ToCharArray()).ToArray();
            }
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var nextBlock = new Point(OriginalGuardPath[OriginalGuardPath.Count - 1]);
            var loopLocations = new HashSet<Point>();
            OriginalGuardPath.RemoveAt(OriginalGuardPath.Count - 1);
            var originalPathLocations = OriginalGuardPath.ToHashSet();
            var locationCount = OriginalGuardPath.Count;
            for (int i = locationCount - 1; i > 0; i--)
            {
                Guard = new Point(OriginalGuardPath[i]);
                originalPathLocations.Remove(Guard);
                originalPathLocations.Remove(nextBlock);
                var currentBlock = new Point(nextBlock.X, nextBlock.Y);
                nextBlock = new Point(OriginalGuardPath[i]);
                OriginalGuardPath.RemoveAt(i);
                if (OriginalGuardPath.Any(p => p.X == currentBlock.X && currentBlock.Y == p.Y))
                {
                    continue;
                }
                Blocks.Add(currentBlock);
                bool? result = null;
                var newLocations = new HashSet<Point>();

                while (result == null)
                {
                    result = LoopCheckMovePos(originalPathLocations, newLocations);
                }

                if (result.Value)
                {
                    loopLocations.Add(currentBlock);
                }

                Blocks.Remove(currentBlock);
            }
            Program.WriteOutput("Loop Locations: " + loopLocations.Count);
        }
    }

    public enum Direction
    {
        N,
        E,
        S,
        W,
    }

    public class Point : IEquatable<Point>
    {
        public int X;
        public int Y;
        public Direction Dir;

        public Point(int x, int y, Direction d = Direction.N)
        {
            X = x;
            Y = y;
            Dir = d;
        }

        public Point(Point p)
        {
            X = p.X;
            Y = p.Y;
            Dir = p.Dir;
        }

        public bool Equals(Point other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y && Dir == other.Dir;
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
                var hashCode = X;
                hashCode = (hashCode * 397) ^ Y;
                hashCode = (hashCode * 397) ^ (int)Dir;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"({X},{Y}) {Dir.ToString()}";
        }
    }
}