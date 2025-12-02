using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace AdventOfCode2023
{
    public class Day12
    {
        private static long Count = 0;
        private static Dictionary<(string, string), long> Results = new Dictionary<(string, string), long>();
        private static List<long> New = new List<long>();
        private static List<long> Old = new List<long>();

        public static void Run()
        {
            P1();
            P2();
            Console.ReadKey();
        }

        private static void P1()
        {
            Console.WriteLine("Day12 P1");
            using (var stream = new StreamReader(new FileStream("Day12.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                var finalTotal = 0L;
                var lineCount = 0;
                while (line != null)
                {
                    var split = line.Split(' ');
                    var springs = split[0];
                    var list = split[1].Trim().Split(',').Select(str => int.Parse(str));
                    var order = new Queue<int>();
                    foreach (var i in list)
                    {
                        order.Enqueue(i);
                    }
                    var temp = Process(springs, order);
                    New.Add(temp);
                    finalTotal += temp;
                    line = stream.ReadLine();
                    lineCount++;
                }
                var test = Results[(".???#", "1,2")];
                Console.WriteLine("Total: " + finalTotal);
            }
        }

        private static void P2()
        {
            Console.WriteLine("Day12 P2");
            using (var stream = new StreamReader(new FileStream("Day12.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                var finalTotal = 0L;
                var lineCount = 0;
                while (line != null)
                {
                    var split = line.Split(' ');
                    var springs = split[0] + "?" + split[0] + "?" + split[0] + "?" + split[0] + "?" + split[0];
                    var list = split[1].Trim().Split(',').Select(str => int.Parse(str));
                    var order = new Queue<int>();
                    for (int r = 0; r < 5; r++)
                    {
                        foreach (var i in list)
                        {
                            order.Enqueue(i);
                        }
                    }
                    var temp = Process(springs, order);
                    New.Add(temp);
                    finalTotal += temp;
                    line = stream.ReadLine();
                    lineCount++;
                }
                var test = Results[(".???#", "1,2")];
                Console.WriteLine("Total: " + finalTotal);
            }
        }

        private static long Process(string springs, Queue<int> order)
        {
            if (Results.ContainsKey((springs, OrderString(order))))
            {
                return Results[(springs, OrderString(order))];
            }

            if (order.Count == 0)
            {
                return springs.Contains("#") ? 0 : 1;
            }

            if (springs.Length == 0)
            {
                return 0;
            }

            if (springs[0] == '.')
            {
                if(!Results.ContainsKey((springs.Substring(1), OrderString(order))))
                {
                    Results.Add((springs.Substring(1), OrderString(order)), Process(springs.Substring(1), new Queue<int>(order)));
                }

                return Results[(springs.Substring(1), OrderString(order))];
            }

            if (springs[0] == '#')
            {
                var set = order.Dequeue();
                //match damaged spring count to section
                if (set > springs.Length)
                {
                    return 0;
                }

                for (int i = 0; i < set; i++)
                {
                    if (springs[i] != '#' && springs[i] != '?')
                    {
                        return 0;
                    }
                }

                if (order.Count == 0 && springs.Length == set)
                {
                    return 1;
                }

                // there must be a non damaged spring after a set
                if (springs.Length == set || springs[set] == '#')
                {
                    return 0;
                }

                var newSprings = springs.Substring(set + 1);
                if(!Results.ContainsKey((newSprings, OrderString(order))))
                {
                    Results.Add((newSprings, OrderString(order)), Process(springs.Substring
                    (set + 1), new Queue<int>(order)));
                }

                return Results[(newSprings, OrderString(order))];
            }
            
            // try it as good
            var goodSpring = springs.ToCharArray();
            goodSpring[0] = '.';
            if (!Results.ContainsKey((new string(goodSpring), OrderString(order))))
            {
                var goodResult = Process(new string(goodSpring), new Queue<int>(order));
                Results.Add((new string(goodSpring), OrderString(order)), goodResult);
            }

            var badSpring = springs.ToCharArray();
            badSpring[0] = '#';
            if (!Results.ContainsKey((new string(badSpring), OrderString(order))))
            {
                var badResult = Process(new string(badSpring), new Queue<int>(order));
                Results.Add((new string(badSpring), OrderString(order)), badResult);
            }

            return Results[(new string(goodSpring), OrderString(order))] +
                   Results[(new string(badSpring), OrderString(order))];
        }

        private static string OrderString(Queue<int> order)
        {
            var array = new int[order.Count];
            order.CopyTo(array,0);
            return string.Join(",", array);
        }

        public static void Problem1()
        {
            Console.WriteLine("Day12 P1");
            using (var stream = new StreamReader(new FileStream("Day12.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                var finalTotal = 0L;
                var count = 0;
                while (line != null)
                {
                    var split = line.Split(' ');
                    var springs = split[0].Trim('.');
                    var order = split[1].Trim().Split(',').Select(str => int.Parse(str)).ToArray();
                    var temp = Permutations(springs, order);
                    Old.Add(temp);
                    finalTotal += temp;
                    line = stream.ReadLine();
                    count++;
                    if (count % 100 == 0)
                    {
                        Console.Write("*");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Total: " + Count);
            }
        }

        private static long Permutations(string springs, int[] order)
        {
            //ReplaceUnknowns(springs, 0, order);
            //return Count;
            var count = 0L;
            var hash = new HashSet<string>();
            ReplaceUnknowns(springs, 0, hash);
            foreach (var str in hash)
            {
                var match = true;
                var split = str.Split('.').Where(st => !string.IsNullOrEmpty(st)).ToArray();
                if (split.Length == order.Length)
                {
                    for (int j = 0; j < order.Length && match; j++)
                    {
                        if (order[j] != split[j].Length)
                        {
                            match = false;
                        }
                    }
                }
                else
                {
                    match = false;
                }

                if (match)
                {
                    match = true;
                }
                count += match ? 1 : 0;
            }

                return count;
        }

        private static void ReplaceUnknowns(string str, int i, int[] order)
        {
            if (true)
            {
                var subStr = str.Substring(0, i);
                if (subStr.Length > 0 && subStr.EndsWith("."))
                {
                    var groups = subStr.Split('.').Where(s => !string.IsNullOrEmpty(s)).ToArray();
                    if (groups.Length > order.Length)
                    {
                        return;
                    }
                    for (int k = 0; k < groups.Length; k++)
                    {
                        if (groups[k].Length != order[k])
                        {
                            return;
                        }
                    }

                    var spaceNeeded = 0;
                    for (int k = groups.Length; k < order.Length; k++)
                    {
                        spaceNeeded += k == order.Length - 1 ? order[k] : order[k] + 1;
                    }

                    if (str.Length - i < spaceNeeded)
                    {
                        return;
                    }
                }
            }

            if (i >= str.Length)
            {
                if (CheckString(str, order))
                {
                    Count++;
                }
                return;
            }

            if (str[i] == '?')
            {
                var str2 = str.ToCharArray();
                str2[i] = '.';
                 ReplaceUnknowns(new string(str2), i + 1, order);
                str2[i] = '#';
                ReplaceUnknowns(new string(str2), i + 1, order);
                return;
            }

            ReplaceUnknowns(str, i + 1, order);
        }

        private static bool CheckString(string str, int[] order)
        {
            var match = true;
            var split = str.Split('.').Where(st => !string.IsNullOrEmpty(st)).ToArray();
            if (split.Length == order.Length)
            {
                for (int j = 0; j < order.Length && match; j++)
                {
                    if (order[j] != split[j].Length)
                    {
                        match = false;
                    }
                }
            }
            else
            {
                return false;
            }

            return match;
        }

        private static void ReplaceUnknowns(string str, int i, HashSet<string> hash)
        {
            if (i >= str.Length)
            {
                hash.Add(str);
                return;
            }

            if (str[i] == '?')
            {
                var str2 = str.ToCharArray();
                str2[i] = '.';
                ReplaceUnknowns(new string(str2), i + 1, hash);
                str2[i] = '#';
                ReplaceUnknowns(new string(str2), i + 1, hash);
                return;
            }

            ReplaceUnknowns(str, i + 1, hash);
        }

        public static void Problem2Threaded()
        {
            Console.WriteLine("Day12 P2");
            var input = File.ReadAllLines("Day12.txt");
            var array1 = new string[100];
            Array.Copy(input, 0, array1, 0, 100 );
            var t1 = new Thread(() => Problem2(array1));
            var array2 = new string[100];
            Array.Copy(input, 100, array2, 0, 100);
            var t2 = new Thread(() => Problem2(array2));
            var array3 = new string[100];
            Array.Copy(input, 200, array3, 0, 100);
            var t3 = new Thread(() => Problem2(array3));
            var array4 = new string[100];
            Array.Copy(input, 300, array4, 0, 100);
            var t4 = new Thread(() => Problem2(array4));
            var array5 = new string[100];
            Array.Copy(input, 400, array5, 0, 100);
            var t5 = new Thread(() => Problem2(array5));
            var array6 = new string[100];
            Array.Copy(input, 500, array6, 0, 100);
            var t6 = new Thread(() => Problem2(array6));
            var array7 = new string[100];
            Array.Copy(input, 600, array7, 0, 100);
            var t7 = new Thread(() => Problem2(array7));
            var array8 = new string[100];
            Array.Copy(input, 700, array8, 0, 100);
            var t8 = new Thread(() => Problem2(array8));
            var array9 = new string[100];
            Array.Copy(input, 800, array9, 0, 100);
            var t9 = new Thread(() => Problem2(array9));
            var array0 = new string[100];
            Array.Copy(input, 900, array0, 0, 100);
            var t0 = new Thread(() => Problem2(array0));
            var threads = new List<Thread>();
            threads.Add(t0);
            threads.Add(t1);
            threads.Add(t2);
            threads.Add(t3);
            threads.Add(t4);
            threads.Add(t5);
            threads.Add(t6);
            threads.Add(t7);
            threads.Add(t8);
            threads.Add(t9);

            threads.ForEach(t => t.Start());
            while (threads.Any(t => t.IsAlive))
            {

            }
            Console.WriteLine($"Count: {Count}");
            return;
            using (var stream = new StreamReader(new FileStream("Day12Test.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                //var finalTotal = 0L;
                var count = 0;
                var sw = new Stopwatch();
                long elapsed = 0;
                while (line != null)
                {
                    sw.Restart();
                    var split = line.Split(' ');
                    var springs = split[0] + "?" + split[0] + "?" + split[0] + "?" + split[0] + "?" + split[0];
                    springs.TrimStart(new char[] {'.'});
                    var orderbig = split[1].Trim() + "," + split[1].Trim() + "," + split[1].Trim() + "," +
                                   split[1].Trim() + "," + split[1].Trim();
                    var order = orderbig.Split(',').Select(int.Parse).ToArray();
                    ReplaceUnknowns(springs, 0, order);
                    //finalTotal += Permutations(springs, order);
                    line = stream.ReadLine();
                    count++;
                    Console.Write("*");
                    sw.Stop();
                    elapsed += sw.ElapsedMilliseconds / 1000;
                    long temp;
                    if (count % 10 == 0)
                    {
                        temp = elapsed / count;
                        Console.WriteLine($"Average time over {count} runs: {temp} seconds");
                    }

                    temp = Count;
                }
                Console.WriteLine($"Total time (minutes): {elapsed / 60}");
                Console.WriteLine("Total: " + Count);
            }
        }

        private static void Problem2()
        {
            using (var stream = new StreamReader(new FileStream("Day12Test.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                //var finalTotal = 0L;
                var count = 0;
                var sw = new Stopwatch();
                long elapsed = 0;
                while (line != null)
                {
                    sw.Restart();
                    var split = line.Split(' ');
                    var springs = split[0] + "?" + split[0] + "?" + split[0] + "?" + split[0] + "?" + split[0];
                    springs.TrimStart(new char[] { '.' });
                    var orderbig = split[1].Trim() + "," + split[1].Trim() + "," + split[1].Trim() + "," +
                                   split[1].Trim() + "," + split[1].Trim();
                    var order = orderbig.Split(',').Select(int.Parse).ToArray();
                    ReplaceUnknowns(springs, 0, order);
                    //finalTotal += Permutations(springs, order);
                    line = stream.ReadLine();
                    count++;
                    Console.Write("*");
                    sw.Stop();
                    elapsed += sw.ElapsedMilliseconds / 1000;
                    long temp;
                    if (count % 10 == 0)
                    {
                        temp = elapsed / count;
                        Console.WriteLine($"Average time over {count} runs: {temp} seconds");
                    }

                    temp = Count;
                }
                Console.WriteLine($"Total time (minutes): {elapsed / 60}");
                Console.WriteLine("Total: " + Count);
            }
        }

        private static void Problem2(string[] lines)
        {
            var count = 0;
            foreach (var line in lines)
            {
                var split = line.Split(' ');
                var springs = split[0] + "?" + split[0] + "?" + split[0] + "?" + split[0] + "?" + split[0];
                springs.TrimStart(new char[] { '.' });
                var orderbig = split[1].Trim() + "," + split[1].Trim() + "," + split[1].Trim() + "," +
                               split[1].Trim() + "," + split[1].Trim();
                var order = orderbig.Split(',').Select(int.Parse).ToArray();
                ReplaceUnknowns(springs, 0, order);
                count++;
                if (count % 10 == 0)
                {
                    Console.Write("*");
                }
            }
            Console.WriteLine("Thread complete!");
        }
    }
}