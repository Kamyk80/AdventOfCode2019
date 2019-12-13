using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Day13
{
    internal class Program
    {
        private static void Main()
        {
            var program = File.ReadAllText("input.txt")
                .Split(',')
                .Select(long.Parse)
                .ToList();
            var computer = new Computer(program);
            var output = computer.Run();

            var blocks = output
                .Select((o, i) => new {o, i})
                .GroupBy(p => p.i / 3, p => p.o)
                .Select(g => g.ToArray()[2])
                .Count(t => t == 2);

            Console.WriteLine(blocks);
            Console.ReadKey(true);
        }
    }
}
