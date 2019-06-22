using System;
using OgSim.Resources;
using OgSim.Misc;
using OgSim.Battle;
using System.Collections.Generic;

namespace OgSim
{
    class Program
    {
        static void Main(string[] args)
        {
            Profiler profiler = new OgSim.Misc.Profiler();
            profiler.StartTask("Ogsim");


            profiler.StartTask("json_parsing");
            ResourceLoader rl = new ResourceLoader();
            Universe u = rl.GetUniverse( "OgSim.data.universe.json" );
            Dictionary<int, Dictionary<int, int>> rapidfire = rl.GetRapidFire("OgSim.data.rapidfire.json");
            Dictionary<int, Resource> resources = rl.GetResources("OgSim.data.resources.json");
            foreach (KeyValuePair<int, Resource> kvp in resources)
            {
                if (rapidfire.TryGetValue(kvp.Key, out Dictionary<int, int> rf))
                {
                    kvp.Value.ProcessRapidfires(rf);
                }
            }
            rl.GetFactions("OgSim.data.factions.json", out List<Fleet> attackers, out List<Fleet> defenders);
            profiler.StopTask("json_parsing");


            profiler.StartTask("fleet_expansion");
            Faction attackerGroup = new Faction("attackers", resources, attackers);
            Faction defenderGroup = new Faction("defenders", resources, defenders);

            //Debugger.ConsoleLog(attackers);
            //Debugger.ConsoleLog(defenders);
            //return;

            attackerGroup.ExpandFleets();
            defenderGroup.ExpandFleets();
            profiler.StopTask("fleet_expansion");


            profiler.StartTask("battle_rounds");
            int turns = 6;
            bool exitBattle = false;
            //TODO: mostrar el estado de las flotas completas
            for (int round = 1; round < turns + 1; round++)
            {
                if (attackerGroup.ships.Count < 1)
                {
                    Console.WriteLine("Attacker group has no remaining ships in battle");
                    exitBattle = true;
                }
                if (defenderGroup.ships.Count < 1)
                {
                    Console.WriteLine("Defender group has no remaining ships in battle");
                    exitBattle = true;
                }
                //TODO: if both factions are too weak, just end the battle.
                if (exitBattle)
                {
                    break;
                }
                profiler.StartTask("round_" + round);
                profiler.StartTask("round_attackers_" + round);
                attackerGroup.Attack(defenderGroup);
                profiler.StopTask("round_attackers_" + round);
                profiler.StartTask("round_defenders_" + round);
                defenderGroup.Attack(attackerGroup);
                profiler.StopTask("round_defenders_" + round);
                profiler.StartTask("cleanup_attackers_"+round);
                attackerGroup.CleanUp();
                profiler.StopTask("cleanup_attackers_" + round);
                profiler.StartTask("cleanup_defenders_" + round);
                defenderGroup.CleanUp();
                profiler.StopTask("cleanup_defenders_" + round);

                attackerGroup.AfterRound(round);
                defenderGroup.AfterRound(round);

                profiler.StopTask("round_" + round);
            }
            profiler.StopTask("battle_rounds");

            profiler.StopTask("Ogsim");
            Debugger.ConsoleLog(profiler.GetResults());
            System.Console.WriteLine("Finit");
        }

    }
}
