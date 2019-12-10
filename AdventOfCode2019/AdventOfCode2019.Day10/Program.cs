using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Day10
{
    internal class Program
    {
        private static void Main()
        {
            var asteroids = File.ReadAllLines("input.txt")
                .SelectMany((l, y) => l.Select((c, x) => (x, y, c)))
                .Where(p => p.c == '#')
                .Select(p => (p.x, p.y))
                .ToHashSet();

            var station = asteroids
                .Select(a => new
                {
                    Coords = a,
                    Visible = asteroids
                        .Where(a1 => a != a1)
                        .Select(a1 => Math.PI - Math.Atan2(a1.x - a.x, a1.y - a.y))
                        .Distinct()
                        .Count()
                })
                .OrderBy(a => a.Visible)
                .Last();

            var vaporized = asteroids
                .Where(a => a != station.Coords)
                .GroupBy(a => Math.PI - Math.Atan2(a.x - station.Coords.x, a.y - station.Coords.y))
                .SelectMany(g => g
                    .OrderBy(a => Math.Abs(a.x - station.Coords.x))
                    .ThenBy(a => Math.Abs(a.y - station.Coords.y))
                    .Select((a, i) => new
                    {
                        Seq = i,
                        Angle = g.Key,
                        Coords = a
                    }))
                .OrderBy(a => a.Seq)
                .ThenBy(g => g.Angle)
                .Skip(199)
                .First();

            Console.WriteLine(station.Visible);
            Console.WriteLine(vaporized.Coords.x * 100 + vaporized.Coords.y);
            Console.ReadKey(true);
        }
    }
}
