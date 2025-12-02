using System;
using System.IO;

namespace AdventOfCode2024
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Day02.Run();
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
                Console.ForegroundColor = ConsoleColor.DarkRed;
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
