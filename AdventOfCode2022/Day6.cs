using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2022
{
    public static class Day6
    {
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }
        public static void Problem1()
        {
            Console.WriteLine("D6 P1");
            using (var stream = new FileStream("Day6.txt", FileMode.Open, FileAccess.Read))
            {
                var current = stream.ReadByte();
                var count = 0;
                var queue = new Queue<int>();

                // get initial state
                while (stream.CanRead)
                {
                    if (queue.Count == 4)
                    {
                        queue.Dequeue();
                    }

                    while (queue.Contains(current))
                    {
                        queue.Dequeue();
                    }

                    queue.Enqueue(current);
                    count++;

                    if (queue.Count == 4)
                    {
                        break;
                    }

                    current = stream.ReadByte();
                }

                if (current != -1)
                {
                    Console.WriteLine("Length: " + count);
                }
            }
        }

        public static void Problem2()
        {
            Console.WriteLine("D6 P2");
            using (var stream = new FileStream("Day6.txt", FileMode.Open, FileAccess.Read))
            {
                var current = stream.ReadByte();
                var count = 0;
                var queue = new Queue<int>();

                // get initial state
                while (stream.CanRead)
                {
                    if (queue.Count == 14)
                    {
                        queue.Dequeue();
                    }

                    while (queue.Contains(current))
                    {
                        queue.Dequeue();
                    }

                    queue.Enqueue(current);
                    count++;

                    if (queue.Count == 14)
                    {
                        break;
                    }

                    current = stream.ReadByte();
                }

                if (current != -1)
                {
                    Console.WriteLine("Length: " + count);
                }
            }
        }
    }
}
