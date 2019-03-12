using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Week04.Task05 {
    public sealed class Program {
        private static StreamWriter _out;

        private static void Main(string[] args) {
            if (!args.Contains("console")) {
                SetupIO();
            }

            new Quack().Run(File.ReadAllLines("input.txt"));

            if (args.Contains("console")) {
                Console.ReadLine();
            }

            DisposeIO();
        }

        private static void SetupIO() {
            _out = new StreamWriter("output.txt");
            Console.SetOut(_out);
        }

        private static void DisposeIO() {
            _out?.Dispose();
        }
    }

    internal class Quack {
        private readonly Queue<ushort> _queue = new Queue<ushort>();

        private Dictionary<int, ushort> _registers;

        private int _cursor;

        private Dictionary<char, Action<string>> _instructions;
        private Dictionary<string, int> _labels;
        private bool _stopped;

        public void Run(string[] lines) {
            _registers = Enumerable.Range('a', 26)
                .ToDictionary(x => x, y => (ushort) 0);

            _queue.Clear();
            DefineLabels(lines);
            DefineInstructions();

            while (_cursor < lines.Length && !_stopped) {
                Interpret(lines[_cursor]);
                _cursor += 1;
            }
        }

        private void Interpret(string s) {
            if (char.IsDigit(s[0])) {
                _queue.Enqueue(ushort.Parse(s));
                return;
            }

            _instructions[s[0]](s);
        }

        private void DefineInstructions() {
            _instructions = new Dictionary<char, Action<string>> {
                ['+'] = s => { Put(Get() + Get()); },
                ['-'] = s => { Put(Get() - Get()); },
                ['*'] = s => { Put(Get() * Get()); },
                ['/'] = s => {
                    var a = Get();
                    var b = Get();
                    Put(b == 0 ? 0 : a / b);
                },
                ['%'] = s => {
                    var a = Get();
                    var b = Get();
                    Put(b == 0 ? 0 : a % b);
                },
                ['>'] = s => { _registers[s[1]] = Get(); },
                ['<'] = s => { Put(_registers[s[1]]); },
                ['P'] = s => { Console.WriteLine(s.Length == 1 ? Get() : _registers[s[1]]); },
                ['C'] = s => { Console.Write((char) ((s.Length == 1 ? Get() : _registers[s[1]]) % 256)); },
                [':'] = s => { },
                ['J'] = s => { _cursor = _labels[new string(s.Skip(1).ToArray())]; },
                ['Z'] = s => {
                    if (_registers[s[1]] == 0) {
                        _cursor = _labels[new string(s.Skip(2).ToArray())];
                    }
                },
                ['E'] = s => {
                    if (_registers[s[1]] == _registers[s[2]]) {
                        _cursor = _labels[new string(s.Skip(3).ToArray())];
                    }
                },
                ['G'] = s => {
                    if (_registers[s[1]] > _registers[s[2]]) {
                        _cursor = _labels[new string(s.Skip(3).ToArray())];
                    }
                },
                ['Q'] = s => { _stopped = true; }
            };

            ushort Get() {
                return _queue.Dequeue();
            }

            void Put(int value) {
                _queue.Enqueue((ushort) (value % 65536));
            }
        }

        private void DefineLabels(string[] lines) {
            _labels = lines
                .Select((s, i) => (label: s, line: i))
                .Where(s => s.label[0] == ':')
                .ToDictionary(tuple => new string(tuple.label.Skip(1).ToArray()), tuple => tuple.line);
        }
    }
}