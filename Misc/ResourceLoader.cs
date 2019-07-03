using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using OgSim.Resources;
using OgSim.Battle;

namespace OgSim.Misc
{
    public class ResourceLoader
    {
        Assembly assembly;

        public ResourceLoader()
        {
            this.assembly = typeof(ResourceLoader).GetTypeInfo().Assembly;
        }

        public Universe GetUniverse(string path)
        {
            //JObject universeObj = ParseFileAsJObject(path);
            //Universe u = (Universe) universeObj["default_universe"].ToObject<Universe>();

            Universe u;
            using (var stream = LoadStream(path))
            using (var reader = new JsonTextReader(stream))
            {
                JsonSerializer serializer = new JsonSerializer();
                u = serializer.Deserialize<Universe>(reader);
            }
            return u;
        }

        public Dictionary<int, Dictionary<int, int>> GetRapidFire(string path)
        {
            //string json = LoadResource(path);
            //var values = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>(json);Dictionary<int, Dictionary<int, int>> values;

            Dictionary<int, Dictionary<int, int>> values;
            using (var stream = LoadStream(path))
            using (var reader = new JsonTextReader(stream))
            {
                JsonSerializer serializer = new JsonSerializer();
                values = serializer.Deserialize<Dictionary<int, Dictionary<int, int>>>(reader);
            }

            return values;
        }

        public Dictionary<int, Resource> GetResources(string path)
        {
            //string json = LoadResource(path);
            //var resources = JsonConvert.DeserializeObject<Dictionary<int, Resource>>(json);

            Dictionary<int, Resource> resources;
            using (var stream = LoadStream(path))
            using (var reader = new JsonTextReader(stream))
            {
                JsonSerializer serializer = new JsonSerializer();
                resources = serializer.Deserialize<Dictionary<int, Resource>>(reader);
            }

            return resources;

        }

        public void GetFactions(string path, out List<Fleet> attackers, out List<Fleet> defenders)
        {
            //JObject factions = ParseFileAsJObject(path);
            //attackers = factions["attackers"].ToObject<List<Fleet>>();
            //defenders = factions["defenders"].ToObject<List<Fleet>>();

            Dictionary<string, List<Fleet>> factions;

            using (var stream = LoadStream(path))
            using (var reader = new JsonTextReader(stream))
            {
                JsonSerializer serializer = new JsonSerializer();
                factions = serializer.Deserialize<Dictionary<string, List<Fleet>>>(reader);
            }

            attackers = factions["attackers"];
            defenders = factions["defenders"];
        }

        protected JArray ParseFileAsJArray(string path)
        {
            string json = LoadResource(path);
            return JArray.Parse(json);
        }

        protected JObject ParseFileAsJObject(string path)
        {
            string json = LoadResource(path);
            return JObject.Parse(json);
        }

        protected string LoadFile(string path)
        {
            return String.Join("",File.ReadAllLines(path));
        }

        protected StreamReader LoadStream(string Id)
        {
            Stream stream = assembly.GetManifestResourceStream(Id);
            return new System.IO.StreamReader(stream);
        }

        protected string LoadResource(string Id)
        {
            
            Stream stream = assembly.GetManifestResourceStream(Id);
            string text = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            return text;
        }
    }
}
