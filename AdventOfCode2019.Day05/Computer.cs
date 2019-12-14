using System.Collections.Generic;

namespace AdventOfCode2019.Day05
{
    public class Computer
    {
        private IList<int> _program;
        private int _pointer;
        private int _output;

        public int Run(IEnumerable<int> program, int input)
        {
            _program = new List<int>(program);
            _pointer = 0;

            while (true)
            {
                var (opcode, mode1, mode2) = DecodeInstruction(_program[_pointer]);

                switch (opcode)
                {
                    case 1:
                        Add(mode1, mode2);
                        break;
                    case 2:
                        Multiply(mode1, mode2);
                        break;
                    case 3:
                        Read(input);
                        break;
                    case 4:
                        Write(mode1);
                        break;
                    case 5:
                        JumpIfTrue(mode1, mode2);
                        break;
                    case 6:
                        JumpIfFalse(mode1, mode2);
                        break;
                    case 7:
                        LessThan(mode1, mode2);
                        break;
                    case 8:
                        Equals(mode1, mode2);
                        break;
                    default:
                        return _output;
                }
            }
        }

        private static (int opcode, int mode1, int mode2) DecodeInstruction(int instruction)
        {
            var opcode = instruction % 100;
            var mode1 = instruction % 1000 - opcode;
            var mode2 = instruction % 10000 - mode1 - opcode;
            return (opcode, mode1 / 100, mode2 / 1000);
        }

        private void Add(int mode1, int mode2)
        {
            var value1 = GetValue(_program[_pointer + 1], mode1);
            var value2 = GetValue(_program[_pointer + 2], mode2);
            SetValue(_program[_pointer + 3], value1 + value2);
            _pointer += 4;
        }

        private void Multiply(int mode1, int mode2)
        {
            var value1 = GetValue(_program[_pointer + 1], mode1);
            var value2 = GetValue(_program[_pointer + 2], mode2);
            SetValue(_program[_pointer + 3], value1 * value2);
            _pointer += 4;
        }

        private void Read(int input)
        {
            SetValue(_program[_pointer + 1], input);
            _pointer += 2;
        }

        private void Write(int mode1)
        {
            _output = GetValue(_program[_pointer + 1], mode1);
            _pointer += 2;
        }

        private void JumpIfTrue(int mode1, int mode2)
        {
            var value1 = GetValue(_program[_pointer + 1], mode1);
            var value2 = GetValue(_program[_pointer + 2], mode2);
            _pointer = value1 != 0 ? value2 : _pointer + 3;
        }

        private void JumpIfFalse(int mode1, int mode2)
        {
            var value1 = GetValue(_program[_pointer + 1], mode1);
            var value2 = GetValue(_program[_pointer + 2], mode2);
            _pointer = value1 == 0 ? value2 : _pointer + 3;
        }

        private void LessThan(int mode1, int mode2)
        {
            var value1 = GetValue(_program[_pointer + 1], mode1);
            var value2 = GetValue(_program[_pointer + 2], mode2);
            SetValue(_program[_pointer + 3], value1 < value2 ? 1 : 0);
            _pointer += 4;
        }

        private void Equals(int mode1, int mode2)
        {
            var value1 = GetValue(_program[_pointer + 1], mode1);
            var value2 = GetValue(_program[_pointer + 2], mode2);
            SetValue(_program[_pointer + 3], value1 == value2 ? 1 : 0);
            _pointer += 4;
        }

        private int GetValue(int reference, int mode)
        {
            return mode switch
            {
                0 => _program[reference],
                1 => reference,
            };
        }

        private void SetValue(int reference, int value)
        {
            _program[reference] = value;
        }
    }
}
