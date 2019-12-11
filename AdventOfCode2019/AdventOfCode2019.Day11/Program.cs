using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#pragma warning disable 8509
namespace AdventOfCode2019.Day11
{
    internal class Program
    {
        private static void Main()
        {
            var program = File.ReadAllText("input.txt")
                .Split(',')
                .Select(long.Parse)
                .ToList();

            var first = Draw(program, 0);
            var second = Draw(program, 1);

            Console.WriteLine(first.Count);

            for (var y = second.Min(p => p.Key.y); y <= second.Max(p => p.Key.y); y++)
            {
                for (var x = second.Min(p => p.Key.x); x <= second.Max(p => p.Key.x); x++)
                {
                    second.TryGetValue((x, y), out var col);
                    Console.Write(col == 0 ? ' ' : '█');
                }

                Console.WriteLine();
            }

            Console.ReadKey(true);
        }

        private static Dictionary<(int x, int y), long> Draw(IEnumerable<long> program, long color)
        {
            var hull = new Dictionary<(int x, int y), long>();
            var computer = new Computer(program);
            var pos = (x: 0, y: 0);
            var dir = (x: 0, y: -1);

            hull.Add(pos, color);

            while (true)
            {
                hull.TryGetValue(pos, out var col);
                var result = computer.Run(col);
                if (!result.HasValue)
                {
                    return hull;
                }

                dir = result.Value.dir switch
                {
                    0 when dir.x == 0 => (dir.y, 0),
                    1 when dir.x == 0 => (-dir.y, 0),
                    0 when dir.y == 0 => (0, -dir.x),
                    1 when dir.y == 0 => (0, dir.x)
                };

                hull[pos] = result.Value.col;
                pos.x += dir.x;
                pos.y += dir.y;
            }
        }
    }
}
#pragma warning restore 8509
