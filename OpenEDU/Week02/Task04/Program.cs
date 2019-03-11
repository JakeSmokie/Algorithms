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
            var (n, k1, k2) = ReadFirstLine();
            var numbers = GenerateNumbers(n);

            QuickSort(numbers, 0, n - 1, k1, k2);

            for (var i = k1 - 1; i < k2; i++) {
                Console.Write($"{numbers[i]} ");
            }
        }

        private static void QuickSort<T>(IList<T> elements, int left, int right, int k1, int k2)
            where T : IComparable<T> {
            while (true) {
                if (left >= right || left > k2 - 1 || right < k1 - 1) {
                    return;
                }

                var i = left;
                var j = right;
                var pivot = elements[(left + right) / 2];

                while (i <= j) {
                    while (elements[i].CompareTo(pivot) < 0) {
                        i++;
                    }

                    while (elements[j].CompareTo(pivot) > 0) {
                        j--;
                    }

                    if (i > j) {
                        continue;
                    }

                    (elements[i], elements[j]) = (elements[j], elements[i]);

                    i++;
                    j--;
                }

                QuickSort(elements, left, j, k1, k2);
                left = i; // recursion -> iteration
            }
        }

        private static List<int> GenerateNumbers(int n) {
            var (A, B, C, a1, a2) = ReadSecondLine();
            var numbers = new List<int>(n) {a1, a2};

            for (var i = 2; i < n; i++) {
                var a = unchecked(A * numbers[i - 2] + B * numbers[i - 1] + C);
                numbers.Add(a);
            }

            return numbers;
        }

        private static (int n, int k1, int k2) ReadFirstLine() {
            var p = ReadIntList();
            return (p[0], p[1], p[2]);
        }

        private static (int A, int B, int C, int a1, int a2) ReadSecondLine() {
            var p = ReadIntList();
            return (p[0], p[1], p[2], p[3], p[4]);
        }

        private static List<int> ReadIntList() {
            return Console.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToList();
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