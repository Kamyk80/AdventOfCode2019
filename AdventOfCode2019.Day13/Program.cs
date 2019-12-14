using System;
using System.Collections.Generic;
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

            program[0] = 2;

            var computer = new Computer(program);
            var board = computer.Run()
                .Select((o, i) => new {o, i})
                .GroupBy(p => p.i / 3, p => p.o)
                .Select(g => g.ToArray())
                .ToDictionary(g => (x: g[0], y: g[1]), g => g[2]);

            Console.WriteLine(board.Count(t => t.Value == 2));

            var paddle = board.First(p => p.Value == 3).Key.x;
            var ball = board.First(p => p.Value == 4).Key.x;

            while (!computer.Finished)
            {
                var input = Math.Sign(ball - paddle);
                var changes = computer.Continue(input)
                    .Select((o, i) => new {o, i})
                    .GroupBy(p => p.i / 3, p => p.o)
                    .Select(g => g.ToArray())
                    .ToList();

                changes.ForEach(c => board[(c[0], c[1])] = c[2]);

                paddle = changes.FirstOrDefault(c => c[2] == 3)?.First() ?? paddle;
                ball = changes.FirstOrDefault(c => c[2] == 4)?.First() ?? ball;
            }

            Console.WriteLine(board.First(p => p.Key.x == -1).Value);
            Console.ReadKey(true);
        }

        // ReSharper disable once UnusedMember.Local
        private static void DrawBoard(IReadOnlyDictionary<(int x, int y), int> board)
        {
            Console.Clear();

            for (var y = 0; y <= board.Max(t => t.Key.y); y++)
            {
                for (var x = 0; x <= board.Max(t => t.Key.x); x++)
                {
                    board.TryGetValue((x, y), out var id);
                    var ch = id switch
                    {
                        1 => '#',
                        2 => 'B',
                        3 => 'P',
                        4 => 'O',
                        _ => ' '
                    };

                    Console.Write(ch);
                }

                Console.WriteLine();
            }
        }
    }
}
