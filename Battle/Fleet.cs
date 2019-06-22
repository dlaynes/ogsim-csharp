using System;
using System.Collections.Generic;
using OgSim.Resources;
using OgSim.Misc;

namespace OgSim.Battle
{
    public class Fleet
    {
        public int id;
        public string name;
        public Location mainPlanet;
        public Dictionary<int, int> originalFleet;
        public int militaryTech;
        public int defenseTech;
        public int hullTech;

        public Dictionary<int, ShipType> shipTypes;

        public Fleet()
        {
            shipTypes = new Dictionary<int, ShipType>();
            //originalFleet = new Dictionary<string, int>();
        }

        public void ExpandTo
            (
                Dictionary<int, Resource> resourceList,
                List<(double, double, ShipType)> fleetList
            )
        {
            foreach (KeyValuePair<int, int> stk in originalFleet)
            {
                ShipType shipType = new ShipType(
                    resourceList.GetValueOrDefault(stk.Key),
                    defenseTech,
                    militaryTech,
                    hullTech,
                    stk.Value
                );

                double h = shipType.baseHull;
                double d = shipType.baseAttack;

                //Debugger.ConsoleLog(this);
                //Debugger.ConsoleLog(shipType);

                for (int i = 0; i < stk.Value; i++)
                {
                    fleetList.Add((h, d, shipType));
                }
                shipTypes.Add(stk.Key, shipType);

            }
        }
    }

}
