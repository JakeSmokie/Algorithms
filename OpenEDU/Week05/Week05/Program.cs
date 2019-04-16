using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Week04.Task04 {
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
            var n = ReadIntList()[0];
            var heap = ReadIntList();

            for (var i = 1; i <= n; i++) {
                if (IsHeap(i)) {
                    continue;
                }

                Console.WriteLine("NO");
                return;
            }

            Console.WriteLine("YES");

            bool IsHeap(int i) {
                return 2 * i <= n && heap[i - 1] <= heap[2 * i - 1] ||
                       2 * i + 1 <= n && heap[i - 1] <= heap[2 * i];
            }
        }

        private static int[] ReadIntList() {
            return Console.ReadLine()
                .Split(' ')
                .Select(int.Parse)
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