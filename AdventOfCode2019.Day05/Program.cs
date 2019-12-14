using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Day05
{
    internal class Program
    {
        public static void Main()
        {
            var program = File.ReadAllText("input.txt")
                .Split(',')
                .Select(int.Parse)
                .ToList();
            var computer = new Computer();

            Console.WriteLine(computer.Run(program, 1));
            Console.WriteLine(computer.Run(program, 5));
            Console.ReadKey(true);
        }
    }
}
