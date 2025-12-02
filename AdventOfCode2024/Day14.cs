using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace AdventOfCode2024
{
    public class Day14
    {
        private const string Day = "Day14";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";
        private const int Width = 101;
        private const int Height = 103;
        private const int TestWidth = 11;
        private const int TestHeight = 7;


        public static void Run()
        {
            Program.WriteTitle("--- Day 14: Restroom Redoubt ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var width = Width;
            var height = Height;
            var robots = GetRobots();
            long q1, q2, q3, q4;
            q1 = q2 = q3 = q4 = 0;
            foreach (var robot in robots)
            {
                for (int i = 0; i < 100; i++)
                {
                    robot.Loc.X += robot.Vec.X;
                    if (robot.Loc.X < 0)
                    {
                        robot.Loc.X += width;
                    }

                    if (robot.Loc.X >= width)
                    {
                        robot.Loc.X -= width;
                    }

                    robot.Loc.Y += robot.Vec.Y;
                    if (robot.Loc.Y < 0)
                    {
                        robot.Loc.Y += height;
                    }

                    if (robot.Loc.Y >= height)
                    {
                        robot.Loc.Y -= height;
                    }
                }

                if (robot.Loc.X < width / 2)
                {
                    //Left
                    if (robot.Loc.Y < height / 2)
                    {
                        //Top
                        q1++;
                    }
                    else if (robot.Loc.Y >= (height / 2) + 1)
                    {
                        //Bottom
                        q3++;
                    }
                }
                else if ((robot.Loc.X >= (width / 2) + 1))
                {
                    //Right
                    if (robot.Loc.Y < height / 2)
                    {
                        //Top
                        q2++;
                    }
                    else if (robot.Loc.Y >= (height / 2) + 1)
                    {
                        //Bottom
                        q4++;
                    }
                }
            }

            Program.WriteOutput("Safety Factor: " + (q1 * q2 * q3 * q4));
        }

        private static void MoveRobots(int width, int height, bool print)
        {
            var robots = GetRobots();
            var avgX = 0.0;
            var avgY = 0.0;
            for (int i = 0; i < 7000; i++)
            {
                var runX = 0.0;
                var runY = 0.0;
                foreach (var robot in robots)
                {
                    robot.Loc.X += robot.Vec.X;
                    if (robot.Loc.X < 0)
                    {
                        robot.Loc.X += width;
                    }

                    if (robot.Loc.X >= width)
                    {
                        robot.Loc.X -= width;
                    }

                    robot.Loc.Y += robot.Vec.Y;
                    if (robot.Loc.Y < 0)
                    {
                        robot.Loc.Y += height;
                    }

                    if (robot.Loc.Y >= height)
                    {
                        robot.Loc.Y -= height;
                    }

                    runX = (runX + robot.Loc.X);
                    runY = (runY + robot.Loc.Y);

                }

                runX /= robots.Count;
                runY /= robots.Count;

                if ( i > 500 &&
                    (runX / (avgX / i) < .90  || runX / (avgX / i) > 1.1)
                    &&
                    (runY / (avgY / i) < .90 || runY / (avgY / i) > 1.1))
                {
                    Program.WriteOutput("Tree at iteration: " + (i + 1));
                    if (print)
                    {
                        var bitmap = new Bitmap(width, height);
                        foreach (var bot in robots)
                        {
                            bitmap.SetPixel(bot.Loc.X, bot.Loc.Y, Color.CadetBlue);
                        }

                        bitmap.Save("Robots" + i + ".bmp");
                    }
                    return;
                }

                avgX += runX;
                avgY += runY;
            }
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            MoveRobots(Width, Height, false);
        }

        private static List<Robot> GetRobots()
        {
            using (var reader = Program.GetReader(FileLocation))
            {
                var robots = new List<Robot>();
                var currentLine = reader.ReadLine();
                while (currentLine != null)
                {
                    var split = currentLine.Split(' ');
                    var loc = split[0].Split('=')[1].Split(',');
                    var vec = split[1].Split('=')[1].Split(',');
                    robots.Add(new Robot(new Point(int.Parse(loc[0]),int.Parse(loc[1])),
                        new Point(int.Parse(vec[0]), int.Parse(vec[1]))));
                    currentLine = reader.ReadLine();
                }
                return robots;
            }
        }
    }

    public class Robot
    {
        public Point Loc;
        public Point Vec;

        public Robot(Point l, Point v)
        {
            Loc = l;
            Vec = v;
        }
    }
}