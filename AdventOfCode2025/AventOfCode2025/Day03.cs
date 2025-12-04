using System;
using System.Diagnostics.PerformanceData;
using System.Dynamic;
using System.Security.Cryptography;

namespace AdventOfCode2024
{
    public class Day03
    {
        private const string Day = "Day03";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";

        public static void Run()
        {
            Program.WriteTitle("--- Day 3: Lobby ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            using (var sr = Program.GetReader(FileLocation))
            {
                var line = sr.ReadLine();
                var total = 0L;
                do
                {
                    var lmax = '0';
                    var rmax = '0';
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (i < line.Length - 1
                            && line[i] > lmax)
                        {
                            lmax = line[i];
                            rmax = line[i + 1];
                        }
                        else if (line[i] > rmax)
                        {
                            rmax = line[i];
                        }
                    }

                    total += int.Parse("" + lmax + rmax);
                    line = sr.ReadLine();
                } while (!string.IsNullOrEmpty(line));
                Program.WriteOutput("2-Battery Joltage Potential: " + total);
            }
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            using (var sr = Program.GetReader(FileLocation))
            {
                var line = sr.ReadLine();
                var total = 0L;
                do
                {
                    var temp = GetHighestStartingFromIndex(line, 0, "");
                    total += long.Parse(GetHighestStartingFromIndex(line,0,""));
                    line = sr.ReadLine();
                } while (!string.IsNullOrEmpty(line));
                Program.WriteOutput("12-Battery Joltage Potential: " + total);
            }
        }

        private static string GetHighestStartingFromIndex(string str, int index, string current)
        {
            if (current.Length == 12)
            {
                return current;
            }

            for (char num = '9'; num >= '0'; num--)
            {
                var nexti = str.IndexOf(num, index);
                if (nexti > -1
                    && str.Length - nexti >= 12 - current.Length)
                {
                    current += num;
                    return GetHighestStartingFromIndex(str, nexti + 1, current);
                }
            }
            Program.WriteOutput("Whoops, We shouldn't have gotten here!");
            throw new Exception();
        }
    }
}