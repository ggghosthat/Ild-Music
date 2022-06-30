using ShareInstances;
using System;
using System.Collections.Generic;
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
        public IPlayer PlayerInstance => _playerInstance;
        public IList<IPlayer> _listPlayers => _players;

        public Dictionary<string, string> path = new();
        #endregion

        //Synch fields
        #region SynchFielfs
        private static IList<ISynchArea> _synchAreas = new List<ISynchArea>();
        private ISynchArea _synchAreaInstance;
        public ISynchArea SynchArea => _synchAreaInstance;
        #endregion

        #region InitializeMethods
        public void InitPlayer(string playerAssembly)
        {
            AssemblyProcess<IPlayer>(playerAssembly, _playerInstance);
            //_playerInstance = _players[0];
        }

        public void InitSynch(string synchAssembly)
        {
            AssemblyProcess(synchAssembly, typeof(ISynchArea));
            _synchAreaInstance = _synchAreas[0];
        }

        public void Init(string playerAssembly, string synchAssembly)
        {
            AssemblyProcess(playerAssembly, _playerInstance);
            AssemblyProcess(synchAssembly, _synchAreaInstance);

            _playerInstance = _players[0];
            _synchAreaInstance = _synchAreas[0];
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
                
                //if (assemblyType.GetType() is ISynchArea)
                //    specialTypes.ToList()
                //                .ForEach(synch => _synchAreas.Add((ISynchArea)synch));
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region AssemblySearchingMethods
        private IEnumerable<string> FindDlls(string path)
        {
            var directory = Directory.EnumerateFileSystemEntries(path, "*.dll");

            return directory;
        }
        
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
