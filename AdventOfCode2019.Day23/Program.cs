using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Day23
{
    internal class Program
    {
        private static void Main()
        {
            var program = File.ReadAllText("input.txt")
                .Split(',')
                .Select(long.Parse)
                .ToList();

            var computersFirst = Enumerable.Range(0, 50)
                .Select(a => new Computer(program, a))
                .ToList();

            var computersSecond = Enumerable.Range(0, 50)
                .Select(a => new Computer(program, a))
                .ToList();

            Console.WriteLine(RunFirst(computersFirst));
            Console.WriteLine(RunSecond(computersSecond));
            Console.ReadKey(true);
        }

        private static long RunFirst(IList<Computer> computers)
        {
            while (true)
            {
                foreach (var computer in computers)
                {
                    computer.Run();
                }

                foreach (var computer in computers)
                {
                    if (computer.Output.Count == 3)
                    {
                        var address = (int) computer.Output.Dequeue();
                        var x = computer.Output.Dequeue();
                        var y = computer.Output.Dequeue();

                        if (address == 255)
                        {
                            return y;
                        }

                        computers[address].Input.Enqueue(x);
                        computers[address].Input.Enqueue(y);
                    }
                }
            }
        }

        private static long RunSecond(IList<Computer> computers)
        {
            var natX = 0L;
            var natY = 0L;
            var natSent = new HashSet<long>();

            while (true)
            {
                foreach (var computer in computers)
                {
                    computer.Run();
                }

                foreach (var computer in computers)
                {
                    if (computer.Output.Count == 3)
                    {
                        var address = (int) computer.Output.Dequeue();
                        var x = computer.Output.Dequeue();
                        var y = computer.Output.Dequeue();

                        if (address == 255)
                        {
                            natX = x;
                            natY = y;
                        }
                        else
                        {
                            computers[address].Input.Enqueue(x);
                            computers[address].Input.Enqueue(y);
                        }
                    }
                }

                if (computers.All(c => c.Input.Count == 0 && c.Idle))
                {
                    if (natSent.Contains(natY))
                    {
                        return natY;
                    }

                    computers[0].Input.Enqueue(natX);
                    computers[0].Input.Enqueue(natY);
                    natSent.Add(natY);
                }
            }
        }
    }
}
