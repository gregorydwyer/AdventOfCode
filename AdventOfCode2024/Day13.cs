using System;
using System.Collections.Generic;

namespace AdventOfCode2024
{
    public class Day13
    {
        private const string Day = "Day13";
        private const long UserErrorOffset = 10000000000000;
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";

        public static void Run()
        {
            Program.WriteTitle("--- Day 13: Claw Contraption ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var total = 0L;
            var machines = BuildMachines();
            foreach (var machine in machines)
            {
                var cost = RunMachine(machine);
                total += Math.Max(cost, 0);
            }
            Program.WriteOutput("Tokens required: " + total);
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var total = 0L;
            var machines = BuildMachines();
            foreach (var machine in machines)
            {
                machine.P.X += UserErrorOffset;
                machine.P.Y += UserErrorOffset;
                var cost = MathPartTwo(machine);
                total += Math.Max(cost, 0);
            }
            Program.WriteOutput("Tokens required: " + total);
        }

        private static long RunMachine(Claw c)
        {
            for (long a = 0; a <= 100; a++)
            {
                var b = (c.P.X - (a * c.A.X)) % c.B.X;
                if (b == 0)
                {
                    b = (c.P.X - (a * c.A.X)) / c.B.X;
                    if (b <= 100)
                    {
                        if (c.P.Y - (a * c.A.Y) - (b * c.B.Y) == 0)
                        {
                            return a * 3 + b;
                        }
                    }
                }
            }

            return -1;
        }


        private static long MathPartTwo(Claw c)
        {
            //yP(xb)-xP(yb) / (ya(xb) - xa(yb)) = A
            var a = (c.P.Y * c.B.X - c.P.X * c.B.Y);
            if( a % (c.A.Y * c.B.X - c.A.X * c.B.Y) != 0)
            {
                return -1;
            }

            a = a / (c.A.Y * c.B.X - c.A.X * c.B.Y);
            //(xP - Axa) / xb = B
            var b = (c.P.X - a * c.A.X);
            if (b % c.B.X != 0)
            {
                return -1;
            }

            b = b / c.B.X;
            if (a < 0 || b < 0)
            {
                return -1;
            }
            return a * 3 + b;
        }

        private static List<Claw> BuildMachines()
        {
            using (var reader = Program.GetReader(FileLocation))
            {
                var machines = new List<Claw>();
                var currentLine = reader.ReadLine();
                LongPoint a = null;
                LongPoint b = null;
                while (currentLine != null)
                {
                    if (currentLine.Contains("A"))
                    {
                        a = ParseLine(currentLine);
                    }
                    else if (currentLine.Contains("B"))
                    {
                        b = ParseLine(currentLine);
                    }
                    else if (currentLine.Contains("P"))
                    {
                        if (a != null && b != null)
                        {
                            machines.Add(new Claw(ParseLine(currentLine), a, b));
                            a = null;
                            b = null;
                        }
                    }
                    currentLine = reader.ReadLine();
                }

                return machines;
            }
        }

        public static LongPoint ParseLine(string s)
        {
            var offsets = s.Split(':')[1].Trim().Split(',');
            var x = offsets[0].Substring(2);
            var y = offsets[1].Substring(3);
            return new LongPoint(long.Parse(x), long.Parse(y));
        }
    }

    public class Claw
    {
        public LongPoint P;
        public LongPoint A;
        public LongPoint B;

        public Claw(LongPoint p, LongPoint a, LongPoint b)
        {
            P = p;
            A = a;
            B = b;
        }
    }

    public class LongPoint
    {
        public long X;
        public long Y;

        public LongPoint(long x, long y)
        {
            X = x;
            Y = y;
        }
    }
}