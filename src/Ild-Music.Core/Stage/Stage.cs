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
    
    public IRepository? CubeInstance => castle.GetCurrentCube();

    public bool CompletionResult { get; private set; }

    public List<ErrorFlag> Errors { get; private set; } = [];

    public event Action OnInitialized;

    public event Action OnComponentMuted;

    public async Task Build()
    {
        try
        {
            CompletionResult = await DockComponents();

            if (!CompletionResult)
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

    private async Task<bool> DockComponents()
    {
        bool isCompleted;
       
        using (var docker = new Docker(Configure))
        {
            var dock = await docker.Dock();

            if(dock == 0)
            {
                await castle.RegisterPlayers(docker.Players);
                await castle.RegisterCubes(docker.Cubes);

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
        castle.GetCubesAsync().Result;

    public IEventBag? GetEventBag() =>
        castle.GetEventBag().Result;

    public void SwitchPlayer(int playerId) =>
        castle.SwitchPlayer(playerId);
    
    public void SwitchCube(int cubeId) =>
        castle.SwitchCube(cubeId);

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
