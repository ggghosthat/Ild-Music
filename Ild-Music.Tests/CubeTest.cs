using Cube;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;

using Xunit;
using Xunit.Abstractions;

namespace Ild_Music.Tests;

public class CubeTest : IClassFixture<Cube.Cube>, IDisposable
{
    private string path;
    private readonly Cube.Cube cube = new();

    private readonly ITestOutputHelper _output;

    public CubeTest(ITestOutputHelper output)
    {
        path = "./storage_cube_test.db";
        cube.SetPath(ref path);
        cube.Init();
        _output = output; 
    }

    [Fact]
    public void InstanceRegistrationTest()
    {
        var guid = Guid.NewGuid();
        var artist = new Artist(guid, "Zohan".AsMemory(), "No no no".AsMemory(), new byte[0], 2023);

        cube.AddArtistObj(artist);
    }
    

    public void Dispose()
    {

    }
}
