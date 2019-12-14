using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Day02
{
    internal class Program
    {
        private static void Main()
        {
            var program = File.ReadAllText("input.txt")
                .Split(',')
                .Select(int.Parse)
                .ToList();

            Console.WriteLine(RunProgram(program, 12, 2));
            Console.WriteLine(FindSolution(program));
            Console.ReadKey(true);
        }

        private static int RunProgram(IEnumerable<int> input, int noun, int verb)
        {
            var program = input.ToList();
            var current = 0;

            program[1] = noun;
            program[2] = verb;

            while (true)
            {
                switch (program[current])
                {
                    case 1:
                        program[program[current + 3]] = program[program[current + 1]] + program[program[current + 2]];
                        break;
                    case 2:
                        program[program[current + 3]] = program[program[current + 1]] * program[program[current + 2]];
                        break;
                    default:
                        return program[0];
                }

                current += 4;
            }
        }

        private static int FindSolution(IList<int> program)
        {
            for (var noun = 0; noun < 100; noun++)
            {
                for (var verb = 0; verb < 100; verb++)
                {
                    var result = RunProgram(program, noun, verb);
                    if (result == 19690720)
                    {
                        return 100 * noun + verb;
                    }
                }
            }

            throw new Exception();
        }
    }
}
