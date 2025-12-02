using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
    public class Day20
    {
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }
        public static void Problem1()
        {
            Console.WriteLine("D20 P1");
            var nums = BuildLinkedList();
            var movingItem = nums.First();
            for (int i = 0; i < nums.Count; i++)
            {
                var next = movingItem.Next;
                if (nums.All(n => n.Moved))
                {
                    break;
                }
                // move over previously visited items
                while (movingItem.Moved)
                {
                    movingItem = movingItem.Next;
                }

                movingItem.Moved = true;

                if (movingItem.Value == 0)
                {
                    continue;
                }

                //pull moving item out of the linked list
                movingItem.Prev.Next = movingItem.Next;
                movingItem.Next.Prev = movingItem.Prev;

                // move the item
                EncryptedNum newPlace = null;
                for (int j = 0; j < Math.Abs(movingItem.Value); j++)
                {
                    if (movingItem.Value > 0)
                    {
                        if (newPlace == null)
                        {
                            newPlace = movingItem.Next;
                        }
                        else
                        {
                            newPlace = newPlace.Next;
                        }
                    }
                    else if (movingItem.Value < 0)
                    {
                        if (newPlace == null)
                        {
                            newPlace = movingItem.Prev;
                        }
                        else
                        {
                            newPlace = newPlace.Prev;
                        }
                    }
                }

                //update moving item
                movingItem.Prev = newPlace;
                movingItem.Next = newPlace.Next;
                // put item back in list
                newPlace.Next = movingItem;
                newPlace.Next.Prev = movingItem;

                movingItem = next;
                var temp2 = nums.First(n => n.Value == 1);
                for (int j = 0; j < nums.Count; j++)
                {
                    Console.Write(temp2.Value);
                    temp2 = temp2.Next;
                }

                Console.ReadKey();
                Console.WriteLine();
            }

            var zero = nums.First(n => n.Value == 0);
            var total = 0;
            EncryptedNum temp = null;
            for (int i = 1; i <= 3000; i++)
            {
                if (temp == null)
                {
                    temp = zero.Next;
                }
                else
                {
                    temp = temp.Next;
                }

                if (i % 1000 == 0)
                {
                    total += temp.Value;
                }
            }

            Console.WriteLine("Total: " + total);

        }

        public static void Problem2()
        {
            Console.WriteLine("D20 P2");

        }

        public static Collection<EncryptedNum> BuildLinkedList()
        {
            using (var stream = new StreamReader(new FileStream("Test20.txt", FileMode.Open, FileAccess.Read)))
            {
                var collection = new Collection<EncryptedNum>();
                EncryptedNum first = null;
                EncryptedNum current = null;
                EncryptedNum previous = null;
                var line = stream.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    previous = current;
                    current = new EncryptedNum(int.Parse(line));
                    if (first == null)
                    {
                        first = current;
                    }

                    current.Prev = previous;
                    if (previous != null)
                    {
                        previous.Next = current;
                    }

                    collection.Add(current);
                    line = stream.ReadLine();
                }

                if(first != null && current != null)
                {
                    first.Prev = current;
                    current.Next = first;
                }
                return collection;
            }
        }
    }

    public class EncryptedNum
    {
        public EncryptedNum Next;
        public EncryptedNum Prev;
        public int Value;
        public bool Moved;

        public EncryptedNum(int val)
        {
            Value = val;
        }

        public override string ToString()
        {
            return "Value: " + Value + ", Next: " + Next.Value + ", Prev: " + Prev.Value;
        }
    }
}