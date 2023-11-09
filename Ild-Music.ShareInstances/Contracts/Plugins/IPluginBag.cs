using System.Threading.Tasks;
using System.Collections.Generic;
namespace ShareInstances.Contracts.Services;
public interface IPluginBag 
{
    public int CurrentPlayerId {get;}
    public int CurrentCubeId {get;}

    public int PlayersCount {get;}
    public int CubesCount {get;}


    public void AddPlayerPlugin(IPlayer newPlayer);
    public void AddCubePlugin(ICube newCube);

    public Task AddPlayerPluginsAsync(IEnumerable<IPlayer> players);
    public Task AddCubePluginsAsync(IEnumerable<ICube> cubes);
   

    public IEnumerable<IPlayer> GetPlayers();
    public IEnumerable<ICube> GetCubes();

    public IPlayer GetSinglePlayer(int index);
    public ICube GetSingleCube(int index);

    public IPlayer GetCurrentPlayer();
    public ICube GetCurrentCube();


    public void SetCurrentPlayer(int newPlayerId);
    public void SetCurrentCube(int newCubeId);

    public void SetCurrentPlayer(IPlayer newPlayerId);
    public void SetCurrentCube(ICube newCubeId);

    public void DeletePlayer(int playerId);
    public void DeleteCube(int cubeId);

    public void ClearPlayers();
    public void ClearCubes();
}
