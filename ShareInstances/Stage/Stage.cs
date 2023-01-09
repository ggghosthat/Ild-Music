using ShareInstances.Services.Center;
using ShareInstances.Services.Interfaces;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ShareInstances.Stage
{
    public record struct DumpStructure(string Name, string Path, string Type);

    public class Stage 
    {
        #region Dump Region
        public ICollection<DumpStructure> dumps = new List<DumpStructure>();
        private bool IsDumpIgnore = false;
        #endregion

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

        public string DumpPath { get; set; } = Environment.CurrentDirectory + "\\components.json";
        #endregion
        
        #region Event
        public event Action OnInittialized;
        public event Action OnComponentMuted;
        #endregion

        #region Service Supply
        public ServiceCenter serviceCenter = new();
        #endregion

        #region Constructors
        //Pwease, Do not code in this constructor!
        public Stage(){}
        
        public Stage(string file)
        {
            DumpPath = file;
            Deserialize();
            OnComponentMuted += () => serviceCenter.ResolveSupporter(AreaInstace);
            OnComponentMuted += () => serviceCenter.ResolvePlayer(PlayerInstance);
            OnComponentMuted?.Invoke();
        }

        public Stage(string playerPath, string synchPath)
        {
            Init(playerPath, synchPath);
            OnComponentMuted += () => serviceCenter.ResolveSupporter(AreaInstace);
            OnComponentMuted?.Invoke();
        }
        #endregion


        #region Inits
        public void Init(string playerAssembly, string synchAssembly)
        {
            AssemblyProcess(playerAssembly, PlayerInstance);
            AssemblyProcess(synchAssembly, AreaInstace);

            PlayerInstance = _players[0];
            AreaInstace = _areas[0];

            serviceCenter.ResolveSupporter(AreaInstace);
        }
        
        private void InitUnit(string path, string type)
        {
            if (type == "player")
            {
                AssemblyProcess(path, PlayerInstance);
                PlayerInstance = _players[0];
            }
            if (type == "syncharea")
            {
                AssemblyProcess(path, AreaInstace);
                AreaInstace = _areas[0];
            }
        }
        #endregion
        
        #region AssemblySearchingMethods
        
        private void AssemblyProcess<T>(string assemblyPath, T assemblyType)
        {
            try
            {
                IEnumerable<string> dlls = FindDlls(assemblyPath);

                (Type, IEnumerable<T>) result = FindSpecialTypes<T>(ref dlls);


                if (typeof(IPlayer).IsAssignableFrom(result.Item1))
                    result.Item2.ToList()
                                .ForEach(player => _players.Add((IPlayer)player));

                else if (typeof(ISynchArea).IsAssignableFrom(result.Item1))
                    result.Item2.ToList()
                                .ForEach(area => _areas.Add((ISynchArea)area));
            
            }
            catch
            {
                throw;
            }
        }

        private IEnumerable<string> FindDlls(string path) 
        {
            if (path.EndsWith(".dll"))
                return new List<string>(){path};
            return Directory.EnumerateFileSystemEntries(path, "*.dll");
        }
        
        private (Type,IEnumerable<T>) FindSpecialTypes<T>(ref IEnumerable<string> dllsPath)
        {
            var list = new List<T>();
            foreach (string path in dllsPath)
            {
                var assembly = Assembly.LoadFrom(path);
                var exportedTypes = assembly.ExportedTypes;
                exportedTypes.Where(t => t.IsClass && t.GetInterfaces().Contains(typeof(T)))
                             .Select(t => t)
                             .ToList()
                             .ForEach(t => 
                             {
                                var instance = (T)Activator.CreateInstance(t);
                                if (!IsDumpIgnore)
                                {
                                    if (instance is IPlayer player) 
                                        dumps.Add(new DumpStructure(player.PlayerName, path.Replace("\\","/"), "player"));
                                    if (instance is ISynchArea area)
                                        dumps.Add(new DumpStructure(area.AreaName, path.Replace("\\","/"), "syncharea"));
                                }
                                list.Add(instance);
                             });
                dllsPath.ToList().Remove(path);

            }

            return (typeof(T), list);
        }
        #endregion
        
        #region Serialize&Desirialize
        public void Serialize()
        {
            try
            {
                if (!File.Exists(DumpPath))
                    File.Create(DumpPath);

                var jsonString = JsonConvert.SerializeObject(dumps);
                File.WriteAllText(DumpPath, string.Empty);
                File.WriteAllText(DumpPath, jsonString);
            }
            catch 
            {
                throw; 
            }
        }

        public void Deserialize()
        {
            try
            {
                string jsonString = File.ReadAllText(DumpPath);
                dumps = JsonConvert.DeserializeObject<List<DumpStructure>>(jsonString);
                IsDumpIgnore = true;

                foreach (var dump in dumps)
                    InitUnit(dump.Path, dump.Type);
            }
            catch
            {
                throw;
            }
        }
        #endregion
    
        #region Clear
        public void Clear()
        {
            _players.Clear();
            _areas.Clear();
            dumps.Clear();
        }
        #endregion    
    
        #region Service Center methods
        public IService GetServiceInstance(string name) => 
            serviceCenter.GetService(name);
        #endregion

        #region Mutabillity
        public void ChangeComponent(IShare component)
        {
            if (component is IPlayer playerInstance)
                PlayerInstance = playerInstance;
            else if(component is ISynchArea areaInstance)
                AreaInstace = areaInstance;


            OnComponentMuted?.Invoke();
        }

        public async Task ChangeComponentAsync(IShare component) =>
            await new Task( () =>
            {
                if (component is IPlayer playerInstance)
                    PlayerInstance = playerInstance;
                else if(component is ISynchArea areaInstance)
                    AreaInstace = areaInstance;


                OnComponentMuted?.Invoke();
            });  

        #endregion  
    }
}