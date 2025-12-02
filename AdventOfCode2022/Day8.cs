using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public static class Day8
    {
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        public static void Problem1()
        {
            Console.WriteLine("D8 P1");
            var table = BuildTable();
            var visibleTrees = GetVisibleTrees(table);
            Console.WriteLine("Visible Trees: " + visibleTrees.Count);
        }

        private static List<List<Tree>> BuildTable()
        {
            using (var stream = new StreamReader(new FileStream("Day8.txt", FileMode.Open, FileAccess.Read)))
            {
                var table = new List<List<Tree>>();
                var line = stream.ReadLine();
                table.Add(new List<Tree>());
                foreach (var tree in line)
                {
                    table.Last().Add(new Tree(tree));
                }

                line = stream.ReadLine();
                while (line != null)
                {
                    table.Add(new List<Tree>());
                    foreach (var tree in line)
                    {
                        table.Last().Add(new Tree(tree));
                    }

                    line = stream.ReadLine();
                }

                return table;
            }
        }

        private static HashSet<(int,int)> GetVisibleTrees(List<List<Tree>> table)
        {
            var rows = table.Count;
            var cols = table[0].Count;

            var final = new HashSet<(int, int)>();

            var ltor = new HashSet<(int,int)>();
            for (var r = 0; r < rows; r++)
            {
                var max = table[r][0];
                ltor.Add((r, 0));
                for (var c = 0; c < cols; c++)
                {
                    if (table[r][c].Value > max.Value)
                    {
                        ltor.Add((r,c));
                        table[r][c].Counted = true;
                        max = table[r][c];
                    }
                }
            }

            var rtol = new HashSet<(int, int)>();
            for (var r = 0; r < rows; r++)
            {
                var max = table[r][table[0].Count - 1];
                rtol.Add((r, table[0].Count - 1));
                for (var c = cols-1; c > 0; c--)
                {
                    if (table[r][c].Value > max.Value)
                    {
                        rtol.Add((r, c));
                        table[r][c].Counted = true;
                        max = table[r][c];
                    }
                }
            }

            var ttob = new HashSet<(int, int)>();
            for (var c = 0; c < cols; c++)
            {
                var max = table[0][c];
                ttob.Add((0, c));
                for (var r = 0; r < rows; r++)
                {
                    if (table[r][c].Value > max.Value)
                    {
                        ttob.Add((r, c));
                        table[r][c].Counted = true;
                        max = table[r][c];
                    }
                }
            }

            var btot = new HashSet<(int, int)>();
            for (var c = 0; c < cols; c++)
            {
                var max = table[table.Count - 1][c];
                btot.Add((table.Count - 1, c));
                for (var r = rows - 1; r > 0; r--)
                {
                    if (table[r][c].Value > max.Value)
                    {
                        btot.Add((r, c));
                        table[r][c].Counted = true;
                        max = table[r][c];
                    }
                }
            }

            final.UnionWith(ltor);
            final.UnionWith(rtol);
            final.UnionWith(ttob);
            final.UnionWith(btot);

            return final;
        }

        public static void Problem2()
        {
            Console.WriteLine("D8 P2");
            var table = BuildTable();
            CalculateTreeScores(table);
            var max = table.Max(row => row.Max(tree => tree.Score));
            Console.WriteLine("Highest Score: " + max);
        }

        public static void CalculateTreeScores(List<List<Tree>> table)
        {
            var rows = table.Count;
            var cols = table[0].Count;

            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < cols; c++)
                {
                    var current = table[r][c];
                    bool left, right, up, down;
                    left = right = up = down = true;
                    var i = 1;
                    while (left || right || up || down)
                    {
                        //left
                        if (left && c - i >= 0 && table[r][c-i].Value < current.Value)
                        {
                            // do nothing, keep looking
                        }
                        else if(left && c - i < 0)
                        {
                            current.Left = (i - 1);
                            left = false;
                        }
                        else if (left)
                        {
                            current.Left = i;
                            left = false;
                        }
                        //right
                        if (right && c + i < cols && table[r][c + i].Value < current.Value)
                        {
                            // do nothing, keep looking
                        }
                        else if (right && c + i >= cols)
                        {
                            current.Right = (i - 1);
                            right = false;
                        }
                        else if (right)
                        {
                            current.Right = i;
                            right = false;
                        }
                        //up
                        if (up && r - i >= 0 && table[r - i][c].Value < current.Value)
                        {
                            // do nothing, keep looking
                        }
                        else if (up && r - i < 0)
                        {
                            current.Up = (i - 1);
                            up = false;
                        }
                        else if (up)
                        {
                            current.Up = i;
                            up = false;
                        }
                        //down
                        if (down && r + i < rows && table[r + i][c].Value < current.Value)
                        {
                            // do nothing, keep looking
                        }
                        else if (down && r + i >= rows)
                        {
                            current.Down = (i - 1);
                            down = false;
                        }
                        else if (down)
                        {
                            current.Down = i;
                            down = false;
                        }
                        i++;
                    }
                    current.Score = current.Left * current.Right * current.Up * current.Down;
                }
            }
        }
    }

    public class Tree
    {
        public int Value;
        public bool Counted;
        public int Left;
        public int Right;
        public int Up;
        public int Down;
        public int Score;

        public Tree(int value)
        {
            Value = value;
        }
    }
}
