using System;
using System.Diagnostics;
using System.IO;

namespace AdventOfCode2024
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Day01.Run();
            Day02.Run();
            Day03.Run();
            Day04.Run();
            Day05.Run();
            Day06.Run();
            Day07.Run();
            Day08.Run();
            Day09.Run();
            Day10.Run();
            Day11.Run();
            Day12.Run();
            Day13.Run();
            Day14.Run();
            Day15.Run();
            Day16.Run();
            Day17.Run();
            Day18.Run();
            Day19.Run();
            Day20.Run();
            Console.ReadKey();
        }

        public static StreamReader GetReader(string fileLocation)
        {
            try
            {
                var reader = new StreamReader(new FileStream(fileLocation, FileMode.Open, FileAccess.Read));
                return reader;
            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ReadKey();
                Environment.Exit(0);
                throw e;
            }
        }

        public static void WriteTitle(string title)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(title);
        }

        public static void WriteProblemNumber(string problemNumber)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(problemNumber);
        }

        public static void WriteOutput(string output)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(output);
        }
    }
}
