using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Task05 {
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
            var (n, k) = ReadFirstLine();
            var numbers = ReadIntList();

            Sort(numbers, n, k);

            var notSorted = Enumerable.Range(0, n - 1)
                .Any(i => numbers[i] > numbers[i + 1]);

            Console.WriteLine(notSorted ? "NO" : "YES");
        }

        private static void Sort(List<int> numbers, int n, int k) {
            if (k == 1) {
                QuickSort(numbers, 0, n - 1, k);
                return;
            }

            var offset = n - n % k;

            for (var i = 0; i < k; i++) {
                var cursor = i + offset;
                QuickSort(numbers, i, cursor < n ? cursor : cursor - k, k);
            }
        }

        private static void QuickSort<T>(IList<T> elements, int left, int right, int k) where T : IComparable<T> {
            while (true) {
                if (left >= right) {
                    return;
                }

                var i = left;
                var j = right;
                var pivot = elements[(left + right) / (k * 2) * k + left % k];

                while (i <= j) {
                    while (elements[i].CompareTo(pivot) < 0) {
                        i += k;
                    }

                    while (elements[j].CompareTo(pivot) > 0) {
                        j -= k;
                    }

                    if (i > j) {
                        continue;
                    }

                    (elements[i], elements[j]) = (elements[j], elements[i]);

                    i += k;
                    j -= k;
                }

                QuickSort(elements, left, j, k);
                left = i; // recursion -> iteration
            }
        }

        private static (int N, int K) ReadFirstLine() {
            var p = ReadIntList();
            return (p[0], p[1]);
        }

        private static List<int> ReadIntList() {
            return Console.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToList();
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