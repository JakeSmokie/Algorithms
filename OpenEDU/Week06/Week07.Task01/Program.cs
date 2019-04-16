using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Week07.Task01 {
    public sealed class TreeNode {
        private int _depth = int.MinValue;

        public int Key { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }
        public TreeNode Parent { get; set; }

        public int Depth {
            get {
                if (_depth == int.MinValue) {
                    _depth = Math.Max(Left?.Depth ?? 0, Right?.Depth ?? 0) + 1;
                }

                return _depth;
            }

            set { _depth = value; }
        }

        public int Balance {
            get { return (Right?.Depth ?? 0) - (Left?.Depth ?? 0); }
        }

        private void UpdateDepth() {
            var node = this;

            while (node != null) {
                var rH = node.Right?.Depth ?? -1;
                var lH = node.Left?.Depth ?? -1;

                node.Depth = rH > lH ? rH + 1 : lH + 1;
                node = node.Parent;
            }
        }

        public override string ToString() {
            return $"{nameof(Key)}: {Key}, {nameof(Depth)}: {Depth}, {nameof(Balance)}: {Balance}";
        }

        public static List<TreeNode> ReadTree(int n) {
            var tree = Enumerable.Range(0, n)
                .Select(x => new TreeNode())
                .ToList();

            for (var i = 0; i < n; i++) {
                var a = ReadIntList();
                var node = tree[i];

                var l = a[1] - 1;
                var r = a[2] - 1;

                (node.Key, node.Left, node.Right) =
                    (a[0], l != -1 ? tree[l] : null, r != -1 ? tree[r] : null);

                if (node.Left != null) {
                    node.Left.Parent = node;
                }

                if (node.Right != null) {
                    node.Right.Parent = node;
                }
            }

            return tree;
        }

        private static int[] ReadIntList() {
            return Console.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToArray();
        }
    }

    public sealed class Program {
        private static StreamReader _in;
        private static StreamWriter _out;

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
            var tree = TreeNode.ReadTree(n);

            tree.ForEach(x => Console.WriteLine(x.Balance));
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