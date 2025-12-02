using System;
using System.Numerics;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
    public static class Day11
    {
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        public static void Problem1()
        {
            Console.WriteLine("D11 P1");
            var rounds = 20;
            var monkeys = MakeMonkies();
            for (long i = 0; i < rounds; i++)
            {
                foreach (var monkey in monkeys)
                {
                    var itemcount = monkey.Items.Count;
                    for (long j = 0; j < itemcount; j++)
                    {
                        monkey.InspectedCount++;
                        var item = monkey.Items.Dequeue();
                        item = monkey.DoOperation(item)/3;
                        if (item % monkey.TestInt == 0)
                        {
                            monkeys[monkey.TrueInt].Items.Enqueue(item);
                        }
                        else
                        {
                            monkeys[monkey.FalseInt].Items.Enqueue(item);
                        }
                    }
                }
            }
            var ordered = monkeys.Select(monk => monk.InspectedCount).OrderByDescending(num => num).ToArray();
            Console.WriteLine("Output: " + (ordered[0]*ordered[1]));
        }

        public static void Problem2()
        {
            Console.WriteLine("D11 P2");
            var rounds = 10000;
            var monkeys = MakeMonkies();
            var tests = monkeys.Select(monk => monk.TestInt).ToList();
            var worryAdjustment = 1;
            foreach (var t in tests)
            {
                worryAdjustment *= t;
            }

            for (long i = 1; i <= rounds ; i++)
            {
                foreach (var monkey in monkeys)
                {
                    var itemcount = monkey.Items.Count;
                    for (long j = 0; j < itemcount; j++)
                    {
                        monkey.InspectedCount++;
                        var item = monkey.Items.Dequeue();
                        item = monkey.DoOperation(item) % worryAdjustment;
                        if (item % monkey.TestInt == 0)
                        {
                            monkeys[monkey.TrueInt].Items.Enqueue(item);
                        }
                        else
                        {
                            monkeys[monkey.FalseInt].Items.Enqueue(item);
                        }
                    }
                }

                if (i == 10000)
                {
                    Console.WriteLine("Round:" + i);
                    monkeys.ForEach(monk => Console.WriteLine(monk.InspectedCount));
                }
            }
            var ordered = monkeys.Select(monk => monk.InspectedCount).OrderByDescending(num => num).ToArray();
            long final = ordered[0] * ordered[1];
            Console.WriteLine("Output: " + final);
        }

        private static List<Monkey> MakeMonkies()
        {
            using (var stream = new StreamReader(new FileStream("Day11.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                var monkeys = new List<Monkey>();
                while (line != null)
                {
                    line = line.Trim();
                    if (line.StartsWith("Monkey"))
                    {
                        monkeys.Add(new Monkey());
                    }
                    else if (line.StartsWith("Start"))
                    {
                        var split = line.IndexOf(':');
                        var newLine = line.Remove(0, split + 1);
                        var items = newLine.Split(',');
                        foreach (var item in items)
                        {
                            monkeys.Last().Items.Enqueue(long.Parse(item));
                        }
                    }
                    else if (line.StartsWith("Operation"))
                    {
                        var split = line.IndexOf('=');
                        var newLine = line.Remove(0, split + 2);
                        monkeys.Last().Operation = newLine;
                    }
                    else if (line.StartsWith("Test"))
                    {
                        var parts = line.Split(' ');
                        foreach (var part in parts)
                        {
                            if (int.TryParse(part, out var result))
                            {
                                monkeys.Last().TestInt = result;
                                break;
                            }
                        }
                    }
                    else if (line.StartsWith("If"))
                    {
                        var parts = line.Split(' ');
                        foreach (var part in parts)
                        {
                            if (int.TryParse(part, out var result))
                            {
                                if (line.Contains("true"))
                                {
                                    monkeys.Last().TrueInt = result;
                                }
                                else if (line.Contains("false"))
                                {
                                    monkeys.Last().FalseInt = result;
                                }
                                break;
                            }
                        }
                    }

                    line = stream.ReadLine();
                }
                return monkeys;
            }
        }
    }

    public class Monkey
    {
        public Queue<long> Items = new Queue<long>();
        public long InspectedCount;
        public int TestInt;
        public int TrueInt;
        public int FalseInt;
        public string Operation;

        public long DoOperation(long old)
        {
            var newVal = long.MinValue;
            var parts = Operation.Split(' ');
            if (!parts[0].Equals("old"))
            {
                throw new Exception();
            }

            if (!long.TryParse(parts[2], out var secondVal))
            {
                secondVal = old;
            }

            switch (parts[1])
            {
                case "*":
                    newVal = old * secondVal;
                    break;
                case "+":
                    newVal = old + secondVal;
                    break;
                default:
                    break;
            }
            return newVal;
        }
    }
}
