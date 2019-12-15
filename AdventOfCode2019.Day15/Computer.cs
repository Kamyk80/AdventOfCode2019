using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Day15
{
    public class Computer
    {
        private readonly Dictionary<long, long> _program;
        private long _pointer;
        private long _base;

        public Computer(IEnumerable<long> program)
        {
            _program = program
                .Select((v, i) => new {v, i})
                .ToDictionary(p => (long) p.i, p => p.v);
        }

        public Computer(Computer computer)
        {
            _program = new Dictionary<long, long>(computer._program);
            _pointer = computer._pointer;
            _base = computer._base;
        }

        public int Run(int input)
        {
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
                        Read(mode1, input);
                        break;
                    case 4:
                        return Write(mode1);
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

        private void Read(long mode, int input)
        {
            SetValue(_program[_pointer + 1], mode, input);
            _pointer += 2;
        }

        private int Write(long mode1)
        {
            var value = (int) GetValue(_program[_pointer + 1], mode1);
            _pointer += 2;

            return value;
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
