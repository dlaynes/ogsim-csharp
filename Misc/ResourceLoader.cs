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
            JObject universeObj = ParseFileAsJObject(path);

            //Debugger.ConsoleLog(universeObj["name"].Value<string>());

            Universe u = (Universe) universeObj["default_universe"].ToObject<Universe>();

            //JsonSerializer serializer = new JsonSerializer();
            //Universe u = (Universe) serializer.Deserialize(new JTokenReader(universeObj), typeof(Universe));
            return u;
        }

        public Dictionary<int, Dictionary<int, int>> GetRapidFire(string path)
        {

            string json = LoadResource(path);
            var values = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, int>>>(json);

            return values;
        }

        public Dictionary<int, Resource> GetResources(string path)
        {
            string json = LoadResource(path);
            var resources = JsonConvert.DeserializeObject<Dictionary<int, Resource>>(json);

            return resources;

        }

        public void GetFactions(string path, out List<Fleet> attackers, out List<Fleet> defenders)
        {
            JObject factions = ParseFileAsJObject(path);

            attackers = factions["attackers"].ToObject<List<Fleet>>();
            defenders = factions["defenders"].ToObject<List<Fleet>>();
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
