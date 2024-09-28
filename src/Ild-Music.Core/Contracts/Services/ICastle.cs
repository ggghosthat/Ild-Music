namespace Ild_Music.Core.Contracts.Services.Interfaces;

public enum Ghosts {SUPPORT, FACTORY, PLAYER}

public interface ICastle
{
    public bool IsActive {get; set;}

    public void Pack();
    
    public void RegisterCube(ICube cube); 

    public void RegisterPlayer(IPlayer player);

    public IGhost? ResolveGhost(Ghosts ghostTag);

    public Task<IGhost?> ResolveGhostAsync(Ghosts ghostTag);
}
