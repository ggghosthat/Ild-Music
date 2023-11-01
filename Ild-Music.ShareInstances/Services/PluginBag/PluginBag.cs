using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
namespace ShareInstances.Services.PluginBag;
public class PluginBag : IPluginBag
{
    private readonly IMediator _mediator;

    private IList<IPlayer> playerPlugins = new List<IPlayer>();
    private IList<ICube> cubePlugins = new List<ICube>();

    private int currentPlayerId = 0;
    private int currentCubeId = 0;

    public int CurrentPlayerId => currentPlayerId;
    public int CurrentCubeId => currentCubeId;

    public int PlayersCount => playerPlugins.Count();
    public int CubesCount => cubePlugins.Count();

    

    public PluginBag(IMediator mediator)
    {
        _mediator = mediator;
    }


    public void AddPlayerPlugin(IPlayer newPlayer)
    {
        if(playerPlugins.Where(p => p.PlayerId.Equals(newPlayer.PlayerId) ).Count() == 0)
        {
            newPlayer.ConnectMediator(_mediator);
            playerPlugins.Add(newPlayer);
        }
    }

    public void AddCubePlugin(ICube newCube)
    {
        if(cubePlugins.Where(c => c.CubeId.Equals(newCube.CubeId) ).Count() == 0)
        {
            newCube.ConnectMediator(_mediator);
            cubePlugins.Add(newCube);
        }
    }

    public async Task AddPlayerPluginsAsync(IEnumerable<IPlayer> players)
    {
        foreach(IPlayer player in players)
        {
            player.ConnectMediator(_mediator);
            playerPlugins.Add(player);
        }
    }
    
    public async Task AddCubePluginsAsync(IEnumerable<ICube> cubes)
    {
        foreach(ICube cube in cubes)
        {
            cube.ConnectMediator(_mediator);
            cubePlugins.Add(cube);
        }
    }



    public IEnumerable<IPlayer> GetPlayers()
    {
        return playerPlugins;
    }

    public IEnumerable<ICube> GetCubes()
    {
        return cubePlugins;
    }


    public IPlayer GetSinglePlayer(int index)
    {
        if(index < 0 && index >= playerPlugins.Count)
            return playerPlugins[0];

        return playerPlugins[index];
    }

    public ICube GetSingleCube(int index)
    {
        if(index < 0 && index >= cubePlugins.Count)
            return cubePlugins[0];

        return cubePlugins[index];

    }

    public IPlayer GetCurrentPlayer()
    {
        return playerPlugins[currentPlayerId];
    }

    public ICube GetCurrentCube()
    {
        return cubePlugins[currentCubeId];
    }


    public void SetCurrentPlayer(int newPlayerId)
    {
        if(newPlayerId >= 0 && newPlayerId < playerPlugins.Count)
            currentPlayerId = newPlayerId;
    }

    public void SetCurrentCube(int newCubeId)
    {
        if(newCubeId >= 0 && newCubeId < cubePlugins.Count)
            currentPlayerId = newCubeId;
    }

    public void SetCurrentPlayer(IPlayer newPlayer)
    {
        if(playerPlugins.Contains(newPlayer))
            currentPlayerId = playerPlugins.IndexOf(newPlayer);
    }

    public void SetCurrentCube(ICube newCube)
    {
        if(cubePlugins.Contains(newCube))
            currentCubeId = cubePlugins.IndexOf(newCube);
    }


    public void DeletePlayer(int playerId)
    {
        if(playerId >= 0 && playerId < playerPlugins.Count)
            playerPlugins.RemoveAt(playerId);
    }

    public void DeleteCube(int cubeId)
    {
        if(cubeId >= 0 && cubeId < cubePlugins.Count)
            cubePlugins.RemoveAt(cubeId);
    }


    public void ClearPlayers()
    {
        playerPlugins.Clear();        
    }

    public void ClearCubes()
    {
        cubePlugins.Clear();
    }
}
