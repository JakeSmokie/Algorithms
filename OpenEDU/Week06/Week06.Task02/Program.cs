using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace Week06.Task02 {
    public sealed class Program {
        private static StreamReader _in;
        private static StreamWriter _out;

        private static void Main(string[] args) {
            if (!args.Contains("console")) {
                SetupIO();
            }

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Run();

            if (args.Contains("console")) {
                Console.ReadLine();
            }

            DisposeIO();
        }

        private static void Run() {
            var list = ReadList();

            var (n, a) = (int.Parse(list[0]), double.Parse(list[1]));
            var (l, r) = (0.0, a);

            var h = new double[n];
            h[0] = a;

            while (r - l > 0.000000000001) {
                h[1] = (r + l) / 2;
                var ok = true;

                for (var i = 2; i < n; i++) {
                    h[i] = 2 * h[i - 1] - h[i - 2] + 2;

                    if (h[i] < 0) {
                        ok = false;
                        break;
                    }
                }

                if (ok) {
                    r = h[1];
                }
                else {
                    l = h[1];
                }
            }

            Console.WriteLine($"{h[n - 1]:F8}");
        }

        private static string[] ReadList() {
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