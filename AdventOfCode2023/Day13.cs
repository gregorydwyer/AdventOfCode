using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace AdventOfCode2023
{
    public class Day13
    {
        private const string File = "Day13.txt";
        public static void Run()
        {
            var reflections = Problem1();
            Problem2(reflections);
            Console.ReadKey();
        }

        private static List<Reflection> Problem1()
        {
            Console.WriteLine("Day13 P1");
            using (var stream = new StreamReader(new FileStream(File, FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                var reflections = new List<Reflection>();
                long finalTotal = 0;
                while (line != null)
                {
                    // build pattern
                    var pattern = new List<string>();
                    while (!string.IsNullOrEmpty(line))
                    {
                        pattern.Add(line);
                        line = stream.ReadLine();
                    }
                    // parse pattern
                    // vertical mirror scan
                    long vertIndex = 0;
                    for (int i = 0; i < pattern[0].Length - 1; i++)
                    {
                        if (CheckVerticalMirror(pattern, 0, i))
                        {
                            vertIndex = i + 1;
                            reflections.Add(new Reflection(){Index = vertIndex, Axis = Direction.Vertical});
                            break;
                        }
                    }

                    long horIndex = 0;
                    if (vertIndex == 0)
                    {

                        // horizontal mirror scan
                        for (int i = 0; i < pattern.Count - 1; i++)
                        {
                            if (CheckHorizontalMirror(pattern, 0, i))
                            {
                                horIndex = i + 1;
                                reflections.Add(new Reflection() { Index = horIndex, Axis = Direction.Horizontal });
                                break;
                            }
                        }
                    }

                    finalTotal += vertIndex + (100 * horIndex);
                    line = stream.ReadLine();
                }
                Console.WriteLine("Total: " + finalTotal);
                return reflections;
            }
        }

        private static bool CheckVerticalMirror(List<string> pattern, int line, int mirrorLeft)
        {
            if (line >= pattern.Count)
            {
                return true;
            }

            var mirrorRight = mirrorLeft + 1;
            for (int i = 0; mirrorLeft - i >= 0 && mirrorRight + i < pattern[line].Length; i++)
            {
                var l = pattern[line][mirrorLeft - i];
                var r = pattern[line][mirrorRight + i];
                if (pattern[line][mirrorLeft - i] != pattern[line][mirrorRight + i])
                {
                    return false;
                }
            }

            return CheckVerticalMirror(pattern, line + 1, mirrorLeft);
        }

        private static bool CheckHorizontalMirror(List<string> pattern, int line, int mirrorTop)
        {
            if (line > pattern.Count)
            {
                return true;
            }

            var mirrorBottom = mirrorTop + 1;
            for (int i = 0; mirrorTop - i >= 0 && mirrorBottom + i < pattern.Count; i++)
            {
                if (pattern[mirrorTop - i] != pattern[mirrorBottom + i])
                {
                    return false;
                }
            }

            return CheckHorizontalMirror(pattern, line + 1, mirrorTop);
        }


        private static void Problem2(List<Reflection> refs)
        {
            Console.WriteLine("Day13 P2");
            using (var stream = new StreamReader(new FileStream(File, FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                long finalTotal = 0;
                var count = 0;
                while (line != null)
                {
                    // build pattern
                    var pattern = new List<string>();
                    while (!string.IsNullOrEmpty(line))
                    {
                        pattern.Add(line);
                        line = stream.ReadLine();
                    }
                    // parse pattern
                    // vertical mirror scan
                    long vertIndex = 0;
                    for (int row = 0; row < pattern.Count; row++)
                    {
                        for (int col = 0; col < pattern[row].Length; col++)
                        {
                            pattern[row] = Swap(pattern[row], col);
                            for (int i = 0; i < pattern[0].Length - 1; i++)
                            {
                                if (CheckVerticalMirror(pattern, 0, i))
                                {
                                    if (refs[count].Axis == Direction.Vertical && refs[count].Index == i + 1)
                                    {
                                        continue;
                                    }
                                    vertIndex = i + 1;
                                    break;
                                }
                            }

                            if (vertIndex > 0)
                            {
                                break;
                            }
                            pattern[row] = Swap(pattern[row], col);
                        }
                        if (vertIndex > 0)
                        {
                            break;
                        }
                    }


                    long horIndex = 0;
                    if (vertIndex == 0)
                    {
                        for (int row = 0; row < pattern.Count; row++)
                        {
                            for (int col = 0; col < pattern[row].Length; col++)
                            {
                                pattern[row] = Swap(pattern[row], col);
                                // horizontal mirror scan
                                for (int i = 0; i < pattern.Count - 1; i++)
                                {
                                    if (CheckHorizontalMirror(pattern, 0, i))
                                    {
                                        if (refs[count].Axis == Direction.Horizontal && refs[count].Index == i + 1)
                                        {
                                            continue;
                                        }
                                        horIndex = i + 1;
                                        break;
                                    }
                                }

                                if (horIndex > 0)
                                {
                                    break;
                                }
                                pattern[row] = Swap(pattern[row], col);
                            }
                            if (horIndex > 0)
                            {
                                break;
                            }
                        }
                    }

                    finalTotal += vertIndex + (100 * horIndex);
                    count++;
                    line = stream.ReadLine();
                }
                Console.WriteLine("Total: " + finalTotal);
            }
        }

        private static string Swap(string line, int col)
        {
            var chars = line.ToCharArray();

            chars[col] = chars[col] == '.' ? '#' : '.';

            return new string(chars);
        }

        private struct Reflection
        {
            public long Index;
            public Direction Axis;
        }

        private enum Direction
        {
            Horizontal = 0,
            Vertical = 1
        }
    }
}