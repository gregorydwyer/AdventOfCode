using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;

namespace AdventOfCode2023
{
    public class Day05
    {
        private static HashSet<long> mins = new HashSet<long>();
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        public static void Problem1()
        {
            Console.WriteLine("Day05 P1");
            var map = BuildMap(out var seeds);
            var destinations = new List<long>();
            foreach (var seed in seeds)
            {
                destinations.Add(FindDest(seed, map));
            }
            destinations.Sort();
            Console.WriteLine("Min: " + destinations.First());
        }

        private static long FindDest(long seed, LinkedList<List<MapItem>> map)
        {
            var node = map.First;
            var value = seed;
            while (node != null)
            {
                foreach (var item in node.Value)
                {
                    var itemMax = item.InStart + item.Range - 1;
                    if (value >= item.InStart && value <= itemMax)
                    {
                        var offset = value - item.InStart;
                        value = item.OutStart + offset;
                        break;
                    }
                }

                node = node.Next;
            }

            return value;
        }

        public static void Problem2()
        {
            Console.WriteLine("Day05 P2");
            var map = BuildMap(out var seeds);
            var threads = new List<Thread>();
            for (int i = 0; i <=18; i += 2)
            {
                var temp = seeds[i];
                var temp2 = seeds[i + 1];
                var thread = new Thread(() => FindMin(temp, temp2, map));
                thread.Start();
                threads.Add(thread);
            }
            while(threads.Any(th => th.IsAlive)){}
            var min = mins.Min();
            Console.WriteLine("Min: " + min);
        }

        private static void FindMin(long start, long range, LinkedList<List<MapItem>> map)
        {
            var min = long.MaxValue;
            for (int j = 0; j < range; j++)
            {
                var dest = FindDest(start + j, map);
                min = dest < min ? dest : min;
            }

            mins.Add(min);
        }

        private static LinkedList<List<MapItem>> BuildMap(out List<long> seeds)
        {
            using (var stream = new StreamReader(new FileStream("Day05.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                var map = new LinkedList<List<MapItem>>();
                seeds = new List<long>();
                var currentList = new List<MapItem>();
                while (line != null)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        line = stream.ReadLine();
                        continue;
                    }

                    if (line.Contains(":"))
                    {
                        if(seeds.Count == 0)
                        {
                            //Seeds line
                            seeds = line.Split(':')[1].Trim().Split(' ').Select(st => long.Parse(st)).ToList();
                        }

                        if (currentList.Any())
                        {
                            map.AddLast(currentList.OrderBy(item => item.InStart).ToList());
                            currentList = new List<MapItem>();
                        }
                        line = stream.ReadLine();
                        continue;
                    }

                    var mapLine = line.Trim().Split(' ').Select(long.Parse).ToArray();
                    var currentMap = new MapItem(mapLine[1], mapLine[0], mapLine[2]);
                    currentList.Add(currentMap);
                    line = stream.ReadLine();
                }

                if (currentList.Any())
                {
                    map.AddLast(currentList.OrderBy(item => item.InStart).ToList());
                }
                return map;
            }
        }

        private struct MapItem
        {
            public long InStart;
            public long OutStart;
            public long Range;

            public MapItem(long inStart, long ourStart, long range)
            {
                InStart = inStart;
                OutStart = ourStart;
                Range = range;
            }
        }
    }
}