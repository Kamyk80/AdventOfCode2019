using System;
using System.IO;

namespace AdventOfCode2019.Day14
{
    internal class Program
    {
        private static void Main()
        {
            var recipes = File.ReadAllLines("input.txt");
            var refinery = new Refinery(recipes);

            Console.WriteLine(refinery.OreForFuel());
            Console.WriteLine(refinery.FuelFromOre());
            Console.ReadKey(true);
        }
    }
}
