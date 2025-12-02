using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace AdventOfCode2022
{
    public static class Day15
    {
        public static void Run()
        {
            Problem1();
            Problem2();
            Console.ReadKey();
        }

        public static void Problem1()
        {
            Console.WriteLine("D15 P1");
            var sensors = new HashSet<Sensor>();
            var locations = new HashSet<Point>();
            using (var stream = new StreamReader(new FileStream("Day15.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                while (line != null)
                {
                    var sensorText = line.Split(':')[0].Substring(10);
                    var beaconText = line.Split(':')[1].Substring(22);
                    var sensor = new Sensor(TextToPoint(sensorText), TextToPoint(beaconText));
                    locations.Add(sensor);
                    locations.Add(sensor.Beacon);
                    sensors.Add(sensor);
                    line = stream.ReadLine();
                }
            }
            Console.Write("Searching near sensors...");
            var count = 0;
            foreach (var sensor in sensors)
            {
                if (!(sensor.Y + sensor.ScanDistance >= 2000000 || sensor.Y - sensor.ScanDistance <= 2000000))
                {
                    continue;
                }
                count++;
                for (int c = sensor.X - sensor.ScanDistance; c <= sensor.X + sensor.ScanDistance; c++)
                {
                    var point = new Point(c, 2000000);
                    if (IsWithingSensorRange(sensor, point))
                    {
                        locations.Add(point);
                    }
                    else if (c > sensor.X)
                    {
                        break;
                    }
                }
                Console.Write((count * 4) + "%...");
            }
            Console.Write("100%");
            Console.WriteLine();

            Console.WriteLine("Blocked locations: " + locations.Count(point => point.Y == 2000000 && !(point is Beacon)));
        }

        public static void Problem2()
        {
            Console.WriteLine("D15 P2");
            var sensors = new HashSet<Sensor>();
            using (var stream = new StreamReader(new FileStream("Day15.txt", FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                while (line != null)
                {
                    var sensorText = line.Split(':')[0].Substring(10);
                    var beaconText = line.Split(':')[1].Substring(22);
                    var sensor = new Sensor(TextToPoint(sensorText), TextToPoint(beaconText));
                    sensors.Add(sensor);
                    line = stream.ReadLine();
                }
            }

            var rows = new Row[4000001];
            Console.Write("Scanning...");
            var count = 1;
            foreach (var sensor in sensors)
            {
                var rMin = Math.Max(0, sensor.Y - sensor.ScanDistance);
                var rMax = Math.Min(4000000, sensor.Y + sensor.ScanDistance);
                for (int r = rMin; r <= rMax; r++)
                {
                    var height = Math.Abs(r - sensor.Y);
                    if (rows[r] == null)
                    {
                        rows[r] = new Row();
                    }

                    var range = new Range(Math.Max(0, sensor.X - (sensor.ScanDistance - height)),
                        Math.Min(4000000, sensor.X + (sensor.ScanDistance - height)));
                    rows[r].AddCoverage(range);
                    if (string.IsNullOrEmpty(rows[r].ToString()))
                    {
                        Debugger.Break();
                    }
                }
                Console.Write((int)(((double)count / sensors.Count) * 100.0) + "%");
                if (count < sensors.Count - 1)
                {
                    Console.Write("...");
                }
                count++;
            }
            Console.WriteLine();

            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i] == null)
                {
                    Console.WriteLine("Row {0} was blank.", i);
                    continue;
                }

                rows[i].CollapseCoverage();
                if (rows[i].CoverageSectionCount() > 1 || !rows[i].Contains(0, 4000000))
                {

                    Console.WriteLine("Row: " + i);
                    Console.WriteLine("Range(s): " + rows[i]);
                    var temp = rows[i].GetUncoveredIndices();
                    var frequency = new BigInteger(rows[i].GetUncoveredIndices()[0]) * 4000000 + i;
                    Console.WriteLine("Frequency: " + frequency);
                }
            }

        }

        public static Point TextToPoint(string text)
        {
            var parts = text.Split(' ').Select(str => str.Trim(',').Substring(2)).ToArray();
            return new Point(int.Parse(parts[0]), int.Parse(parts[1]));
        }

        public static bool IsWithingSensorRange(Sensor sensor, Point point)
        {
            return Math.Abs(sensor.X - point.X) + Math.Abs(sensor.Y - point.Y) <= sensor.ScanDistance;
        }
    }

    public class Sensor : Point
    {
        public Beacon Beacon;

        public int ScanDistance;

        public Sensor(int x, int y, Point beacon) : base(x, y)
        {
            Beacon = new Beacon(beacon);
            ScanDistance = Math.Abs(X - Beacon.X) + Math.Abs(Y - Beacon.Y);
        }

        public Sensor(Point s, Point b) : base(s)
        {
            Beacon = new Beacon(b);
            ScanDistance = Math.Abs(X - Beacon.X) + Math.Abs(Y - Beacon.Y);
        }
    }

    public class Beacon : Point
    {
        private bool IsBeacon = true;
        public Beacon(int x, int y) : base(x, y)
        {
        }

        public Beacon(Point p) : base(p)
        {
        }
    }

    public class Row
    {
        private List<Range> _coverage = new List<Range>();

        public void CollapseCoverage()
        {
            _coverage = Range.CollapseRanges(_coverage);
        }

        public void AddCoverage(Range r)
        {
            _coverage.Add(r);
            CollapseCoverage();
        }

        public int CoverageSectionCount()
        {
            CollapseCoverage();
            return _coverage.Count;
        }

        public bool Contains(int s, int e)
        {
            CollapseCoverage();
            return _coverage.Any(range => range.Start <= s && range.End >= e);
        }

        public override string ToString()
        {
            var output = "";
            foreach (var range in _coverage)
            {
                output += " " + range.ToString();
            }

            return output;
        }

        public int[] GetUncoveredIndices()
        {
            var indices = new List<int>();
            for (int i = 0; i < 4000000; i++)
            {
                var wasFound = false;
                foreach (var range in _coverage)
                {
                    if (range.IsInRange(i))
                    {
                        wasFound = true;
                        break;
                    }
                }

                if (!wasFound)
                {
                    indices.Add(i);
                }
            }

            return indices.ToArray();
        }
    }

    public class Range : IComparable<Range>
    {
        public int Start;
        public int End;

        public Range(int start, int end)
        {
            Start = start;
            End = end;
        }

        public bool IsInRange(int i)
        {
            return i <= End && i >= Start;
        }

        public Range Union(Range other)
        {
            if ((Start <= other.End && Start >= other.Start)
                ||(other.Start <= End && other.Start >= Start))
            {
                return new Range(Math.Min(Start, other.Start), Math.Max(End, other.End));
            }

            return null;

        }

        public override string ToString()
        {
            return "(" + Start + ", " + End + ")";
        }

        public static List<Range> CollapseRanges(List<Range> ranges)
        {
            if (ranges.Count < 2)
            {
                return ranges;
            }

            ranges.Sort();
            var newList = new List<Range>();
            var newRange = ranges[0];
            for (int i = 0; i < ranges.Count - 1; i++)
            {
                var temp = newRange.Union(ranges[i + 1]);
                if (temp == null)
                {
                    newList.Add(newRange);
                    newRange = ranges[i + 1];
                }
                else
                {
                    newRange = temp;
                }
            }
            newList.Add(newRange);
            return newList;

        }

        public int CompareTo(Range other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var startComparison = Start.CompareTo(other.Start);
            if (startComparison != 0) return startComparison;
            return End.CompareTo(other.End);
        }
    }
}