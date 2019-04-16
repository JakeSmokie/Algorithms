using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;

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
            var (n, lines) = ReadInitValues();
            var heap = new List<(int i, int x)>();

            for (var i = 0; i < n; i++) {
                InterpretLine(lines[i], i);
            }

            void InterpretLine(string[] line, int index) {
                if (line[0] == "A") {
                    heap.Add((index, int.Parse(line[1])));
                    SiftUp(heap.Count - 1);

                    return;
                }

                if (line[0] == "X") {
                    DeleteElement();
                    return;
                }

                SubstituteElement(line, index);
            }

            void DeleteElement() {
                if (heap.Count == 0) {
                    Console.WriteLine("*");
                    return;
                }

                Console.WriteLine(heap[0].x);

                heap[0] = heap.Last();
                heap.RemoveAt(heap.Count - 1);

                SiftDown(0);
            }

            void SubstituteElement(string[] strings, int index1) {
                var ints = strings.Skip(1)
                    .Select(int.Parse)
                    .ToArray();

                var (x, y) = (ints[0] - 1, ints[1]);

                for (var i = 0; i < heap.Count; i++) {
                    if (heap[i].i == x) {
                        heap[i] = (index1, y);

                        SiftDown(i);
                        SiftUp(i);

                        return;
                    }
                }
            }

            void SiftUp(int i) {
                while (heap[i].x < heap[(i - 1) / 2].x) {
                    (heap[i], heap[(i - 1) / 2]) = (heap[(i - 1) / 2], heap[i]);
                    i = (i - 1) / 2;
                }
            }

            void SiftDown(int i) {
                while (2 * i + 1 < heap.Count) {
                    var left = 2 * i + 1;
                    var right = 2 * i + 2;

                    var j = left;

                    if (right < heap.Count && heap[right].x < heap[left].x) {
                        j = right;
                    }

                    if (heap[i].x <= heap[j].x) {
                        break;
                    }

                    (heap[i], heap[j]) = (heap[j], heap[i]);
                    i = j;
                }
            }
        }

        private static (int n, List<string[]> lines) ReadInitValues() {
            var n = int.Parse(Console.ReadLine());

            var lines = Enumerable.Range(0, n)
                .Select(x => ReadArray())
                .ToList();

            return (n, lines);
        }

        private static string[] ReadArray() {
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