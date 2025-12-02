

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2023
{
    public class Day04
    {
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        public static void Problem1()
        {
            Console.WriteLine("Day04 P1");
            using (var stream = new StreamReader(new FileStream("Day04.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                var finalTotal = 0;
                while (line != null)
                {

                    var round = line.Split(':')[1].Split('|');
                    var win = round[0].Trim().Split(' ').Where(num => !string.IsNullOrEmpty(num)).ToHashSet();
                    var yours = round[1].Trim().Split(' ').Where(num => !string.IsNullOrEmpty(num)).ToHashSet();
                    win.IntersectWith(yours);
                    var temp = round[0].Trim().Split(' ').ToHashSet();

                    if (win.Count > 0)
                    {
                        finalTotal += (int) Math.Pow(2, win.Count - 1);
                    }
                    line = stream.ReadLine();
                }
                Console.WriteLine("Total: " + finalTotal);
            }
        }
        public static void Problem2()
        {
            Console.WriteLine("Day04 P2");
            var finalTotal = 0;
            var winDict = BuildWinDictionary();
            var winList = new Queue<int>();
            foreach (var win in winDict)
            {
                finalTotal++;
                if (win.Value > 0)
                {
                    for (int i = 1; i <= win.Value; i++)
                    {
                        winList.Enqueue(win.Key + i);
                    }
                }
            }

            while (winList.Count > 0)
            {
                var win = winList.Dequeue();
                finalTotal++;
                if (winDict[win] > 0)
                {
                    for (int i = 1; i <= winDict[win]; i++)
                    {
                        winList.Enqueue(win + i);
                    }
                }
            }
            Console.WriteLine("Total: " + finalTotal);
        }

        private static Dictionary<int, int> BuildWinDictionary()
        {
            using (var stream = new StreamReader(new FileStream("Day04.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                var finalTotal = 0;
                var dict = new Dictionary<int, int>();
                var cardNum = 1;
                while (line != null)
                {

                    var round = line.Split(':')[1].Split('|');
                    var win = round[0].Trim().Split(' ').Where(num => !string.IsNullOrEmpty(num)).ToHashSet();
                    var yours = round[1].Trim().Split(' ').Where(num => !string.IsNullOrEmpty(num)).ToHashSet();
                    win.IntersectWith(yours);

                    dict.Add(cardNum, win.Count);
                    cardNum++;
                    line = stream.ReadLine();
                }

                return dict;
            }
        }
    }
}
