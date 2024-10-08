using Ild_Music.Core.Contracts;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Events;
using Ild_Music.Core.Contracts.Services.Interfaces;

using Autofac;
using Ild_Music.Core.Helpers;

namespace Ild_Music.Core.Services.Castle;

public sealed class ScopeCastle : ICastle, IDisposable
{

    //IoC container
    private static ContainerBuilder builder = new ContainerBuilder();
    private static IContainer container;

    private static IDictionary<Ghosts, IGhost> ghosts = new Dictionary<Ghosts, IGhost>();

    //available components
    private static IEnumerable<IPlayer> availlablePlayers;
    private static IEnumerable<IRepository> availlableCubes;

    private static IEnumerable<string> allowedTrackFileExtensions;

    //current components
    private static int currentPlayerId;    
    private static int currentCubeId;

    private readonly static string cubeStoragePath = Environment.CurrentDirectory;
    private readonly bool cubeIsMoveTrackFiles = true;

    public ScopeCastle()
    {}
    
    //live state indicator
    public bool IsActive { get; set; } = false;

    public void Pack()
    {
        try
        {  
            var eventBag = new EventBag();
            builder.RegisterInstance<IEventBag>(eventBag);

            //Building container
            container = builder.Build();
                     
            //ghosts initialization
            SupplyCube();
            SupplyPlayer();
            
            IsActive = true;
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

    private void SupplyCube()
    {
        if(container.IsRegisteredWithKey<IRepository>(currentCubeId))
        {
            using (var preScope = container.BeginLifetimeScope())
            {
                var currentCube = container.ResolveKeyed<IRepository>(currentCubeId);
                currentCube.Init(cubeStoragePath, cubeIsMoveTrackFiles);               
                
                var eventBag = preScope.Resolve<IEventBag>();
                currentCube.InjectEventBag(eventBag);

                var supportGhost = new SupportGhost();
                var factoryGhost = new FactoryGhost();
                supportGhost.Init(currentCube);
                factoryGhost.Init(currentCube);

                ghosts[Ghosts.SUPPORT] = supportGhost;
                ghosts[Ghosts.FACTORY] = factoryGhost;

                FileHelper.SetFactoryGhost(factoryGhost);
            }
        } 
    }

    private void SupplyPlayer()
    {
        if(container.IsRegisteredWithKey<IPlayer>(currentPlayerId))
        {
            using (var preScope = container.BeginLifetimeScope())
            {
                var currentPlayer = preScope.ResolveKeyed<IPlayer>(currentPlayerId);
                
                var eventBag = preScope.Resolve<IEventBag>();
                currentPlayer.InjectEventBag(eventBag);

                var mimeTypes = currentPlayer.GetSupportedMimeTypes().Result;
                FileHelper.SetMimeTypes(mimeTypes);

                var playerGhost = new PlayerGhost();
                playerGhost.Init(currentPlayer);
                ghosts[Ghosts.PLAYER] = playerGhost;
            }
        } 
    }

    public void RegisterPlayer(IPlayer player)
    {
        if(IsActive) 
            throw new Exception();

        currentPlayerId = player.GetHashCode();
        builder.RegisterInstance<IPlayer>(player)
            .SingleInstance()
            .Keyed<IPlayer>(player.GetHashCode());
    }

    public void RegisterCube(IRepository cube)
    {
        if(IsActive) 
            throw new Exception();

        currentCubeId = cube.GetHashCode();
        builder.RegisterInstance<IRepository>(cube)
            .SingleInstance()
            .Keyed<IRepository>(cube.GetHashCode()); 
    }

    public async Task RegisterPlayers(ICollection<IPlayer> players)
    {
        if ((players is null) || (players.Count == 0))
            return;

        if (IsActive) 
            throw new Exception();

        currentPlayerId = players.Last().GetHashCode();
        foreach (var player in players)
        {
            builder.RegisterInstance<IPlayer>(player)
                .SingleInstance()
                .Keyed<IPlayer>(player.GetHashCode());
        }
    }

    public async Task RegisterCubes(ICollection<IRepository> cubes)
    {
        if ((cubes is null) || (cubes.Count == 0))
            return;

        if (IsActive) 
            throw new Exception();

        currentCubeId = cubes.Last().GetHashCode();
        foreach (var cube in cubes)
        {
            builder.RegisterInstance<IRepository>(cube)
                .SingleInstance()
                .Keyed<IRepository>(cube.GetHashCode());
        }    
    }

    public IGhost? ResolveGhost(Ghosts ghostTag)
    {
        if (!IsActive) 
            throw new Exception();

        if (!ghosts.ContainsKey(ghostTag))
            return null;

        return ghosts[ghostTag];
    }

    public Task<IGhost?> ResolveGhostAsync(Ghosts ghostTag)
    {
        if(!IsActive) 
            throw new Exception();

        return Task.FromResult(ghosts[ghostTag]);
    }

    public IPlayer? GetCurrentPlayer()
    {
        if(!IsActive) 
            throw new Exception();

        IPlayer player;
        using (var scope = container.BeginLifetimeScope())
            player = scope.ResolveKeyed<IPlayer>(currentPlayerId);

        return player;
    }

    public IRepository? GetCurrentCube()
    {
        if(!IsActive) 
            throw new Exception();

        IRepository cube;
        using (var scope = container.BeginLifetimeScope())
            cube = scope.ResolveKeyed<IRepository>(currentCubeId);

        return cube;
    }

    //reolve all players from IoC
    public Task<IEnumerable<IPlayer>> GetPlayersAsync()
    {
        if(!IsActive) 
            throw new Exception();

        return Task.FromResult(container.Resolve<IEnumerable<IPlayer>>());
    }

    //resolve all cube from IoC
    public Task<IEnumerable<IRepository>> GetCubesAsync()
    {
        if(!IsActive)
            throw new Exception();

        return Task.FromResult(container.Resolve<IEnumerable<IRepository>>());
    }

    public Task<IEventBag> GetEventBag()
    {
        if(!IsActive)
            throw new Exception();

        return Task.FromResult(container.Resolve<IEventBag>());
    }

    public void SwitchPlayer(int newPlayerId)
    {
        if(!IsActive)
            throw new Exception();

        if(container.IsRegisteredWithKey<int>(newPlayerId))
            currentPlayerId = newPlayerId; 
    }

    public void SwitchCube(int newCubeId)
    {
        if(!IsActive)
            throw new Exception();

        if(container.IsRegisteredWithKey<int>(newCubeId))
            currentCubeId = newCubeId;
    }

    public void Dispose()
    {
        IsActive = false;
        currentPlayerId = 0;
        currentCubeId = 0;
        availlableCubes = null;
        availlablePlayers = null;
        Task.Run(() => container.DisposeAsync());
    }
}
