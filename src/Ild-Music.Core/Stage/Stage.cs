using Ild_Music.Core.Contracts;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Events;
using Ild_Music.Core.Exceptions.Flag;
using Ild_Music.Core.Instances.Filing;
using Ild_Music.Core.Services.Castle;

namespace Ild_Music.Core.Stage;

public sealed class Stage : IErrorTracable
{
    private static ScopeCastle castle = new();
    
    public Stage(IConfigure configure)
    {
        Configure = configure;
        SuppliedExtensions.SupplySingleExtension(".mp3");
    }

    public IConfigure Configure {get; private set;}

    public IPlayer? PlayerInstance => castle.GetCurrentPlayer();
    
    public IRepository? CubeInstance => castle.GetCurrentRepository();

    public bool ComponentsCompletionResult { get; private set; }

    public bool PluginsCompletionResult { get; private set; }

    public List<ErrorFlag> Errors { get; private set; } = [];

    public event Action OnInitialized;

    public event Action OnComponentMuted;

    public async Task Build()
    {
        try
        {
            ComponentsCompletionResult = await LoadComponents();
            PluginsCompletionResult = await LoadPlugins();

            if (!ComponentsCompletionResult)
                throw new Exception("Could not upload configured components.");
            
            castle.Pack();
            OnInitialized?.Invoke();            
        }
        catch (Exception ex) 
        {
            var error = new ErrorFlag("stage", "build", ex.Message);
            Errors.Add(error);
        }
    }       

    private async Task<bool> LoadComponents()
    {
        bool isCompleted;
       
        using (var docker = new Docker(Configure))
        {
            var dock = await docker.Dock();

            if (dock == 0)
            {
                await castle.RegisterPlayers(docker.Players);
                await castle.RegisterRepositories(docker.Repositories);

                isCompleted = true;
            }
            else
            { 
                foreach (var err in docker.Errors)
                    Errors.Add(err);

                isCompleted = false;
            }
        }        
       
        return isCompleted;
    }
    
    public async Task<bool> LoadPlugins()
    {
        bool isCompleted;

        using (var docker = new Docker(Configure))
        {
            var plugins = docker.DockPlugins(Configure.ConfigSheet.Plugins).Result;

            if (plugins.Count() > 0)
            {
                await castle.RegisterPlugins(plugins);
                isCompleted = true;
            }
            else
            { 
                foreach (var err in docker.Errors)
                    Errors.Add(err);

                isCompleted = false;
            }
        }

        return isCompleted;
    }

    public IEnumerable<IPlayer> GetPlayers() =>
        castle.GetPlayersAsync().Result;

    public IEnumerable<IRepository> GetCubes() =>
        castle.GetRepositoriesAsync().Result;

    public IEnumerable<IPlugin> GetPlugins() =>
        castle.GetPluginsAsync().Result;

    public IEventBag? GetEventBag() =>
        castle.GetEventBag().Result;

    public void SwitchPlayer(int playerId) =>
        castle.SwitchPlayer(playerId);
    
    public void SwitchCube(int cubeId) =>
        castle.SwitchRepository(cubeId);

    public IGhost? GetGhost(Ghosts ghostTag) =>
        castle.ResolveGhost(ghostTag);

    public bool CheckErrors(List<ErrorFlag> errorList)
    {
        bool result = false;

        if (Errors.Count > 0)
        {
            errorList.AddRange(Errors);
            Errors.Clear();
            result = true;
        }

        return result;
    }
}
