using System;
using System.Collections.Generic;

namespace OgSim.Resources
{
    public class Resource
    {
        public int id;
        public string name;
        public int metal;
        public int crystal;
        public int deuterium;
        public int energy;
        public double factor;
        public int capacity;
        public double attack;
        public double defense;
        public double hull;
        public Dictionary<int, int> motors;
        public Dictionary<int, int> speeds;
        public Dictionary<int, int> comsumptions;

        public Dictionary<int, double> rapidFires = new Dictionary<int, double>();

        public void ProcessRapidfires(Dictionary<int, int> rfRaw)
        {
            foreach (KeyValuePair<int, int> kvp in rfRaw)
            {
                rapidFires.Add(kvp.Key, (1.0 - (1.0 / kvp.Value) ) );
            }
        }
            
    }
}
