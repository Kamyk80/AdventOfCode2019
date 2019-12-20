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
 
            var entrance = FindEntrance();

            Console.WriteLine(PathPortals(entrance));
            Console.WriteLine(PathRecursive(entrance));
            Console.ReadKey(true);
        }

        private static (int x, int y) FindEntrance() =>
            Enumerable.Range(2, _maze.Length - 4)
                .SelectMany(y => Enumerable.Range(2, _maze[0].Length - 4), (y, x) => (x, y))
                .Where(c => _maze[c.y][c.x] == '.')
                .First(c => _maze[c.y][c.x - 2] == 'A' && _maze[c.y][c.x - 1] == 'A' ||
                            _maze[c.y][c.x + 1] == 'A' && _maze[c.y][c.x + 2] == 'A' ||
                            _maze[c.y - 2][c.x] == 'A' && _maze[c.y - 1][c.x] == 'A' ||
                            _maze[c.y + 1][c.x] == 'A' && _maze[c.y + 2][c.x] == 'A');

        private static int PathPortals((int x, int y) entrance)
        {
            var nodes = new Queue<(int x, int y, int time)>(new[] {(entrance.x, entrance.y, 0)});
            var visited = new HashSet<(int x, int y)>(new[] {(entrance.x, entrance.y)});

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

                var portal = FindPortal((node.x, node.y));

                if (portal.HasValue)
                {
                    if (portal.Value == (0, 0))
                    {
                        return node.time;
                    }

                    if (!visited.Contains(portal.Value))
                    {
                        nodes.Enqueue((portal.Value.x, portal.Value.y, node.time + 1));
                        visited.Add((portal.Value.x, portal.Value.y));
                    }
                }
            }

            throw new Exception();
        }

        private static (int x, int y)? FindPortal((int x, int y) tile)
        {
            var portal = new[]
                {
                    new[] {_maze[tile.y - 2][tile.x], _maze[tile.y - 1][tile.x]},
                    new[] {_maze[tile.y + 1][tile.x], _maze[tile.y + 2][tile.x]},
                    new[] {_maze[tile.y][tile.x - 2], _maze[tile.y][tile.x - 1]},
                    new[] {_maze[tile.y][tile.x + 1], _maze[tile.y][tile.x + 2]}
                }
                .FirstOrDefault(p => p.All(c => c >= 'A' && c <= 'Z'));

            if (portal == null)
            {
                return null;
            }

            if (portal[0] == 'A' && portal[1] == 'A')
            {
                return null;
            }

            if (portal[0] == 'Z' && portal[1] == 'Z')
            {
                return (0, 0);
            }

            return Enumerable
                .Range(2, _maze.Length - 4)
                .SelectMany(y => Enumerable.Range(2, _maze[0].Length - 4), (y, x) => (x, y))
                .Where(c => _maze[c.y][c.x] == '.')
                .Where(c => c != tile)
                .First(c => _maze[c.y][c.x - 2] == portal[0] && _maze[c.y][c.x - 1] == portal[1] ||
                            _maze[c.y][c.x + 1] == portal[0] && _maze[c.y][c.x + 2] == portal[1] ||
                            _maze[c.y - 2][c.x] == portal[0] && _maze[c.y - 1][c.x] == portal[1] ||
                            _maze[c.y + 1][c.x] == portal[0] && _maze[c.y + 2][c.x] == portal[1]);
        }

        private static int PathRecursive((int x, int y) entrance)
        {
            var start = FindEntrance();
            var nodes = new Queue<(int x, int y, int level, int time)>(new[] {(start.x, start.y, 0, 0)});
            var visited = new HashSet<(int x, int y, int level)>(new[] {(start.x, start.y, 0)});

            while (nodes.Count > 0)
            {
                var node = nodes.Dequeue();

                var moves = Offsets
                    .Select(o => (x: node.x + o.dx, y: node.y + o.dy))
                    .Where(p => _maze[p.y][p.x] == '.')
                    .Where(p => !visited.Contains((p.x, p.y, node.level)))
                    .ToList();

                foreach (var move in moves)
                {
                    nodes.Enqueue((move.x, move.y, node.level, node.time + 1));
                    visited.Add((move.x, move.y, node.level));
                }

                var portal = FindPortal((node.x, node.y, node.level));

                if (portal.HasValue)
                {
                    if (portal.Value == (0, 0, 0))
                    {
                        return node.time;
                    }

                    if (!visited.Contains(portal.Value))
                    {
                        nodes.Enqueue((portal.Value.x, portal.Value.y, portal.Value.level, node.time + 1));
                        visited.Add((portal.Value.x, portal.Value.y, portal.Value.level));
                    }
                }
            }

            throw new Exception();
        }

        private static (int x, int y, int level)? FindPortal((int x, int y, int level) tile)
        {
            var portal = new[]
                {
                    new[] {_maze[tile.y - 2][tile.x], _maze[tile.y - 1][tile.x]},
                    new[] {_maze[tile.y + 1][tile.x], _maze[tile.y + 2][tile.x]},
                    new[] {_maze[tile.y][tile.x - 2], _maze[tile.y][tile.x - 1]},
                    new[] {_maze[tile.y][tile.x + 1], _maze[tile.y][tile.x + 2]}
                }
                .FirstOrDefault(p => p.All(c => c >= 'A' && c <= 'Z'));

            if (portal == null)
            {
                return null;
            }

            if (portal[0] == 'A' && portal[1] == 'A')
            {
                return null;
            }

            if (portal[0] == 'Z' && portal[1] == 'Z')
            {
                if (tile.level == 0)
                {
                    return (0, 0, 0);
                }

                return null;
            }

            var outer = tile.y == 2 || tile.x == 2 || tile.y == _maze.Length - 3 || tile.x == _maze[0].Length - 3;

            if (tile.level == 0 && outer)
            {
                return null;
            }

            var level = tile.level + (outer ? -1 : 1);

            return Enumerable
                .Range(2, _maze.Length - 4)
                .SelectMany(y => Enumerable.Range(2, _maze[0].Length - 4), (y, x) => (x, y))
                .Where(c => _maze[c.y][c.x] == '.')
                .Where(c => c.x != tile.x && c.y != tile.y)
                .Select(c => (c.x, c.y, level))
                .First(c => _maze[c.y][c.x - 2] == portal[0] && _maze[c.y][c.x - 1] == portal[1] ||
                            _maze[c.y][c.x + 1] == portal[0] && _maze[c.y][c.x + 2] == portal[1] ||
                            _maze[c.y - 2][c.x] == portal[0] && _maze[c.y - 1][c.x] == portal[1] ||
                            _maze[c.y + 1][c.x] == portal[0] && _maze[c.y + 2][c.x] == portal[1]);
        }
    }
}
