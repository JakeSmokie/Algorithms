using System;
using System.IO;
using System.Linq;

namespace Week04.Task02 {
    public sealed class Program {
        private static StreamReader In;
        private static StreamWriter Out;

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
            var queue = new string[1000000];
            var cursorTop = 0;
            var cursorBottom = 0;

            for (var i = 0; i < n; i++) {
                var line = ReadLineArray();

                if (line[0] == "+") {
                    queue[cursorTop++] = line[1];
                    continue;
                }

                Console.WriteLine(queue[cursorBottom++]);
            }
        }

        private static string[] ReadLineArray() {
            return Console.ReadLine()
                .Split(' ')
                .ToArray();
        }

        private static void SetupIO() {
            In = new StreamReader("input.txt");
            Out = new StreamWriter("output.txt");

            Console.SetIn(In);
            Console.SetOut(Out);
        }

        private static void DisposeIO() {
            In?.Dispose();
            Out?.Dispose();
        }
    }
}