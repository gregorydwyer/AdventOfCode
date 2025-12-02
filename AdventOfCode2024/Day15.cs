using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace AdventOfCode2024
{
    public class Day15
    {
        private const string Day = "Day15";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";
        private static Point Bot;

        public static void Run()
        {
            Program.WriteTitle("--- Day 15: Warehouse Woes ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var map = new List<List<char>>();
            var moves = new Queue<char>();
            using (var reader = Program.GetReader(FileLocation))
            {
                map = BuildMap(reader);
                moves = BuildPath(reader);
            }
            //Draw(map);
            while (moves.Count > 0)
            {
                var next = moves.Dequeue();
                switch (next)
                {
                    case '^':
                        MoveUp(map);
                        break;
                    case '>':
                        MoveRight(map);
                        break;
                    case 'v':
                        MoveDown(map);
                        break;
                    case '<':
                        MoveLeft(map);
                        break;
                }
            }

            var total = 0L;
            for (int r = 0; r < map.Count; r++)
            {
                for (int c = 0; c < map[r].Count; c++)
                {
                    if (map[r][c] == 'O')
                    {
                        total += (100 * r) + c;
                    }
                }
            }
            Program.WriteOutput("GPS Sum: " + total);
        }

        private static void Draw(List<List<char>> map)
        {
            for (int r = 0; r < map.Count; r++)
            {
                for (int c = 0; c < map[r].Count; c++)
                {
                    Console.Write(map[r][c]);
                }
                Console.WriteLine();
            }
        }

        #region Part 1
        private static bool MoveUp(List<List<char>> map, int x, int y)
        {
            if (map[y][x] == '.')
            {
                return true;
            }

            if (map[y][x] == '#')
            {
                return false;
            }

            if (map[y][x] == 'O' &&
                MoveUp(map, x, y - 1))
            {
                map[y][x] = '.';
                map[y - 1][x] = 'O';
                return true;
            }

            return false;
        }

        private static bool MoveDown(List<List<char>> map, int x, int y)
        {
            if (map[y][x] == '.')
            {
                return true;
            }

            if (map[y][x] == '#')
            {
                return false;
            }

            if (map[y][x] == 'O' &&
                MoveDown(map, x, y + 1))
            {
                map[y][x] = '.';
                map[y + 1][x] = 'O';
                return true;
            }

            return false;
        }

        private static bool MoveLeft(List<List<char>> map, int x, int y)
        {
            if (map[y][x] == '.')
            {
                return true;
            }

            if (map[y][x] == '#')
            {
                return false;
            }

            if (map[y][x] == 'O' &&
                MoveLeft(map, x - 1, y))
            {
                map[y][x] = '.';
                map[y][x - 1] = 'O';
                return true;
            }

            return false;
        }

        private static bool MoveRight(List<List<char>> map, int x, int y)
        {
            if (map[y][x] == '.')
            {
                return true;
            }

            if (map[y][x] == '#')
            {
                return false;
            }

            if (map[y][x] == 'O' &&
                MoveRight(map, x + 1, y))
            {
                map[y][x] = '.';
                map[y][x + 1] = 'O';
                return true;
            }

            return false;
        }

        private static void MoveUp(List<List<char>> map)
        {
            if (MoveUp(map, Bot.X, Bot.Y - 1))
            {
                map[Bot.Y][Bot.X] = '.';
                Bot.Y--;
                map[Bot.Y][Bot.X] = '@';
            }
        }

        private static void MoveDown(List<List<char>> map)
        {
            if (MoveDown(map, Bot.X, Bot.Y + 1))
            {
                map[Bot.Y][Bot.X] = '.';
                Bot.Y++;
                map[Bot.Y][Bot.X] = '@';
            }
        }

        private static void MoveLeft(List<List<char>> map)
        {
            if (MoveLeft(map, Bot.X - 1, Bot.Y))
            {
                map[Bot.Y][Bot.X] = '.';
                Bot.X--;
                map[Bot.Y][Bot.X] = '@';
            }
        }

        private static void MoveRight(List<List<char>> map)
        {
            if (MoveRight(map, Bot.X + 1, Bot.Y))
            {
                map[Bot.Y][Bot.X] = '.';
                Bot.X++;
                map[Bot.Y][Bot.X] = '@';
            }
        }
        #endregion

        #region Part 2
        private static bool PartTwoCanMoveUp(List<List<char>> map, int x, int y)
        {
            if (map[y][x] == '.')
            {
                return true;
            }

            if (map[y][x] == '#')
            {
                return false;
            }

            if (map[y][x] == '[' &&
                PartTwoCanMoveUp(map, x, y - 1) &&
                PartTwoCanMoveUp(map, x + 1, y - 1))
            {
                return true;
            }

            if (map[y][x] == ']' &&
                PartTwoCanMoveUp(map, x, y - 1) &&
                PartTwoCanMoveUp(map, x - 1, y - 1))
            {
                return true;
            }

            return false;
        }

        private static bool PartTwoCanMoveDown(List<List<char>> map, int x, int y)
        {
            if (map[y][x] == '.')
            {
                return true;
            }

            if (map[y][x] == '#')
            {
                return false;
            }

            if (map[y][x] == '[' &&
                PartTwoCanMoveDown(map, x, y + 1) &&
                PartTwoCanMoveDown(map, x + 1, y + 1))
            {
                return true;
            }

            if (map[y][x] == ']' &&
                PartTwoCanMoveDown(map, x, y + 1) &&
                PartTwoCanMoveDown(map, x - 1, y + 1))
            {
                return true;
            }

            return false;
        }

        private static bool PartTwoCanMoveLeft(List<List<char>> map, int x, int y)
        {
            if (map[y][x] == '.')
            {
                return true;
            }

            if (map[y][x] == '#')
            {
                return false;
            }

            if (map[y][x] == ']' &&
                PartTwoCanMoveLeft(map, x - 2, y) )
            {
                return true;
            }

            return false;
        }

        private static bool PartTwoCanMoveRight(List<List<char>> map, int x, int y)
        {
            if (map[y][x] == '.')
            {
                return true;
            }

            if (map[y][x] == '#')
            {
                return false;
            }

            if (map[y][x] == '[' &&
                PartTwoCanMoveRight(map, x + 2, y))
            {
                return true;
            }

            return false;
        }

        private static void PartTwoMoveUp(List<List<char>> map, int x, int y)
        {
            if (map[y][x] == '[')
            {
                PartTwoMoveUp(map, x, y - 1);
                PartTwoMoveUp(map, x + 1, y - 1);
                map[y][x] = '.';
                map[y - 1][x] = '[';
                map[y][x + 1] = '.';
                map[y - 1][x + 1] = ']';
            }
            else if (map[y][x] == ']')
            {
                PartTwoMoveUp(map, x, y - 1);
                PartTwoMoveUp(map, x - 1, y - 1);
                map[y][x] = '.';
                map[y - 1][x] = ']';
                map[y][x - 1] = '.';
                map[y - 1][x - 1] = '[';
            }
        }

        private static void PartTwoMoveDown(List<List<char>> map, int x, int y)
        {
            if (map[y][x] == '[')
            {
                PartTwoMoveDown(map, x, y + 1);
                PartTwoMoveDown(map, x + 1, y + 1);
                map[y][x] = '.';
                map[y + 1][x] = '[';
                map[y][x + 1] = '.';
                map[y + 1][x + 1] = ']';
            }
            else if (map[y][x] == ']')
            {
                PartTwoMoveDown(map, x, y + 1);
                PartTwoMoveDown(map, x - 1, y + 1);
                map[y][x] = '.';
                map[y + 1][x] = ']';
                map[y][x - 1] = '.';
                map[y + 1][x - 1] = '[';
            }
        }

        private static void PartTwoMoveLeft(List<List<char>> map, int x, int y)
        {
            if (map[y][x] == ']')
            {
                PartTwoMoveLeft(map, x - 2, y);
                map[y][x] = '.';
                map[y][x - 2] = '[';
                map[y][x - 1] = ']';
            }
        }

        private static void PartTwoMoveRight(List<List<char>> map, int x, int y)
        {
            if (map[y][x] == '[')
            {
                PartTwoMoveRight(map, x + 2, y);
                map[y][x] = '.';
                map[y][x + 2] = ']';
                map[y][x + 1] = '[';
            }
        }

        private static void PartTwoMoveUp(List<List<char>> map)
        {
            if (PartTwoCanMoveUp(map, Bot.X, Bot.Y - 1))
            {
                PartTwoMoveUp(map, Bot.X, Bot.Y - 1);
                map[Bot.Y][Bot.X] = '.';
                Bot.Y--;
                map[Bot.Y][Bot.X] = '@';
            }
        }

        private static void PartTwoMoveDown(List<List<char>> map)
        {
            if (PartTwoCanMoveDown(map, Bot.X, Bot.Y + 1))
            {
                PartTwoMoveDown(map, Bot.X, Bot.Y + 1);
                map[Bot.Y][Bot.X] = '.';
                Bot.Y++;
                map[Bot.Y][Bot.X] = '@';
            }
        }

        private static void PartTwoMoveLeft(List<List<char>> map)
        {
            if (PartTwoCanMoveLeft(map, Bot.X - 1, Bot.Y))
            {
                PartTwoMoveLeft(map, Bot.X - 1, Bot.Y);
                map[Bot.Y][Bot.X] = '.';
                Bot.X--;
                map[Bot.Y][Bot.X] = '@';
            }
        }

        private static void PartTwoMoveRight(List<List<char>> map)
        {
            if (PartTwoCanMoveRight(map, Bot.X + 1, Bot.Y))
            {
                PartTwoMoveRight(map, Bot.X + 1, Bot.Y);
                map[Bot.Y][Bot.X] = '.';
                Bot.X++;
                map[Bot.Y][Bot.X] = '@';
            }
        }
        #endregion

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var map = new List<List<char>>();
            var moves = new Queue<char>();
            using (var reader = Program.GetReader(FileLocation))
            {
                map = BuildPartTwoMap(reader);
                moves = BuildPath(reader);
            }
            while (moves.Count > 0)
            {
                var next = moves.Dequeue();
                switch (next)
                {
                    case '^':
                        PartTwoMoveUp(map);
                        map[Bot.Y][Bot.X] = '^';
                        break;
                    case '>':
                        PartTwoMoveRight(map);
                        map[Bot.Y][Bot.X] = '>';
                        break;
                    case 'v':
                        PartTwoMoveDown(map);
                        map[Bot.Y][Bot.X] = 'v';
                        break;
                    case '<':
                        PartTwoMoveLeft(map);
                        map[Bot.Y][Bot.X] = '<';
                        break;
                }

                //Console.Clear();
                //Console.WriteLine("Move " + next + ":");
                //foreach (var line in map)
                //{
                //    Console.WriteLine(new string(line.ToArray()));
                //}
                //Thread.Sleep(200);
            }

            var total = 0L;
            for (int r = 0; r < map.Count; r++)
            {
                for (int c = 0; c < map[r].Count; c++)
                {
                    if (map[r][c] == '[')
                    {
                        total += (100 * r) + c;
                    }
                }
            }

            //foreach (var line in map)
            //{
            //    Console.WriteLine(new string(line.ToArray()));
            //}
            Program.WriteOutput("GPS Sum: " + total);
        }

        private static List<List<char>> BuildMap(StreamReader reader)
        {
            var map = new List<List<char>>();

            var currentLine = reader.ReadLine();
            while (!string.IsNullOrEmpty(currentLine))
            {
                map.Add(currentLine.ToCharArray().ToList());
                if (currentLine.Contains("@"))
                {
                    Bot = new Point(currentLine.IndexOf("@"), map.Count - 1);
                }
                currentLine = reader.ReadLine();
            }

            return map;
        }

        private static List<List<char>> BuildPartTwoMap(StreamReader reader)
        {
            var map = new List<List<char>>();

            var currentLine = reader.ReadLine();
            var row = -1;
            while (!string.IsNullOrEmpty(currentLine))
            {

                map.Add(new List<char>());
                row++;
                for (int i = 0; i < currentLine.Length; i++)
                {
                    if (currentLine[i] == '#')
                    {
                        map[row].Add('#');
                        map[row].Add('#');
                    }
                    if (currentLine[i] == '.')
                    {
                        map[row].Add('.');
                        map[row].Add('.');
                    }
                    if (currentLine[i] == 'O')
                    {
                        map[row].Add('[');
                        map[row].Add(']');
                    }
                    if (currentLine[i] == '@')
                    {
                        Bot = new Point(map[row].Count, row);
                        map[row].Add('@');
                        map[row].Add('.');
                    }
                }
                currentLine = reader.ReadLine();
            }

            return map;
        }

        private static Queue<char> BuildPath(StreamReader reader)
        {
            var queue = new Queue<char>();
            var currentLine = reader.ReadLine();
            while (currentLine != null)
            {
                foreach (var c in currentLine)
                {
                    queue.Enqueue(c);
                }
                currentLine = reader.ReadLine();
            }

            return queue;
        }
    }
}