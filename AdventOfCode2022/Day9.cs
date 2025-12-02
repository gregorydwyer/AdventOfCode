using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
    public static class Day9
    {
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        public static void Problem1()
        {
            Console.WriteLine("D9 P1");
            using (var stream = new StreamReader(new FileStream("Day9.txt", FileMode.Open, FileAccess.Read)))
            {
                var visited = new HashSet<(int,int)>();
                var line = stream.ReadLine();
                var head = new Knot();
                var tail = new Knot();
                visited.Add((tail.Hor, tail.Ver));
                while (line != null)
                {
                    var args = line.Split(' ');
                    for (var i = 0; i < int.Parse(args[1]); i++)
                    {
                        MoveKnots(head, tail, StringToDirection(args[0]));
                        visited.Add((tail.Hor, tail.Ver));
                    }

                    line = stream.ReadLine();
                }
                Console.WriteLine("Total visited: " + visited.Count);
            }
        }

        private static Direction StringToDirection(string input)
        {
            switch (input.ToUpperInvariant())
            {
                case "U":
                    return Direction.North;
                case "D":
                    return Direction.South;
                case "L":
                    return Direction.West;
                case "R":
                    return Direction.East;
                default:
                    throw new ArgumentException("Not a recognized input: " + input);
            }
        }

        private static void MoveKnots(Knot head, Knot tail, Direction dir)
        {
            head.Move(dir);
            var tailDir = tail.GetPullDirection(head);
            tail.Move(tailDir);
        }

        private static void MoveKnots(List<Knot> knots, Direction dir)
        {
            for (var i = 0; i < knots.Count; i++)
            {
                knots[i].Move(dir);
                if (i != knots.Count - 1)
                {
                    dir = knots[i + 1].GetPullDirection(knots[i]);
                }
            }
        }

        public static void Problem2()
        {
            Console.WriteLine("D9 P2");
            using (var stream = new StreamReader(new FileStream("Day9.txt", FileMode.Open, FileAccess.Read)))
            {
                var visited = new HashSet<(int, int)>();
                var line = stream.ReadLine();
                var knots = new List<Knot>();
                for (var i = 0; i < 10; i++)
                {
                    knots.Add(new Knot());
                }
                visited.Add((knots.Last().Hor, knots.Last().Ver));
                while (line != null)
                {
                    var args = line.Split(' ');
                    for (var i = 0; i < int.Parse(args[1]); i++)
                    {
                        MoveKnots(knots, StringToDirection(args[0]));
                        visited.Add((knots.Last().Hor, knots.Last().Ver));
                    }

                    //Console.WriteLine(line);
                    //DrawResult(new HashSet<(int h, int v)>(knots.Select(point => (point.Hor, point.Ver))));
                    //Console.ReadKey();
                    line = stream.ReadLine();
                }
                Console.WriteLine("Total visited: " + visited.Count);
                DrawResult(visited);
            }
        }

        private static void DrawResult(HashSet<(int h, int v)> points)
        {
            var hmin = points.Min(point => point.h);
            var vmin = points.Min(point => point.v);
            var hmax = points.Max(point => point.h);
            var vmax = points.Max(point => point.v);
            for (var i = vmax; i >= vmin; i--)
            {
                for(var j = hmin; j <= hmax; j++)
                {
                    var output = points.Contains((j, i)) ? "#" : ".";
                    Console.Write(output);
                }
                Console.WriteLine();
            }
        }
    }

    public class Knot
    {
        public int Hor;
        public int Ver;

        public Direction GetPullDirection(Knot other)
        {
            var horDif = other.Hor - Hor;
            var verDif = other.Ver - Ver;
            var horAbs = Math.Abs(horDif);
            var verAbs = Math.Abs(verDif);
            if (verAbs + horAbs == 0
                || verAbs + horAbs == 1
                || (verAbs == 1 && horAbs == 1))
            {
                return Direction.None;
            }
            if (verAbs + horAbs >= 3)
            {
                switch (horDif)
                {
                    case -2:
                        return verDif > 0 ? Direction.NorthWest : Direction.SouthWest;
                        break;
                    case 2:
                        return verDif > 0 ? Direction.NorthEast : Direction.SouthEast;
                        break;
                    default:
                        break;
                }
                switch (verDif)
                {
                    case -2:
                        return horDif > 0 ? Direction.SouthEast : Direction.SouthWest;
                        break;
                    case 2:
                        return horDif > 0 ? Direction.NorthEast : Direction.NorthWest;
                        break;
                    default:
                        break;
                }
            }
            else if (horAbs == 2)
            {
                return horDif > 0 ? Direction.East : Direction.West;
            }
            else if (verAbs == 2)
            {
                return verDif > 0 ? Direction.North : Direction.South;
            }

            throw new Exception("This shouldn't be happening...");
        }

        public void Move(Direction dir)
        {
            Move(dir,1);
        }

        public void Move(Direction dir, int dist)
        {
            switch (dir)
            {
                case Direction.None:
                    break;
                case Direction.North:
                    Ver++;
                    break;
                case Direction.NorthEast:
                    Ver++;
                    Hor++;
                    break;
                case Direction.East:
                    Hor++;
                    break;
                case Direction.SouthEast:
                    Ver--;
                    Hor++;
                    break;
                case Direction.South:
                    Ver--;
                    break;
                case Direction.SouthWest:
                    Ver--;
                    Hor--;
                    break;
                case Direction.West:
                    Hor--;
                    break;
                case Direction.NorthWest:
                    Hor--;
                    Ver++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
            }
        }
    }

    public enum Direction
    {
        None,
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
    }
}