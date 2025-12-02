using System;
using System.CodeDom;
using System.Collections.Generic;

namespace AdventOfCode2024
{
    public class Day01
    {
        private const string Day = "Day01";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";

        public static void Run()
        {
            Program.WriteTitle("--- Day 1: Historian Hysteria ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var col1 = new List<int>();
            var col2 = new List<int>();
            using (var sr = Program.GetReader(FileLocation))
            {
                string currentLine = null;
                do
                {
                    currentLine = sr.ReadLine();
                    var values = currentLine.Split();
                    col1.Add(int.Parse(values[0]));
                    col2.Add(int.Parse(values[3]));
                } while (!sr.EndOfStream);
                col1.Sort();
                col2.Sort();
                var totalDistance = 0;
                for (int i = 0; i < col1.Count; i++)
                {
                    totalDistance += Math.Abs(col1[i] - col2[i]);
                }

                Program.WriteOutput("Total Distance: "+totalDistance);
            }
        }


        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var col1 = new List<int>();
            var dict = new Dictionary<int, int>();
            using (var sr = Program.GetReader(FileLocation))
            {
                string currentLine = null;
                do
                {
                    currentLine = sr.ReadLine();
                    var values = currentLine.Split();
                    col1.Add(int.Parse(values[0]));
                    var col2 = int.Parse(values[3]);
                    if (dict.ContainsKey(col2))
                    {
                        dict[col2]++;
                    }
                    else
                    {
                        dict.Add(col2, 1);
                    }
                } while (!sr.EndOfStream);
                var totalSimilarity = 0;
                for (int i = 0; i < col1.Count; i++)
                {
                    if (dict.ContainsKey(col1[i]))
                    {
                        totalSimilarity += col1[i] * dict[col1[i]];
                    }
                }

                Program.WriteOutput("Total Similarity: " + totalSimilarity);
            }
        }
    }
}