using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024
{
    public class Day06
    {
        private const string Day = "Day06";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";

        public static void Run()
        {
            Program.WriteTitle("--- Day 6: Trash Compactor ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var problems = BuildProblems();
            var op = problems.Count - 1;
            var total = 0L;
            for (int col = 0; col < problems[0].Count; col++)
            {
                var prob = long.Parse(problems[0][col]);
                for (int row = 1; row < problems.Count - 1; row++)
                {
                    if (problems[op][col] == "+")
                    {
                        prob += long.Parse(problems[row][col]);
                    }
                    else
                    {
                        prob *= long.Parse(problems[row][col]);
                    }
                }

                total += prob;
            }

            Program.WriteOutput("Cephalopod Math Homework Total: " + total);
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var arrays = new List<char[]>();
            var ops = new List<string>();
            using (var sr = Program.GetReader(FileLocation))
            {
                var line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    if (line[0] == '*' || line[0] == '+')
                    {
                        ops = line.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries).ToList();
                        ops.Reverse();
                    }
                    else
                    {
                        arrays.Add(line.ToCharArray());
                    }
                    line = sr.ReadLine();
                }
            }

            var newLines = new List<string>();
            for (int col = arrays[0].Length - 1; col >= 0; col--)
            {
                var line = "";
                for (int row = 0; row < arrays.Count; row++)
                {
                    line += arrays[row][col];
                }
                newLines.Add(line);
            }

            var total = 0L;
            var prob = 0L;
            var opCount = 0;
            for (int i = 0; i < newLines.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(newLines[i]))
                {
                    total += prob;
                    prob = 0L;
                    opCount++;
                    continue;
                }

                if (prob == 0)
                {
                    prob = long.Parse(newLines[i]);
                    continue;
                }

                if (ops[opCount] == "+")
                {
                    prob += long.Parse(newLines[i]);
                }
                else
                {
                    prob *= long.Parse(newLines[i]);
                }
            }

            total += prob;

            Program.WriteOutput("Corrected Cephalopod Math Homework Total: " + total);
        }

        private static List<List<string>> BuildProblems()
        {
            using (var sr = Program.GetReader(FileLocation))
            {
                var problems = new List<List<string>>();
                var line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    problems.Add(line.Split(Array.Empty<char>(),StringSplitOptions.RemoveEmptyEntries).ToList());
                    line = sr.ReadLine();
                }

                return problems;
            }

        }
    }
}