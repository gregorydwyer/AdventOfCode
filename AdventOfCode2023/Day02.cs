using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AdventOfCode2023
{
    public class Day02
    {
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        public static void Problem1()
        {
            Console.WriteLine("Day02 P1");
            using (var stream = new StreamReader(new FileStream("Day02.txt", FileMode.Open, FileAccess.Read)))
            {
                var r = 12;
                var g = 13;
                var b = 14;
                var line = stream.ReadLine();
                var finalTotal = 0;
                while (line != null)
                {

                    var split = line.Split(':');
                    var gameId = split[0].Split(' ')[1];
                    var rounds = split[1].Split(';');
                    var gameIsValid = true;
                    foreach (var round in rounds)
                    {
                        var colors = round.Split(',');
                        foreach (var color in colors)
                        {
                            var draw = color.Trim().Split(' ');
                            switch (draw[1])
                            {
                                case "red":
                                    if (int.Parse(draw[0]) > r)
                                    {
                                        gameIsValid = false;
                                    }
                                    break;
                                case "green":
                                    if (int.Parse(draw[0]) > g)
                                    {
                                        gameIsValid = false;
                                    }
                                    break;
                                case "blue":
                                    if (int.Parse(draw[0]) > b)
                                    {
                                        gameIsValid = false;
                                    }
                                    break;
                                default:
                                    throw new NotSupportedException();
                            }

                            if (!gameIsValid)
                            {
                                break;
                            }
                        }

                        if (!gameIsValid)
                        {
                            break;
                        }
                    }

                    if(gameIsValid)
                    {
                        finalTotal += int.Parse(gameId);
                    }

                    line = stream.ReadLine();
                }
                Console.WriteLine("Total: " + finalTotal);
            }
        }
        public static void Problem2()
        {
            Console.WriteLine("Day02 P2");
            using (var stream = new StreamReader(new FileStream("Day02.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                var finalTotal = 0;
                while (line != null)
                {

                    var split = line.Split(':');
                    var gameId = split[0].Split(' ')[1];
                    var rounds = split[1].Split(';');
                    var r = 0;
                    var g = 0;
                    var b = 0;
                    foreach (var round in rounds)
                    {
                        var colors = round.Split(',');
                        foreach (var color in colors)
                        {
                            var draw = color.Trim().Split(' ');
                            switch (draw[1])
                            {
                                case "red":
                                    if (int.Parse(draw[0]) > r)
                                    {
                                        r = int.Parse(draw[0]);
                                    }
                                    break;
                                case "green":
                                    if (int.Parse(draw[0]) > g)
                                    {
                                        g = int.Parse(draw[0]);
                                    }
                                    break;
                                case "blue":
                                    if (int.Parse(draw[0]) > b)
                                    {
                                        b = int.Parse(draw[0]);
                                    }
                                    break;
                                default:
                                    throw new NotSupportedException();
                            }
                        }
                    }

                    finalTotal += r * g * b;

                    line = stream.ReadLine();
                }
                Console.WriteLine("Total: " + finalTotal);
            }
        }
    }
}