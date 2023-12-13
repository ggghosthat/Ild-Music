using Ild_Music.Core.Services.Castle;
using Ild_Music.Core.Contracts;
using Ild_Music.Core.Configure;
using Ild_Music.Core.Stage;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Services.Entities;

using Xunit;
using System.Threading.Tasks;
namespace Ild_Music.Tests;

public class CastleTest : IClassFixture<Castle>, IDisposable
{
    private readonly Configure configure = new("./config.json");
    private readonly Castle castle;

    public CastleTest(Castle inputCastle)
    {
        castle = inputCastle;
        Init().Wait();
        castle.Pack();
    }

    [Fact]
    public void ResolveSupportGhostTest()
    {
        var ghost = castle.ResolveGhost(Ghosts.SUPPORT);

        var ghost1 = castle.ResolveGhost(Ghosts.FACTORY);

        Assert.NotNull(ghost);
        Assert.NotNull(ghost1);

        Assert.IsType<SupportGhost>(ghost);
        Assert.IsType<FactoryGhost>(ghost1);
    }
 
    [Fact]
    public void ResolveFactoryGhostTest()
    {
        var ghost = castle.ResolveGhost(Ghosts.FACTORY);

        Assert.NotNull(ghost);
        Assert.IsType<FactoryGhost>(ghost);
    }

    [Fact]
    public void ResolvePlayerGhostTest()
    {
        var ghost = castle.ResolveGhost(Ghosts.PLAYER);

        Assert.NotNull(ghost);
        Assert.IsType<PlayerGhost>(ghost);
    }


    #region pre-init methods
    private async Task Init()
    {
        using (var docker = new Docker(configure))
        {
            var dock = await docker.Dock();

            if(dock == 0)
            {
                Console.WriteLine($"Cubes count: {docker.Cubes.Count}");
                Console.WriteLine($"Players count: {docker.Players.Count}");

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
