using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2023
{
    public class Day06
    {
        private static List<int> Times = new List<int>() {46, 85, 75, 82};//{ 7, 15, 30 };
        private static List<int> Distances = new List<int>() { 208, 1412, 1257, 1410 };//{ 9, 40, 200 };
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        public static void Problem1()
        {
            Console.WriteLine("Day06 P1");
            var finalTotal = 1;
            var winCount = 0;
            for (int race = 0; race < Times.Count; race++)
            {
                for (int ms = 0; ms < Times[race]; ms++)
                {
                    if (ms * (Times[race] - ms) > Distances[race])
                    {
                        winCount++;
                    }
                }

                finalTotal *= winCount;
                winCount = 0;
            }

            Console.WriteLine("Total:" + finalTotal);

        }

        public static void Problem2()
        {
            Console.WriteLine("Day06 P2");
            var finalTotal = 1;
            var winCount = 0;
            for (long ms = 0; ms < 46857582; ms++)
            {
                if (ms * (46857582 - ms) > 208141212571410)
                {
                    winCount++;
                }
            }
            Console.WriteLine("Total:" + winCount);
        }
    }
}