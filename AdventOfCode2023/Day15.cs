using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2023
{
    public class Day15
    {
        private const string FileName = "Day15.txt";
        private const string Day = "Day 15";
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        public static void Problem1()
        {
            Console.WriteLine($"{Day} P1");
            var input = File.ReadAllLines(FileName);
            var sequences = input[0].Split(',');
            var finalTotal = 0L;
            foreach (var str in sequences)
            {
                finalTotal += GetHash(str);
            }
            Console.WriteLine($"Total: {finalTotal}");
        }

        private static int GetHash(string st)
        {
            var val = 0;
            foreach (var c in st)
            {
                val += (int) c;
                val *= 17;
                val = val % 256;
            }
            return val;
        }

        public static void Problem2()
        {
            Console.WriteLine($"{Day} P2");
            var input = File.ReadAllLines(FileName);
            var sequences = input[0].Split(',');
            var dict = new Dictionary<int, List<Lens>>();
            var finalTotal = 0L;
            foreach (var str in sequences)
            {
                var split = str.Split(new char[]{'-','='});
                var key = GetHash(split[0]);
                if (string.IsNullOrEmpty(split[1]))
                {
                    //remove lens
                    if (dict.ContainsKey(key))
                    {
                        dict[key].RemoveAll(lens => lens.Label == split[0]);
                    }
                }
                else
                {
                    //insert/replace lens
                    if (dict.ContainsKey(key))
                    {
                        if (dict[key].Any(lens => lens.Label == split[0]))
                        {
                            dict[key].First(lens => lens.Label == split[0]).Length = int.Parse(split[1]);
                        }
                        else
                        {
                            dict[key].Add(new Lens(split[0], int.Parse(split[1])));
                        }
                    }
                    else
                    {
                        dict.Add(key, new List<Lens>() { new Lens(split[0], int.Parse(split[1])) });
                    }
                }
            }

            foreach (var kvp in dict)
            {
                var count = 1;
                foreach (var lens in kvp.Value)
                {
                    var current = (kvp.Key + 1) * count * lens.Length;
                    finalTotal += current;
                    count++;
                }
            }
            Console.WriteLine($"Total: {finalTotal}");
        }

        private class Lens
        {
            public int Length;
            public string Label;

            public Lens(string label, int length)
            {
                Label = label;
                Length = length;
            }

            public override string ToString()
            {
                return $"{Label}, {Length}";
            }
        }

    }
}