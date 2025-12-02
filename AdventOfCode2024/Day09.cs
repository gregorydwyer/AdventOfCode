using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2024
{
    public class Day09
    {
        private const string Day = "Day09";
        private static readonly string FileLocation = Day + ".txt";
        private static readonly string TestFileLocation = Day + "test.txt";
        private static Stack<File> Files = new Stack<File>();
        private static List<File> Space = new List<File>();

        public static void Run()
        {
            Program.WriteTitle("--- Day 9: Disk Fragmenter ---");
            Problem1();
            Problem2();
        }

        public static void Problem1()
        {
            Program.WriteProblemNumber("Part One");
            var drive = BuildDrive();
            SortDrive(drive);
            long total = 0;
            int i = 0;
            while (drive[i].HasValue)
            {
                total += i * drive[i].Value;
                i++;
            }

            Program.WriteOutput("Total: " + total);
        }

        private static void SortDrive(List<int?> drive)
        {
            var end = drive.Count - 1;
            var start = 0;

            while (start < end)
            {
                while (drive[start] != null)
                {
                    start++;
                }

                while (drive[end] == null)
                {
                    end--;
                }

                if (start < end)
                {
                    var item = drive[end];
                    drive[start] = item;
                    drive[end] = null;
                }
            }

        }

        private static List<int?> BuildDrive()
        {
            Space.Clear();
            Files.Clear();
            using (var reader = Program.GetReader(FileLocation))
            {
                var drive = new List<int?>();
                var currentLine = reader.ReadLine();
                var id = 0;
                while (currentLine != null)
                {
                    for (int i = 0; i < currentLine.Length; i++)
                    {
                        var fileStart = drive.Count;
                        var currentInt = int.Parse(currentLine[i].ToString());
                        if (i % 2 == 0)
                        {
                            Files.Push(new File(id, currentInt, fileStart));
                            //file
                            for (int j = 0; j < currentInt; j++)
                            {
                                drive.Add(id);
                            }

                            id++;
                        }
                        else
                        {
                            Space.Add(new File(id - 1, currentInt, fileStart));
                            //empty space
                            for (int j = 0; j < currentInt; j++)
                            {
                                drive.Add(null);
                            }
                        }
                    }

                    currentLine = reader.ReadLine();
                }

                return drive;
            }
        }

        public static void Problem2()
        {
            Program.WriteProblemNumber("Part Two");
            var drive = BuildDrive();
            SortByFile(drive);
            long total = 0;
            for (int j = 0; j < drive.Count; j++)
            {
                if(drive[j].HasValue)
                {
                    total += j * drive[j].Value;
                }
            }
            Program.WriteOutput("Total: " + total);

        }

        private static void SortByFile(List<int?> drive)
        {
            while (Files.Count != 0)
            {
                var currentFile = Files.Pop();
                var validSpace = Space.FirstOrDefault(space => space.Size >= currentFile.Size && space.Start < currentFile.Start);
                if (validSpace != null)
                {
                    // copy file to free space
                    for (int i = validSpace.Start; i < validSpace.Start + currentFile.Size; i++)
                    {
                        drive[i] = currentFile.Id;
                    }
                    // clear the original file's space
                    for (int i = currentFile.Start; i < currentFile.Start + currentFile.Size; i++)
                    {
                        drive[i] = null;
                    }
                    // update the open space markers
                    if (validSpace.Size == currentFile.Size)
                    {
                        Space.Remove(validSpace);
                    }
                    else
                    {
                        validSpace.Size -= currentFile.Size;
                        validSpace.Start += currentFile.Size;
                    }
                }
            }
        }
    }

    public class File
    {
        public int Id;
        public int Size;
        public int Start;

        public File(int id, int size, int start)
        {
            Id = id;
            Size = size;
            Start = start;
        }
    }
}