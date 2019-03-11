using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Task02 {
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

            var numbers = Console.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToList();

            var sortInfo = new MergeSorter().Sort(numbers);
            Console.WriteLine(sortInfo.InverseAmount);
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

    internal class MergeSorter {
        public SortInfo Sort<T>(IEnumerable<T> input) where T : IComparable<T> {
            var inverseAmount = 0L;

            var array = input.ToArray();
            SortPart(0, array.Length - 1);

            return new SortInfo(inverseAmount);


            void SortPart(int left, int right) {
                if (right <= left) {
                    return;
                }

                var mid = (left + right) / 2;

                SortPart(left, mid);
                SortPart(mid + 1, right);
                MergePart(left, mid, right);
            }

            void MergePart(int left, int mid, int right) {
                var buffer = new List<T>(right - left + 1);

                var leftCursor = left;
                var rightCursor = mid + 1;

                for (var i = 0; i < buffer.Capacity; i++) {
                    if (leftCursor <= mid && rightCursor <= right) {
                        if (array[leftCursor].CompareTo(array[rightCursor]) <= 0) {
                            buffer.Add(array[leftCursor]);
                            leftCursor += 1;
                        }
                        else {
                            buffer.Add(array[rightCursor]);
                            rightCursor += 1;

                            inverseAmount += mid - leftCursor + 1; //
                        }

                        continue;
                    }

                    if (leftCursor <= mid) {
                        buffer.Add(array[leftCursor]);
                        leftCursor += 1;

                        continue;
                    }

                    buffer.Add(array[rightCursor]);
                    rightCursor += 1;
                }


                for (var i = 0; i < buffer.Count; i++) {
                    array[left + i] = buffer[i];
                }
            }
        }

        internal class SortInfo {
            public SortInfo(long inverseAmount) {
                InverseAmount = inverseAmount;
            }

            public long InverseAmount { get; }
        }
    }
}