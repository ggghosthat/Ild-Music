using Ild_Music.Core.Contracts;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Events;
using Ild_Music.Core.Contracts.Services.Interfaces;

using Autofac;

namespace Ild_Music.Core.Services.Castle;

public sealed class ScopeCastle : ICastle, IDisposable
{
    //live state indicator
    public bool IsActive { get; set; } = false;

    //IoC container
    private static ContainerBuilder builder = new ContainerBuilder();
    private static IContainer container;

    private static IDictionary<Ghosts, IGhost> ghosts = new Dictionary<Ghosts, IGhost>();
    private static IDictionary<string, IWaiter> waiters = new Dictionary<string, IWaiter>();

    //available components
    private static IEnumerable<IPlayer> availlablePlayers;
    private static IEnumerable<ICube> availlableCubes;

    //current components
    private static int currentPlayerId;    
    private static int currentCubeId;

    private readonly static string cubeStoragePath = Environment.CurrentDirectory;
    private readonly int cubeCapacity = 300;
    private readonly bool cubeIsMoveTrackFiles = true;

    public ScopeCastle()
    {}

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
       if(container.IsRegisteredWithKey<ICube>(currentCubeId))
       {
           using (var preScope = container.BeginLifetimeScope())
           {
               var currentCube = container.ResolveKeyed<ICube>(currentCubeId);
               currentCube.Init(cubeStoragePath, cubeIsMoveTrackFiles);               
               var eventBag = preScope.Resolve<IEventBag>();
               currentCube.InjectEventBag(eventBag);
               var supportGhost = new SupportGhost();
               var factoryGhost = new FactoryGhost();
               supportGhost.Init(currentCube);
               factoryGhost.Init(currentCube);

               ghosts[Ghosts.SUPPORT] = supportGhost;
               ghosts[Ghosts.FACTORY] = factoryGhost;
               var filer = new Filer();
               filer.WakeUp(factoryGhost);
               waiters["Filer"] = filer;
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

    public void RegisterCube(ICube cube)
    {
        if(IsActive) 
            throw new Exception();

        currentCubeId = cube.GetHashCode();
        builder.RegisterInstance<ICube>(cube)
            .SingleInstance()
            .Keyed<ICube>(cube.GetHashCode()); 
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

    public async Task RegisterCubes(ICollection<ICube> cubes)
    {
        if ((cubes is null) || (cubes.Count == 0))
            return;

        if (IsActive) 
            throw new Exception();

        currentCubeId = cubes.Last().GetHashCode();
        foreach (var cube in cubes)
        {
            builder.RegisterInstance<ICube>(cube)
                .SingleInstance()
                .Keyed<ICube>(cube.GetHashCode());
        }    
    }

    //resolve ghosts sychronously and asynchronously
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


    //waiter resolve methods (synchronously and asynchronously)
    public IWaiter ResolveWaiter(string waiterTag)
    {
        if(!IsActive) 
            throw new Exception();

        return waiters[waiterTag];
    }

    public Task<IWaiter> ResolveWaiterAsync(string waiterTag)
    {
        if(!IsActive) 
            throw new Exception();

        return Task.FromResult(waiters[waiterTag]);
    }
   
    //return current component instances
    public IPlayer? GetCurrentPlayer()
    {
        if(!IsActive) 
            throw new Exception();

        IPlayer player;
        using (var scope = container.BeginLifetimeScope())
            player = scope.ResolveKeyed<IPlayer>(currentPlayerId);

        return player;
    }

    public ICube? GetCurrentCube()
    {
        if(!IsActive) 
            throw new Exception();

        ICube cube;
        using (var scope = container.BeginLifetimeScope())
            cube = scope.ResolveKeyed<ICube>(currentCubeId);

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
    public Task<IEnumerable<ICube>> GetCubesAsync()
    {
       if(!IsActive)
            throw new Exception();

        return Task.FromResult(container.Resolve<IEnumerable<ICube>>());
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
