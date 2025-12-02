using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
    public static class Day13
    {
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        public static void Problem1()
        {
            Console.WriteLine("D13 P1");
            using (var stream = new StreamReader(new FileStream("Day13.txt", FileMode.Open, FileAccess.Read)))
            {
                var line1 = stream.ReadLine();
                var line2 = stream.ReadLine();
                var total = 0;
                var index = 1;
                while (line1 != null && line2 != null)
                {
                    if (LinesAreOrdered(line1, line2) == true)
                    {
                        total += index;
                    }
                    //discard empty line
                    stream.ReadLine();
                    line1 = stream.ReadLine();
                    line2 = stream.ReadLine();
                    index++;
                }
                Console.WriteLine("Indices Sum: " + total);
            }
        }

        public static void Problem2()
        {
            Console.WriteLine("D13 P2");
            using (var stream = new StreamReader(new FileStream("Day13.txt", FileMode.Open, FileAccess.Read)))
            {
                var lines = new List<string>();
                lines.Add("[[2]]");
                lines.Add("[[6]]");
                while (!stream.EndOfStream)
                {
                    var line = stream.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        lines.Add(line);
                    }
                }
                lines.Sort(new LineComparer());

                var key1 = lines.IndexOf("[[2]]") + 1;
                var key2 = lines.IndexOf("[[6]]") + 1;
                Console.WriteLine("Decoder Key: " + (key1 * key2));
            }
        }

        public static bool? LinesAreOrdered(string line1, string line2)
        {
            return LinesAreOrdered(MakeArrayFromLine(line1), MakeArrayFromLine(line2));
        }

        public static bool? LinesAreOrdered(string[] line1, string[] line2)
        {
            var i = 0;
            for ( ; i < line1.Length; i++)
            {
                // Line1 has more elements than line2
                if (i >= line2.Length)
                {
                    return false;
                }

                // base case of comparing two values
                if (int.TryParse(line1[i], out var one) && int.TryParse(line2[i], out var two))
                {
                    if (one == two)
                    {
                        continue;
                    }

                    return one < two;
                }
                // at least one of the elements is a list
                else
                {
                    var item1 = MakeArrayFromLine(line1[i]);
                    var item2 = MakeArrayFromLine(line2[i]);
                    var result = LinesAreOrdered(item1, item2);
                    if (result.HasValue)
                    {
                        return result;
                    }
                }

            }

            if (i < line2.Length)
            {
                return true;
            }
            else if (i > line2.Length)
            {
                return false;
            }

            return null;
        }

        public static string[] MakeArrayFromLine(string line)
        {
            if (!(line.StartsWith("[") && line.EndsWith("]")))
            {
                if (int.TryParse(line, out var temp))
                {
                    return new string[] {line};
                }
                throw new Exception();
            }

            var trimmed = line.Substring(1, line.Length - 2);
            return MakeArray(trimmed);
        }

        public static string[] MakeArray(string line)
        {
            var list = new List<string>();
            var subList = 0;
            var element = "";
            for (int index = 0; index < line.Length; index++)
            {
                if (line[index] == '[')
                {
                    if (subList == 0 && !string.IsNullOrEmpty(element))
                    {
                        list.Add(element);
                    }

                    element += "[";
                    subList++;
                }
                else if (line[index] == ']')
                {
                    element += "]";
                    subList--;
                }
                else if (line[index] == ',')
                {
                    if (subList != 0)
                    {
                        element += ',';
                    }
                    else
                    {
                        list.Add(element);
                        element = "";
                    }
                }
                else
                {
                    element += line[index];
                }
            }
            if (!string.IsNullOrEmpty(element))
            {
                list.Add(element);
            }

            return list.ToArray();
        }
    }

    public class LineComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            var result = LinesAreOrdered(x, y);
            if(!result.HasValue)
            {
                throw new Exception();
            }

            return result.Value;
        }

        public static int? LinesAreOrdered(string line1, string line2)
        {
            return LinesAreOrdered(MakeArrayFromLine(line1), MakeArrayFromLine(line2));
        }

        public static int? LinesAreOrdered(string[] line1, string[] line2)
        {
            var i = 0;
            for (; i < line1.Length; i++)
            {
                // Line1 has more elements than line2
                if (i >= line2.Length)
                {
                    return 1;
                }

                // base case of comparing two values
                if (int.TryParse(line1[i], out var one) && int.TryParse(line2[i], out var two))
                {
                    if (one == two)
                    {
                        continue;
                    }

                    return one < two ? -1 : 1;
                }
                // at least one of the elements is a list
                else
                {
                    var item1 = MakeArrayFromLine(line1[i]);
                    var item2 = MakeArrayFromLine(line2[i]);
                    var result = LinesAreOrdered(item1, item2);
                    if (result.HasValue)
                    {
                        return result;
                    }
                }

            }

            if (i < line2.Length)
            {
                return -1;
            }
            else if (i > line2.Length)
            {
                return 1;
            }

            return null;
        }

        public static string[] MakeArrayFromLine(string line)
        {
            if (!(line.StartsWith("[") && line.EndsWith("]")))
            {
                if (int.TryParse(line, out var temp))
                {
                    return new string[] { line };
                }
                throw new Exception();
            }

            var trimmed = line.Substring(1, line.Length - 2);
            return MakeArray(trimmed);
        }

        public static string[] MakeArray(string line)
        {
            var list = new List<string>();
            var subList = 0;
            var element = "";
            for (int index = 0; index < line.Length; index++)
            {
                if (line[index] == '[')
                {
                    if (subList == 0 && !string.IsNullOrEmpty(element))
                    {
                        list.Add(element);
                    }

                    element += "[";
                    subList++;
                }
                else if (line[index] == ']')
                {
                    element += "]";
                    subList--;
                }
                else if (line[index] == ',')
                {
                    if (subList != 0)
                    {
                        element += ',';
                    }
                    else
                    {
                        list.Add(element);
                        element = "";
                    }
                }
                else
                {
                    element += line[index];
                }
            }
            if (!string.IsNullOrEmpty(element))
            {
                list.Add(element);
            }

            return list.ToArray();
        }
    }

}