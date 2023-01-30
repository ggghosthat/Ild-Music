using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ShareInstances.Configure
{
    public class Configure : IConfigure
    {
        public string ComponentsFile {get; init;}

        public IEnumerable<string> Players {get; set;}
        public IEnumerable<string> Synches {get; set;}

        public Configure()
        {}

        public Configure(string componentsFile)
        {
            ComponentsFile = componentsFile;
            Parse();
        }

        public void Parse()
        {
            if (File.Exists(ComponentsFile))
            {
                using (StreamReader file = File.OpenText(ComponentsFile))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject json = (JObject)JToken.ReadFrom(reader);
                    Players = PassJObject(json["components"]["players"]);
                    Synches = PassJObject(json["components"]["synches"]);
                }
            }
            else
            {
                throw new Exception("Could not find your configuration file!");
            }
        }

        private IEnumerable<string> PassJObject(JToken array)
        {
            foreach (JToken token in array)
                yield return token.ToString();
        }
    }
}
