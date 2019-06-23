using System;
using System.Collections.Generic;
using OgSim.Resources;
using OgSim.Misc;

namespace OgSim.Battle
{
    public class Faction
    {
        public List<Fleet> fleetList;
        readonly Dictionary<int, Resource> resourceList = new Dictionary<int, Resource>();
        readonly string group;

        double turnDamage = 0;
        double turnDefense = 0;
        double turnAttacks;

        public List<(double, double, ShipType)> ships = new List<(double, double, ShipType)>();

        

        public Faction(string group, Dictionary<int, Resource> resources, List<Fleet> fleet)
        {
            this.group = group;
            resourceList = resources;
            this.fleetList = fleet;
        }

        public void ExpandFleets()
        {
            foreach(Fleet f in fleetList)
            {
                f.ExpandTo(resourceList, ships);
            }
        }

        public void AfterRound(int turn, string other)
        {
            Console.WriteLine("The "+group+ " faction attacked "+turnAttacks+" times with "+ turnDamage+" hit points");
            Console.WriteLine("The "+other+" fleet shields absorbed "+turnDefense+" points of damage");

            turnDefense = 0;
            turnDamage = 0;
            //turnAttacks = 0;
        }

        public void CleanUp()
        {
            Console.WriteLine(group + ": Length before cleanup " + ships.Count);

            List<(double, double, ShipType)> newShips = new List<(double, double, ShipType)>();
            //TODO: convertir a llamada de array?
            foreach(var ship in ships)
            {
                if(ship.Item1 > 0.0)
                {
                    newShips.Add( (ship.Item1,ship.Item3.baseShield,ship.Item3) );
                }
            }
            ships = newShips;
            Console.WriteLine(group + ": Length after cleanup " + ships.Count);
        }

        public void Attack(Faction otherGroup, Dictionary<int, Dictionary<int, double>> rapidfireInfo)
        {
            Random rn = new Random();

            int tA = ships.Count;
            double tDm = 0.0;
            double tDf = 0.0;
            var enemyShips = otherGroup.ships;
            int enemyCount = enemyShips.Count;

            double dm;
            double de;
            double remaining;
            double xp;

            int enemyPos;
            (double, double, ShipType) enemyShip;
            ShipType enemyShipType;

            bool running;

            foreach (var ship in ships)
            {
                dm = ship.Item3.baseAttack;

                running = true;
                while (running)
                {
                    enemyPos = rn.Next(enemyCount);
                    enemyShip = enemyShips[enemyPos];
                    enemyShipType = enemyShip.Item3;
                    tDm += dm;

                    if (enemyShip.Item1 > 0.0)
                    {
                        if(dm * 100 > enemyShipType.baseShield)
                        {
                            if ( dm > enemyShip.Item2 )
                            {
                                de = dm - enemyShip.Item2;
                                tDf += enemyShip.Item2;

                                if(de < enemyShip.Item1)
                                {
                                    remaining = enemyShip.Item1 - de;
                                    xp = remaining / enemyShipType.baseHull;

                                    if(xp < 0.7 && ( rn.NextDouble() < 1 - xp) )
                                    {
                                        //Kaboom
                                        enemyShips[enemyPos] = (0.0, 0.0, enemyShipType);
                                        enemyShipType.explosions++;

                                    } else
                                    {
                                        enemyShips[enemyPos] = (remaining, 0.0, enemyShipType);
                                    }

                                } else
                                {
                                    //Kaboom
                                    enemyShips[enemyPos] = (0.0, 0.0, enemyShipType);
                                    enemyShipType.explosions++;
                                }
                            }
                            else
                            {
                                tDf += dm;

                                //Damaged ships from previous rounds should explode anyway
                                xp = enemyShip.Item1 / enemyShipType.baseHull;
                                if (xp < 0.7 && (rn.NextDouble() < 1 - xp))
                                {
                                    //Kaboom
                                    enemyShips[enemyPos] = (0.0, 0.0, enemyShipType);
                                    enemyShipType.explosions++;

                                }
                                else
                                {
                                    enemyShips[enemyPos] = (enemyShip.Item1, enemyShip.Item2 - dm, enemyShipType);
                                }
                            }
                        }
                        else
                        {
                            tDf += dm;
                        }
                    }

                    if(rapidfireInfo[ship.Item3.id][enemyShipType.id] > rn.NextDouble() ){
                        tA++;
                        running = true;
                    } else
                    {
                        running = false;
                    }
                }
            }

            turnAttacks = tA;
            turnDefense = tDf;
            turnDamage = tDm;
        }
    }
}
