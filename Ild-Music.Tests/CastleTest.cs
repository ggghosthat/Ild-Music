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

public class CastleTest : IClassFixture<Castle>, IDisposable
{
    private readonly Configure configure = new("./config.json");
    private readonly Castle castle = new();
    
    private readonly ITestOutputHelper _output;

    public CastleTest(ITestOutputHelper output)
    {
        //castle = inputCastle;
        _output = output;
        Init().Wait();
        castle.Pack();
    }

    [Fact]
    public void GhostResolvingTest()
    {
        var ghost = castle.ResolveGhost(Ghosts.SUPPORT);
        var ghost1 = castle.ResolveGhost(Ghosts.FACTORY);
        var ghost2 = castle.ResolveGhost(Ghosts.PLAYER);

        Assert.NotNull(ghost);
        Assert.NotNull(ghost1);
        Assert.NotNull(ghost2);

        Assert.IsType<SupportGhost>(ghost);
        Assert.IsType<FactoryGhost>(ghost1);
        Assert.IsType<PlayerGhost>(ghost2);
    }
 
    [Fact]
    public void CubesResolvingTest()
    {
        var cubes = castle.GetCubesAsync().Result;

        foreach (var cube in cubes)
        {
            Assert.NotNull(cube);
            Assert.IsType<ICube>(cube);
            Console.WriteLine($"Current cube: {cube.CubeId}  {cube.CubeName}");
        }
    }
    
    //[Fact]
    public void PlayersResolvingTest()
    {
        var players = castle.GetPlayersAsync().Result;

        foreach (var player in players)
        {
            Assert.NotNull(player);
            Assert.IsType<ICube>(player);
            Console.WriteLine($"Current player: {player.PlayerId}  {player.PlayerName}");
        }
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
