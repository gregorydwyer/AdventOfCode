using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AdventOfCode2024
{
    public class Day11
    {
        private const string Day = "Day11";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";
        private static Dictionary<(bool, bool, string), long> FftDacOutPaths = new Dictionary<(bool, bool, string), long>();

        public static void Run()
        {
            Program.WriteTitle("--- Day 11: Reactor ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var map = BuildMap();
            var count = map["you"].OutPaths();
            Program.WriteOutput("Unique Paths to OUT: " + count);
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var map = BuildMap();
            var count = map["svr"].OutPathsWithFftDac(FftDacOutPaths);
            Program.WriteOutput("Unique Paths to OUT: " + count);
        }

        private static Dictionary<string, Node> BuildMap()
        {
            var nodeDict = new Dictionary<string, Node>();
            var connDict = new Dictionary<string, string>();
            nodeDict.Add("out", new Node(){Name = "out"});
            using (var sr = Program.GetReader(FileLocation))
            {
                var line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var split = line.Split(':');
                    var node = new Node();
                    node.Name = split[0];
                    nodeDict.Add(node.Name, node);
                    connDict.Add(node.Name, split[1].Trim());
                    line = sr.ReadLine();
                }
            }

            foreach (var kvp in connDict)
            {
                var node = nodeDict[kvp.Key];
                var split = kvp.Value.Split(' ');
                foreach (var s in split)
                {
                    var conn = nodeDict[s];
                    node.Connections.Add(conn.Name, conn);
                }
            }

            return nodeDict;
        }
    }

    public class Node
    {
        public string Name;
        public Dictionary<string, Node> Connections = new Dictionary<string, Node>();
        private long? OutCount = null;

        public long OutPaths()
        {
            if (OutCount != null)
            {
                return OutCount.Value;
            }

            if (Connections.ContainsKey("out"))
            {
                OutCount = 1;
                return 1;
            }
            else
            {
                var count = 0L;
                foreach (var connection in Connections)
                {
                    count += connection.Value.OutPaths();
                }

                OutCount = count;
                return count;
            }
        }

        public long OutPathsWithFftDac(Dictionary<(bool, bool, string), long> dict)
        {
            return OutPathsWithFftDac(false, false, dict);
        }

        public long OutPathsWithFftDac(bool fft, bool dac, Dictionary<(bool, bool, string), long> dict)
        {
            var count = 0L;
            if (Name == "fft")
            {
                fft = true;
            }

            if (Name == "dac")
            {
                dac = true;
            }

            if (dict.ContainsKey((fft, dac, Name)))
            {
                count = dict[(fft, dac, Name)];
            }
            else if (fft && dac)
            {
                count = OutPaths();
            }
            else if (Connections.ContainsKey("out"))
            {
                count = 0;
            }
            else
            {
                foreach (var connection in Connections)
                {
                    count += connection.Value.OutPathsWithFftDac(fft, dac, dict);
                }

                dict.Add((fft, dac, Name), count);
            }
            return count;
        }

    }
}