using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Week04.Task03 {
    public sealed class Program {
        private static StreamReader _in;
        private static StreamWriter _out;

        private static void Main(string[] args) {
            if (!args.Contains("console")) {
                SetupIO();
            }

            Run();

            if (args.Contains("console")) {
                Console.ReadLine();
            }

            DisposeIO();
        }

        private static void Run() {
            var n = int.Parse(Console.ReadLine());
            var stack = new Stack<char>();

            var braces = new Dictionary<char, char> {
                ['('] = ')',
                ['['] = ']'
            };

            for (var i = 0; i < n; i++) {
                stack.Clear();
                CheckLine(Console.ReadLine());
            }

            void CheckLine(string line) {
                foreach (var c in line) {
                    if (braces.ContainsKey(c)) {
                        stack.Push(c);
                        continue;
                    }

                    if (stack.Count == 0) {
                        Console.WriteLine("NO");
                        return;
                    }

                    if (braces[stack.Pop()] == c) {
                        continue;
                    }

                    Console.WriteLine("NO");
                    return;
                }

                Console.WriteLine(stack.Count == 0 ? "YES" : "NO");
            }
        }

        private static string[] ReadLineArray() {
            return Console.ReadLine()
                .Split(' ')
                .ToArray();
        }

        private static void SetupIO() {
            _in = new StreamReader("input.txt");
            _out = new StreamWriter("output.txt");

            Console.SetIn(_in);
            Console.SetOut(_out);
        }

        private static void DisposeIO() {
            _in?.Dispose();
            _out?.Dispose();
        }
    }
}