using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2019.Day12
{
    public class Moon
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int PosZ { get; set; }
        public int VelX { get; set; }
        public int VelY { get; set; }
        public int VelZ { get; set; }

        public void ApplyGravity(IEnumerable<Moon> moons)
        {
            foreach (var moon in moons.Except(new[] {this}))
                ApplyGravity(moon);
        }

        private void ApplyGravity(Moon moon)
        {
            VelX = moon.PosX switch
            {
                var refX when PosX < refX => ++VelX,
                var refX when PosX > refX => --VelX,
                _ => VelX
            };

            VelY = moon.PosY switch
            {
                var refY when PosY < refY => ++VelY,
                var refY when PosY > refY => --VelY,
                _ => VelY
            };

            VelZ = moon.PosZ switch
            {
                var refZ when PosZ < refZ => ++VelZ,
                var refZ when PosZ > refZ => --VelZ,
                _ => VelZ
            };
        }

        public void Move()
        {
            PosX += VelX;
            PosY += VelY;
            PosZ += VelZ;
        }

        public int TotalEnergy() => PotentialEnergy() * KineticEnergy();

        private int PotentialEnergy() => Math.Abs(PosX) + Math.Abs(PosY) + Math.Abs(PosZ);

        private int KineticEnergy() => Math.Abs(VelX) + Math.Abs(VelY) + Math.Abs(VelZ);
    }

    internal class Program
    {
        private static void Main()
        {
            var regex = new Regex(@"^<x=(?<x>-?\d+), y=(?<y>-?\d+), z=(?<z>-?\d+)>$");
            var moons = File.ReadAllLines("input.txt")
                .Select(l => regex.Match(l))
                .Select(m => new Moon
                {
                    PosX = int.Parse(m.Groups["x"].Value),
                    PosY = int.Parse(m.Groups["y"].Value),
                    PosZ = int.Parse(m.Groups["z"].Value)
                })
                .ToList();

            var startX = moons.Select(m => m.PosX).ToArray();
            var startY = moons.Select(m => m.PosY).ToArray();
            var startZ = moons.Select(m => m.PosZ).ToArray();

            var stepsX = 0;
            var stepsY = 0;
            var stepsZ = 0;

            for (var steps = 0; stepsX == 0 || stepsY == 0 || stepsZ == 0; steps++)
            {
                if (stepsX == 0 && moons.Select((m, i) => m.PosX == startX[i] && m.VelX == 0).All(b => b))
                    stepsX = steps;

                if (stepsY == 0 && moons.Select((m, i) => m.PosY == startY[i] && m.VelY == 0).All(b => b))
                    stepsY = steps;

                if (stepsZ == 0 && moons.Select((m, i) => m.PosZ == startZ[i] && m.VelZ == 0).All(b => b))
                    stepsZ = steps;

                if (steps == 1000)
                    Console.WriteLine(moons.Sum(m => m.TotalEnergy()));

                if (stepsX > 0 && stepsY > 0 && stepsZ > 0)
                    Console.Write(LeastCommonMultiple(LeastCommonMultiple(stepsX, stepsY), stepsZ));

                foreach (var moon in moons)
                    moon.ApplyGravity(moons);

                foreach (var moon in moons)
                    moon.Move();
            }

            Console.ReadKey(true);
        }

        private static long LeastCommonMultiple(long a, long b)
            => a * b / GreatestCommonDivisor(a, b);

        private static long GreatestCommonDivisor(long a, long b)
        {
            while (a != b)
                if (a > b) a -= b;
                else b -= a;
            return a;
        }
    }
}
