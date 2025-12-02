using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022
{
    public static class Day5
    {
        public static void Problem1()
        {
            Console.WriteLine("D5 P1");
            var line = Console.ReadLine();
            var initialState = new List<string>();

            // get initial state
            while (!string.IsNullOrEmpty(line) && !line.Equals("q"))
            {
                initialState.Add(line);

                line = Console.ReadLine();
            }

            var count = initialState.Last().Count(c => !char.IsWhiteSpace(c));

            var stacks = new List<Stack<string>>();

            for (var i = 0; i < count; i++)
            {
                stacks.Add(new Stack<string>());
            }

            for (var i = initialState.Count - 2; i >= 0; i--)
            {
                for (var j = 0; j < stacks.Count; j++)
                {
                    if (string.IsNullOrWhiteSpace(initialState[i].Substring(1 + j * 4, 1)))
                    {
                        continue;
                    }
                    stacks[j].Push(initialState[i].Substring(1 + j * 4, 1));
                }
            }
            // clear blank line
            line = Console.ReadLine();
            while (line != null && !line.Equals("q"))
            {
                var move = line.Split(' ');
                var items = int.Parse(move[1]);
                var from = int.Parse(move[3]) - 1;
                var to = int.Parse(move[5]) - 1;

                for (var i = items; i > 0; i--)
                {
                    stacks[to].Push(stacks[from].Pop());
                }
                line = Console.ReadLine();
            }

            var output = "";
            for (var j = 0; j < stacks.Count; j++)
            {
                output += stacks[j].Pop();

            }
            Console.WriteLine(output);
            Console.ReadKey();
        }

        public static void Problem2()
        {
            Console.WriteLine("D5 P2");
            var line = Console.ReadLine();
            var initialState = new List<string>();

            // get initial state
            while (!string.IsNullOrEmpty(line) && !line.Equals("q"))
            {
                initialState.Add(line);

                line = Console.ReadLine();
            }

            var count = initialState.Last().Count(c => !char.IsWhiteSpace(c));

            var stacks = new List<Stack<string>>();

            for (var i = 0; i < count; i++)
            {
                stacks.Add(new Stack<string>());
            }

            for (var i = initialState.Count - 2; i >= 0; i--)
            {
                for (var j = 0; j < stacks.Count; j++)
                {
                    if (string.IsNullOrWhiteSpace(initialState[i].Substring(1 + j * 4, 1)))
                    {
                        continue;
                    }
                    stacks[j].Push(initialState[i].Substring(1 + j * 4, 1));
                }
            }

            // clear blank line
            line = Console.ReadLine();
            var temp = new Stack<string>();
            while (line != null && !line.Equals("q"))
            {
                var move = line.Split(' ');
                var items = int.Parse(move[1]);
                var from = int.Parse(move[3]) - 1;
                var to = int.Parse(move[5]) - 1;
                for (var i = items; i > 0; i--)
                {
                    temp.Push(stacks[from].Pop());
                }

                for (var i = items; i > 0; i--)
                {
                    stacks[to].Push(temp.Pop());
                }
                line = Console.ReadLine();
            }

            var output = "";
            for (var j = 0; j < stacks.Count; j++)
            {
                output += stacks[j].Pop();

            }
            Console.WriteLine(output);
            Console.ReadKey();
        }
    }
}