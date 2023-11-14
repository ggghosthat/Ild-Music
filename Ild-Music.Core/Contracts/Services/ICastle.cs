namespace Ild_Music.Core.Contracts.Services.Interfaces;

public enum Ghosts {SUPPORT, FACTORY, PLAYER}

public interface ICastle
{
    public bool IsCenterActive {get; set;}

    public void Pack();
    
    public void RegisterCube(ICube cube); 

    public void RegisterPlayer(IPlayer player);

    public IGhost ResolveGhost(Ghosts ghostTag);

    public IWaiter ResolveWaiter(ref string waiterTag);

    public Task<IGhost> ResolveGhostAsync(Ghosts ghostTag);

    public Task<IWaiter> ResolveWaiterAsync(ref string waiterTag);
}
