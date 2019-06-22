using System;
namespace OgSim.Resources
{
    public enum LocationType
    {
        PLANET = 1,
        MOON,
        DEBRIS,
        SPACE
    }

    public class Location
    {

        public int galaxy;
        public int system;
        public int planet;
        public LocationType planetType;

    }
}
