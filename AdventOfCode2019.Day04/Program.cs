using System;
using System.Linq;

namespace AdventOfCode2019.Day04
{
    public class Program
    {
        private const int PasswordMin = 273025;
        private const int PasswordMax = 767253;

        private static void Main()
        {
            var criteria1Met = Enumerable.Range(PasswordMin, PasswordMax - PasswordMin + 1)
                .Count(p => Criteria1Met(p.ToString()));
            var criteria2Met = Enumerable.Range(PasswordMin, PasswordMax - PasswordMin + 1)
                .Count(p => Criteria2Met(p.ToString()));

            Console.WriteLine(criteria1Met);
            Console.WriteLine(criteria2Met);
            Console.ReadKey(true);
        }

        private static bool Criteria1Met(string password)
        {
            return password.GroupBy(c => c).Any(g => g.Count() >= 2) && NeverDecreases(password);
        }

        private static bool Criteria2Met(string password)
        {
            return password.GroupBy(c => c).Any(g => g.Count() == 2) && NeverDecreases(password);
        }

        private static bool NeverDecreases(string password)
        {
            var prev = password[0];

            for (var i = 1; i < 6; i++)
            {
                if (password[i] < prev)
                {
                    return false;
                }

                prev = password[i];
            }

            return true;
        }
    }
}
