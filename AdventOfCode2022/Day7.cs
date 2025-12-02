using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public static class Day7
    {
        private const int TotalSpace = 70000000;
        private const int NeededSpace = 30000000;
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        public static void Problem1()
        {
            Console.WriteLine("D7 P1");
            using (var stream = new StreamReader(new FileStream("Day7.txt", FileMode.Open, FileAccess.Read)))
            {
                var count = 0;
                var queue = new Queue<int>();
                var collection = new Collection<Node>();
                var rootNode = new Node(null, "root");
                collection.Add(rootNode);
                Node currentNode = rootNode;

                // get initial state
                while (!stream.EndOfStream)
                {
                    var line = stream.ReadLine()?.Split(' ') ?? new string[]{};
                    while (line.Length == 0)
                    {
                        line = stream.ReadLine()?.Split(' ') ?? new string[] { };
                    }

                    if (line[0].Equals("$"))
                    {
                        if (line[1].Equals("cd"))
                        {
                            switch (line[2])
                            {
                                case "/":
                                    currentNode = rootNode;
                                    break;
                                case "..":
                                    currentNode = currentNode.Parent;
                                    break;
                                default:
                                    currentNode =
                                        currentNode.Children.FirstOrDefault(node => node.Name.Equals(line[2]));
                                    break;
                            }
                        }
                    }
                    else if (line[0].Equals("dir"))
                    {
                        var child = new Node(currentNode, line[1]);
                        collection.Add(child);
                        currentNode.AddChild(child);
                    }
                    else
                    {
                        currentNode.AddFile(int.Parse(line[0]));
                    }

                }

                _ = rootNode.GetFinalSize();

                var total = 0;
                foreach (var node in collection)
                {
                    if (node.DirSize <= 100000)
                    {
                        total += node.DirSize;
                    }
                }

                Console.WriteLine("Total Size: " + total);

            }
        }

        public static void Problem2()
        {
            Console.WriteLine("D7 P2");
            using (var stream = new StreamReader(new FileStream("Day7.txt", FileMode.Open, FileAccess.Read)))
            {
                var count = 0;
                var queue = new Queue<int>();
                var collection = new Collection<Node>();
                var rootNode = new Node(null, "root");
                collection.Add(rootNode);
                Node currentNode = rootNode;

                // get initial state
                while (!stream.EndOfStream)
                {
                    var line = stream.ReadLine()?.Split(' ') ?? new string[] { };
                    while (line.Length == 0)
                    {
                        line = stream.ReadLine()?.Split(' ') ?? new string[] { };
                    }

                    if (line[0].Equals("$"))
                    {
                        if (line[1].Equals("cd"))
                        {
                            switch (line[2])
                            {
                                case "/":
                                    currentNode = rootNode;
                                    break;
                                case "..":
                                    currentNode = currentNode.Parent;
                                    break;
                                default:
                                    currentNode =
                                        currentNode.Children.FirstOrDefault(node => node.Name.Equals(line[2]));
                                    break;
                            }
                        }
                    }
                    else if (line[0].Equals("dir"))
                    {
                        var child = new Node(currentNode, line[1]);
                        collection.Add(child);
                        currentNode.AddChild(child);
                    }
                    else
                    {
                        currentNode.AddFile(int.Parse(line[0]));
                    }

                }

                var totalSize = rootNode.GetFinalSize();
                var currentUnused = TotalSpace - totalSize;
                var needToClear = NeededSpace - currentUnused;
                var dirSize = totalSize;
                foreach (var node in collection)
                {
                    if (node.DirSize > needToClear)
                    {
                        dirSize = Math.Min(dirSize, node.DirSize);
                    }
                }

                Console.WriteLine("Directory Size: " + dirSize);

            }
        }
    }


    public class Node
    {
        public Node Parent = null;

        public List<Node> Children = new List<Node>();

        public string Name;

        private int _size;

        public int DirSize;

        public Node(Node parent, string name)
        {
            Parent = parent;
            Name = name;
        }

        public void AddFile(int size)
        {
            _size += size;
        }

        public int GetFinalSize()
        {
            var total = _size;
            foreach(var child in Children)
            {
                total += child.GetFinalSize();
            }

            DirSize = total;

            return total;
        }

        public void AddChild(Node child)
        {
            Children.Add(child);
        }

    }
}
