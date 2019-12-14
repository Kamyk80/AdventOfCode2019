using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#pragma warning disable 8509
namespace AdventOfCode2019.Day03
{
    internal class Program
    {
        private static void Main()
        {
            var wires = File.ReadAllLines("input.txt")
                .Select(CalculateWire)
                .ToList();

            var crossings = (
                    from w0 in wires[0]
                    join w1 in wires[1]
                        on w0.Key equals w1.Key
                    select new
                    {
                        Dist = Math.Abs(w0.Key.x) + Math.Abs(w0.Key.y),
                        Steps = w0.Value + w1.Value
                    })
                .ToList();

            Console.WriteLine(crossings.Min(c => c.Dist));
            Console.WriteLine(crossings.Min(c => c.Steps));
            Console.ReadKey(true);
        }

        private static Dictionary<(int x, int y), int> CalculateWire(string desc)
        {
            var wire = new Dictionary<(int x, int y), int>();
            var segments = desc.Split(',')
                .Select(s => new
                {
                    Dir = s[0],
                    Len = int.Parse(s.Substring(1))
                })
                .ToList();
            var pos = (x: 0, y: 0);
            var dist = 1;

            foreach (var segment in segments)
            {
                for (var i = 0; i < segment.Len; i++)
                {
                    pos = segment switch
                    {
                        var s when s.Dir == 'L' => (--pos.x, pos.y),
                        var s when s.Dir == 'R' => (++pos.x, pos.y),
                        var s when s.Dir == 'U' => (pos.x, --pos.y),
                        var s when s.Dir == 'D' => (pos.x, ++pos.y)
                    };

                    wire.TryAdd(pos, dist++);
                }
            }

            return wire;
        }
    }
}
#pragma warning restore 8509
