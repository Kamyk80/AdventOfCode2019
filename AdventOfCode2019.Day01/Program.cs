using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Day01
{
    internal class Program
    {
        private static void Main()
        {
            var modules = File.ReadAllLines("input.txt")
                .Select(int.Parse)
                .ToList();

            var modulesFuel = modules
                .Sum(PartialFuel);

            var totalFuel = modules
                .Sum(ModuleFuel);

            Console.WriteLine(modulesFuel);
            Console.WriteLine(totalFuel);
            Console.ReadKey(true);
        }

        private static int PartialFuel(int mass)
        {
            return mass / 3 - 2;
        }

        private static int ModuleFuel(int mass)
        {
            var fuel = 0;
            while (mass > 0)
            {
                mass = mass / 3 - 2;
                fuel += mass > 0 ? mass : 0;
            }

            return fuel;
        }
    }
}
