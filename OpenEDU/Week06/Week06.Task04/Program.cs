using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Week06.Task04 {
    public sealed class TreeNode {
        public int Key { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Parent { get; set; } = -1;
    }

    public sealed class Program {
        private static StreamReader _in;
        private static StreamWriter _out;

        private static List<TreeNode> _tree;

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
            var n = ReadIntList()[0];

            ReadTree(n);
            Console.ReadLine(); // skip line

            foreach (var m in ReadIntList()) {
                var nodeIndex = FindNodeIndexByKey(m);

                if (nodeIndex == -1) {
                    Console.WriteLine(n);
                    continue;
                }

                var parent = _tree[_tree[nodeIndex].Parent];

                if (parent.Left == nodeIndex) {
                    parent.Left = -1;
                }
                else {
                    parent.Right = -1;
                }

                n -= CountNodes(nodeIndex);
                Console.WriteLine(n);
            }
        }

        private static void ReadTree(int n) {
            _tree = Enumerable.Range(0, n)
                .Select(x => new TreeNode())
                .ToList();

            for (var i = 0; i < n; i++) {
                var a = ReadIntList();
                var node = _tree[i];

                (node.Key, node.Left, node.Right) =
                    (a[0], a[1] - 1, a[2] - 1);

                if (node.Left != -1) {
                    _tree[node.Left].Parent = i;
                }

                if (node.Right != -1) {
                    _tree[node.Right].Parent = i;
                }
            }
        }

        private static int FindNodeIndexByKey(int key) {
            var i = 0;

            while (_tree[i].Key != key) {
                if (key < _tree[i].Key) {
                    if (_tree[i].Left == -1) {
                        return -1;
                    }

                    i = _tree[i].Left;
                    continue;
                }

                if (_tree[i].Right == -1) {
                    return -1;
                }

                i = _tree[i].Right;
            }

            return i;
        }

        private static int CountNodes(int i) {
            var d = 1;

            if (_tree[i].Left != -1) {
                d += CountNodes(_tree[i].Left);
            }

            if (_tree[i].Right != -1) {
                d += CountNodes(_tree[i].Right);
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