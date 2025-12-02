using System;
using System.Text.RegularExpressions;

namespace AdventOfCode2024
{
    public class Day03
    {
        private const string Day = "Day03";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";
        private static readonly string BasicRegex = "mul\\(\\d{1,3},\\d{1,3}\\)";

        private static readonly string GroupedRegex =
            "mul\\(\\d{1,3},\\d{1,3}\\)|do\\(\\)|don't\\(\\)";

        public static void Run()
        {
            Program.WriteTitle("--- Day 3: Mull It Over ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            using (var reader = Program.GetReader(FileLocation))
            {
                var currentLine = reader.ReadLine();
                var total = 0;
                while (currentLine != null)
                {
                    total += SimpleParseLine(currentLine);
                    currentLine = reader.ReadLine();
                }
                Program.WriteOutput("Total: " + total);
            }
        }

        private static int SimpleParseLine(string line)
        {
            var regex = new Regex(BasicRegex);

            var matches = regex.Matches(line);
            var total = 0;
            foreach (Match match in matches)
            {
                var nums = match.Value.Substring(4).Trim(')').Split(',');
                total += int.Parse(nums[0]) * int.Parse(nums[1]);
            }

            return total;
        }

        private static int GroupedParseLine(string line)
        {
            var regex = new Regex(GroupedRegex);

            var matches = regex.Matches(line);
            var total = 0;
            foreach (Match match in matches)
            {
                var nums = match.Value.Substring(4).Trim(')').Split(',');
                total += int.Parse(nums[0]) * int.Parse(nums[1]);
            }

            return total;
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            using (var reader = Program.GetReader(FileLocation))
            {
                var currentLine = reader.ReadLine();
                var total = 0;
                var isEnabled = true;
                while (currentLine != null)
                {
                    var regex = new Regex(GroupedRegex);
                    var matches = regex.Matches(currentLine);
                    foreach (Match match in matches)
                    {
                        if(match.Value.Contains("mul") && isEnabled)
                        {
                            var nums = match.Value.Substring(4).Trim(')').Split(',');
                            total += int.Parse(nums[0]) * int.Parse(nums[1]);
                        }
                        else if (match.Value.Contains("do("))
                        {
                            isEnabled = true;
                        }
                        else if (match.Value.Contains("don't("))
                        {
                            isEnabled = false;
                        }
                    }
                    currentLine = reader.ReadLine();
                }
                Program.WriteOutput("Total: " + total);
            }
        }
    }
}