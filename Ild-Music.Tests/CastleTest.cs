using Ild_Music.Core.Services.Castle;
using Ild_Music.Core.Contracts;
using Ild_Music.Core.Configure;
using Ild_Music.Core.Stage;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Services.Entities;

using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
namespace Ild_Music.Tests;

public class CastleTest : IClassFixture<ScopeCastle>, IDisposable
{
    private readonly Configure configure = new("./config.json");
    private readonly ScopeCastle castle = new();
    
    private readonly ITestOutputHelper _output;

    public CastleTest(ITestOutputHelper output)
    {
        _output = output;
        Init().Wait();
        castle.Pack();
    }

    //[Fact]
    public void GhostResolvingTest()
    {
        var ghost = castle.ResolveGhost(Ghosts.SUPPORT);
        var ghost1 = castle.ResolveGhost(Ghosts.FACTORY);
        var ghost2 = castle.ResolveGhost(Ghosts.PLAYER);
            
        var cube = castle.GetCurrentCube();
        var player = castle.GetCurrentPlayer();

        Assert.NotNull(ghost);
        Assert.NotNull(ghost2);

        Assert.NotNull(cube);
        Assert.NotNull(player);

        Assert.IsAssignableFrom<ICube>(cube);
        Assert.IsAssignableFrom<IPlayer>(player);

        var support = (SupportGhost)ghost;
        var factory = (FactoryGhost)ghost1;

        support.AddArtistInstance(new Core.Instances.Artist(Guid.NewGuid(),
                                                             "Artist 123".AsMemory(),
                                                            "".AsMemory(),
                                                            new byte[0],
                                                            0));
        factory.CreateArtist("Dr. DRE", "Do you need a doctor?", 2000, new byte[0]);
    }
 
    

    #region pre-init methods
    private async Task Init()
    {
        using (var docker = new Docker(configure))
        {
            var dock = await docker.Dock();

            if(dock == 0)
            {
                await castle.RegisterPlayers(docker.Players);
                await castle.RegisterCubes(docker.Cubes);
            }
            else if(dock == -1)
            {
                throw new Exception("Could not load all defined components");
            }
        }

    }
    #endregion

    public void Dispose()
    {
        castle.Dispose();
    }
}
