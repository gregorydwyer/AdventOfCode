using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2023
{
    public class Day08
    {
        private static string Order;
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        public static void Problem1()
        {
            Console.WriteLine("Day08 P1");
            var map = BuildMap();
            var loc = "AAA";
            var steps = 0;
            var len = Order.Length;
            while (loc != "ZZZ")
            {
                var direction = Order[steps % len];
                if (direction == 'L')
                {
                    loc = map[loc].L;
                }
                else
                {
                    loc = map[loc].R;
                }

                steps++;
            }

            Console.WriteLine("Total Steps: " + steps);

        }

        private static Dictionary<string, Node> BuildMap()
        {
            var dict = new Dictionary<string, Node>();
            using (var stream = new StreamReader(new FileStream("Day08.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                Order = line;
                _ = stream.ReadLine();
                line = stream.ReadLine();
                while (line != null)
                {
                    dict.Add(line.Substring(0,3), new Node(line.Substring(7,3), line.Substring(12,3)));

                    line = stream.ReadLine();
                }
            }

            return dict;
        }

        public static void Problem2()
        {
            //TODO: Brute force will NOT work here. Over 14 trillion steps are required. This method will take hours to complete.
            Console.WriteLine("Day08 P2");
            var map = BuildMap();
            var locs = map.Where(kvp => kvp.Key[2] == 'A').Select(kvp => kvp.Key).ToList();
            long steps = 0;
            var len = Order.Length;
            while (locs.Any(l => l[2] != 'Z'))
            {
                var direction = Order[(int) (steps % len)];
                for (int i = 0; i < locs.Count; i++)
                {
                    if (direction == 'L')
                    {
                        locs[i] = map[locs[i]].L;
                    }
                    else
                    {
                        locs[i] = map[locs[i]].R;
                    }
                }
                steps++;
                if (steps % 100000000 == 0)
                {
                    Console.WriteLine(steps);
                }
            }

            Console.WriteLine("Total Steps: " + steps);
        }

        private class Node
        {
            public string L;
            public string R;

            public Node(string l, string r)
            {
                L = l;
                R = r;
            }

            public override string ToString()
            {
                return $"({L},{R})";
            }
        }
    }
}