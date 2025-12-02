using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2024
{
    public class Day18
    {
        private const string Day = "Day18";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";

        public static void Run()
        {
            Program.WriteTitle("--- Day 18: RAM Run ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            Graph<Memory> map;
            using (var reader = Program.GetReader(FileLocation))
            {
                map = BuildMap(reader);
            }
            CountSteps(map);
            Program.WriteOutput("Steps to Exit: " + map[map.Width - 1, map.Height - 1].Steps);
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            Graph<Memory> map;
            Queue<Memory> corruptions = new Queue<Memory>();
            using (var reader = Program.GetReader(FileLocation))
            {
                map = BuildMap(reader);
                var currentLine = reader.ReadLine();
                while (currentLine != null)
                {
                    var indices = currentLine.Split(',').Select(int.Parse).ToArray();
                    var x = indices[0];
                    var y = indices[1];
                    corruptions.Enqueue(new Memory(x, y, true));
                    currentLine = reader.ReadLine();
                }
            }

            Memory blockingCorruption = null;
            var nodes = CountSteps(map, true);
            while (corruptions.Count > 0)
            {
                var current = corruptions.Dequeue();
                map[current.X, current.Y].IsCorrupt = true;
                if (!nodes.Contains(current))
                {
                    continue;
                }
                ResetSteps(map);
                nodes = CountSteps(map, true);
                if (map[map.Width - 1, map.Height - 1].Steps == int.MaxValue)
                {
                    blockingCorruption = current;
                    break;
                }
            }

            if (blockingCorruption == null)
            {
                throw new Exception("The path was never blocked.");
            }
            Program.WriteOutput($"Blocking Corruption: ({blockingCorruption.X},{blockingCorruption.Y})");

        }

        private static void ResetSteps(Graph<Memory> map)
        {
            foreach (var kvp in map.GetSource())
            {
                kvp.Value.Steps = int.MaxValue;
            }
        }

        private static HashSet<Memory> CountSteps(Graph<Memory> map, bool outputPath = false)
        {
            var visited = new HashSet<Memory>();
            var fringe = new List<Memory>();
            map[0, 0].Steps = 0;
            fringe.Add(map[0, 0]);
            while (fringe.Count > 0)
            {
                var current = fringe[0];
                visited.Add(current);
                fringe.RemoveAt(0);

                if (current.X == map.Width - 1 && current.Y == map.Height - 1)
                {
                    break;
                }

                var x = current.X;
                var y = current.Y;
                // N
                if (y > 0 && !map[x, y - 1].IsCorrupt && !visited.Contains(map[x, y - 1]))
                {
                    if (map[x, y - 1].Steps > current.Steps + 1)
                    {
                        map[x, y - 1].Steps = current.Steps + 1;
                        map[x, y - 1].Prev = current;
                        fringe.Add(map[x, y - 1]);
                    }
                }
                // E
                if (x < map.Width - 1 && !map[x + 1, y].IsCorrupt && !visited.Contains(map[x + 1, y]))
                {
                    if (map[x + 1, y].Steps > current.Steps + 1)
                    {
                        map[x + 1, y].Steps = current.Steps + 1;
                        map[x + 1, y].Prev = current;
                        fringe.Add(map[x + 1, y]);
                    }
                }
                // S
                if (y < map.Height - 1 && !map[x, y + 1].IsCorrupt && !visited.Contains(map[x, y + 1]))
                {
                    if (map[x, y + 1].Steps > current.Steps + 1)
                    {
                        map[x, y + 1].Steps = current.Steps + 1;
                        map[x, y + 1].Prev = current;
                        fringe.Add(map[x, y + 1]);
                    }
                }
                // W
                if (x > 0 && !map[x - 1, y].IsCorrupt && !visited.Contains(map[x - 1, y]))
                {
                    if (map[x - 1, y].Steps > current.Steps + 1)
                    {
                        map[x - 1, y].Steps = current.Steps + 1;
                        map[x - 1, y].Prev = current;
                        fringe.Add(map[x - 1, y]);
                    }
                }

                fringe.Sort();
            }

            if (!outputPath)
            {
                return null;
            }

            var end = map[map.Width - 1, map.Height - 1];
            var prev = end.Prev;
            var start = map[0, 0];
            var path = new HashSet<Memory>();
            while (!prev.Equals(start))
            {
                path.Add(prev);
                prev = prev.Prev;
            }
            return path;
        }

        private static Graph<Memory> BuildMap(StreamReader reader)
        {
            var mapSize = 71;
            var map = new Graph<Memory>();
            map.Height = map.Width = mapSize;

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    map[x, y] = new Memory(x, y, false);
                }
            }

            var i = 0;
            while (!(i >= 1024))
            {
                var currentLine = reader.ReadLine();
                var indices = currentLine.Split(',').Select(int.Parse).ToArray();
                var x = indices[0];
                var y = indices[1];
                map[x, y].IsCorrupt = true;
                i++;
            }

            return map;
        }
    }

    public class Memory : Point, IComparable<Memory>
    {
        public bool IsCorrupt;
        public int Steps = int.MaxValue;
        public Memory Prev;

        public Memory(int x, int y, bool isCorrupt) : base(x,y)
        {
            IsCorrupt = isCorrupt;
        }

        public int CompareTo(Memory other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (other is null) return 1;
            if (IsCorrupt)
            {
                return 0;
            }
            return Steps.CompareTo(other.Steps);
        }
    }
}