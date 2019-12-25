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

            Console.WriteLine(RunSingle(bugs));
            Console.WriteLine(RunLevels(bugs));
            Console.ReadKey(true);
        }

        private static int RunSingle(char[][] bugs)
        {
            var ratings = new HashSet<int>();
            var rating = RatingSingle(bugs);

            while (!ratings.Contains(rating))
            {
                ratings.Add(rating);
                bugs = LiveSingle(bugs);
                rating = RatingSingle(bugs);
            }

            return rating;
        }

        private static char[][] LiveSingle(char[][] cur)
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

        private static int RatingSingle(char[][] bugs)
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

        private static int RunLevels(char[][] bugs)
        {
            var curLevels = new List<char[][]>(new[] {bugs});

            for (var time = 0; time < 200; time++)
            {
                if (curLevels.First().Any(r => r.Any(c => c == '#')))
                {
                    curLevels = curLevels.Prepend(EmptyLevel()).ToList();
                }

                if (curLevels.Last().Any(r => r.Any(c => c == '#')))
                {
                    curLevels = curLevels.Append(EmptyLevel()).ToList();
                }

                var nextLevels = new List<char[][]>();

                for (var level = 0; level < curLevels.Count; level++)
                {
                    var curLevel = curLevels[level];
                    var outerLevel = level > 0 ? curLevels[level - 1] : null;
                    var innerLevel = level < curLevels.Count - 1 ? curLevels[level + 1] : null;

                    nextLevels.Add(LiveLevel(curLevel, outerLevel, innerLevel));
                }

                curLevels = nextLevels;
            }

            return curLevels.Sum(l => l.Sum(r => r.Count(c => c == '#')));
        }

        private static char[][] EmptyLevel()
        {
            return Enumerable.Repeat(Enumerable.Repeat('.', 5).ToArray(), 5).ToArray();
        }

        private static char[][] LiveLevel(char[][] curLevel, char[][] outerLevel, char[][] innerLevel)
        {
            var nextLevel = new char[5][];

            for (var y = 0; y < 5; y++)
            {
                nextLevel[y] = new char[5];
                for (var x = 0; x < 5; x++)
                {
                    nextLevel[y][x] = curLevel[y][x];

                    if (x == 2 && y == 2)
                    {
                        continue;
                    }

                    var bugs = Offsets
                        .Sum(o => (x: x + o.dx, y: y + o.dy) switch
                        {
                            var (_, y1) when y1 < 0 => outerLevel?[1][2] == '#' ? 1 : 0,
                            var (_, y1) when y1 > 4 => outerLevel?[3][2] == '#' ? 1 : 0,
                            var (x1, _) when x1 < 0 => outerLevel?[2][1] == '#' ? 1 : 0,
                            var (x1, _) when x1 > 4 => outerLevel?[2][3] == '#' ? 1 : 0,
                            var (x1, y1) when x1 == 2 && y1 == 2 => (x, y) switch
                            {
                                var (_, y2) when y2 == 1 => innerLevel?[0].Count(c => c == '#'),
                                var (_, y2) when y2 == 3 => innerLevel?[4].Count(c => c == '#'),
                                var (x2, _) when x2 == 1 => innerLevel?.Count(r => r[0] == '#'),
                                var (x2, _) when x2 == 3 => innerLevel?.Count(r => r[4] == '#')
                            },
                            var (x1, y1) => curLevel[y1][x1] == '#' ? 1 : 0
                        });

                    if (curLevel[y][x] == '#' && bugs != 1)
                    {
                        nextLevel[y][x] = '.';
                    }

                    if (curLevel[y][x] == '.' && bugs >= 1 && bugs <= 2)
                    {
                        nextLevel[y][x] = '#';
                    }
                }
            }

            return nextLevel;
        }
    }
}
#pragma warning restore 8509
