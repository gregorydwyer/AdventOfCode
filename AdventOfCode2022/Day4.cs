using System;
using System.Linq;

namespace AdventOfCode2022
{
    public static class Day4
    {
        public static void Problem1()
        {
            Console.WriteLine("D4 P1");
            var line = Console.ReadLine();
            var total = 0;
            while (line != null && !line.Equals("q"))
            {
                var input = line.Split(',');
                var rangeEnds1 = input[0].Split('-').Select(str => int.Parse(str)).ToArray();
                var rangeEnds2 = input[1].Split('-').Select(str => int.Parse(str)).ToArray();
                if((rangeEnds1[0] <= rangeEnds2[0] && rangeEnds1[1] >= rangeEnds2[1])
                    || (rangeEnds2[0] <= rangeEnds1[0] && rangeEnds2[1] >= rangeEnds1[1]))
                {
                    total++;
                }


                line = Console.ReadLine();
            }
            Console.WriteLine("Total overlaps: " + total);
            Console.ReadKey();
        }

        public static void Problem2()
        {
            Console.WriteLine("D4 P2");
            var line = Console.ReadLine();
            var total = 0;
            while (line != null && !line.Equals("q"))
            {
                var input = line.Split(',');
                var rangeEnds1 = input[0].Split('-').Select(str => int.Parse(str)).ToArray();
                var rangeEnds2 = input[1].Split('-').Select(str => int.Parse(str)).ToArray();
                if ((rangeEnds1[0] <= rangeEnds2[0] && rangeEnds1[1] >= rangeEnds2[0])
                    || (rangeEnds1[0] <= rangeEnds2[1] && rangeEnds1[1] >= rangeEnds2[1])
                    || (rangeEnds2[0] <= rangeEnds1[0] && rangeEnds2[1] >= rangeEnds1[0])
                    || (rangeEnds2[0] <= rangeEnds1[1] && rangeEnds2[1] >= rangeEnds1[1]))
                {
                    total++;
                }


                line = Console.ReadLine();
            }
            Console.WriteLine("Partial overlaps: " + total);
            Console.ReadKey();
        }
    }
}
