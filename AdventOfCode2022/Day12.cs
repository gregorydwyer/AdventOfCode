using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace AdventOfCode2022
{
    public static class Day12
    {
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }
        public static void Problem1()
        {
            Console.WriteLine("D12 P1");
            var map = BuildMap();
            MapNode startNode = null;
            MapNode endNode = null;
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    var current = map[i][j];
                    if (current.IsStart)
                    {
                        current.Dist = 0;
                        startNode = current;
                    }

                    if (current.IsEnd)
                    {
                        endNode = current;
                    }

                    if (i + 1 < map.Count && current.Rating + 1 >= map[i + 1][j].Rating)
                    {
                        current.Neighbors.Push(map[i + 1][j]);
                    }
                    if (i - 1 >= 0 && current.Rating + 1 >= map[i - 1][j].Rating)
                    {
                        current.Neighbors.Push(map[i - 1][j]);
                    }
                    if (j + 1 < map[i].Count && current.Rating + 1 >= map[i][j + 1].Rating)
                    {
                        current.Neighbors.Push(map[i][j + 1]);
                    }
                    if (j - 1 >= 0 && current.Rating + 1 >= map[i][j - 1].Rating)
                    {
                        current.Neighbors.Push(map[i][j - 1]);
                    }
                }
            }

            if (startNode == null || endNode == null)
            {
                throw new Exception();
            }

            BuildPaths(startNode);
            var length = endNode.Dist;
            var newLength = 0;
            while (length != newLength)
            {
                length = endNode.Dist;
                BuildPaths(startNode);
                newLength = endNode.Dist;
            }

            Console.WriteLine("Shortest path: " + endNode.Dist);
        }

        public static void Problem2()
        {
            Console.WriteLine("D12 P2");
            var map = BuildMap();
            var shortestPath = int.MaxValue;
            MapNode endNode = null;
            var startPoints = new List<MapNode>();
            foreach (var list in map)
            {
                startPoints.Add(list.First());
                foreach (var node in list)
                {
                    if (node.IsEnd)
                    {
                        endNode = node;
                    }
                }
            }
            BuildNeighborLists(map);

            if (endNode == null)
            {
                throw new Exception();
            }

            var nodes = map.SelectMany(list => list.Select(node => node));

            foreach (var startNode in startPoints)
            {
                startNode.Dist = 0;
                BuildPaths(startNode);
                var length = endNode.Dist;
                var newLength = 0;
                while (length != newLength)
                {
                    length = endNode.Dist;
                    BuildPaths(startNode);
                    newLength = endNode.Dist;
                }

                shortestPath = Math.Min(shortestPath, newLength);
                foreach (var mapNode in nodes)
                {
                    mapNode.Dist = int.MaxValue;
                }
            }

            Console.WriteLine("Shortest path: " + shortestPath);
        }

        public static void BuildNeighborLists(List<List<MapNode>> map)
        {
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    var current = map[i][j];

                    if (i + 1 < map.Count && current.Rating + 1 >= map[i + 1][j].Rating)
                    {
                        current.Neighbors.Push(map[i + 1][j]);
                    }
                    if (i - 1 >= 0 && current.Rating + 1 >= map[i - 1][j].Rating)
                    {
                        current.Neighbors.Push(map[i - 1][j]);
                    }
                    if (j + 1 < map[i].Count && current.Rating + 1 >= map[i][j + 1].Rating)
                    {
                        current.Neighbors.Push(map[i][j + 1]);
                    }
                    if (j - 1 >= 0 && current.Rating + 1 >= map[i][j - 1].Rating)
                    {
                        current.Neighbors.Push(map[i][j - 1]);
                    }
                }
            }
        }

        public static void BuildPaths(MapNode node)
        {
            var neighborsArray = node.Neighbors.ToArray().Reverse();
            var neighbors = new Stack<MapNode>(neighborsArray);

            while (neighbors.Count > 0)
            {
                var nextNode = neighbors.Pop();
                if (node.Dist + 1 < nextNode.Dist)
                {
                    nextNode.Dist = node.Dist + 1;
                    if(!nextNode.IsEnd)
                    {
                        BuildPaths(nextNode);
                    }
                }
            }
        }

        public static List<List<MapNode>> BuildMap()
        {
            using (var stream = new StreamReader(new FileStream("Day12.txt", FileMode.Open, FileAccess.Read)))
            {
                var map = new List<List<MapNode>>();
                var line = stream.ReadLine();
                while (line != null)
                {
                    map.Add(new List<MapNode>());
                    for(int c = 0; c < line.Length; c++)
                    {
                        var character = line[c];
                        if (character == 'S')
                        {
                            map.Last().Add(new MapNode()
                            {
                                Rating = 'a',
                                IsStart = true,
                                Dist = 0,
                            });
                        }
                        else if (character == 'E')
                        {
                            map.Last().Add(new MapNode()
                            {
                                Rating = 'z',
                                IsEnd = true,
                            });
                        }
                        else
                        {
                            map.Last().Add(new MapNode() {Rating = character});
                        }

                        map.Last().Last().Pos = (c, map.Count - 1);
                    }
                    line = stream.ReadLine();
                }
                return map;
            }
        }
    }

    public class MapNode
    {
        public int Rating;
        public int Dist = int.MaxValue;
        public Stack<MapNode> Neighbors = new Stack<MapNode>();
        public bool IsStart;
        public bool IsEnd;
        public (int x, int y) Pos;

    }
}
