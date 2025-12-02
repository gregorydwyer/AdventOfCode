using System;
using System.Collections.Generic;

namespace AdventOfCode2024
{
    public class Day17
    {
        private const string Day = "Day17";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";
        private static long A, B, C;
        private static int Pointer;
        private static List<long> Output = new List<long>();

        public static void Run()
        {
            Program.WriteTitle("--- Day 17: Chronospatial Computer ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var program = BuildProgram();
            while (Pointer < program.Count)
            {
                RunOpCode(program[Pointer], program[Pointer + 1]);
            }
            Program.WriteOutput("Final Output: " + string.Join(",", Output));
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var program = BuildProgram();
            var programString = string.Join("",program);
            var matches = new List<List<long>>();
            matches.Add(new List<long>(){0});
            var match = false;
            var rounds = 0;
            while (rounds < 4)
            {
                matches.Add(new List<long>());
                matches[rounds].Sort();
                var offset = matches[rounds][0] << 12;
                var currentString = programString.Substring(programString.Length - 4 * (rounds + 1));
                for (long a = 0; a < 4096; a++)
                {

                    A = a + offset;
                    B = C = 0;
                    Pointer = 0;
                    Output.Clear();
                    while (Pointer < program.Count)
                    {
                        RunOpCode(program[Pointer], program[Pointer + 1]);
                        if (!currentString.StartsWith(string.Join("", Output)))
                        {
                            break;
                        }
                    }

                    if (currentString.Equals(string.Join("", Output)))
                    {
                        matches[rounds + 1].Add(a + offset);
                    }
                }
                rounds++;
            }

            matches[4].Sort();
            Program.WriteOutput("Lowest Match: " + matches[4][0]);
        }

        private static void RunOpCode(int opCode, int operand)
        {
            switch (opCode)
            {
                case 0: //adv
                    A =  A / (int) Math.Pow(2, GetComboOperand(operand));
                    break;
                case 1: //bxl
                    B = B ^ operand;
                    break;
                case 2: //bst
                    B = GetComboOperand(operand) % 8;
                    break;
                case 3: //jnz
                    if(A != 0)
                    {
                        Pointer = operand;
                        return;
                    }
                    break;
                case 4: //bxc
                    B = B ^ C;
                    break;
                case 5: //out
                    Output.Add(GetComboOperand(operand) % 8);
                    break;
                case 6: //bdv
                    B = A / (int) Math.Pow(2, GetComboOperand(operand));
                    break;
                case 7: //cdv
                    C = A / (int)Math.Pow(2, GetComboOperand(operand));
                    break;
            }
            Pointer += 2;
        }

        private static long GetComboOperand(int operand)
        {
            var value = 0L;
            switch (operand)
            {
                case 4:
                    value = A;
                    break;
                case 5:
                    value = B;
                    break;
                case 6:
                    value = C;
                    break;
                case 7:
                    throw new InvalidProgramException();
                default:
                    value = operand;
                    break;
            }
            return value;
        }

        private static List<int> BuildProgram()
        {
            using (var reader = Program.GetReader(FileLocation))
            {
                var program = new List<int>();
                var currentLine = reader.ReadLine();
                while(currentLine != null)
                {
                    if (currentLine.Contains("A"))
                    {
                        A = int.Parse(currentLine.Split(':')[1].Trim());
                    }
                    if (currentLine.Contains("B"))
                    {
                        B = int.Parse(currentLine.Split(':')[1].Trim());
                    }
                    if (currentLine.Contains("C"))
                    {
                        C = int.Parse(currentLine.Split(':')[1].Trim());
                    }
                    if (currentLine.Contains("P"))
                    {
                        var input = currentLine.Split(':')[1].Trim().Split(',');
                        foreach (var item in input)
                        {
                            program.Add(int.Parse(item));
                        }
                    }

                    currentLine = reader.ReadLine();
                }
                return program;

            }
        }
    }
}