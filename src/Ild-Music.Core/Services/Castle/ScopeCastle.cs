using Ild_Music.Core.Contracts;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Events;
using Ild_Music.Core.Contracts.Services.Interfaces;

using Autofac;
using Ild_Music.Core.Helpers;
using Ild_Music.Core.Contracts.Plagination;
using System.Collections.ObjectModel;

namespace Ild_Music.Core.Services.Castle;

public sealed class ScopeCastle : ICastle, IDisposable
{
    private static ContainerBuilder builder = new ();
    private static IContainer container;

    private static Dictionary<Ghosts, IGhost> ghosts = [];

    private static Dictionary<PlaginationTag, List<PlugFunction>> funcs = [];

    private static IEnumerable<IPlayer> availlablePlayers;
    private static IEnumerable<IRepository> availlableCubes;

    private static int currentPlayerId;    
    private static int currentCubeId;

    private readonly static string cubeStoragePath = Environment.CurrentDirectory;
    private readonly bool cubeIsMoveTrackFiles = true;

    public ScopeCastle()
    {}
    
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
            SupplyRepository();
            SupplyPlayer();
            
            IsActive = true;
        }
        catch(Exception ex)
        {
            throw ex;
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

    private void SupplyRepository()
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

    private void ExtendFunctionalityWithPlugin(IDictionary<PlaginationTag, IList<PlugFunction>> pluginFunctionality)
    {
        pluginFunctionality.ToList()
            .ForEach(func => 
            {
                 var tag = func.Key;

                if (!funcs.ContainsKey(tag))
                    funcs[tag] = new List<PlugFunction>();
                
                funcs[tag].AddRange(func.Value);
            });
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

    public void RegisterRepository(IRepository cube)
    {
        if(IsActive) 
            throw new Exception();

        currentCubeId = cube.GetHashCode();
        builder.RegisterInstance<IRepository>(cube)
            .SingleInstance()
            .Keyed<IRepository>(cube.GetHashCode()); 
    }

    public void RegisterPlugin(IPlugin plugin)
    {
        ExtendFunctionalityWithPlugin(plugin.PluginFuncs);

        builder.RegisterInstance<IPlugin>(plugin)
            .SingleInstance()
            .Named<IPlugin>(plugin.PluginName);
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

    public async Task RegisterRepositories(ICollection<IRepository> cubes)
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

    public async Task RegisterPlugins(IEnumerable<IPlugin> plugins)
    {
        foreach (var plugin in plugins)
        {
            builder.RegisterInstance<IPlugin>(plugin)
                .SingleInstance()
                .Named<IPlugin>(plugin.PluginName);
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

    public IRepository? GetCurrentRepository()
    {
        if(!IsActive) 
            throw new Exception();

        IRepository cube;
        using (var scope = container.BeginLifetimeScope())
            cube = scope.ResolveKeyed<IRepository>(currentCubeId);

        return cube;
    }

    public IPlugin? GetPlugin(string pluginName)
    {
        IPlugin plugin;
        using (var scope = container.BeginLifetimeScope())
            plugin = scope.ResolveNamed<IPlugin>(pluginName);

        return plugin;
    }

    public Task<IEnumerable<IPlayer>> GetPlayersAsync()
    {
        if(!IsActive) 
            throw new Exception();

        return Task.FromResult(container.Resolve<IEnumerable<IPlayer>>());
    }

    public Task<IEnumerable<IRepository>> GetRepositoriesAsync()
    {
        if(!IsActive)
            throw new Exception();

        return Task.FromResult(container.Resolve<IEnumerable<IRepository>>());
    }

    public Task<IEnumerable<IPlugin>> GetPluginsAsync()
    {
        return Task.FromResult(container.Resolve<IEnumerable<IPlugin>>());
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

    public void SwitchRepository(int newCubeId)
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
