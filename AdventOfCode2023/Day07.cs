using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2023
{
    public class Day07
    {
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        public static void Problem1()
        {
            Console.WriteLine("Day07 P1");
            var hands = new List<Hand>();
            using (var stream = new StreamReader(new FileStream("Day07.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                while (line != null)
                {
                    var split = line.Split(' ');
                    hands.Add(new Hand(split[0], int.Parse(split[1])));
                    line = stream.ReadLine();
                }
            }
            // sort the hands
            var comp = new HandComparer(false);
            hands.Sort(comp);

            // do the math
            var finalTotal = 0;
            for (int i = 0; i < hands.Count; i++)
            {
                finalTotal += hands[i].Bid * (i + 1);
            }
            Console.WriteLine("Total: " + finalTotal);
        }
        public static void Problem2()
        {
            Console.WriteLine("Day07 P2");
            var hands = new List<Hand>();
            using (var stream = new StreamReader(new FileStream("Day07.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                while (line != null)
                {
                    var split = line.Split(' ');
                    hands.Add(new Hand(split[0], int.Parse(split[1])));
                    line = stream.ReadLine();
                }
            }
            // sort the hands
            var comp = new HandComparer(true);
            hands.Sort(comp);

            // do the math
            var finalTotal = 0;
            for (int i = 0; i < hands.Count; i++)
            {
                finalTotal += hands[i].Bid * (i + 1);
            }
            Console.WriteLine("Total: " + finalTotal);
        }

        private struct Hand
        {
            public int Bid;
            public string Cards;

            public Hand(string cards, int bid)
            {
                Cards = cards;
                Bid = bid;
            }

            public override string ToString()
            {
                return Cards + ", " + Bid;
            }
        }

        private class HandComparer : Comparer<Hand>
        {
            private const string Standard = "xx23456789TJQKA";
            private const string JokerWild = "xxJ23456789TQKA";
            private string Order;
            private bool WildRules;

            public HandComparer(bool jokerWild)
            {
                WildRules = jokerWild;
                if (jokerWild)
                {
                    Order = JokerWild;
                }
                else
                {
                    Order = Standard;
                }
            }

            public override int Compare(Hand x, Hand y)
            {
                // Compare types
                var xType = GetHandType(x.Cards);
                var yType = GetHandType(y.Cards);
                if (xType != yType)
                {
                    return xType - yType;
                }

                // Compare cards
                for (int i = 0; i < x.Cards.Length; i++)
                {
                    var xVal = Value(x.Cards[i]);
                    var yVal = Value(y.Cards[i]);
                    if (xVal != yVal)
                    {
                        return xVal - yVal;
                    }
                }
                return 0;
            }

            private int Value(char c)
            {
                return Order.IndexOf(c);
            }

            private int GetHandType(string hand)
            {
                var hash = hand.ToHashSet();
                var list = new List<int>();
                var i = 0;
                var jokerCount = 0;
                foreach(var c in hash)
                {
                    list.Add(0);
                    foreach (var card in hand)
                    {
                        if (c == card)
                        {
                            if (WildRules && c == 'J')
                            {
                                jokerCount++;
                            }
                            else
                            {
                                list[i]++;
                            }
                        }
                    }

                    i++;
                }
                list.Sort();
                list.Reverse();
                switch (list[0] + jokerCount)
                {
                    case 5:
                        return 7;
                    case 4:
                        return 6;
                    case 3:
                        if (list[1] == 2)
                        {
                            return 5;
                        }
                        return 4;
                    case 2:
                        if (list[1] == 2)
                        {
                            return 3;
                        }
                        return 2;
                    case 1:
                        return 1;
                }

                throw new NotSupportedException();
            }
        }
    }
}