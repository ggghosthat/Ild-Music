using Ild_Music.Core.Contracts;
using Ild_Music.Core.Exceptions.Flag;
using System.Reflection;

namespace Ild_Music.Core.Stage;

public class Docker : IDocker, IDisposable
{
    private List<ErrorFlag> _errors = [];

    public Docker(IConfigure _configure)
    {
        configure = _configure;
    }

    private IConfigure configure;

    public IList<IPlayer> Players {get; private set;}

    public IList<IRepository> Cubes {get; private set;}

    public IList<ErrorFlag> Errors =>_errors;
    
    public ValueTask<int> Dock()
    {
        Players = DefaultDockProcess<IPlayer>(ref configure.ConfigSheet._players);
        Cubes = DefaultDockProcess<IRepository>(ref configure.ConfigSheet._repositories);
        
        return (_errors.Count == 0)
            ? ValueTask.FromResult(0)
            : ValueTask.FromResult(1);
    }
    
    private IList<T> DefaultDockProcess<T>(ref IEnumerable<string> assembliesPaths)
    {
        var assemblies = assembliesPaths.Where(p => File.Exists(p)).Select(p => p);
        return LoadFromAssembly<T>(assemblies);
    }
  
    private List<T> LoadFromAssembly<T>(IEnumerable<string> paths)
    {
        T instance;
        var list = new List<T>();
        
        foreach (string path in paths)
        {
            if (TryLoadInstance<T>(path, out instance))
                list.Add(instance);
             
        }
        return list;
        
    }

    private bool TryLoadInstance<T>(string path, out T instance)
    {
        bool result;
        try
        {
            var assembly = Assembly.LoadFrom(path);
            var exportedTypes = assembly.ExportedTypes;
            instance = exportedTypes
                .Where(t => t.IsClass && t.GetInterfaces().Contains(typeof(T)))
                .Select(t => (T)Activator.CreateInstance(t))
                .First();
            result = instance != null;
        }
        catch(Exception ex)
        {
            _errors.Add(new ErrorFlag("component docker", "instance-scan", $"could not find desired instance in assembly with {path} path"));
            instance = default;
            result = false;
        }

        return result;
    }

    public void Dispose()
    {
        configure = null;
        Players = null;
        Cubes = null;
        _errors.Clear();
        GC.Collect();
    }
}
