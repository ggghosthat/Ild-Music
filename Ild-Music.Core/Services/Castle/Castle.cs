using Ild_Music.Core.Contracts;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Contracts.Services;
using Ild_Music.Core.CQRS.Notifications;
using Ild_Music.Core.CQRS.Handlers.Delegatebag;
using Ild_Music.Core.Contracts.Services.Interfaces;

using Autofac;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;

namespace Ild_Music.Core.Services.Castle;

public sealed class Castle : ICastle
{
    //live state indicator
    public bool IsActive { get; set; } = false;

    //IoC container
    private static ContainerBuilder builder = new ContainerBuilder();
    private static IContainer container;

    private static IEnumerable<IPlayer> availlablePlayers;
    private static IEnumerable<ICube> availlableCubes;
    //current components
    private static Guid currentPlayerId;
    
    private static Guid currentCubeId;

    public Castle()
    {}

    public void Pack()
    {
        try
        {

            Console.WriteLine(1);
            builder.RegisterType<DelegateBag>()
                   .SingleInstance();


            var configuration = MediatRConfigurationBuilder
                .Create(typeof(PlayerNotification).Assembly)
                .WithAllOpenGenericHandlerTypesRegistered()
                .WithRegistrationScope(RegistrationScope.Scoped) 
                .Build();
        
            Console.WriteLine(2);
            builder.RegisterMediatR(configuration);


            Console.WriteLine(3);
            builder.Register((c,p) => 
                    new SupportGhost())
                    .As<IGhost>()
                    .SingleInstance()
                    .Keyed<IGhost>(Ghosts.SUPPORT);

            Console.WriteLine(4);
            builder.Register((c,p) => 
                    new PlayerGhost())
                    .As<IGhost>()
                    .SingleInstance()
                    .Keyed<IGhost>(Ghosts.PLAYER);

            Console.WriteLine(5);
            builder.Register((c, p) =>
                    new FactoryGhost())
                   .As<IGhost>()
                   .SingleInstance()
                   .Keyed<IGhost>(Ghosts.FACTORY);
       
    
            Console.WriteLine(6);
            builder.RegisterType<Filer>()
                   .As<IWaiter>()
                   .SingleInstance()
                   .Named<IWaiter>("Filer");      
        

            //if(availlablePlayers is not null && 
            //    availlablePlayers.Count() > 0)
            //{
            //    foreach(var player in availlablePlayers)
            //    {
            //        builder.RegisterInstance<IPlayer>(player)
            //    }
            //}


            Console.WriteLine(7);
            container = builder.Build();

            Console.WriteLine(8);
            using (var preScope = container.BeginLifetimeScope())
            {
                var mediator = preScope.Resolve<IMediator>();
                //_pluginBag = new PluginBag.PluginBag(mediator);
            }

            Console.WriteLine(9);
            IsActive = true;
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

    #region Components registration region

    public void RegisterPlayer(IPlayer player)
    {
        if(!IsActive)
        {
            currentPlayerId = player.PlayerId;

            builder.RegisterInstance<IPlayer>(player)
                   .SingleInstance()
                   .Keyed<IPlayer>(player.PlayerId);
        }
    }

    public void RegisterCube(ICube cube)
    {
        if(!IsActive)
        {
            currentCubeId = cube.CubeId;

            builder.RegisterInstance<ICube>(cube)
                   .SingleInstance()
                   .Keyed<ICube>(cube.CubeId);
        }
    }


    public async Task RegisterPlayers(ICollection<IPlayer> players)
    {
        if(!IsActive)
        {
            currentPlayerId = players.Last().PlayerId;

            foreach (var player in players)
            {
                builder.RegisterInstance<IPlayer>(player)
                       .SingleInstance()
                       .Keyed<IPlayer>(player.PlayerId);
            }

        }
    }

    public async Task RegisterCubes(ICollection<ICube> cubes)
    {
        if(!IsActive)
        {
            currentCubeId = cubes.Last().CubeId;

            foreach (var cube in cubes)
            {
                builder.RegisterInstance<ICube>(cube)
                       .SingleInstance()
                       .Keyed<ICube>(cube.CubeId);

                Console.WriteLine(cube.CubeId);
            }

            Console.WriteLine($"Id: {currentCubeId}");
        }    
    }
    #endregion


    #region resolve region
    //syncronous way to resolve ghost or waiter from IoC

    public IGhost ResolveGhost(Ghosts ghostTag)
    {
        IGhost? resultGhost = null;
        Console.WriteLine(container is null);
        using (var scope = container.BeginLifetimeScope()) 
        {
            var rawGhost = scope.ResolveKeyed<IGhost>(ghostTag);
            
            //!!!remove!!!
            Console.WriteLine(scope.IsRegistered<ICube>());

            if(rawGhost is SupportGhost supportGhost)
            {
                var cube = scope.ResolveKeyed<ICube>(currentCubeId);
                supportGhost.Init(cube);
                resultGhost = supportGhost;
            }
            else if (rawGhost is PlayerGhost playerGhost)
            {
                var player = scope.ResolveKeyed<IPlayer>(currentPlayerId);
                playerGhost.Init(player);
                resultGhost = playerGhost;
            }
            else if(rawGhost is FactoryGhost factoryGhost)
            {
                var cube = scope.ResolveKeyed<ICube>(currentCubeId);
                factoryGhost.Init(cube);
                resultGhost = factoryGhost;
            }
            
        }
        return resultGhost;
    }

    public IWaiter ResolveWaiter(ref string waiterTag)
    {
        return container.ResolveNamed<IWaiter>(waiterTag);
    }

    //asyncronous way to resolve ghost or waiter from castle IoC
    public Task<IGhost> ResolveGhostAsync(Ghosts ghostTag)
    {
       var ghost = container.ResolveKeyed<IGhost>(ghostTag);
       return Task.FromResult(ghost);
    }

    public Task<IWaiter> ResolveWaiterAsync(ref string waiterTag)
    {
        var waiter = container.ResolveNamed<IWaiter>(waiterTag);
        return Task.FromResult(waiter);
    }
    
    //reolve all players from IoC
    public Task<IEnumerable<IPlayer>> GetPlayersAsync()
    {
        if(IsActive)
            return Task.FromResult(container.Resolve<IEnumerable<IPlayer>>());
        
        return Task.FromResult(Enumerable.Empty<IPlayer>());
    }

    //resolve all cube from IoC
    public Task<IEnumerable<ICube>> GetCubesAsync()
    {
        if(IsActive)
            return Task.FromResult(container.Resolve<IEnumerable<ICube>>());
        
        return Task.FromResult(Enumerable.Empty<ICube>());
    }
    #endregion



    #region Switch region
    public void SwitchPlayer(Guid newPlayerId)
    {
       currentPlayerId = newPlayerId; 
    }

    public void SwitchCube(Guid newCubeId)
    {
        currentCubeId = newCubeId;
    }
    #endregion
}
