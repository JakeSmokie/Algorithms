using System;
using System.IO;
using System.Linq;

namespace Task02 {
    public sealed class Program {
        private const int Base = 26;
        private static StreamReader In;
        private static StreamWriter Out;
        private static readonly int NewLineLength = Environment.NewLine.Length;

        private static void Main(string[] args) {
            if (!args.Contains("console1")) {
                SetupIO();
            }

            Run();
            DisposeIO();
        }

        private static void Run() {
            var (n, len, k, buffer) = GetInitValues();

            // Radix sort
            var indexer = Enumerable.Range(0, n).ToArray();
            var output = new int[n];
            var count = new int[Base];

            for (var phase = 1; phase <= k; phase++) {
                Array.Clear(count, 0, Base);
                var offset = (len - phase) * n;

                for (var i = 0; i < n; i++) {
                    count[buffer[offset + i]]++;
                }

                for (var i = 1; i < Base; i++) {
                    count[i] += count[i - 1];
                }

                for (var i = n - 1; i >= 0; i--) {
                    var a = buffer[offset + indexer[i]];
                    output[--count[a]] = indexer[i];
                }

                for (var i = 0; i < n; i++) {
                    indexer[i] = output[i];
                }
            }

            //
            for (var i = 0; i < indexer.Length; i++) {
                Out.Write($"{indexer[i] + 1} ");
            }

            Out.WriteLine();
        }

        private static (int n, int m, int k, byte[] buffer) GetInitValues() {
//            var random = new Random(1000);
//            var bytes = Enumerable.Range(0, 200)
//                .Select(x => (byte) random.Next(0, 25))
//                .ToArray();
//
//            return (20, 10, 10, bytes);

            var firstLine = In.ReadLine();

            var p = firstLine
                .Split(' ')
                .Select(int.Parse)
                .ToList();

            var (n, m, k) = (p[0], p[1], p[2]);

            var stream = In.BaseStream;
            stream.Seek(firstLine.Length + NewLineLength, SeekOrigin.Begin);

            var buffer = ReadStrings(n, m, k);
            return (n, m, k, buffer);
        }

        private static byte[] ReadStrings(int n, int m, int k) {
            var stream = In.BaseStream;

            var bufferLength = n * m;
            var buffer = new byte[bufferLength];

            for (var i = 0; i < m; i++) {
                stream.Read(buffer, i * n, n);
                stream.Seek(NewLineLength, SeekOrigin.Current);
            }

            for (var i = 0; i < bufferLength; i++) {
                buffer[i] -= 0x61; // 'a'
            }

            return buffer;
        }

        private static void SetupIO() {
            In = new StreamReader("input.txt");
            Out = new StreamWriter("output.txt");
        }

        private static void DisposeIO() {
            In?.Dispose();
            Out?.Dispose();
        }
    }
}