using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Day20
{
    internal class Program
    {
        private static readonly (int dx, int dy)[] Offsets = {(0, -1), (0, 1), (-1, 0), (1, 0)};

        private static char[][] _maze;

        private static void Main()
        {
            _maze = File.ReadAllLines("input.txt")
                .Select(l => l.ToArray())
                .ToArray();

            Console.WriteLine(ShortestPath());
            Console.ReadKey(true);
        }

        private static int ShortestPath()
        {
            var start = FindPortals(new[] {'A', 'A'}).First();
            var nodes = new Queue<(int x, int y, int time)>(new[] {(start.x, start.y, 0)});
            var visited = new HashSet<(int x, int y)>(new[] {(start.x, start.y)});

            while (nodes.Count > 0)
            {
                var node = nodes.Dequeue();

                var moves = Offsets
                    .Select(o => (x: node.x + o.dx, y: node.y + o.dy))
                    .Where(p => _maze[p.y][p.x] == '.')
                    .Where(p => !visited.Contains((p.x, p.y)))
                    .ToList();

                foreach (var move in moves)
                {
                    nodes.Enqueue((move.x, move.y, node.time + 1));
                    visited.Add((move.x, move.y));
                }

                var portals = new[]
                    {
                        new[] {_maze[node.y - 2][node.x], _maze[node.y - 1][node.x]},
                        new[] {_maze[node.y + 1][node.x], _maze[node.y + 2][node.x]},
                        new[] {_maze[node.y][node.x - 2], _maze[node.y][node.x - 1]},
                        new[] {_maze[node.y][node.x + 1], _maze[node.y][node.x + 2]}
                    }
                    .Where(p => p.All(c => c >= 'A' && c <= 'Z'))
                    .ToList();

                foreach (var portal in portals)
                {
                    if (portal[0] == 'Z' && portal[1] == 'Z')
                    {
                        return node.time;
                    }

                    var exits = FindPortals(portal)
                        .Where(p => !visited.Contains((p.x, p.y)))
                        .ToList();

                    foreach (var exit in exits)
                    {
                        nodes.Enqueue((exit.x, exit.y, node.time + 1));
                        visited.Add((exit.x, exit.y));
                    }
                }
            }

            throw new Exception();
        }

        private static IEnumerable<(int x, int y)> FindPortals(char[] portal) =>
            Enumerable.Range(2, _maze.Length - 4)
                .SelectMany(y => Enumerable.Range(2, _maze[0].Length - 4), (y, x) => (x, y))
                .Where(c => _maze[c.y][c.x] == '.')
                .Where(c => _maze[c.y][c.x - 2] == portal[0] && _maze[c.y][c.x - 1] == portal[1] ||
                            _maze[c.y][c.x + 1] == portal[0] && _maze[c.y][c.x + 2] == portal[1] ||
                            _maze[c.y - 2][c.x] == portal[0] && _maze[c.y - 1][c.x] == portal[1] ||
                            _maze[c.y + 1][c.x] == portal[0] && _maze[c.y + 2][c.x] == portal[1])
                .ToList();
    }
}
