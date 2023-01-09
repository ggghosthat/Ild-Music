using ShareInstances;

using System;
using System.Linq;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MainStage.Stage
{
    public class Stage 
    {
        #region Player Region
        private IList<IPlayer> _players = new List<IPlayer>();
        public IList<IPlayer> Players => _players;

        public IPlayer PlayerInstance { get; private set; }

        #endregion

        #region Synch Region
        private IList<ISynchArea> _areas = new List<ISynchArea> ();
        public IList<ISynchArea> Areas => _areas;

        public ISynchArea AreaInstace { get; private set; }

        #endregion
        
        #region Paths
        public List<string> PlayerPaths { get; set; }
        public List<string> SynchPaths { get; set; }

        public string DumpPath { get; init; }
        #endregion
        
        #region Event
        public event Action OnInittialized;
        #endregion

        public Stage() => Deserialize();   
        
        public Stage(string dumpPath)
        {
            DumpPath = dumpPath;
        }

        public Stage(string playerPath, string synchPath)
        {
            Init(playerPath, synchPath);
        }

        #region Inits
        
        public void Init(string playerAssembly, string synchAssembly)
        {
            AssemblyProcess(playerAssembly, PlayerInstance);
            AssemblyProcess(synchAssembly, AreaInstace);

            PlayerInstance = _players[0];
            AreaInstace = _areas[0];
        }
        #endregion
        
        #region AssemblySearchingMethods
        
        private void AssemblyProcess<T>(string assemblyPath, T assemblyType)
        {
            try
            {
                IEnumerable<string> dlls = FindDlls(assemblyPath);
                (Type, IEnumerable<T>) result = FindSpecialTypes<T>(dlls);

                if (typeof(IPlayer).IsAssignableFrom(result.Item1))
                    result.Item2.ToList()
                                .ForEach(player => _players.Add((IPlayer)player));

                if (typeof(ISynchArea).IsAssignableFrom(result.Item1))
                    result.Item2.ToList()
                                .ForEach(area => _areas.Add((ISynchArea)area));
            }
            catch
            {
                throw;
            }
        }

        private IEnumerable<string> FindDlls(string path) =>
            Directory.EnumerateFileSystemEntries(path, "*.dll");
        
        
        private (Type,IEnumerable<T>) FindSpecialTypes<T>(IEnumerable<string> dllsPath)
        {
            var list = new List<T>();
            foreach (string path in dllsPath)
            {
                var assembly = Assembly.LoadFrom(path);
                var exportedTypes = assembly.ExportedTypes;
                exportedTypes.Where(t => t.IsClass && typeof(T).GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()))
                             .Select(t => t)
                             .ToList()
                             .ForEach(t => {
                                 var tmp = (T)Activator.CreateInstance(t);
                                 list.Add(tmp);
                             });


            }
            return (typeof(T), list);
        }
        #endregion
        
        #region Serialize&Desirialize
        private void Serialize()
        {
            try
            {
                if (_players.Count > 0)
                {
                    string file = Environment.CurrentDirectory + "\\platform_players.json";

                    if (!File.Exists(file))
                        File.Create(file);

                    var jsonString = JsonConvert.SerializeObject(_players);
                    File.WriteAllText(file, string.Empty);
                    File.WriteAllText(file, jsonString);
                }

                if (_areas.Count > 0)
                {
                    string file = Environment.CurrentDirectory + "\\platform_areas.json";

                    if (!File.Exists(file))
                        File.Create(file);

                    var jsonString = JsonConvert.SerializeObject(_areas);
                    File.WriteAllText(file, string.Empty);
                    File.WriteAllText(file, jsonString);
                }
            }
            catch 
            {
                throw; 
            }
        }

        private void Deserialize()
        {
            try
            {
                string players_file = Environment.CurrentDirectory + "\\platform_players.json";
                string areas_file = Environment.CurrentDirectory + "\\platform_areas.json";
                
                if (File.Exists(players_file))
                {
                    string jsonString = File.ReadAllText(players_file);
                    _players = JsonConvert.DeserializeObject<List<IPlayer>>(jsonString);
                }

                if (File.Exists(areas_file))
                {
                    string jsonString = File.ReadAllText(areas_file);
                    _areas = JsonConvert.DeserializeObject<List<ISynchArea>>(jsonString);
                }

                PlayerInstance = _players[0];
                AreaInstace = _areas[0];
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}