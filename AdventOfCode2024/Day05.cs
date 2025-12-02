using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace AdventOfCode2024
{
    public class Day05
    {
        private const string Day = "Day05";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";

        public static void Run()
        {
            Program.WriteTitle("--- Day 5: Print Queue ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            using (var reader = Program.GetReader(FileLocation))
            {
                var total = 0;
                var rules = BuildRules(reader);
                var currentLine = reader.ReadLine();
                while (currentLine != null)
                {
                    var update = currentLine.Split(',').Select(int.Parse).ToArray();
                    var printedPages = new HashSet<int>();
                    var isBad = false;
                    for (int i = 0; i < update.Length; i++)
                    {
                        if (isBad)
                        {
                            break;
                        }

                        printedPages.Add(update[i]);
                        foreach (var page in printedPages)
                        {
                            if (rules.ContainsKey(update[i])
                                && rules[update[i]].Contains(page))
                            {
                                isBad = true;
                                break;
                            }
                        }
                    }

                    if (!isBad)
                    {
                        total += update[update.Length / 2];
                    }
                    currentLine = reader.ReadLine();
                }

                Program.WriteOutput("Total: " + total);
            }
        }

        private static Dictionary<int, HashSet<int>> BuildRules(StreamReader reader)
        {
            var dict = new Dictionary<int, HashSet<int>>();
            var currentLine = reader.ReadLine();

            while (currentLine?.Contains("|") == true)
            {
                var split = currentLine.Split('|').Select(int.Parse).ToArray();
                if (dict.ContainsKey(split[0]))
                {
                    dict[split[0]].Add(split[1]);
                }
                else
                {
                    dict.Add(split[0], new HashSet<int>(){split[1]});
                }
                currentLine = reader.ReadLine();
            }

            return dict;
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            using (var reader = Program.GetReader(FileLocation))
            {
                var total = 0;
                var rules = BuildRules(reader);
                var currentLine = reader.ReadLine();
                while (currentLine != null)
                {
                    var update = currentLine.Split(',').Select(int.Parse).ToList();
                    var printedPages = new HashSet<int>();
                    var isBad = false;
                    for (int i = 0; i < update.Count; i++)
                    {
                        printedPages.Add(update[i]);
                        foreach (var page in printedPages)
                        {
                            if (rules.ContainsKey(update[i])
                                && rules[update[i]].Contains(page))
                            {
                                isBad = true;
                            }
                        }
                    }

                    if (isBad)
                    {
                        var numList = update.Select(u => new PageNumber(u, rules.ContainsKey(u) ? rules[u] : new HashSet<int>())).ToList();
                        numList.Sort();
                        total += numList[update.Count / 2].Num;
                    }
                    currentLine = reader.ReadLine();
                }

                Program.WriteOutput("Total: " + total);

            }
        }
    }
    public class PageNumber : IComparable<PageNumber>
    {
        public int Num;
        public HashSet<int> Rule;

        public PageNumber(int num, HashSet<int> rule)
        {
            Num = num;
            Rule = rule;
        }

        public int CompareTo(PageNumber other)
        {
            if (Rule.Contains(other.Num))
            {
                return -1;
            }

            if (other.Rule.Contains(Num))
            {
                return 1;
            }

            return 0;
        }
    }
}