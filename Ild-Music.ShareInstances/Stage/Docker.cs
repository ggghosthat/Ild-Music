using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Collections.Generic;

using ShareInstances;
using ShareInstances.Configure;

namespace ShareInstances.Stage;

public class Docker : IDocker, IDisposable
{
    private IConfigure configure;

    public IList<IPlayer> Players {get; private set;}
    public IList<ICube> Cubes {get; private set;}


    public Docker(IConfigure _configure)
    {
        configure = _configure;
    }

    public void Dispose()
    {
        configure = null;
        Players = null;
        Cubes = null;
        GC.Collect();
    }

    public Task<int> Dock()
    {
        try
        {
            Players = DefaultDockProcess<IPlayer>(ref configure.ConfigSheet._playerSource,
                                             ref configure.ConfigSheet._players);
            Cubes = DefaultDockProcess<ICube>(ref configure.ConfigSheet._cubeSource,
                                            ref configure.ConfigSheet._cubes);
            return Task.FromResult(0);
        }
        catch
        {
            return Task.FromResult(-1);
        } 
    }
    
    private IList<T>? DefaultDockProcess<T>(ref string source,
                                          ref IEnumerable<string> assembliesPaths)
    {
        List<T> result;
        (bool, List<T>) components = LoadFromAssembly<T>(ref assembliesPaths);
        if(components.Item1 == true)
        {
            result = components.Item2;
            return result;
        }
        return null;
    }
  
    //upload components by refrelction
    private (bool, List<T>) LoadFromAssembly<T>(ref IEnumerable<string> dllsPath)
    {
        var list = new List<T>();
        try
        {
            foreach (string path in dllsPath)
            {
                if(File.Exists(path))
                {
                    var assembly = Assembly.LoadFrom(path);
                    var exportedTypes = assembly.ExportedTypes;
                    exportedTypes.Where(t => t.IsClass && t.GetInterfaces().Contains(typeof(T)))
                                .Select(t => t)
                                .ToList()
                                .ForEach(t => 
                                {
                                    T instance = (T)Activator.CreateInstance(t);
                                    list.Add(instance);
                                });
                    dllsPath.ToList().Remove(path);
                }
            }
            return (true, list);
        }
        catch
        {
            return (false, null);
        }
    }
}
