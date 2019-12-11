using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable 8509
namespace AdventOfCode2019.Day11
{
    public class Computer
    {
        private readonly Dictionary<long, long> _program;
        private readonly Queue<long> _input;
        private readonly List<long> _output;
        private long _pointer;
        private long _base;

        public Computer(IEnumerable<long> program)
        {
            _program = program
                .Select((v, i) => new {v, i})
                .ToDictionary(p => (long) p.i, p => p.v);
            _input = new Queue<long>();
            _output = new List<long>();
            _pointer = 0;
        }

        public (long col, long dir)? Run(long col)
        {
            _input.Enqueue(col);

            while (true)
            {
                var (opcode, mode1, mode2, mode3) = DecodeInstruction(_program[_pointer]);

                switch (opcode)
                {
                    case 1:
                        Add(mode1, mode2, mode3);
                        break;
                    case 2:
                        Multiply(mode1, mode2, mode3);
                        break;
                    case 3:
                        Read(mode1);
                        break;
                    case 4:
                        Write(mode1);
                        if (_output.Count == 2)
                        {
                            var value = (_output[0], _output[1]);
                            _output.Clear();
                            return value;
                        }
                        break;
                    case 5:
                        JumpIfTrue(mode1, mode2);
                        break;
                    case 6:
                        JumpIfFalse(mode1, mode2);
                        break;
                    case 7:
                        LessThan(mode1, mode2, mode3);
                        break;
                    case 8:
                        Equals(mode1, mode2, mode3);
                        break;
                    case 9:
                        AdjustBase(mode1);
                        break;
                    default:
                        return null;
                }
            }
        }

        private static (long opcode, long mode1, long mode2, long mode3) DecodeInstruction(long instruction)
        {
            var opcode = instruction % 100;
            var mode1 = instruction % 1000 - opcode;
            var mode2 = instruction % 10000 - mode1 - opcode;
            var mode3 = instruction % 100000 - mode2 - mode1 - opcode;

            return (opcode, mode1 / 100, mode2 / 1000, mode3 / 10000);
        }

        private void Add(long mode1, long mode2, long mode3)
        {
            var value1 = GetValue(_program[_pointer + 1], mode1);
            var value2 = GetValue(_program[_pointer + 2], mode2);
            SetValue(_program[_pointer + 3], mode3, value1 + value2);
            _pointer += 4;
        }

        private void Multiply(long mode1, long mode2, long mode3)
        {
            var value1 = GetValue(_program[_pointer + 1], mode1);
            var value2 = GetValue(_program[_pointer + 2], mode2);
            SetValue(_program[_pointer + 3], mode3, value1 * value2);
            _pointer += 4;
        }

        private void Read(long mode)
        {
            SetValue(_program[_pointer + 1], mode, _input.Dequeue());
            _pointer += 2;
        }

        private void Write(long mode1)
        {
            _output.Add(GetValue(_program[_pointer + 1], mode1));
            _pointer += 2;
        }

        private void JumpIfTrue(long mode1, long mode2)
        {
            var value1 = GetValue(_program[_pointer + 1], mode1);
            var value2 = GetValue(_program[_pointer + 2], mode2);
            _pointer = value1 != 0 ? value2 : _pointer + 3;
        }

        private void JumpIfFalse(long mode1, long mode2)
        {
            var value1 = GetValue(_program[_pointer + 1], mode1);
            var value2 = GetValue(_program[_pointer + 2], mode2);
            _pointer = value1 == 0 ? value2 : _pointer + 3;
        }

        private void LessThan(long mode1, long mode2, long mode3)
        {
            var value1 = GetValue(_program[_pointer + 1], mode1);
            var value2 = GetValue(_program[_pointer + 2], mode2);
            SetValue(_program[_pointer + 3], mode3, value1 < value2 ? 1 : 0);
            _pointer += 4;
        }

        private void Equals(long mode1, long mode2, long mode3)
        {
            var value1 = GetValue(_program[_pointer + 1], mode1);
            var value2 = GetValue(_program[_pointer + 2], mode2);
            SetValue(_program[_pointer + 3], mode3, value1 == value2 ? 1 : 0);
            _pointer += 4;
        }

        private void AdjustBase(in long mode1)
        {
            _base += GetValue(_program[_pointer + 1], mode1);
            _pointer += 2;
        }

        private long GetValue(long reference, long mode)
        {
            long value;

            switch (mode)
            {
                case 0:
                    _program.TryGetValue(reference, out value);
                    return value;
                case 1:
                    return reference;
                case 2:
                    _program.TryGetValue(_base + reference, out value);
                    return value;
            }

            throw new ArgumentException();
        }

        private void SetValue(long reference, long mode, long value)
        {
            switch (mode)
            {
                case 0:
                    _program[reference] = value;
                    return;
                case 2:
                    _program[_base + reference] = value;
                    return;
            }

            throw new ArgumentException();
        }
    }
}
#pragma warning restore 8509
