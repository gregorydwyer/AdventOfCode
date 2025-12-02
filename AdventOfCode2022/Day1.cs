using System;
using System.Collections.Generic;
using System.Linq;

namespace AdevntOfCode2022
{
    public static class Day1
    {
        public static void Problem1()
        {
            Console.WriteLine("D1 P1");
            var line = Console.ReadLine();
            var max = 0;
            var current = 0;
            while (line != null && !line.Equals("q"))
            {
                if (int.TryParse(line, out var parsed))
                {
                    current += parsed;
                }
                else
                {
                    max = Math.Max(max, current);
                    current = 0;
                }
                line = Console.ReadLine();
            }

            Console.WriteLine("Max value: " + max);
            Console.ReadKey();
        }

        public static void Problem2()
        {
            Console.WriteLine("D1 P2");
            var line = Console.ReadLine();
            var max = new List<int> {0,0,0};
            var current = 0;
            while (line != null && !line.Equals("q"))
            {
                if (int.TryParse(line, out var parsed))
                {
                    current += parsed;
                }
                else
                {
                    if(current > max[0])
                    {
                        max[0] = current;
                        max.Sort();
                    }
                    current = 0;
                }
                line = Console.ReadLine();
            }

            Console.WriteLine("Top 3 total: " + max.Sum());
            Console.ReadKey();
        }
    }
}