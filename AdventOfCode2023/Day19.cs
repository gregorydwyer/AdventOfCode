using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2023
{
    public static class Day19
    {
        private const string FileName = "Day19.txt";

        public static void Run()
        {
            Problem1();
            //Problem2Threaded();
            Console.ReadKey();
        }

        private static void Problem1()
        {
            Console.WriteLine("Day19 P1");
            var input = BuildRulesAndParts();
            var rules = input.Rules;
            var parts = input.Parts;
            var total = 0L;
            foreach (var part in parts)
            {
                if (IsPartAccepted(rules, part))
                {
                    total += part.Value;
                }
            }
            Console.WriteLine($"Total: {total}");
        }

        private static bool IsPartAccepted(Dictionary<string, Rule> rules, Part part)
        {
            var next = "in";
            while (next != "A" && next != "R")
            {
                next = rules[next].EvaluatePart(part);
            }
            return next == "A";
        }

        private static (Dictionary<string, Rule> Rules, List<Part> Parts) BuildRulesAndParts()
        {
            using (var stream = new StreamReader(new FileStream(FileName, FileMode.Open, FileAccess.Read)))
            {
                var line = stream.ReadLine();
                var rules = new Dictionary<string, Rule>();
                //build rules
                while (!string.IsNullOrEmpty(line))
                {
                    var rule = new Rule(line);
                    rules.Add(rule.Label, rule);
                    line = stream.ReadLine();
                }

                line = stream.ReadLine();
                // get parts
                var parts = new List<Part>();
                while (line != null)
                {
                    parts.Add(new Part(line));
                    line = stream.ReadLine();
                }

                return (rules, parts);
            }
        }

        private static void Problem2()
        {
            Console.WriteLine("Day19 P2");
            Console.WriteLine("Total loss: {total}");
        }

        private class Part
        {
            public int X;
            public int M;
            public int A;
            public int S;
            public int Value => X + M + A + S;

            public Part(string str)
            {
                str = str.Trim(new[] {'}', '{'});
                var split = str.Split(',');
                foreach (var s in split)
                {
                    var value = s.Split('=');
                    switch (value[0])
                    {
                        case "x":
                            X = int.Parse(value[1]);
                            break;
                        case "m":
                            M = int.Parse(value[1]);
                            break;
                        case "a":
                            A = int.Parse(value[1]);
                            break;
                        case "s":
                            S = int.Parse(value[1]);
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
            }
        }

        private class Rule
        {
            public string Label;
            private string[] Checks;

            public Rule(string str)
            {
                var open = str.IndexOf('{');
                Label = str.Substring(0, open);
                var rules = str.Substring(open + 1, str.IndexOf('}') - open - 1).Split(',');
                Checks = rules;
            }

            public string EvaluatePart(Part part)
            {
                foreach (var rule in Checks)
                {
                    if (!rule.Contains(":"))
                    {
                        return rule;
                    }

                    switch (rule[0])
                    {
                        case 'x':
                            if (CheckRule(rule, part.X))
                            {
                                return GetNextRule(rule);
                            }
                            break;
                        case 'm':
                            if (CheckRule(rule, part.M))
                            {
                                return GetNextRule(rule);
                            }
                            break;
                        case 'a':
                            if (CheckRule(rule, part.A))
                            {
                                return GetNextRule(rule);
                            }
                            break;
                        case 's':
                            if (CheckRule(rule, part.S))
                            {
                                return GetNextRule(rule);
                            }
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }

                throw new NotSupportedException();
            }

            private string GetNextRule(string str)
            {
                return str.Split(':')[1];
            }

            private bool CheckRule(string fullRule, int value)
            {
                var rule = fullRule.Substring(0, fullRule.IndexOf(':'));
                if (rule.Contains("<"))
                {
                    var comp = rule.Split('<');
                    return value < int.Parse(comp[1]);
                }
                else if(rule.Contains(">"))
                {
                    var comp = rule.Split('>');
                    return value > int.Parse(comp[1]);
                }

                throw new NotSupportedException();
            }
        }
    }
}