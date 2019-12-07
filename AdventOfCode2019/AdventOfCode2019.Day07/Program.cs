using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Day07
{
    internal class Program
    {
        private static void Main()
        {
            var program = File.ReadAllText("input.txt")
                .Split(',')
                .Select(int.Parse)
                .ToList();

            var maxSignal1 = Permutations(Enumerable.Range(0, 5).ToArray())
                .Max(p => p.Aggregate(0, (last, next) => new Computer(program, next).Run(last).GetValueOrDefault()));

            var maxSignal2 = Permutations(Enumerable.Range(5, 5).ToArray())
                .Max(p => RunFeedbackLoop(program, p));

            Console.WriteLine(maxSignal1);
            Console.WriteLine(maxSignal2);
            Console.ReadKey(true);
        }

        private static IEnumerable<int[]> Permutations(int[] values) =>
            values.Length == 1
                ? new[] {values}
                : values.SelectMany(
                    v => Permutations(values.Except(new[] {v}).ToArray()),
                    (v, p) => new[] {v}.Concat(p).ToArray());

        private static int RunFeedbackLoop(IEnumerable<int> program, IEnumerable<int> phases)
        {
            var computers = phases
                .Select(p => new Computer(program, p))
                .ToList();
            int? signal = null;
            int? result = null;

            while (true)
            {
                foreach (var computer in computers)
                {
                    signal = computer.Run(signal.GetValueOrDefault());

                    if (signal == null)
                    {
                        return result.GetValueOrDefault();
                    }
                }

                result = signal;
            }
        }
    }
}
