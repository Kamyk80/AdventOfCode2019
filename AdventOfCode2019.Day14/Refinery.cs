using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Day14
{
    public class Refinery
    {
        private static List<Recipe> _recipes;
        private static Dictionary<string, long> _stock;

        public Refinery(IEnumerable<string> recipes)
        {
            _recipes = recipes.Select(r => r.Split(" => "))
                .Select(s => new Recipe
                {
                    Input = s[0].Split(", ")
                        .Select(s1 => s1.Split(" "))
                        .Select(s1 => (int.Parse(s1[0]), s1[1]))
                        .ToList(),
                    Output = s[1].Split(", ")
                        .Select(s1 => s1.Split(" "))
                        .Select(s1 => (int.Parse(s1[0]), s1[1]))
                        .ToList()
                })
                .ToList();
        }

        public long OreForFuel(long units = 1)
        {
            _stock = new Dictionary<string, long>();
            return OreFor(units, "FUEL");
        }

        private static long OreFor(long units, string chemical)
        {
            if (chemical == "ORE")
            {
                return units;
            }

            if (_stock.TryGetValue(chemical, out var unitsInStock))
            {
                var unitsFromStock = Math.Min(units, unitsInStock);
                _stock[chemical] -= unitsFromStock;
                units -= unitsFromStock;

                if (units == 0)
                {
                    return 0;
                }
            }

            var recipe = _recipes.First(r => r.Output.Any(o => o.Chemical == chemical));
            var produces = recipe.Output.First(o => o.Chemical == chemical).Units;
            var cycles = units / produces + (units % produces > 0 ? 1 : 0);

            var oreNeeded = recipe.Input
                .Sum(i => OreFor(i.Units * cycles, i.Chemical));

            recipe.Output
                .ForEach(o => AddToStock(o.Units * cycles - (o.Chemical == chemical ? units : 0), o.Chemical));

            return oreNeeded;
        }

        private static void AddToStock(long units, string chemical)
        {
            _stock.TryGetValue(chemical, out var stock);
            _stock[chemical] = stock + units;
        }

        public long FuelFromOre(long units = 1000000000000L)
        {
            var oreForOneFuel = OreForFuel();
            var fuel = units / oreForOneFuel;
            var ore = OreForFuel(fuel);

            while (ore <= units)
            {
                var inc = (units - ore) / oreForOneFuel;
                fuel += Math.Max(inc, 1);
                ore = OreForFuel(fuel);
            }

            return fuel - 1;
        }

        private class Recipe
        {
            public List<(int Units, string Chemical)> Input;
            public List<(int Units, string Chemical)> Output;
        }
    }
}
