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
            {
                ApplyGravity(moon);
            }
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

            for (var i = 0; i < 1000; i++)
            {
                foreach (var moon in moons)
                    moon.ApplyGravity(moons);

                foreach (var moon in moons)
                    moon.Move();
            }

            Console.WriteLine(moons.Sum(m => m.TotalEnergy()));
            Console.ReadKey(true);
        }
    }
}
