using System;
using System.IO;
using System.Linq;

namespace Task01 {
    public sealed class Program {
        private const int Base = 256;
        private static StreamReader _in;
        private static StreamWriter _out;

        private static void Main() {
            _in = new StreamReader("input.txt");
            _out = new StreamWriter("output.txt");

            Run();

            _in?.Dispose();
            _out?.Dispose();
        }

        private static void Run() {
            var (n, m, arrayA, arrayB) = GetInitValues();

            var array = new int[n * m];
            var max = 0;
            var cursor = 0;

            for (var i = 0; i < arrayA.Length; i++) {
                var a = arrayA[i];

                for (var j = 0; j < arrayB.Length; j++) {
                    var c = a * arrayB[j];
                    array[cursor++] = c;

                    if (c > max) {
                        max = c;
                    }
                }
            }

            var output = new int[array.Length];
            var count = new int[Base];

            for (var pow = 0; 1L << pow <= max; pow += 8) {
                Array.Clear(count, 0, Base);

                for (var i = 0; i < array.Length; i++) {
                    var t = array[i];
                    count[(t >> pow) & 255]++;
                }

                for (var i = 1; i < Base; i++) {
                    count[i] += count[i - 1];
                }

                for (var i = array.Length - 1; i >= 0; i--) {
                    var a = array[i];
                    var index = (a >> pow) & 255;
                    output[--count[index]] = a;
                }

                for (var i = 0; i < array.Length; i++) {
                    array[i] = output[i];
                }
            }

            long sum = 0;

            for (var i = 0; i < array.Length; i += 10) {
                sum += array[i];
            }

            _out?.WriteLine(sum);
        }

        private static (int, int, int[], int[]) GetInitValues() {
            var (n, m) = ReadFirstLine();
            var arrayA = ReadIntList();
            var arrayB = ReadIntList();

            return (n, m, arrayA, arrayB);
        }

        private static (int N, int K) ReadFirstLine() {
            var p = ReadIntList();
            return (p[0], p[1]);
        }

        private static int[] ReadIntList() {
            return _in.ReadLine()
                .Split(' ')
                .AsParallel()
                .Select(int.Parse)
                .ToArray();
        }
    }
}