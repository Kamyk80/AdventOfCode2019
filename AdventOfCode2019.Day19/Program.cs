using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Day19
{
    internal class Program
    {
        private static void Main()
        {
            var program = File.ReadAllText("input.txt")
                .Split(',')
                .Select(long.Parse)
                .ToList();

            var beamPoints = Enumerable.Range(0, 50)
                .SelectMany(x => Enumerable.Range(0, 50), (x, y) => (x, y))
                .Count(c => new Computer(program).Run(new[] {c.x, c.y}).Last() == 1);

            Console.WriteLine(beamPoints);
            Console.WriteLine(ShipPos(program));
            Console.ReadKey(true);
        }

        private static int ShipPos(ICollection<long> program)
        {
            var last = 0;

            for (var x1 = 0;; x1++)
            {
                var x2 = x1 + 99;
                var y1 = Enumerable.Range(last, int.MaxValue - last)
                    .First(y => new Computer(program).Run(new[] {x2, y}).Last() == 1);
                var y2 = y1 + 99;

                if (new Computer(program).Run(new[] {x1, y2}).Last() == 1)
                {
                    return x1 * 10000 + y1;
                }

                last = y1;
            }
        }
    }
}
