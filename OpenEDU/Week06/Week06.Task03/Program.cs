using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Week06.Task03 {
    public sealed class TreeNode {
        public TreeNode(int left, int right) {
            Left = left;
            Right = right;
        }

        public int Left { get; set; }
        public int Right { get; set; }
    }

    public sealed class Program {
        private static StreamReader _in;
        private static StreamWriter _out;

        private static TreeNode[] _tree;

        private static void Main(string[] args) {
            if (!args.Contains("console")) {
                SetupIO();
            }

            var thread = new Thread(Run, int.MaxValue / 10);
            thread.Start();
            thread.Join();

            if (args.Contains("console")) {
                Console.ReadLine();
            }

            DisposeIO();
        }

        private static void Run() {
            var n = ReadIntList()[0];

            if (n == 0) {
                Console.WriteLine(0);
                return;
            }

            _tree = new TreeNode[n];

            for (var i = 0; i < n; i++) {
                var a = ReadIntList();
                _tree[i] = new TreeNode(a[1] - 1, a[2] - 1);
            }

            Console.WriteLine(CountDepth(0) + 1);
        }

        private static int CountDepth(int i) {
            var d = 0;

            if (_tree[i].Left != -1) {
                d = Math.Max(CountDepth(_tree[i].Left) + 1, d);
            }

            if (_tree[i].Right != -1) {
                d = Math.Max(CountDepth(_tree[i].Right) + 1, d);
            }

            return d;
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