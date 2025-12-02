using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024
{
    public class Day11
    {
        private const string Day = "Day11";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";
        private static Dictionary<(long, int), long> Results = new Dictionary<(long, int), long>();

        public static void Run()
        {
            Program.WriteTitle("--- Day 11: Plutonian Pebbles ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var count = Blink(25);
            Program.WriteOutput("Total stones: " + count);
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var count = Blink(75);
            Program.WriteOutput("Total stones: " + count);
        }

        private static List<long> BuildStoneLine()
        {
            using (var reader = Program.GetReader(FileLocation))
            {
                var nums = reader.ReadLine().Split(' ').Select(long.Parse).ToList();
                return nums;
            }
        }

        private static long Blink(int times)
        {
            var stones = BuildStoneLine();
            long count = 0;
            foreach (var stone in stones)
            {
                count += BlinkRecursively(stone, times);
            }
            return count;
        }

        private static long BlinkRecursively(long value, int times)
        {
            if (Results.ContainsKey((value, times)))
            {
                return Results[(value, times)];
            }

            if (times == 0)
            {
                return 1;
            }

            times--;

            // 0 becomes 1
            if (value == 0)
            {
                if (!Results.ContainsKey((1, times)))
                {
                    Results.Add((1, times), BlinkRecursively(1, times));
                }
                return Results[(1, times)];
            }

            // even number of digits is split
            if (value.ToString().Length % 2 == 0)
            {
                var val = value.ToString();
                var half = val.Length / 2;
                var left = long.Parse(val.Substring(0, half));
                var right = long.Parse(val.Substring(half));
                if (!Results.ContainsKey((left, times)))
                {
                    Results.Add((left, times), BlinkRecursively(left, times));
                }
                if (!Results.ContainsKey((right, times)))
                {
                    Results.Add((right, times), BlinkRecursively(right, times));
                }
                return Results[(left, times)] + Results[(right, times)];
            }

            // all others * 2024
            var newVal = value * 2024;
            if (!Results.ContainsKey((newVal, times)))
            {
                Results.Add((newVal, times), BlinkRecursively(newVal, times));
            }
            return Results[(newVal, times)];
        }
    }
}