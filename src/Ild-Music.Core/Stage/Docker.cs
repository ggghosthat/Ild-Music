using Ild_Music.Core.Contracts;
using Ild_Music.Core.Exceptions.Flag;
using System.Net;
using System.Reflection;

namespace Ild_Music.Core.Stage;

public class Docker : IDocker, IDisposable
{
    private List<ErrorFlag> _errors = [];

    private List<ErrorFlag> _pluginLoadErrors = [];

    public Docker(IConfigure _configure)
    {
        configure = _configure;
    }

    private IConfigure configure;

    public IList<IPlayer> Players {get; private set;}

    public IList<IRepository> Repositories {get; private set;}

    public IList<ErrorFlag> Errors =>_errors;

    public IList<ErrorFlag> PluginLoadErrors => _pluginLoadErrors;
    
    public ValueTask<int> Dock()
    {
        Players = DefaultDockProcess<IPlayer>(configure.ConfigSheet._players).ToList();
        Repositories = DefaultDockProcess<IRepository>(configure.ConfigSheet._repositories).ToList();
        
        return (_errors.Count == 0)
            ? ValueTask.FromResult(0)
            : ValueTask.FromResult(1);
    }
    
    public Task<IEnumerable<IPlugin>> DockPlugins(IEnumerable<string> pluginPaths)
    {
        var plugins = new List<IPlugin>();
        
        try
        {
            foreach (string path in pluginPaths)
            {
                if (!File.Exists(path))
                    continue;

                var plugin = LoadInstance<IPlugin>(path);
                plugins.Add(plugin);
            }
        }
        catch(Exception ex)
        {
            _pluginLoadErrors.Add(new ErrorFlag("component docker", "plugin-scan", ex.Message));
        }

        return Task.FromResult<IEnumerable<IPlugin>>(plugins);
    }

    private IEnumerable<T> DefaultDockProcess<T>(IEnumerable<string> assembliesPaths) where T : class
    {
        var dockedResult = new List<T>();
        
        try
        {
            foreach (string path in assembliesPaths)
            {
                if (!File.Exists(path))
                    continue;

                var instance = LoadInstance<T>(path);
                dockedResult.Add(instance);
            }
        }
        catch(Exception ex)
        {
            _errors.Add(new ErrorFlag("component docker", "component-scan", ex.Message));
        }
        
        return dockedResult;
    }
  
    private T LoadInstance<T>(string path) where T : class
    {
        var exportedTypes = Assembly.LoadFrom(path).ExportedTypes;
        return exportedTypes
            .Where(t => t.IsClass && t.GetInterfaces().Contains(typeof(T)))
            .Select(t => (T)Activator.CreateInstance(t)).First();
    }

    public void Dispose()
    {
        configure = null;
        Players = null;
        Repositories = null;
        _errors.Clear();
        GC.Collect();
    }
}