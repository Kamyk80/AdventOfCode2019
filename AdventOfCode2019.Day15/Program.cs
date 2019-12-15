using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Day15
{
    internal class Program
    {
        private static readonly (int dx, int dy)[] Offsets = {(0, -1), (0, 1), (-1, 0), (1, 0)};
        private static readonly HashSet<(int x, int y)> Spaces = new HashSet<(int x, int y)>();
        private static (int x, int y) _generator;

        public class Position
        {
            public Computer Computer { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Dist { get; set; }

            public Position(Computer computer)
            {
                Computer = computer;
            }

            public Position(Computer computer, int x, int y, int dist)
                : this(computer)
            {
                X = x;
                Y = y;
                Dist = dist;
            }
        }

        public class Oxygen
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Time { get; set; }

            public Oxygen(int x, int y, int time = 1)
            {
                X = x;
                Y = y;
                Time = time;
            }
        }

        private static void Main()
        {
            var program = File.ReadAllText("input.txt")
                .Split(',')
                .Select(long.Parse)
                .ToList();

            Console.WriteLine(ScanRoom(program));
            Console.WriteLine(FillRoom());
            Console.ReadKey(true);
        }

        private static int ScanRoom(IEnumerable<long> program)
        {
            var nodes = new Queue<Position>(new[] {new Position(new Computer(program))});
            var dist = 0;

            while (nodes.Count > 0)
            {
                var pos = nodes.Dequeue();
                Spaces.Add((pos.X, pos.Y));

                for (var dir = 0; dir < 4; dir++)
                {
                    var coords = (x: pos.X + Offsets[dir].dx, y: pos.Y + Offsets[dir].dy);

                    if (Spaces.Contains(coords))
                    {
                        continue;
                    }

                    var computer = new Computer(pos.Computer);
                    var result = computer.Run(dir + 1);

                    switch (result)
                    {
                        case 1:
                            nodes.Enqueue(new Position(computer, coords.x, coords.y, pos.Dist + 1));
                            break;
                        case 2:
                            _generator = (pos.X, pos.Y);
                            dist = pos.Dist + 1;
                            break;
                    }
                }
            }

            return dist;
        }

        private static int FillRoom()
        {
            var nodes = new Queue<Oxygen>(new[] {new Oxygen(_generator.x, _generator.y)});
            var time = 0;

            while (nodes.Count > 0)
            {
                var oxy = nodes.Dequeue();
                Spaces.Remove((oxy.X, oxy.Y));
                time = oxy.Time;

                for (var dir = 0; dir < 4; dir++)
                {
                    var coords = (x: oxy.X + Offsets[dir].dx, y: oxy.Y + Offsets[dir].dy);

                    if (!Spaces.Contains(coords))
                    {
                        continue;
                    }

                    nodes.Enqueue(new Oxygen(coords.x, coords.y, oxy.Time + 1));
                }
            }

            return time;
        }
    }
}
