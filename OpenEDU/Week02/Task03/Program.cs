using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Task03 {
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
            var numbers = new List<int>(n);

            for (var i = 1; i <= n; i++) {
                numbers.Add(i);
                var mid = (i - 1) / 2;

                if (i > 2) {
                    (numbers[i - 1], numbers[mid]) = (numbers[mid], numbers[i - 1]);
                }
            }

            Console.WriteLine(string.Join(" ", numbers));
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