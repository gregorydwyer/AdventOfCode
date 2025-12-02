using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace AdventOfCode2022
{
    public static class Day16
    {
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }
        public static void Problem1()
        {
            Console.WriteLine("D16 P1");
            var valves = MakeValves();
            var rand = new Random();
            var totalFlow = DFS(valves["AA"], 0, -1);
            Console.WriteLine("Total flow: " + totalFlow);
        }

        public static int DFS(Valve valve, int total, int count)
        {
            count++;
            if (count == 30)
            {
                return total;
            }
            else if (!valve.IsOpen && valve.FlowRate > 0)
            {
                valve.OpenValve();
                count++;
                total += (30 - count) * valve.FlowRate;
            }
            if (count == 30)
            {
                return total;
            }
            var options = new List<int>();
            //foreach (var conn in valve.Connections.Where(v => !v.IsOpen))
            //{
            //    options.Add(DFS(conn, total, count));
            //}
            return 0;
            return options.Max();
        }

        public static void Problem2()
        {
            Console.WriteLine("D16 P2");
            var valves = MakeValves();
        }

        public static Dictionary<string, Valve> MakeValves()
        {
            var allValves = new Dictionary<string, Valve>();
            using (var stream = new StreamReader(new FileStream("Test16.txt", FileMode.Open, FileAccess.Read), Encoding.GetEncoding(1252)))
            {
                var line = stream.ReadLine();
                while (line != null)
                {
                    var name = line.Substring(6, 2);
                    var flow = int.Parse(line.Split(';')[0].Substring(line.IndexOf('=') + 1));
                    var parts = line.Split(';')[1].Split(' ').Where(str => !string.IsNullOrEmpty(str));
                    var connections = parts.Select(str => str.Substring(0, 2))
                        .Where(str => char.IsUpper(str[0]) && char.IsUpper(str[1]));
                    allValves.Add(name, new Valve(name, flow, connections));
                    line = stream.ReadLine();
                }
            }

            var useful = allValves.Where(v => v.Value.FlowRate > 0).Select(v => v.Value).ToList();

            foreach (var valve in useful)
            {
                valve.BuildConnections(allValves, useful);
            }

            var temp = new Dictionary<string, Valve>();
            foreach (var valve in useful)
            {
                temp.Add(valve.Name, valve);
            }
            return temp;
        }
    }

    public class Valve : IEquatable<Valve>
    {
        public string Name;
        public int FlowRate;
        public bool IsOpen = false;
        public List<(int cost, Valve dest)> Connections = new List<(int cost, Valve)>();
        public List<string> Paths;

        public Valve(string name, int flowRate, IEnumerable<string> connections)
        {
            Name = name;
            FlowRate = flowRate;
            Paths = new List<string>(connections);
        }

        public void OpenValve()
        {
            IsOpen = true;
        }

        public void BuildConnections(Dictionary<string, Valve> all, List<Valve> useful)
        {
            foreach (var valve in useful)
            {
                var shortestPath = int.MaxValue;
                foreach (var path in Paths)
                {
                    var visited = new Dictionary<Valve, bool>();
                    foreach (var v in all)
                    {
                        visited.Add(v.Value, false);
                    }

                    visited[this] = true;
                    shortestPath = Math.Min(shortestPath, BFS(valve, all, 0, visited));
                }

            }

        }

        public int BFS(Valve valve, Dictionary<string, Valve> all, int total, Dictionary<Valve, bool> visited)
        {
            if (this.Equals(valve))
            {
                return total;
            }

            visited[valve] = true;
            if (Connections.Any(v => v.dest.Equals(valve)))
            {
                return total + Connections.First(v => v.dest.Equals(valve)).cost;
            }
            if (valve.Paths.Any(p => !visited[all[p]]))
            {

                foreach (var str in valve.Paths.Where(p => !visited[all[p]]))
                {
                    var temp = all[str];
                    return temp.BFS(valve, all, ++total, visited);
                }
            }
            return int.MaxValue;
        }

        public bool Equals(Valve other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && FlowRate == other.FlowRate && IsOpen == other.IsOpen;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Valve) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ FlowRate;
                hashCode = (hashCode * 397) ^ IsOpen.GetHashCode();
                return hashCode;
            }
        }
    }
}