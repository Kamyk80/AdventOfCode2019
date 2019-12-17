using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Day17
{
    internal class Program
    {
        private static void Main()
        {
            var program = File.ReadAllText("input.txt")
                .Split(',')
                .Select(long.Parse)
                .ToList();

            var scaffolds = ToText(new Computer(program).Run());

            var alignment = 0;
            for (var y = 1; y < scaffolds.Length - 1; y++)
            for (var x = 1; x < scaffolds[0].Length - 1; x++)
                if (IsScaffold(scaffolds[y][x])
                    && IsScaffold(scaffolds[y - 1][x]) && IsScaffold(scaffolds[y + 1][x])
                    && IsScaffold(scaffolds[y][x - 1]) && IsScaffold(scaffolds[y][x + 1]))
                    alignment += x * y;

            program[0] = 2;

            var input = "A,B,A,B,C,B,A,C,B,C\nL,12,L,8,R,10,R,10\nL,6,L,4,L,12\nR,10,L,8,L,4,R,10\nn\n"
                .Select(c => (int) c)
                .ToList();

            var dust = new Computer(program).Run(input).ToList();

            ToText(dust.AsEnumerable().Reverse().Skip(1).Reverse()).ToList().ForEach(Console.WriteLine);

            Console.WriteLine(alignment);
            Console.WriteLine(dust.Last());
            Console.ReadKey();
        }

        private static string[] ToText(IEnumerable<int> output) =>
            new string(output
                    .Select(i => (char) i)
                    .ToArray())
                .Trim()
                .Split("\n");

        private static bool IsScaffold(char c) =>
            c == '#' || c == '^' || c == 'v' || c == '<' || c == '>';
    }
}
