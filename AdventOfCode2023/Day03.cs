using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2023
{
    public class Day03
    {
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        private static char[][] BuildArray()
        {
            using (var stream = new StreamReader(new FileStream("Day03.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                var array = new List<char[]>();
                while (line != null)
                {
                    array.Add(line.ToCharArray());
                    line = stream.ReadLine();
                }

                return array.ToArray();
            }
        }

        private static bool IsTouchingSymbol(char[][] sch, int row, int col)
        {
            if (row > 0)
            {
                if (IsSymbol(sch[row - 1][col]))
                {
                    return true;
                }

                if (col > 0 && IsSymbol(sch[row - 1][col - 1]))
                {
                    return true;
                }

                if (col < sch[row].Length - 1 && IsSymbol(sch[row - 1][col + 1]))
                {
                    return true;
                }
            }

            if (col > 0 && IsSymbol(sch[row][col - 1]))
            {
                return true;
            }

            if (col < sch[row].Length - 1 && IsSymbol(sch[row][col + 1]))
            {
                return true;
            }

            if (row < sch.Length - 1)
            {
                if (IsSymbol(sch[row + 1][col]))
                {
                    return true;
                }

                if (col > 0 && IsSymbol(sch[row + 1][col - 1]))
                {
                    return true;
                }

                if (col < sch[row].Length - 1 && IsSymbol(sch[row + 1][col + 1]))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsTouchingGear(char[][] sch, int row, int col, out Point touchedGear)
        {
            var asterisks = new HashSet<Point>();
            if (row > 0)
            {
                if (IsAsterisk(sch[row - 1][col]))
                {
                    asterisks.Add(new Point(row - 1, col));
                }

                if (col > 0 && IsAsterisk(sch[row - 1][col - 1]))
                {
                    asterisks.Add(new Point(row - 1, col - 1));
                }

                if (col < sch[row].Length - 1 && IsAsterisk(sch[row - 1][col + 1]))
                {
                    asterisks.Add(new Point(row - 1, col + 1));
                }
            }

            if (col > 0 && IsAsterisk(sch[row][col - 1]))
            {
                asterisks.Add(new Point(row, col - 1));
            }

            if (col < sch[row].Length - 1 && IsAsterisk(sch[row][col + 1]))
            {
                asterisks.Add(new Point(row, col + 1));
            }

            if (row < sch.Length - 1)
            {
                if (IsAsterisk(sch[row + 1][col]))
                {
                    asterisks.Add(new Point(row + 1, col));
                }

                if (col > 0 && IsAsterisk(sch[row + 1][col - 1]))
                {
                    asterisks.Add(new Point(row + 1, col - 1));
                }

                if (col < sch[row].Length - 1 && IsAsterisk(sch[row + 1][col + 1]))
                {
                    asterisks.Add(new Point(row + 1, col + 1));
                }
            }

            if (asterisks.Count != 1)
            {
                touchedGear = new Point(-1, -1);
                return false;
            }

            touchedGear = asterisks.First();
            return true;
        }

        private static bool IsSymbol(char c)
        {
            return c != '.' && !char.IsNumber(c);
        }

        private static bool IsAsterisk(char c)
        {
            return c == '*';
        }

        public static void Problem1()
        {
            Console.WriteLine("Day02 P1");
            var sch = BuildArray();
            var finalTotal = 0;
            var isPart = false;
            var part = "";
            for (int row = 0; row < sch.Length; row++)
            {
                part = "";
                for (int col = 0; col < sch[0].Length; col++)
                {
                    var temp = new string(sch[row]);
                    //read this char
                    if (char.IsNumber(sch[row][col]))
                    {
                        part += sch[row][col];
                        // check for symbols near it
                        if (IsTouchingSymbol(sch, row, col))
                        {
                            isPart = true;
                        }
                    }
                    else
                    {
                        if (isPart)
                        {
                            finalTotal += int.Parse(part);
                            isPart = false;
                        }
                        part = "";
                        continue;
                    }
                }
                if (isPart)
                {
                    finalTotal += int.Parse(part);
                    isPart = false;
                }
            }
            Console.WriteLine("Total: " + finalTotal);
        }

        public static void Problem2()
        {
            Console.WriteLine("Day02 P2");

            var sch = BuildArray();
            var map = BuildGearMap();
            var finalTotal = 0;
            for (int row = 0; row < sch.Length; row++)
            {
                for (int col = 0; col < sch[0].Length; col++)
                {
                    if (sch[row][col] == '*')
                    {
                        // look for two gears
                        finalTotal += GetGearRatio(map, row, col);
                    }
                }
            }
            Console.WriteLine("Total: " + finalTotal);
        }

        private static int GetGearRatio(Dictionary<Point, List<int>> map, int row, int col)
        {
            var point = new Point(row, col);
            if (map.ContainsKey(point) && map[point].Count != 2)
            {
                return 0;
            }

            return map[point].First() * map[point].Last();
        }

        private static Dictionary<Point,List<int>> BuildGearMap()
        {
            var sch = BuildArray();
            var isGear = false;
            var gearMap = new Dictionary<Point, List<int>>();
            var part = "";
            var touchedGear = new Point(-1, -1);
            for (int row = 0; row < sch.Length; row++)
            {
                part = "";
                for (int col = 0; col < sch[0].Length; col++)
                {
                    var temp = new string(sch[row]);
                    //read this char
                    if (char.IsNumber(sch[row][col]))
                    {
                        part += sch[row][col];
                        // check for symbols near it
                        if (!isGear && IsTouchingGear(sch, row, col, out touchedGear))
                        {
                            isGear = true;
                        }
                    }
                    else
                    {
                        if (isGear)
                        {
                            if (gearMap.ContainsKey(touchedGear))
                            {
                                gearMap[touchedGear].Add(int.Parse(part));
                            }
                            else
                            {
                                gearMap.Add(touchedGear, new List<int>(){int.Parse(part)});
                            }
                        }

                        isGear = false;
                        part = "";
                        continue;
                    }
                }

                if (isGear)
                {
                    if (gearMap.ContainsKey(touchedGear))
                    {
                        gearMap[touchedGear].Add(int.Parse(part));
                    }
                    else
                    {
                        gearMap.Add(touchedGear, new List<int>() { int.Parse(part) });
                    }
                    isGear = false;
                }
            }


            return gearMap;
        }
        internal struct Point
        {
            public int Row;
            public int Col;

            public Point(int row, int col)
            {
                Row = row;
                Col = col;
            }

            public override string ToString()
            {
                return "(" + Row + ", " + Col + ")";
            }
        }
    }
}