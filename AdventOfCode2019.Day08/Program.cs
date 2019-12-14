using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Day08
{
    internal class Program
    {
        private static void Main()
        {
            var layers = File.ReadAllText("input.txt").Trim()
                .Select((c, i) => new {c, i})
                .GroupBy(p => p.i / (25 * 6), p => p.c)
                .Select(g => g.ToArray())
                .ToList();

            var zeroes = layers
                .OrderBy(l => l.Count(c => c == '0'))
                .First();

            Console.WriteLine(zeroes.Count(c => c == '1') * zeroes.Count(c => c == '2'));

            layers.First()
                .Select((c, i) => layers.Select(l => l[i]).ToArray())
                .Select((l, i) => new {c = l.First(c => c != '2'), i})
                .GroupBy(p => p.i / 25, p => p.c)
                .Select(g => g.Select(c => c == '0' ? ' ' : '█').ToArray())
                .ToList()
                .ForEach(Console.WriteLine);

            Console.ReadKey(true);
        }
    }
}
