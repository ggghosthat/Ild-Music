using ShareInstances;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace MainStage.Stage
{
    public class Platform
    {
        #region Allocations
        private readonly string allocation_path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName.ToString();
        #endregion

        //player fields
        #region PlayerFields
        private static IList<IPlayer> _players = new List<IPlayer>();
        public IList<IPlayer> listPlayers => _players;

        private IPlayer _playerInstance;
        public IPlayer PlayerInstance 
        {
            get => _playerInstance;
            set => _playerInstance = value;
        }
        #endregion

        //Synch fields
        #region SynchFielfs
        private static IList<ISynchArea> _synchAreas = new List<ISynchArea>();
        public IList<ISynchArea> listSynchAreas => _synchAreas;

        private ISynchArea _synchAreaInstance;
        public ISynchArea SynchAreaInstance
        {
            get => _synchAreaInstance;
            set => _synchAreaInstance = value;
        }
        #endregion

        #region PathProps
        public string PlayerRow { get; private set; }
        public string SynchRow { get; private set; }
        #endregion

        #region Events
        public event Action OnInitialized;
        #endregion

        #region InitializeMethods
        public Platform() =>
            Deserialize();

        public void InitPlayer(string playerAssembly)
        {
            AssemblyProcess<IPlayer>(playerAssembly, _playerInstance);
        }

        public void InitSynch(string synchAssembly)
        {
            AssemblyProcess<ISynchArea>(synchAssembly, _synchAreaInstance);
        }

        public void Init(string playerAssembly, string synchAssembly)
        {
            AssemblyProcess(playerAssembly, _playerInstance);
            AssemblyProcess(synchAssembly, _synchAreaInstance);

            _playerInstance = _players[0];
            _synchAreaInstance = _synchAreas[0];
        }

        #endregion

        #region Destructor
        ~ Platform() => Serialize();
        #endregion

        #region BuildMthods

        public Platform CallPlatform()
        {
            PathsInit();
            InitPlayer(PlayerRow);
            InitSynch(SynchRow);
            OnInitialized?.Invoke();
            return this;
        }

        private void PathsInit()
        {
            string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName.ToString().Replace("\\", "/");
            PlayerRow = startupPath + "/Ild-Music-Core/bin/Debug/net5.0-windows";
            SynchRow = startupPath + "/SynchronizationBlock/bin/Debug/net5.0";
        }        
        #endregion

        #region AssemblySearchingAPIMethod
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
                                .ForEach(area => _synchAreas.Add((ISynchArea)area));
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region AssemblySearchingMethods
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

        #region Serialization
        private void Serialize()
        {
            try
            {
                if (_players.Count > 0)
                {
                    string file = allocation_path + "\\platform_players.json";

                    if (!File.Exists(file))
                        File.Create(file);

                    var jsonString = JsonConvert.SerializeObject(_players);
                    File.WriteAllText(file, string.Empty);
                    File.WriteAllText(file, jsonString);
                }

                if (_synchAreas.Count > 0)
                {
                    string file = allocation_path + "\\platform_areas.json";

                    if (!File.Exists(file))
                        File.Create(file);

                    var jsonString = JsonConvert.SerializeObject(_synchAreas);
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
                string players_file = allocation_path + "\\platform_players.json";
                string areas_file = allocation_path + "\\platform_areas.json";
                
                if (File.Exists(players_file))
                {
                    string jsonString = File.ReadAllText(players_file);
                    _players = JsonConvert.DeserializeObject<List<IPlayer>>(jsonString);
                }

                if (File.Exists(areas_file))
                {
                    string jsonString = File.ReadAllText(areas_file);
                    _synchAreas = JsonConvert.DeserializeObject<List<ISynchArea>>(jsonString);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

    }
}
