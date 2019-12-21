using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Day21
{
    internal class Program
    {
        private static void Main()
        {
            var program = File.ReadAllText("input.txt")
                .Split(',')
                .Select(long.Parse)
                .ToList();

            var walk = File.ReadAllLines("walk.txt")
                .SelectMany(l => l.Append('\n'))
                .Select(c => (int) c)
                .ToList();
            var walking = new Computer(program).Run(walk).ToList();

            ToText(walking.AsEnumerable().Reverse().Skip(1).Reverse()).ToList().ForEach(Console.WriteLine);

            var run = File.ReadAllLines("run.txt")
                .SelectMany(l => l.Append('\n'))
                .Select(c => (int) c)
                .ToList();
            var running = new Computer(program).Run(run).ToList();

            ToText(running.AsEnumerable().Reverse().Skip(1).Reverse()).ToList().ForEach(Console.WriteLine);

            Console.WriteLine(walking.Last());
            Console.WriteLine(running.Last());
            Console.ReadKey(true);
        }

        private static string[] ToText(IEnumerable<int> output) =>
            new string(output
                    .Select(i => (char) i)
                    .ToArray())
                .Trim()
                .Split("\n");
    }
}
