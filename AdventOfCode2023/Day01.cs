using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2023
{
    public static class Day01
    {
        private static string[] numberStrings = {"one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        public static void Problem1()
        {
            Console.WriteLine("D01 P1");
            using (var stream = new StreamReader(new FileStream("Day01.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                var finalTotal = 0;
                while (line != null)
                {
                    var first = line.First(c => char.IsNumber(c));
                    var last = line.Last(c => char.IsNumber(c));
                    var lineVal = int.Parse(""+ first + last);
                    finalTotal += lineVal;
                    line = stream.ReadLine();
                }
                Console.WriteLine("Total: " + finalTotal);
            }
        }
        public static void Problem2()
        {
            Console.WriteLine("D01 P2");
            using (var stream = new StreamReader(new FileStream("Day01.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                var finalTotal = 0;
                while (line != null)
                {
                    var numsInLine = new Dictionary<int, int>();
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (char.IsNumber(line[i]))
                        {
                            numsInLine.Add(i, int.Parse(line[i].ToString()));
                        }
                    }
                    var num = 1;
                    foreach (var str in numberStrings)
                    {
                        for (int i = 0; i < line.Length - str.Length + 1; i++)
                        {
                            var sub = line.Substring(i);
                            if (sub.StartsWith(str))
                            {
                                numsInLine.Add(i, num);
                            }
                        }
                        num++;
                    }

                    var ordered = numsInLine.OrderBy(kvp => kvp.Key).ToList();
                    var lineVal = int.Parse("" + ordered.First().Value.ToString() + ordered.Last().Value.ToString());
                    finalTotal += lineVal;
                    line = stream.ReadLine();
                }
                Console.WriteLine("Total: " + finalTotal);
            }
        }
    }
}