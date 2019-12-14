using System.Collections.Generic;

#pragma warning disable 8509
namespace AdventOfCode2019.Day07
{
    public class Computer
    {
        private readonly List<int> _program;
        private readonly Queue<int> _input;
        private int _pointer;

        public Computer(IEnumerable<int> program)
        {
            _program = new List<int>(program);
            _input = new Queue<int>();
            _pointer = 0;
        }

        public Computer(IEnumerable<int> program, int phase)
            : this(program)
        {
            _input.Enqueue(phase);
        }

        public int? Run(int input)
        {
            _input.Enqueue(input);

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
                        Read();
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
                        LessThan(mode1, mode2);
                        break;
                    case 8:
                        Equals(mode1, mode2);
                        break;
                    default:
                        return null;
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

        private void Read()
        {
            SetValue(_program[_pointer + 1], _input.Dequeue());
            _pointer += 2;
        }

        private int Write(int mode1)
        {
            var output = GetValue(_program[_pointer + 1], mode1);
            _pointer += 2;
            return output;
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
#pragma warning restore 8509
