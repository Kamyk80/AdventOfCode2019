using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Day06
{
    internal class Program
    {
        private static Dictionary<string, string> _orbits;

        private static void Main()
        {
            _orbits = File.ReadAllLines("input.txt")
                .Select(l => l.Split(')'))
                .ToDictionary(s => s[1], s => s[0]);

            var checksum = _orbits
                .Select(o => o.Value)
                .Sum(CountOrbits);

            var paths = new[] {PathToRoot("YOU"), PathToRoot("SAN")};

            var transfers = paths
                .Select(p1 => p1.SkipWhile((c, i) => paths.All(p2 => p2[i] == c)))
                .Sum(p => p.Count(c => c == ')'));

            Console.WriteLine(checksum);
            Console.WriteLine(transfers);
            Console.ReadKey(true);
        }

        private static int CountOrbits(string orbit) =>
            _orbits.TryGetValue(orbit, out var parent) ? CountOrbits(parent) + 1 : 1;

        private static string PathToRoot(string orbit) =>
            _orbits.TryGetValue(orbit, out var parent) ? PathToRoot(parent) + ")" + orbit : orbit;
    }
}
