using Ild_Music.Core.Services.Castle;
using Ild_Music.Core.Contracts;
using Ild_Music.Core.Configure;
using Ild_Music.Core.Stage;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Services.Entities;

using Xunit;
using System.Threading.Tasks;
namespace Ild_Music.Tests;

public class CastleTest
{
    Configure configure = new("./config.json");
    Castle castle = new();

    [Fact]
    public void Test()
    {
        Init().Wait();

        castle.Pack();

        Console.WriteLine(castle.GetCubesAsync().Result.Count());
        var ghost = castle.ResolveGhost(Ghosts.SUPPORT);

        Assert.Null(ghost);
        Assert.IsType<SupportGhost>(ghost);
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
}
