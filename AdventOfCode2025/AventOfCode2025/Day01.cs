using System;
using System.Runtime.InteropServices;

namespace AdventOfCode2024
{
    public class Day01
    {
        private const string Day = "Day01";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";
        private static int curr = 50;

        public static void Run()
        {
            Program.WriteTitle("--- Day 1: Secret Entrance ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber(Day + " P1");
            using (var sr = Program.GetReader(FileLocation))
            {
                var line = sr.ReadLine();
                var count = 0;
                curr = 50;
                do
                {
                    if (line[0] == 'R')
                    {
                        curr = TurnRight(line, curr, out var _);
                    }
                    else
                    {
                        curr = TurnLeft(line, curr, out var _);
                    }

                    if (curr == 0)
                    {
                        count++;
                    }
                    line = sr.ReadLine();
                } while (!string.IsNullOrEmpty(line));
                Program.WriteOutput("Zero Count: " + count);
            }
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber(Day + " P2");
            using (var sr = Program.GetReader(FileLocation))
            {
                var line = sr.ReadLine();
                var count = 0;
                curr = 50;
                do
                {
                    var turnCount = 0;
                    if (line[0] == 'R')
                    {
                        curr = TurnRight(line, curr, out turnCount);
                    }
                    else
                    {
                        curr = TurnLeft(line, curr, out turnCount);
                    }

                    count += turnCount;

                    if (curr == 0)
                    {
                        count++;
                    }
                    line = sr.ReadLine();
                } while (!string.IsNullOrEmpty(line));
                Program.WriteOutput("Zero Count: " + count);
            }
        }

        private static int TurnRight(string dir, int curr, out int count)
        {
            var num = int.Parse(dir.Substring(1));
            count = (int) Math.Floor(((double) num) / 100.0);
            num = num % 100;
            curr += num;
            if(curr > 99)
            {
                curr = curr % 100;
                count += curr == 0 ? 0 : 1;
            }
            return curr;
        }

        private static int TurnLeft(string dir, int curr, out int count)
        {
            var num = int.Parse(dir.Substring(1));
            var startAtZero = curr == 0;
            count = (int)Math.Floor(((double)num) / 100.0);
            num = num % 100;
            curr -= num;
            curr = curr % 100;
            if (curr < 0)
            {
                curr = 100 + curr;
                count += startAtZero || curr == 0 ? 0 : 1;
            }
            return curr;
        }
    }
}