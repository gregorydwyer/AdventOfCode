using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public static class Day10
    {
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }
        public static void Problem1()
        {
            Console.WriteLine("D10 P1");
            using (var stream = new StreamReader(new FileStream("Day10.txt", FileMode.Open, FileAccess.Read)))
            {
                var cycle = 0;
                var line = stream.ReadLine();
                var finalTotal = 0;
                var x = 1;
                while (line != null)
                {
                    cycle++; 
                    if ((cycle - 20) % 40 == 0)
                    {
                        finalTotal += (cycle * x);
                    }
                    var args = line.Split(' ');

                    if (args[0].StartsWith("add"))
                    {
                        cycle++;
                        if ((cycle - 20) % 40 == 0)
                        {
                            finalTotal += (cycle * x);
                        }
                        x += int.Parse(args[1]);
                    }
                    line = stream.ReadLine();
                }
                Console.WriteLine("Total: " + finalTotal);
            }
        }

        public static void Problem2()
        {
            Console.WriteLine("D10 P2");
            using (var stream = new StreamReader(new FileStream("Day10.txt", FileMode.Open, FileAccess.Read)))
            {
                var cycle = 0;
                var line = stream.ReadLine();
                var x = 1;
                while (line != null)
                {
                    cycle++;
                    DrawPixel(cycle, x);
                    var args = line.Split(' ');

                    if (args[0].StartsWith("add"))
                    {
                        cycle++;
                        DrawPixel(cycle, x);
                        x += int.Parse(args[1]);
                    }

                    line = stream.ReadLine();
                }
                Console.WriteLine();
            }
        }

        public static void DrawPixel(int cycle, int x)
        {
            var pixel = (cycle - 1) % 40;
            var output = '.';
            if (pixel == 0)
            {
                Console.WriteLine();
            }

            if (pixel == x - 1 || pixel == x || pixel == x + 1)
            {
                output = '#';
            }
            Console.Write(output);

        }
    }

}
