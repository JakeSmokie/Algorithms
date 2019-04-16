using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Week07.Task02 {
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

        public TreeNode Insert(int value) {
            var node = this;

            while (true) {
                if (node.Key < value) {
                    if (node.Left != null) {
                        node = node.Left;
                        continue;
                    }

                    node.Left = new TreeNode {
                        Parent = node,
                        Key = value
                    };

                    node.Left.UpdateDepth();
                    return node.Left;
                }

                if (node.Right != null) {
                    node = node.Right;
                    continue;
                }

                node.Right = new TreeNode {
                    Parent = node,
                    Key = value
                };

                node.Right.UpdateDepth();
                return node.Right;
            }
        }

        public TreeNode BalanceTree() {
            var current = this;

            while (current != null) {
                var balance = current.Balance;

                if (balance > 1) {
                    current = current.Right?.Balance == -1 ? current.BigLeftTurn() : current.SmallLeftTurn();
                }

                if (balance < -1) {
                    current = current.Left?.Balance == 1 ? current.BigRightTurn() : current.SmallRightTurn();
                }

                if (current.Parent == null) {
                    return current;
                }

                current = current.Parent;
            }

            return current;
        }

        private TreeNode SmallLeftTurn() {
            var child = Right;
            var parent = Parent;
            var x = Left;
            var y = Right.Left;
            var z = Right.Right;

            //Parents
            child.Parent = parent;
            Parent = child;

            if (x != null) {
                x.Parent = this;
            }

            if (y != null) {
                y.Parent = this;
            }

            if (z != null) {
                z.Parent = child;
            }

            //Childs
            Left = x;
            Right = y;
            child.Left = this;
            child.Right = z;

            if (parent != null) {
                if (parent.Right != this) {
                    parent.Left = child;
                }
                else {
                    parent.Right = child;
                }
            }

            //Depths
            var xH = x?.Depth ?? -1;
            var yH = y?.Depth ?? -1;
            var zH = z?.Depth ?? -1;

            Depth = xH > yH ? xH + 1 : yH + 1;
            child.Depth = Depth > zH ? Depth + 1 : zH + 1;

            child.UpdateDepth();
            return child;
        }

        private TreeNode SmallRightTurn() {
            var child = Left;
            var parent = Parent;
            var x = Left;
            var y = Left.Left;
            var z = Left.Right;

            //Parents
            child.Parent = parent;
            Parent = child;

            if (x != null) {
                x.Parent = this;
            }

            if (y != null) {
                y.Parent = child;
            }

            if (z != null) {
                z.Parent = this;
            }

            //Childs
            Left = z;
            Right = x;
            child.Left = y;
            child.Right = this;

            if (parent != null) {
                if (parent.Right == this) {
                    parent.Right = child;
                }
                else {
                    parent.Left = child;
                }
            }

            //Depths
            var xH = x?.Depth ?? -1;
            var yH = y?.Depth ?? -1;
            var zH = z?.Depth ?? -1;

            Depth = zH > xH ? zH + 1 : xH + 1;
            child.Depth = y.Depth > Depth ? yH + 1 : Depth + 1;

            child.UpdateDepth();
            return child;
        }

        public TreeNode BigRightTurn() {
            var w = Right;
            var parent = Parent;
            var b = Left;
            var c = Left.Right;
            var z = b.Left;
            var x = c.Left;
            var y = c.Right;

            //Parents
            c.Parent = parent;
            b.Parent = c;
            Parent = c;

            if (w != null) {
                w.Parent = this;
            }

            if (z != null) {
                z.Parent = b;
            }

            if (y != null) {
                y.Parent = this;
            }

            if (x != null) {
                x.Parent = b;
            }

            //Childs
            if (parent != null) {
                if (parent.Right == this) {
                    parent.Right = c;
                }
                else {
                    parent.Left = c;
                }
            }

            c.Left = b;
            c.Right = this;
            b.Left = z;
            b.Right = x;
            Left = y;
            Right = w;

            //Depths
            var xH = x?.Depth ?? -1;
            var yH = y?.Depth ?? -1;
            var zH = z?.Depth ?? -1;
            var wH = w?.Depth ?? -1;

            b.Depth = zH > xH ? zH + 1 : xH + 1;
            Depth = yH > wH ? yH + 1 : wH + 1;

            c.Depth = b.Depth > Depth ? b.Depth + 1 : Depth + 1;
            c.UpdateDepth();

            return c;
        }

        public TreeNode BigLeftTurn() {
            var w = Left;
            var parent = Parent;
            var b = Right;
            var c = Right.Left;
            var z = b.Right;
            var x = c.Left;
            var y = c.Right;

            //Parents
            c.Parent = parent;
            b.Parent = c;
            Parent = c;

            if (w != null) {
                w.Parent = this;
            }

            if (z != null) {
                z.Parent = b;
            }

            if (y != null) {
                y.Parent = b;
            }

            if (x != null) {
                x.Parent = this;
            }

            //Childs
            if (parent != null) {
                if (parent.Right == this) {
                    parent.Right = c;
                }
                else {
                    parent.Left = c;
                }
            }

            c.Left = this;
            c.Right = b;
            b.Left = y;
            b.Right = z;
            Left = w;
            Right = x;

            //Depths
            var xH = x?.Depth ?? -1;
            var yH = y?.Depth ?? -1;
            var zH = z?.Depth ?? -1;
            var wH = w?.Depth ?? -1;

            Depth = wH > xH ? wH + 1 : xH + 1;
            b.Depth = yH > zH ? yH + 1 : zH + 1;
            c.Depth = b.Depth > Depth ? b.Depth + 1 : Depth + 1;

            c.UpdateDepth();
            return c;
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
            var root = tree[0].BalanceTree();

            Console.WriteLine(n);
            PrintTree(root);

            void PrintTree(TreeNode node) {
                var queue = new Queue<TreeNode>();
                var counter = 1;

                queue.Enqueue(node);

                while (queue.Count > 0) {
                    var current = queue.Dequeue();
                    var l = 0;
                    var r = 0;

                    if (current.Left != null) {
                        queue.Enqueue(current.Left);
                        l = ++counter;
                    }

                    if (current.Right != null) {
                        queue.Enqueue(current.Right);
                        r = ++counter;
                    }

                    Console.WriteLine($"{current.Key} {l} {r}");
                }
            }
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