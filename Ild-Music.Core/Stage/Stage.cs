using Ild_Music.Core.Events;
using Ild_Music.Core.Contracts;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Services.Castle;

namespace Ild_Music.Core.Stage;

public sealed class Stage 
{
    private static ScopeCastle castle = new();
    
    public Stage(IConfigure configure)
    {
        Configure = configure;
    }

    public IConfigure Configure {get; private set;}

    public IPlayer? PlayerInstance => castle.GetCurrentPlayer();
    public ICube? CubeInstance => castle.GetCurrentCube();

    public bool CompletionResult {get; private set;}

    public event Action OnInitialized;
    public event Action OnComponentMuted;

    public async Task Build()
    {
        try 
        {
            CompletionResult = await DockComponents();
            castle.Pack();
            OnInitialized?.Invoke();
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }       

    private async Task<bool> DockComponents()
    {
        bool isCompleted = false;
        try
        {
            using (var docker = new Docker(Configure))
            {
                var dock = await docker.Dock();

                if(dock == 0)
                {
                    await castle.RegisterPlayers(docker.Players);
                    await castle.RegisterCubes(docker.Cubes);
                }
                else if(dock == -1)
                {
                    throw new Exception("Could not load defined components");
                }
            }

            isCompleted = true;
        }
        catch(Exception ex)
        {
            throw ex;
        }
       
        return isCompleted;
    }

    public IEnumerable<IPlayer> GetPlayers() =>
        castle.GetPlayersAsync().Result;

    public IEnumerable<ICube> GetCubes() =>
        castle.GetCubesAsync().Result;

    public IEventBag? GetEventBag() =>
        castle.GetEventBag().Result;

    public void SwitchPlayer(int playerId) =>
        castle.SwitchPlayer(playerId);
    
    public void SwitchCube(int cubeId) =>
        castle.SwitchCube(cubeId);

    public IGhost? GetGhost(Ghosts ghostTag) =>
        castle.ResolveGhost(ghostTag);

    public IWaiter GetWaiter(string waiterName) =>
        castle.ResolveWaiter(waiterName);
}
