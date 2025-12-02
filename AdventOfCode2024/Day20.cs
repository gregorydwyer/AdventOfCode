using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace AdventOfCode2024
{
    public class Day20
    {
        private const string Day = "Day20";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";
        private static int Width, Height;
        private static Track Start, End;
        private static Graph<Track> Map;

        public static void Run()
        {
            Program.WriteTitle("--- Day 20: Race Condition ---");
            BuildMap();
            SetInitialTrackValues();
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var threshold = 101L;
            var potentials = Map.GetSource().Where(kvp => !kvp.Value.IsWall && kvp.Value.Cost + threshold < End.Cost).Select(kvp => kvp.Value).ToList();
            var count = 0L;
            foreach (var current in potentials)
            {

                var x = current.X;
                var y = current.Y;
                // N
                if (Map.Contains(x,y - 2) &&
                    !Map[x, y - 2].IsWall &&
                    Map[x, y - 2].Cost > current.Cost + threshold)
                {
                    count++;
                }
                // S
                if (Map.Contains(x, y + 2) &&
                    !Map[x, y + 2].IsWall &&
                    Map[x, y + 2].Cost > current.Cost + threshold)
                {
                    count++;
                }
                // W
                if (Map.Contains(x - 2, y) &&
                    !Map[x - 2, y].IsWall &&
                    Map[x - 2, y].Cost > current.Cost + threshold)
                {
                    count++;
                }
                // E
                if (Map.Contains(x + 2, y) &&
                    !Map[x + 2, y].IsWall &&
                    Map[x + 2, y].Cost > current.Cost + threshold)
                {
                    count++;
                }
            }
            Program.WriteOutput($"Viable 2ps Cheats: {count}");
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var threshold = 100L;
            var potentials = Map.GetSource().Where(kvp => !kvp.Value.IsWall && kvp.Value.Cost + threshold < End.Cost).Select(kvp => kvp.Value).ToList();
            var count = 0L;
            var cheatLength = 20;
            foreach (var current in potentials)
            {
                for (int i = 2; i <= cheatLength; i++)
                {
                    // N
                    var nextX = current.X;
                    var nextY = current.Y + i;
                    while(nextY > current.Y)
                    {
                        if (Map.Contains(nextX, nextY) &&
                            !Map[nextX, nextY].IsWall &&
                            Map[nextX, nextY].Cost - current.Cost - i >= threshold)
                        {
                            count++;
                        }

                        nextY--;
                        nextX++;
                    }

                    // S
                    nextX = current.X;
                    nextY = current.Y - i;
                    while (nextY < current.Y)
                    {
                        if (Map.Contains(nextX, nextY) &&
                            !Map[nextX, nextY].IsWall &&
                            Map[nextX, nextY].Cost - current.Cost - i >= threshold)
                        {
                            count++;
                        }

                        nextY++;
                        nextX--;
                    }

                    // E
                    nextX = current.X + i;
                    nextY = current.Y;
                    while (nextX > current.X)
                    {
                        if (Map.Contains(nextX, nextY) &&
                            !Map[nextX, nextY].IsWall &&
                            Map[nextX, nextY].Cost - current.Cost - i >= threshold)
                        {
                            count++;
                        }

                        nextY--;
                        nextX--;
                    }

                    // W
                    nextX = current.X - i;
                    nextY = current.Y;
                    while (nextX < current.X)
                    {
                        if (Map.Contains(nextX, nextY) &&
                            !Map[nextX, nextY].IsWall &&
                            Map[nextX, nextY].Cost - current.Cost - i >= threshold)
                        {
                            count++;
                        }

                        nextY++;
                        nextX++;
                    }
                }
            }
            Program.WriteOutput($"Viable 20ps Cheats: {count}");
        }

        private static void BuildMap()
        {
            using (var reader = Program.GetReader(FileLocation))
            {
                Map = new Graph<Track>();
                var currentLine = reader.ReadLine();
                Width = currentLine.Length;
                var y = -1;
                while (currentLine != null)
                {
                    y++;
                    for (var x = 0; x < currentLine.Length; x++)
                    {
                        var c = currentLine[x];
                        Map[x, y] = new Track(x,y);
                        if (c == '#')
                        {
                            Map[x, y].IsWall = true;
                        }
                        else if (c == 'S')
                        {
                            Start = Map[x, y];
                            Start.Cost = 0;
                        }
                        else if (c == 'E')
                        {
                            End = Map[x, y];
                        }
                    }

                    currentLine = reader.ReadLine();
                }
                Height = y + 1;
            }
        }

        private static void SetInitialTrackValues()
        {
            var next = Start;
            while (!next.Equals(End))
            {
                var current = next;

                var x = current.X;
                var y = current.Y;
                // N
                if (!Map[x, y - 1].IsWall && Map[x, y - 1].Cost > current.Cost)
                {
                    Map[x, y - 1].Cost = current.Cost + 1;
                    next = Map[x, y - 1];
                }
                // E
                if (!Map[x + 1, y].IsWall && Map[x + 1, y].Cost > current.Cost)
                {
                    Map[x + 1, y].Cost = current.Cost + 1;
                    next = Map[x + 1, y];
                }
                // S
                if (!Map[x, y + 1].IsWall && Map[x, y + 1].Cost > current.Cost)
                {
                    Map[x, y + 1].Cost = current.Cost + 1;
                    next = Map[x, y + 1];
                }
                // W
                if (!Map[x - 1, y].IsWall && Map[x - 1, y].Cost > current.Cost)
                {
                    Map[x - 1, y].Cost = current.Cost + 1;
                    next = Map[x - 1, y];
                }

            }
        }
    }

    public class Track : Point
    {
        public long Cost = long.MaxValue;
        public bool IsWall;

        public Track(int x, int y) : base(x, y)
        {

        }
    }
}
