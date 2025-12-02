//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Numerics;

//namespace AdventOfCode2022
//{
//    class Day17
//    {
//        public static void Run()
//        {
//            Problem1();
//            Problem2();
//            Console.ReadKey();
//        }
//        public static void Problem1()
//        {
//            Console.WriteLine("D17 P1");
//            var move = BuildMovementList();
//            var tower = new HashSet<Point>();
//            //make floor
//            for (int i = 0; i < 7; i++)
//            {
//                tower.Add(new Point(i, 0));
//            }

//            var nextMove = 0;
//            for (int i = 0; i < 2022; i++)
//            {
//                var rock = GetNextShape(i, tower.Max(p => p.Y));
//                do
//                {
//                    MoveRock(tower, rock, move[nextMove]);
//                    nextMove = (nextMove + 1) % move.Count;
//                } while (MoveRockDown(tower, rock));

//                foreach (var point in rock)
//                {
//                    tower.Add(point);
//                }
//            }

//            Console.WriteLine("Highest point: " + tower.Max(p => p.Y));
//        }
//        public static void Problem2()
//        {
//            Console.WriteLine("D17 P2");
//            var move = BuildMovementList();

//        }
//        public static List<Dir> BuildMovementList()
//        {
//            using (var stream = new StreamReader(new FileStream("Day17.txt", FileMode.Open, FileAccess.Read)))
//            {
//                var list = new List<Dir>();

//                while (!stream.EndOfStream)
//                {
//                    var item = stream.Read();
//                    if (item == '<')
//                    {
//                        list.Add(Dir.Left);
//                    }
//                    else if (item == '>')
//                    {
//                        list.Add(Dir.Right);
//                    }

//                }

//                return list;
//            }
//        }
//        public static HashSet<Point> GetNextShape(int shapeInt, BigInteger topRow)
//        {
//            var set = new HashSet<Point>();
//            var bottom = topRow + 4;
//            var shape = (Shapes) (shapeInt % 5);
//            switch (shape)
//            {
//                case Shapes.HLine:
//                    for (int i = 0; i < 4; i++)
//                    {
//                        set.Add(new Point(2 + i, bottom));
//                    }
//                    break;
//                case Shapes.Plus:
//                    set.Add(new Point(2, bottom + 1));
//                    set.Add(new Point(3, bottom + 1));
//                    set.Add(new Point(4, bottom + 1));
//                    set.Add(new Point(3, bottom + 2));
//                    set.Add(new Point(3, bottom));
//                    break;
//                case Shapes.L:
//                    set.Add(new Point(2, bottom));
//                    set.Add(new Point(3, bottom));
//                    set.Add(new Point(4, bottom));
//                    set.Add(new Point(4, bottom + 1));
//                    set.Add(new Point(4, bottom + 2));
//                    break;
//                case Shapes.VLine:
//                    for (int i = 0; i < 4; i++)
//                    {
//                        set.Add(new Point(2, bottom + i));
//                    }
//                    break;
//                case Shapes.Square:
//                    set.Add(new Point(2, bottom));
//                    set.Add(new Point(3, bottom));
//                    set.Add(new Point(2, bottom + 1));
//                    set.Add(new Point(3, bottom + 1));
//                    break;
//                default:
//                    break;
//            }

//            return set;
//        }

//        public static void MoveRock(HashSet<Point> tower, HashSet<Point> rock, Dir dir )
//        {
//            if (dir == Dir.Left)
//            {
//                if (!rock.Any(p => p.X - 1 < 0 || tower.Contains(new Point(p.X - 1, p.Y))))
//                {
//                    foreach (var point in rock)
//                    {
//                        point.MoveLeft();
//                    }
//                }
//            }
//            if (dir == Dir.Right)
//            {
//                if (!rock.Any(p => p.X + 1 >= 7 || tower.Contains(new Point(p.X + 1, p.Y))))
//                {
//                    foreach (var point in rock)
//                    {
//                        point.MoveRight();
//                    }
//                }
//            }
//        }

//        public static bool MoveRockDown(HashSet<Point> tower, HashSet<Point> rock)
//        {

//            if (!rock.Any(p => tower.Contains(new Point(p.X, p.Y - 1))))
//            {
//                foreach (var point in rock)
//                {
//                    point.MoveDown();
//                }

//                return true;
//            }

//            return false;
//        }
//    }

//    public enum Dir
//    {
//        Left,
//        Right,
//    }

//    public enum Shapes
//    {
//        HLine,
//        Plus,
//        L,
//        VLine,
//        Square,
//    }

//}
