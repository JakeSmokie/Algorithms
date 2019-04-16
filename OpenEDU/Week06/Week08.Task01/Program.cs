using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Week08.Task01 {
    public struct Int64Wrapper {
        private int _hashCode;
        
        public long X { get; }

        public Int64Wrapper(long x) {
            X = x;
            _hashCode = GetHashCode(x);
        }

        public bool Equals(Int64Wrapper other) {
            return X == other.X;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }

            return obj is Int64Wrapper wrapper && Equals(wrapper);
        }

        public override int GetHashCode() {
            return _hashCode;
        }

        public override string ToString() {
            return X.ToString();
        }

        private static int GetHashCode(long x) {
            var bytes = BitConverter.GetBytes(x);
            var hash = 0;

            foreach (var b in bytes) {
                hash += b;
                hash += hash << 10;
                hash ^= hash >> 6;
            }

            hash += hash << 3;
            hash ^= hash >> 11;
            hash += hash << 15;
            return hash;
        }
    }

    public class MyDictionary<TKey, TValue> {
        private sealed class Entry : IEquatable<Entry> {
            public TKey Key;
            public TValue Value;
            public Entry Next;

            public Entry(TKey key, TValue value) {
                Key = key;
                Value = value;
            }

            public bool Equals(Entry other) {
                if (ReferenceEquals(null, other)) {
                    return false;
                }

                if (ReferenceEquals(this, other)) {
                    return true;
                }

                return EqualityComparer<TKey>.Default.Equals(Key, other.Key);
            }

            public override bool Equals(object obj) {
                if (ReferenceEquals(null, obj)) {
                    return false;
                }

                if (ReferenceEquals(this, obj)) {
                    return true;
                }

                return obj is Entry && Equals((Entry) obj);
            }

            public override int GetHashCode() {
                return EqualityComparer<TKey>.Default.GetHashCode(Key);
            }

            public static bool operator ==(Entry left, Entry right) {
                return Equals(left, right);
            }

            public static bool operator !=(Entry left, Entry right) {
                return !Equals(left, right);
            }

            public override string ToString() {
                return $"K: {Key}, V: {Value}";
            }
        }

        private const int Size = 21_123_381;
        private Entry[] _buckets = new Entry[Size];

        public void Add(TKey key, TValue value) {
            if (ContainsKey(key)) {
                return;
            }

            Insert(new Entry(key, value));
        }

        public void Remove(TKey key) {
            var bucketHash = GetHash(key);
            var node = _buckets[bucketHash];

            if (node?.Key?.Equals(key) ?? false) {
                _buckets[bucketHash] = node?.Next;
                return;
            }

            while (node?.Next != null) {
                if (node.Next.Key.Equals(key)) {
                    node.Next = node.Next.Next;
                    break;
                }

                node = node.Next;
            }
        }

        public bool ContainsKey(TKey key) {
            var bucketHash = GetHash(key);
            var node = _buckets[bucketHash];

            while (node != null) {
                if (node.Key.Equals(key)) {
                    return true;
                }

                node = node.Next;
            }

            return false;
        }

        private void Insert(Entry entry) {
            var hash = GetHash(entry.Key);

            entry.Next = _buckets[hash];
            _buckets[hash] = entry;
        }

        private int GetHash(TKey key) {
            return (key.GetHashCode() & int.MaxValue) % Size;
        }
    }

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
            var dic = new MyDictionary<Int64Wrapper, int>();
            var n = int.Parse(ReadStringList()[0]);

            for (var i = 0; i < n; i++) {
                var (command, x) = ReadCommand();

                switch (command) {
                    case "A":
                        dic.Add(x, 0);
                        break;
                    case "D":
                        dic.Remove(x);
                        break;
                    case "?":
                        Console.WriteLine(dic.ContainsKey(x) ? "Y" : "N");
                        break;
                }
            }
        }

        private static (string Command, Int64Wrapper Value) ReadCommand() {
            var a = ReadStringList();
            return (a[0], new Int64Wrapper(long.Parse(a[1])));
        }

        private static string[] ReadStringList() {
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