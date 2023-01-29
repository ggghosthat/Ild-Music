using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ild_Music.Configure
{
    public class Configure
    {
        private string ComponentsFile {get; init;}

        private IList<string> Players {get; set;}
        private IList<string> Synches {get; set;}

        public Configure()
        {}

        public Configure(string componentsFile)
        {
            ComponentsFile = componentsFile;
            Parse();
        }

        private static void Parse()
        {
            using (StreamReader file = File.OpenText(ComponentsFile))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject json = (JObject)JToken.ReadFrom(reader);
                Players = json["components"]["player"].ToList();
                Synches = json["components"]["synch"].ToList();
            }
        }
    }
}
