using ShareInstances;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MainStage.Stage
{
    internal class Platform
    {
        //player fields
        private IList<IPlayer> _players = new List<IPlayer>();
        private IPlayer _playerInstance;
        public IPlayer PlayerInstance => _playerInstance;

        //Synch fields
        private IList<ISynchArea> _synchAreas = new List<ISynchArea>();
        private ISynchArea _synchAreaInstance;
        public ISynchArea SynchArea => _synchAreaInstance;

        public Platform(string playerAssembly, string synchAssembly)
        {
            AssemblyProcess(playerAssembly, _playerInstance);
            AssemblyProcess(synchAssembly, _synchAreaInstance);

            _playerInstance = _players[0];
            _synchAreaInstance = _synchAreas[0];
        }

        

        #region AssemblySearchingAPIMethod
        private void AssemblyProcess<T>(string assemblyPath, T assemblyType)
        {
            try
            {
                IEnumerable<string> dlls = FindDlls(assemblyPath);
                IEnumerable<Type> specialTypes = FindSpecialTypes<T>(dlls);

                if (assemblyType is IPlayer)
                    specialTypes.ToList()
                                .ForEach(player => _players.Add((IPlayer)player));
                
                if (assemblyType is ISynchArea)
                    specialTypes.ToList()
                                .ForEach(synch => _synchAreas.Add((ISynchArea)synch));
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region AssemblySearchingMethods
        private IEnumerable<string> FindDlls(string path) =>
            Directory.EnumerateDirectories(path, "*.dll");  
        
        private IEnumerable<Type> FindSpecialTypes<T>(IEnumerable<string> dllsPath)
        {
            var result = new List<Type>();
            foreach (string path in dllsPath)
            {
                var assembly = Assembly.Load(path);
                var exportedTypes = assembly.ExportedTypes;
                exportedTypes.Where(t => t.IsClass && typeof(T).GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()))
                             .Select(t => t)
                             .ToList()
                             .ForEach(t => result.Add(t));               
            }

            result.ForEach(r => Activator.CreateInstance(r));
            return result;
        }
        #endregion
    }
}
