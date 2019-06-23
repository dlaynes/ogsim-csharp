using System;
using System.Collections.Generic;
using OgSim.Resources;

namespace OgSim.Battle
{
    public class ShipType
    {
        public int id;
        public double baseShield;
        public double baseAttack;
        public double baseHull;

        public int amount;
        public int explosions;

        //public Dictionary<int, double> rapidFires;
        public List<int> statistics = new List<int>();

        public ShipType(
            Resource res,
            double defenseTech,
            double militaryTech,
            double hullTech,
            int amount
        )
        {
            double d = 1 + (militaryTech * 0.1);
            double s = 1 + (defenseTech * 0.1);
            //The next formula was changed on purpose, in order to lower the baseHull
            double h = (1 + (hullTech * 0.1)) * 0.1;

            id = res.id;
            baseAttack = d * res.attack;
            baseShield = s * res.defense;
            baseHull = h * res.hull;

            //rapidFires = res.rapidFires;

            this.amount = amount;
        }
    }
}
