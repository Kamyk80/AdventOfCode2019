using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Day09
{
    internal class Program
    {
        private static void Main()
        {
            var program = File.ReadAllText("input.txt")
                .Split(',')
                .Select(long.Parse)
                .ToList();

            Console.WriteLine(string.Join(',', new Computer(program).Run(1)));
            Console.WriteLine(string.Join(',', new Computer(program).Run(2)));
            Console.ReadKey(true);
        }
    }
}
