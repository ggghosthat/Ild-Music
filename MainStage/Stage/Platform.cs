using ShareInstances;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MainStage.Stage
{
    public class Platform
    {
        //player fields
        #region PlayerFields
        private static IList<IPlayer> _players = new List<IPlayer>();
        private IPlayer _playerInstance;
        public IList<IPlayer> listPlayers => _players;
        #endregion

        //Synch fields
        #region SynchFielfs
        private static IList<ISynchArea> _synchAreas = new List<ISynchArea>();
        private ISynchArea _synchAreaInstance;
        public IList<ISynchArea> listSynchAreas => _synchAreas;

        #endregion

        #region PathProps
        public string PlayerRow { get; private set; }
        public string SynchRow { get; private set; }
        #endregion

        #region Events
        public event Action OnInitialized;
        #endregion


        #region InitializeMethods
        public Platform() { }


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

        #region BuildMthods

        public Platform CallPlatform()
        {
            PathsInit();
            InitPlayer(PlayerRow);
            InitSynch(SynchRow);
            OnInitialized?.Invoke();
            Debug.WriteLine("Platform is ready 2 use");
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
    }
}
