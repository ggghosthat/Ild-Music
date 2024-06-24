using Ild_Music.Core.Contracts;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Services.Castle;

namespace Ild_Music.Core.Stage;

public sealed class Stage 
{
    private static ScopeCastle castle = new();

    public Stage(){}
    
    public Stage(ref IConfigure configure)
    {
        Configure = configure;
    }

    public IConfigure Configure {get; set;}

    public IPlayer PlayerInstance => castle.GetCurrentPlayer(); //castle.ResolvePluginBag().GetCurrentPlayer() ?? null;
    public ICube CubeInstance => castle.GetCurrentCube(); //castle.ResolvePluginBag().GetCurrentCube() ?? null;

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
            Console.WriteLine(ex.Message);
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

    public IEnumerable<IPlayer> GetPlayers()
    {
        return castle.GetPlayersAsync().Result;
    }

    public IEnumerable<ICube> GetCubes()
    {
        return castle.GetCubesAsync().Result;
    }

    public void SwitchPlayer(int playerId)
    {
        castle.SwitchPlayer(playerId);
    }

    public void SwitchCube(int cubeId)
    {
        castle.SwitchCube(cubeId);
    }

    public IGhost GetGhost(Ghosts ghostTag)
    {
        return castle.ResolveGhost(ghostTag);
    }

    public IWaiter GetWaiter(ref string waiterName)
    {
        return castle.ResolveWaiter(ref waiterName);
    }

    public void Clear()
    {
    }
}
