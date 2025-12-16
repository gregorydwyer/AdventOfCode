using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2024
{
    public class Day05
    {
        private const string Day = "Day05";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";

        public static void Run()
        {
            Program.WriteTitle("--- Day 5: Cafeteria ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            using (var sr = Program.GetReader(FileLocation))
            {
                var ranges = BuildRanges(sr);
                var line = sr.ReadLine();
                var count = 0;
                while (!string.IsNullOrEmpty(line))
                {
                    var num = long.Parse(line);
                    foreach (var range in ranges)
                    {
                        if (range.Contains(num))
                        {
                            count++;
                            break;
                        }
                    }

                    line = sr.ReadLine();
                }
                Program.WriteOutput("# of Fresh Ingredients: " + count);
            }
        }

        private static List<Range> BuildRanges(StreamReader sr)
        {
            var buckets = new List<Range>();
            var line = sr.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                buckets.Add(new Range(line));
                line = sr.ReadLine();
            }

            return buckets;
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            List<Range> ranges;
            using (var sr = Program.GetReader(FileLocation))
            {
                ranges = BuildRanges(sr);
            }

            ranges.Sort();
            for (int i = 0; i < ranges.Count - 1; i++)
            {
                if (ranges[i + 1].Contains(ranges[i].Upper))
                {
                    ranges[i + 1].Lower = ranges[i].Lower;
                    ranges.RemoveAt(i);
                    i--;
                }
                else if (ranges[i].Contains(ranges[i + 1].Upper))
                {
                    ranges.RemoveAt(i + 1);
                    i--;
                }
            }

            var total = 0L;
            foreach (var range in ranges)
            {
                total += range.Upper - range.Lower + 1;
            }

            Program.WriteOutput("# of Fresh Ingredient Ids: " + total);
        }
    }

    public class Range : IComparable<Range>
    {
        public long Lower;
        public long Upper;

        public Range(long lower, long upper)
        {
            Lower = lower;
            Upper = upper;
        }

        public Range(string range)
        {
            var split = range.Split('-');
            Lower = long.Parse(split[0]);
            Upper = long.Parse(split[1]);

        }

        public bool Contains(long num)
        {
            return num >= Lower && num <= Upper;
        }

        public int CompareTo(Range other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (other is null) return 1;
            var lowerComparison = Lower.CompareTo(other.Lower);
            if (lowerComparison != 0) return lowerComparison;
            return Upper.CompareTo(other.Upper);
        }
    }
}