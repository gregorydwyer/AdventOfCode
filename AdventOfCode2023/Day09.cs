using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace AdventOfCode2023
{
    public class Day09
    {
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        public static void Problem1()
        {
            Console.WriteLine("Day09 P1");
            using (var stream = new StreamReader(new FileStream("Day09.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                long finalTotal = 0;
                while (line != null)
                {
                    var input = line.Split(' ').Select(str => long.Parse(str)).ToList();
                    var next = GetNextVal(input);
                    finalTotal += next;
                    line = stream.ReadLine();
                }
                Console.WriteLine($"Total: {finalTotal}");
            }
        }

        private static long GetNextVal(List<long> list)
        {
            var diffList = new List<long>();
            for (int i = 1; i < list.Count; i++)
            {
                diffList.Add(list[i] - list[i-1]);
            }

            if (diffList.Any(num => num != 0))
            {
                return list.Last() + GetNextVal(diffList);
            }

            return list.Last();
        }

        private static long GetPreviousVal(List<long> list)
        {
            var diffList = new List<long>();
            for (int i = 1; i < list.Count; i++)
            {
                diffList.Add(list[i] - list[i - 1]);
            }

            if (diffList.Any(num => num != 0))
            {
                return list.First() - GetPreviousVal(diffList);
            }

            return list.First();
        }

        public static void Problem2()
        {
            Console.WriteLine("Day09 P2");
            using (var stream = new StreamReader(new FileStream("Day09.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                long finalTotal = 0;
                while (line != null)
                {
                    var input = line.Split(' ').Select(str => long.Parse(str)).ToList();
                    var next = GetPreviousVal(input);
                    finalTotal += next;
                    line = stream.ReadLine();
                }
                Console.WriteLine($"Total: {finalTotal}");
            }
        }
    }
}