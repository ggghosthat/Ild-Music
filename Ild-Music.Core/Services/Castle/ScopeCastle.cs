using Ild_Music.Core.Contracts;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Events.Notifications;
using Ild_Music.Core.Contracts.Services.Interfaces;

using Autofac;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;

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

    //cube literals (damn!!!, trash 'em out ＼(｀0´)／)
    private readonly static string cubeStoragePath = Path.Combine(Environment.CurrentDirectory, "storage.db");
    private readonly int cubeCapacity = 300;

    public ScopeCastle()
    {}


    public void Pack()
    {
        try
        {
            //CQRS mediator registration
            var configuration = MediatRConfigurationBuilder
                .Create(typeof(PlayerNotification).Assembly)
                .WithAllOpenGenericHandlerTypesRegistered()
                .WithRegistrationScope(RegistrationScope.Scoped) 
                .Build();
            
            //add mediator for CQRS things
            builder.RegisterMediatR(configuration);
        
            //Building container
            container = builder.Build();
           
            //supplying mediator to components for CQRS
            SupplyMediatR<IPlayer>();
            SupplyMediatR<ICube>();
          
            //supplying components for ghosts
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
               currentCube.Init(cubeStoragePath, cubeCapacity);
               var supportGhost = new SupportGhost();
               var factoryGhost = new FactoryGhost();

               supportGhost.Init(currentCube);
               factoryGhost.Init(currentCube);

               ghosts[Ghosts.SUPPORT] = supportGhost;
               ghosts[Ghosts.FACTORY] = factoryGhost;

               waiters["Filer"] = new Filer();
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
               var playerGhost = new PlayerGhost();
               playerGhost.Init(currentPlayer);

               ghosts[Ghosts.PLAYER] = playerGhost;
           }
       } 
    }

    //Resolve Mediaor dependecy for component objects (mean IPlayer, ICube, etc
    //Desired type must contain "ConnectMediator" method with single "IMediator" param
    private void SupplyMediatR<T>() where T : IShare
    {
       if(container.IsRegistered<T>())
       {
           using (var preScope = container.BeginLifetimeScope())
           {
               var mediator = preScope.Resolve<IMediator>();
               var desiredObjects = preScope.Resolve<IEnumerable<T>>();

               foreach (var obj in desiredObjects)
                   obj.ConnectMediator(mediator);
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
        if(IsActive) 
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
        if(IsActive) 
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
        if(!IsActive) 
            throw new Exception();

        IGhost? resultGhost = ghosts[ghostTag];
        return resultGhost;
    }


    public Task<IGhost?> ResolveGhostAsync(Ghosts ghostTag)
    {
        if(!IsActive) 
            throw new Exception();

        IGhost? resultGhost = ghosts[ghostTag];
        return Task.FromResult(resultGhost);
    }


    //waiter resolve methods (synchronously and asynchronously)
    public IWaiter ResolveWaiter(ref string waiterTag)
    {
        if(!IsActive) 
            throw new Exception();

        IWaiter waiter = waiters[waiterTag];
        return waiter;
    }

    public Task<IWaiter> ResolveWaiterAsync(ref string waiterTag)
    {
        if(!IsActive) 
            throw new Exception();

        IWaiter waiter = waiters[waiterTag];
        return Task.FromResult(waiter);
    }
   
    //return current component instances
    public IPlayer? GetCurrentPlayer()
    {
        if(!IsActive) 
            throw new Exception();

        IPlayer player;
        using (var scope = container.BeginLifetimeScope())
        {
            player = scope.ResolveKeyed<IPlayer>(currentPlayerId);
        }

        return player;
    }

    public ICube GetCurrentCube()
    {
        if(!IsActive) 
            throw new Exception();

        ICube cube;
        using (var scope = container.BeginLifetimeScope())
        {
            cube = scope.ResolveKeyed<ICube>(currentCubeId);
        }

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
