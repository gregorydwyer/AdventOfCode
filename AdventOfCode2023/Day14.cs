using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdventOfCode2023
{
    public class Day14
    {
        private const char Rock = 'O';
        private const char Wall = '#';
        private const char Empty = '.';

        private const string File = "Day14.txt";
        public static void Run()
        {
           Problem1();
            Problem2();
            Console.ReadKey();
        }

        private static void Problem1()
        {
            Console.WriteLine("Day14 P1");
            var map = BuildMap();
            var sw = new Stopwatch();
            sw.Start();
            for (int row = 1; row < map.Length; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    if (map[row][col] != Rock)
                    {
                        continue;
                    }
                    var currentRow = row;
                    var stuck = false;
                    while (currentRow > 0 && !stuck)
                    {
                        stuck = !MoveNorth(map, currentRow, col);
                        currentRow--;
                    }
                }
            }
            sw.Stop();
            long total = 0;
            long weight = map.Length;
            foreach (var row in map)
            {
                var rocks = row.Count(c => c == Rock);
                total += rocks * weight;
                weight--;
            }

            Console.WriteLine($"Total Weight: {total}");
            Console.WriteLine($"Runtime: {sw.ElapsedMilliseconds}");
        }

        private static bool MoveNorth(char[][] map, int row, int col)
        {
            switch (map[row-1][col])
            {
                case Empty:
                    map[row - 1][col] = Rock;
                    map[row][col] = Empty;
                    return true;
                case Rock:
                case Wall:
                    return false;
                default:
                    throw new NotSupportedException();
            }
        }
        private static (bool Moved, int NewRow, int NewCol) Move(char[][] map, int row, int col, int direction)
        {
            var nextRow = row;
            var nextCol = col;
            switch (direction)
            {
                case 0:
                    nextRow--;
                    if (nextRow < 0)
                    {
                        return (false, row, col);

                    }
                    break;
                case 1:
                    nextCol--;
                    if (nextCol < 0)
                    {
                        return (false, row, col);

                    }
                    break;
                case 2:
                    nextRow++;
                    if (nextRow > map.Length - 1)
                    {
                        return (false, row, col);

                    }
                    break;
                case 3:
                    nextCol++;
                    if (nextCol > map[nextRow].Length - 1)
                    {
                        return (false, row, col);

                    }
                    break;
                default:
                    throw new NotSupportedException();
            }
            switch (map[nextRow][nextCol])
            {
                case Empty:
                    map[nextRow][nextCol] = Rock;
                    map[row][col] = Empty;
                    return (true, nextRow, nextCol);
                case Rock:
                case Wall:
                    return (false, row, col);
                default:
                    throw new NotSupportedException();
            }
        }

        private static char[][] BuildMap()
        {
            using (var stream = new StreamReader(new FileStream(File, FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                var map = new List<char[]>();
                while (line != null)
                {
                    map.Add(line.ToCharArray());
                    line = stream.ReadLine();
                }

                return map.ToArray();
            }
        }

        private static void Problem2()
        {
            Console.WriteLine("Day14 P2");
            var map = BuildMap();
            var cycleLength = DoRotations(map);
            if (cycleLength > 0)
            {
                map = BuildMap();
                DoRotations(map, cycleLength);
            }

            long total = 0;
            long weight = map.Length;
            foreach (var row in map)
            {
                var rocks = row.Count(c => c == Rock);
                total += rocks * weight;
                weight--;
            }

            Console.WriteLine($"Total Weight: {total}");
        }

        private static int DoRotations(char[][] map, int cycles = 1000000000)
        {
            var dict = new Dictionary<string, int>();
            var loopLength = -1;
            var loopStart = -1;
            var origMapString = string.Join("\n", map.Select(row => new string(row)));
            var loopFound = false;
            dict.Add(origMapString, 0);
            for (int i = 0; i < cycles; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var reverse = false;
                    var colFirst = false;
                    switch (j)
                    {
                        case 0:
                            break;
                        case 1:
                            colFirst = true;
                            break;
                        case 2:
                            reverse = true;
                            break;
                        case 3:
                            colFirst = true;
                            reverse = true;
                            break;
                        default:
                            throw new NotSupportedException();
                    }

                    for (int outer = reverse ? 99 : 0; (!reverse && outer < 100) || (reverse && outer >= 0); outer = reverse ? outer - 1 : outer + 1)
                    {
                        for (int inner = 0; inner < 100; inner++)
                        {
                            var currentRow = colFirst ? inner : outer;
                            var currentCol = colFirst ? outer : inner;

                            if (map[currentRow][currentCol] != Rock)
                            {
                                continue;
                            }
                            var stuck = false;
                            while (!stuck)
                            {
                                var result = Move(map, currentRow, currentCol, j);
                                stuck = !result.Moved;
                                currentRow = result.NewRow;
                                currentCol = result.NewCol;
                            }
                        }
                    }
                }

                var mapString = string.Join("\n", map.Select(row => new string(row)));

                if (!dict.ContainsKey(mapString))
                {
                    dict.Add(mapString, i);
                }
                else
                {
                    loopStart = dict[mapString];
                    loopLength = i - loopStart;
                    loopFound = true;
                    break;
                }
            }

            if (loopFound)
            {
                var cycleLength = (1000000000 - loopStart) % loopLength;
                return cycleLength + loopStart;
            }

            return -1;
        }
    }
}
