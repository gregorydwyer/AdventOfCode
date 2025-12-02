using System;

namespace AdevntOfCode2022
{
    public static class Day2
    {
        private const int NormalizingValue = -1 * ('X' - 'A');
        public static void Problem1()
        {
            Console.WriteLine("D2 P1");
            var line = Console.ReadLine();
            var total = 0;
            while (line != null && !line.Equals("q"))
            {
                var elf = line[0] - 64;
                var me = line[2] + NormalizingValue - 64;
                total += me;
                var result = RPS(elf, me);
                if (result >= 0)
                {
                    total += result == 0 ? 3 : 6;
                }

                line = Console.ReadLine();
            }

            Console.WriteLine("Final Score: " + total);
            Console.ReadKey();
        }

        public static int RPS(int them, int me)
        {
            var outcome = me - them;
            if (outcome == 1 || outcome == -2)
            {
                return 1;
            }

            if (outcome == 0)
            {
                return 0;
            }

            return -1;

        }

        public static void Problem2()
        {
            Console.WriteLine("D2 P2");
            var line = Console.ReadLine();
            var total = 0;
            while (line != null && !line.Equals("q"))
            {
                var elf = line[0] - 64;
                var outcome = line[2] + NormalizingValue - 64;
                switch (outcome)
                {
                    case 1:
                        total += elf == 1 ? 3 : (elf + 2) % 3;
                        break;
                    case 2:
                        total += 3 + elf;
                        break;
                    case 3:
                        total += 6;
                        total += elf == 2 ? 3 : ((elf +1) % 3);
                        break;
                }
                line = Console.ReadLine();
            }
            Console.WriteLine("Final Score: " + total);
            Console.ReadKey();
        }

        public static void Problem2Char()
        {
            Console.WriteLine("D2 P2");
            var line = Console.ReadLine();
            var total = 0;
            while (line != null && !line.Equals("q"))
            {
                var elf = line[0];
                var outcome = line[2];
                switch (outcome)
                {
                    case 'X':
                        if (elf == 'A') total += 3;
                        if (elf == 'B') total += 1;
                        if (elf == 'C') total += 2;
                        break;
                    case 'Y':
                        total += 3;
                        if (elf == 'A') total += 1;
                        if (elf == 'B') total += 2;
                        if (elf == 'C') total += 3;
                        break;
                    case 'Z':
                        total += 6;
                        if (elf == 'A') total += 2;
                        if (elf == 'B') total += 3;
                        if (elf == 'C') total += 1;
                        break;
                }

                line = Console.ReadLine();
            }
            Console.WriteLine("Final Score: " + total);
            Console.ReadKey();
        }
    }
}

