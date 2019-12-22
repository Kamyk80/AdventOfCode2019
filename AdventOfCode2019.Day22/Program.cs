using System;
using System.IO;
using System.Linq;
using System.Numerics;

#pragma warning disable 8509
namespace AdventOfCode2019.Day22
{
    internal class Program
    {
        private const int SmallDeck = 10007;
        private const long LargeDeck = 119315717514047;
        private const long Repeats = 101741582076661;

        private static void Main()
        {
            var lines = File.ReadAllLines("input.txt");

            Console.WriteLine(Position(lines));
            Console.WriteLine(Card(lines));
            Console.ReadKey(true);
        }

        private static int Position(string[] lines) =>
            lines.Aggregate(Enumerable.Range(0, SmallDeck).ToArray(), (deck, line) => line switch
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

        private static BigInteger Card(string[] lines)
        {
            var incrementOne = BigInteger.One;
            var offsetOne = BigInteger.Zero;

            foreach (var line in lines)
            {
                if (line.Equals("deal into new stack"))
                {
                    incrementOne *= -1;
                    incrementOne %= LargeDeck;
                    offsetOne += incrementOne;
                    offsetOne %= LargeDeck;
                }
                else if (line.StartsWith("cut "))
                {
                    var value = int.Parse(line.Substring(4));
                    offsetOne += incrementOne * value;
                    offsetOne %= LargeDeck;
                }
                else if (line.StartsWith("deal with increment "))
                {
                    var value = int.Parse(line.Substring(20));
                    incrementOne *= ModularInverse(value);
                    incrementOne %= LargeDeck;
                }
            }

            var incrementAll = BigInteger.ModPow(incrementOne, Repeats, LargeDeck);
            var offsetAll = offsetOne * (1 - incrementAll) * ModularInverse((1 - incrementOne) % LargeDeck);
            offsetAll %= LargeDeck;

            return (offsetAll + 2020 * incrementAll) % LargeDeck;
        }

        private static BigInteger ModularInverse(BigInteger value) =>
            BigInteger.ModPow(value, LargeDeck - 2, LargeDeck);
    }
}
#pragma warning restore 8509
