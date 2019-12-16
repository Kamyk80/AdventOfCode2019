using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Day16
{
    internal class Program
    {
        private static void Main()
        {
            var input = File.ReadAllText("input.txt").Trim()
                .Select(c => int.Parse(c.ToString()))
                .ToArray();

            Console.WriteLine(TestSignal(input));
            Console.WriteLine(RealSignal(input));
            Console.ReadKey(true);
        }

        private static char[] TestSignal(int[] input)
        {
            for (var phase = 0; phase < 100; phase++)
            {
                var nextInput = new int[input.Length];

                for (var digit = 0; digit < input.Length; digit++)
                {
                    // ReSharper disable once AccessToModifiedClosure
                    // ReSharper disable once GenericEnumeratorNotDisposed
                    using var pattern = Enumerable.Repeat(new[] {0, 1, 0, -1}, int.MaxValue)
                        .SelectMany(a => a)
                        .SelectMany(d => Enumerable.Repeat(d, digit + 1))
                        .GetEnumerator();

                    pattern.MoveNext();

                    var sum = input
                        .Select(d =>
                        {
                            pattern.MoveNext();
                            return pattern.Current * d;
                        })
                        .Sum();

                    nextInput[digit] = Math.Abs(sum % 10);
                }

                input = nextInput;
            }

            return input.Take(8).Select(d => (char) ('0' + d)).ToArray();
        }

        private static char[] RealSignal(int[] input)
        {
            var offset = 10000 * input.Length - input
                             .Take(7)
                             .Reverse()
                             .Select((d, i) => d * (int) Math.Pow(10, i))
                             .Sum();

            var result = new List<int>();

            // ReSharper disable once GenericEnumeratorNotDisposed
            using var enumerator = Enumerable.Repeat(input.Reverse().Select(d => d), int.MaxValue)
                .SelectMany(a => a).GetEnumerator();

            enumerator.MoveNext();
            var prev = Enumerable.Repeat(enumerator.Current, 100)
                .ToArray();

            for (var digit = 1; digit < offset; digit++)
            {
                enumerator.MoveNext();
                var current = enumerator.Current;
                var next = new int[100];

                for (var phase = 0; phase < 100; phase++)
                {
                    current += prev[phase];
                    next[phase] = current % 10;
                }

                prev = next;

                if (digit >= offset - 8)
                {
                    result.Add(current % 10);
                }
            }

            return result.Select(d => (char) ('0' + d)).Reverse().ToArray();
        }
    }
}
