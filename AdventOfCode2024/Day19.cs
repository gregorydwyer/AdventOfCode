using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024
{
    public class Day19
    {
        private const string Day = "Day19";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";
        private static HashSet<string> Towels;
        private static Stack<string> PossiblePatterns = new Stack<string>();
        private static int MaxLength = 1;
        private static Dictionary<string, long> Combos = new Dictionary<string, long>();

        public static void Run()
        {
            Program.WriteTitle("--- Day 19: Linen Layout ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var patterns = BuildInput();
            var count = 0;
            foreach (var pattern in patterns)
            {
                if (IsPatternPossible(pattern))
                {
                    PossiblePatterns.Push(pattern);
                    count++;
                }
            }
            Program.WriteOutput($"Possible Patterns: {count}");
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var count = 0L;
            while(PossiblePatterns.Count > 0)
            {
                count += CountPossibleCombinations(PossiblePatterns.Pop());
            }
            Program.WriteOutput($"Possible combinations: {count}");
        }

        private static bool IsPatternPossible(string pattern)
        {
            var substrings = new Stack<string>();
            for (int i = 1; i <= MaxLength && i <= pattern.Length; i++)
            {
                if (Towels.Contains(pattern.Substring(0, i)))
                {
                    var substring = pattern.Substring(i);
                    if (string.IsNullOrEmpty(substring))
                    {
                        return true;
                    }
                    substrings.Push(substring);
                }
            }

            while (substrings.Count > 0)
            {
                var next = substrings.Pop();
                if (IsPatternPossible(next))
                {
                    return true;
                }
            }

            return false;
        }

        private static long CountPossibleCombinations(string pattern)
        {
            if (Combos.ContainsKey(pattern))
            {
                return Combos[pattern];
            }
            var substrings = new Stack<string>();
            var total = 0L;
            for (int i = 1; i <= MaxLength && i <= pattern.Length; i++)
            {
                if (Towels.Contains(pattern.Substring(0, i)))
                {
                    var substring = pattern.Substring(i);
                    if (string.IsNullOrEmpty(substring))
                    {
                        total++;
                    }
                    substrings.Push(substring);
                }
            }

            while (substrings.Count > 0)
            {
                var next = substrings.Pop();
                total += CountPossibleCombinations(next);
            }

            Combos.Add(pattern, total);
            return total;
        }

        private static List<string> BuildInput()
        {
            using (var reader = Program.GetReader(FileLocation))
            {
                var patterns = new List<string>();
                var currentLine = reader.ReadLine();
                while (!string.IsNullOrEmpty(currentLine))
                {
                    // build towel list
                    var towelList = currentLine.Split(',').Select(t => t.Trim()).ToList();
                    MaxLength = towelList.Select(t => t.Length).Max();
                    Towels = towelList.ToHashSet();
                    currentLine = reader.ReadLine();
                }

                currentLine = reader.ReadLine();
                while (!string.IsNullOrEmpty(currentLine))
                {
                    // build pattern list
                    patterns.Add(currentLine);
                    currentLine = reader.ReadLine();
                }

                return patterns;
            }
        }
    }
}