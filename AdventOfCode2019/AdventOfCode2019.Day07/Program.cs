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
            var computer = new Computer();

            var maxSignal = Permutations(Enumerable.Range(0, 5).ToArray())
                .Max(p => p.Aggregate(0, (last, next) => computer.Run(program, new [] {next, last})));

            Console.WriteLine(maxSignal);
            Console.ReadKey(true);
        }

        private static IEnumerable<int[]> Permutations(int[] values) =>
            values.Length == 1
                ? new[] {values}
                : values.SelectMany(
                    v => Permutations(values.Except(new[] {v}).ToArray()),
                    (v, p) => new[] {v}.Concat(p).ToArray());
    }
}
