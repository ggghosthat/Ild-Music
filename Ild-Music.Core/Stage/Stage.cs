using Ild_Music.Core.Contracts;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Services.Castle;
//using ShareInstances.Configure;
namespace Ild_Music.Core.Stage;
public sealed class Stage 
{
    #region Castle
    private static ScopeCastle castle = new();
    #endregion

    #region Configuration Region
    public IConfigure Configure {get; set;}
    #endregion

    #region Default Waiters
    //public static Filer Filer {get; private set;}
    #endregion
    
    #region Current Components
    public IPlayer PlayerInstance => castle.GetCurrentPlayer(); //castle.ResolvePluginBag().GetCurrentPlayer() ?? null;
    public ICube CubeInstance => castle.GetCurrentCube(); //castle.ResolvePluginBag().GetCurrentCube() ?? null;
    #endregion
        
    #region Tagging result
    public bool CompletionResult {get; private set;}

    public event Action OnInitialized;
    public event Action OnComponentMuted;
    #endregion

       

    #region Constructors
    public Stage(){}
    
    public Stage(ref IConfigure configure)
    {
        Configure = configure;
    }
    #endregion


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

    //TODO: this method is needed in more impored reliable solution
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
                    throw new Exception("Could not load all defined components");
                }
            }

            isCompleted = true;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            throw ex;
        }
       
        return isCompleted;
    }

    #region Instances lists retrieving
    public IEnumerable<IPlayer> GetPlayers()
    {
        return castle.GetPlayersAsync().Result;
    }

    public IEnumerable<ICube> GetCubes()
    {
        return castle.GetCubesAsync().Result;
    }
    #endregion

    #region Current instance Switching
    public void SwitchPlayer(int playerId)
    {
        castle.SwitchPlayer(playerId);
    }

    public void SwitchCube(int cubeId)
    {
        castle.SwitchCube(cubeId);
    }
    #endregion

    #region Service entities resolve
    public IGhost GetGhost(Ghosts ghostTag)
    {
        return castle.ResolveGhost(ghostTag);
    }

    public IWaiter GetWaiter(ref string waiterName)
    {
        return castle.ResolveWaiter(ref waiterName);
    }

    #endregion

    #region Clear
    public void Clear()
    {
    }
    #endregion    

}
