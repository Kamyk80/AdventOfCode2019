using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#pragma warning disable 8509
namespace AdventOfCode2019.Day24
{
    internal class Program
    {
        private static readonly (int dx, int dy)[] Offsets = {(0, -1), (0, 1), (-1, 0), (1, 0)};

        private static void Main()
        {
            var bugs = File.ReadAllLines("input.txt")
                .Select(l => l.ToArray())
                .ToArray();

            var ratings = new HashSet<int>();
            var rating = Rating(bugs);

            while (!ratings.Contains(rating))
            {
                ratings.Add(rating);
                bugs = Live(bugs);
                rating = Rating(bugs);
            }

            Console.WriteLine(rating);
            Console.ReadKey(true);
        }

        private static char[][] Live(char[][] cur)
        {
            var next = new char[cur.Length][];
            for (var y = 0; y < cur.Length; y++)
            {
                next[y] = new char[cur[0].Length];
                for (var x = 0; x < cur[0].Length; x++)
                {
                    next[y][x] = cur[y][x];

                    var bugs = Offsets
                        .Count(o => (x: x + o.dx, y: y + o.dy) switch
                        {
                            var (_, y1) when y1 < 0 || y1 > cur.Length - 1 => false,
                            var (x1, _) when x1 < 0 || x1 > cur[0].Length - 1 => false,
                            var (x1, y1) => cur[y1][x1] == '#'
                        });

                    if (cur[y][x] == '#' && bugs != 1)
                    {
                        next[y][x] = '.';
                    }

                    if (cur[y][x] == '.' && bugs >= 1 && bugs <= 2)
                    {
                        next[y][x] = '#';
                    }
                }
            }

            return next;
        }

        private static int Rating(char[][] bugs)
        {
            var rating = 0;
            var bit = 1;

            foreach (var line in bugs)
            {
                foreach (var bug in line)
                {
                    if (bug == '#')
                    {
                        rating |= bit;
                    }

                    bit <<= 1;
                }
            }

            return rating;
        }
    }
}
#pragma warning restore 8509
