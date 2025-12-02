using System;

namespace AdevntOfCode2022
{
    public static class Day3
    {
        private const int LowerAdjustment = -1 * ('a') + 1;
        private const int UpperAdjustment = -1 * ('A') + 27;

        public static void Problem1()
        {
            Console.WriteLine("D3 P1");
            var line = Console.ReadLine();
            var total = 0;
            while (line != null && !line.Equals("q"))
            {
                if (line.Length % 2 != 0)
                {
                    throw new ArgumentException();
                }

                var size = line.Length / 2;
                var comp1 = line.Substring(0, size);
                var comp2 = line.Substring(size, size);
                char match = '\0';

                for (var i = 0; i < size; i++)
                {
                    for (var j = 0; j < size; j++)
                    {
                        if (comp1[i].Equals(comp2[j]))
                        {
                            match = comp1[i];
                            break;
                        }
                    }

                    if (match != '\0')
                    {
                        break;
                    }
                }

                total += Char.IsUpper(match) ? match + UpperAdjustment : match + LowerAdjustment;
                line = Console.ReadLine();
            }
            Console.WriteLine("Final Score: " + total);
            Console.ReadKey();
        }

        public static void Problem2()
        {
            Console.WriteLine("D3 P2");
            var line = Console.ReadLine();
            var total = 0;
            while (line != null && !line.Equals("q"))
            {
                var pack1 = line;
                var pack2 = Console.ReadLine();
                var pack3 = Console.ReadLine();
                char match = Search3Packs(pack1, pack2, pack3);
                total += Char.IsUpper(match) ? match + UpperAdjustment : match + LowerAdjustment;
                line = Console.ReadLine();
            }
            Console.WriteLine("Final Score: " + total);
            Console.ReadKey();
        }

        public static char Search3Packs(string pack1, string pack2, string pack3)
        {
            char match = '\0';

            for (var i = 0; i < pack1.Length; i++)
            {
                for (var j = 0; j < pack2.Length; j++)
                {
                    if (pack1[i].Equals(pack2[j]))
                    {
                        match = pack1[i];
                    }

                    if (match != '\0')
                    {
                        for (var k = 0; k < pack3.Length; k++)
                        {
                            if (match == pack3[k])
                            {
                                return match;
                            }
                        }
                        match = '\0';
                    }
                }
            }

            throw new Exception("None of the packs have a matching item! Check your input!");
        }
    }
}