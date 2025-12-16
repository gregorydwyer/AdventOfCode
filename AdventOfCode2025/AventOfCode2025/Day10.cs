using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2024
{
    public class Day10
    {
        private const string Day = "Day10";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";

        public static void Run()
        {
            Program.WriteTitle("--- Day 10: Factory ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var machines = BuildMachines();
            var count = 0L;
            foreach (var machine in machines)
            {
                count += machine.FewestPressesForLights();
            }

            Program.WriteOutput("Fewest Button Pushes: " + count);
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var machines = BuildMachines();
            var count = 0L;
            foreach (var machine in machines)
            {
                
            }

            Program.WriteOutput("Fewest Presses to Match Joltages: " + count);
        }

        private static List<Machine> BuildMachines()
        {
            var list = new List<Machine>();
            using (var sr = Program.GetReader(FileLocation))
            {
                var line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var split = line.Split(' ');
                    var machine = new Machine();
                    foreach (var s in split)
                    {
                        if (s[0] == '[')
                        {
                            var lights = s.Trim(new char[] { '[',']' });
                            machine.Lights = lights.Select(l => l == '#' ? 1 : 0).ToArray();
                        }
                        else if (s[0] == '(')
                        {
                            var wires = s.Split(new char[] { '(',')', ',' }, StringSplitOptions.RemoveEmptyEntries);
                            machine.Buttons.Add(wires.Select(w => int.Parse(w)).ToArray());
                        }
                        else if (s[0] == '{')
                        {
                            var jolts = s.Split(new char[] { '{','}', ',' }, StringSplitOptions.RemoveEmptyEntries);
                            machine.Joltages = jolts.Select(j => int.Parse(j)).ToList();
                        }

                    }
                    list.Add(machine);
                    line = sr.ReadLine();
                }
            }

            return list;
        }

    }

    public class Machine
    {
        public int[] Lights;
        public List<int[]> Buttons = new List<int[]>();
        public List<int> Joltages;


        public int FewestPressesForLights()
        {
            var min = Buttons.Count;
            foreach (var set in PowerSets(Buttons))
            {
                var buttons = set.ToList();
                var combo = new int[Lights.Length];
                var length = buttons.Count;
                foreach (var button in buttons)
                {
                    foreach (var i in button)
                    {
                        combo[i]++;
                    }
                }

                if (CheckCombo(combo) && length < min)
                {
                    min = length;
                }
            }

            return min;
        }

        private bool CheckCombo(int[] combo)
        {
            for (int i = 0; i < Lights.Length; i++)
            {
                if (Lights[i] != combo[i] % 2)
                {
                    return false;
                }
            }

            return true;
        }

        public static IEnumerable<IEnumerable<T>> PowerSets<T>(IList<T> set)
        {
            var totalSets = (int)Math.Pow(2, set.Count);
            for (int i = 0; i < totalSets; i++)
            {
                yield return SubSet(set, i);
            }
        }

        public static IEnumerable<T> SubSet<T>(IList<T> set, int n)
        {
            for (int i = 0; i < set.Count && n > 0; i++)
            {
                if ((n & 1) == 1)
                {
                    yield return set[i];
                }

                n = n >> 1;
            }
        }

    }
}