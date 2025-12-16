using System;

namespace AdventOfCode2024
{
    public class Day02
    {
        private const string Day = "Day02";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";

        public static void Run()
        {
            Program.WriteTitle("--- Day 2: Gift Shop ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            using (var sr = Program.GetReader(FileLocation))
            {
                var line = sr.ReadLine();
                var ranges = line.Split(',');
                var total = 0L;
                foreach (var range in ranges)
                {
                    var bounds = range.Split('-');
                    total += AddTwiceRepeatedCodes(long.Parse(bounds[0]), long.Parse(bounds[1]));
                }
                Program.WriteOutput("Sum of invalid IDs: " + total);
            }
        }

        private static long AddTwiceRepeatedCodes(long lower, long upper)
        {
            var total = 0L;

            for (long i = lower; i <= upper; i++)
            {
                if (IsRepeatedTwice(i.ToString()))
                {
                    total += i;
                }
            }

            return total;
        }

        private static bool IsRepeatedTwice(string input)
        {
            if (input.Length % 2 == 1)
            {
                return false;
            }

            var half = input.Length / 2;

            return input.Substring(0, half).Equals(input.Substring(half));
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            using (var sr = Program.GetReader(FileLocation))
            {
                var line = sr.ReadLine();
                var ranges = line.Split(',');
                var total = 0L;
                foreach (var range in ranges)
                {
                    var bounds = range.Split('-');
                    total += AddAllRepeatingCodes(long.Parse(bounds[0]), long.Parse(bounds[1]));
                }
                Program.WriteOutput("Sum of invalid IDs: " + total);
            }
        }

        private static long AddAllRepeatingCodes(long lower, long upper)
        {
            var total = 0L;

            for (long i = lower; i <= upper; i++)
            {
                if (IsRepeatingCode(i.ToString()))
                {
                    total += i;
                }
            }

            return total;
        }

        private static bool IsRepeatingCode(string input)
        {
            // Longest pattern can only be half the input length
            for (var i = 1; i <= input.Length / 2; i++)
            {
                // if the length of the input is not divisible by the pattern length, it can't repeat and fit
                if (input.Length % i != 0)
                {
                    continue;
                }

                // set the starting index to the current index
                var start = i;
                var sub = input.Substring(0, i);
                // repeatedly check substrings of pattern length and return if we get to the end of the string
                while (sub.Equals(input.Substring(start, i)))
                {
                    start += i;
                    if (start >= input.Length)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}