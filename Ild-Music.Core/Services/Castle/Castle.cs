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

public class Castle : ICastle
{
    public bool IsCenterActive { get; set; } = false;

    private static IContainer container;

    private static IPluginBag _pluginBag;

    public Castle()
    {}

    public void Pack()
    {
        try
        {
            var builder = new ContainerBuilder();
        
            var configuration = MediatRConfigurationBuilder
                .Create(typeof(PlayerNotification).Assembly)
                .WithAllOpenGenericHandlerTypesRegistered()
                .WithRegistrationScope(RegistrationScope.Scoped) 
                .Build();

            builder.RegisterType<DelegateBag>().SingleInstance();
        
        
            builder.RegisterMediatR(configuration);

            builder.RegisterType<SupportGhost>().As<IGhost>().SingleInstance().Keyed<IGhost>(Ghosts.SUPPORT);
            builder.RegisterType<FactoryGhost>().As<IGhost>().SingleInstance().Keyed<IGhost>(Ghosts.FACTORY);
            builder.RegisterType<PlayerGhost>().As<IGhost>().SingleInstance().Keyed<IGhost>(Ghosts.PLAYER);
       
    
            builder.RegisterType<Filer>().As<IWaiter>().Named<IWaiter>("Filer");      
        
            container = builder.Build();

            using (var preScope = container.BeginLifetimeScope())
            {
                var mediator = preScope.Resolve<IMediator>();
                _pluginBag = new PluginBag.PluginBag(mediator);
            }

            IsCenterActive = true;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    #region resolve region
    //syncronous way to resolve ghost or waiter from IoC
    public IPluginBag ResolvePluginBag()
    { 
        return _pluginBag;
    }

    public IGhost ResolveGhost(Ghosts ghostTag)
    {
        IGhost ghost;
        using (var scope = container.BeginLifetimeScope()) 
        {
            ghost = scope.ResolveKeyed<IGhost>(ghostTag);
        }
        return ghost;
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



    public void RegisterCube(ICube cube)
    {
        _pluginBag?.AddCubePlugin(cube); 
    }

    public void RegisterPlayer(IPlayer player)
    {
        _pluginBag?.AddPlayerPlugin(player);
    }

    public async Task RegisterPlayers(ICollection<IPlayer> players)
    {
        await _pluginBag.AddPlayerPluginsAsync(players);
    }

    public async Task RegisterCubes(ICollection<ICube> cubes)
    {
        await _pluginBag.AddCubePluginsAsync(cubes); 
    }



    public Task<IEnumerable<IPlayer>> GetPlayersAsync()
    {
        return Task.FromResult(_pluginBag.GetPlayers());
    }

    public Task<IEnumerable<ICube>> GetCubesAsync()
    {
        return Task.FromResult(_pluginBag.GetCubes());
    }




    public void SwitchPlayer(int playerId)
    {
        _pluginBag.SetCurrentPlayer(playerId); 
    }

    public void SwitchCube(int cubeId)
    {
        _pluginBag.SetCurrentCube(cubeId);
    }

    public void SwitchPlayer(IPlayer playerInstance)
    {
        _pluginBag.SetCurrentPlayer(playerInstance); 
    }

    public void SwitchCube(ICube cubeInstance)
    {
        _pluginBag.SetCurrentCube(cubeInstance);
    }
    #endregion
}
