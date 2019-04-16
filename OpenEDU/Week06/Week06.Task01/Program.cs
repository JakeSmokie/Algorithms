using System;
using System.IO;
using System.Linq;

namespace Week06.Task01 {
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
            Console.ReadLine(); // skip line
            var a = ReadIntList();

            Console.ReadLine();
            var b = ReadIntList();

            foreach (var x in b) {
                var l = BinarySearchLeft(x);

                if (l == -1) {
                    Console.WriteLine("-1 -1");
                    continue;
                }

                Console.WriteLine($"{l + 1} {BinarySearchRight(x)}");
            }

            int BinarySearchRight(int x) {
                var (l, r) = (0, a.Length);

                while (l < r) {
                    var m = (l + r) / 2;

                    if (x < a[m]) {
                        r = m;
                    }
                    else {
                        l = m + 1;
                    }
                }

                return l;
            }

            int BinarySearchLeft(int x) {
                var (l, r) = (0, a.Length - 1);

                while (l < r) {
                    var m = (l + r) / 2;

                    if (a[m] < x) {
                        l = m + 1;
                    }
                    else {
                        r = m;
                    }
                }

                return l < a.Length && a[l] == x ? l : -1;
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