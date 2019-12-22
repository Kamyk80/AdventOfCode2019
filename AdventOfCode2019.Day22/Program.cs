using System;
using System.IO;
using System.Linq;

#pragma warning disable 8509
namespace AdventOfCode2019.Day22
{
    internal class Program
    {
        private static void Main()
        {
            var lines = File.ReadAllLines("input.txt");

            Console.WriteLine(Position(lines));
            Console.ReadKey(true);
        }

        private static int Position(string[] lines) =>
            lines.Aggregate(Enumerable.Range(0, 10007).ToArray(), (deck, line) => line switch
                {
                    var l when l.Equals("deal into new stack") => DealIntoNewStack(deck),
                    var l when l.StartsWith("cut ") => CutCards(deck, int.Parse(l.Substring(4))),
                    var l when l.StartsWith("deal with increment ") => DealWithIncrement(deck, int.Parse(l.Substring(20)))
                })
                .Select((c, i) => (c, i))
                .First(p => p.c == 2019).i;

        private static int[] DealIntoNewStack(int[] deck) =>
            deck
                .Reverse()
                .ToArray();

        private static int[] CutCards(int[] deck, int value) =>
            deck
                .Skip((value + deck.Length) % deck.Length)
                .Concat(deck
                    .Take((value + deck.Length) % deck.Length))
                .ToArray();

        private static int[] DealWithIncrement(int[] deck, int value) =>
            deck
                .Select((c, i) => (c, i))
                .OrderBy(p => p.i * value % deck.Length)
                .Select(p => p.c)
                .ToArray();
    }
}
#pragma warning restore 8509
