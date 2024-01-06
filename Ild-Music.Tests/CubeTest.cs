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
    public void BasicEntityOperationTest()
    {
        Console.WriteLine("Starting basic entities' operations test.");

        var guidA1 = Guid.NewGuid();
        var guidA2 = Guid.NewGuid();
        var guidA3 = Guid.NewGuid();
        var guidP1 = Guid.NewGuid();
        var guidP2 = Guid.NewGuid();
        var guidP3 = Guid.NewGuid();
        var guidT1 = Guid.NewGuid();
        var guidT2 = Guid.NewGuid();
        var guidT3 = Guid.NewGuid();
        var guidTag1 = Guid.NewGuid();

        Console.WriteLine("Generating entities...");

        var artist1 = new Artist(guidA1, "Zohan".AsMemory(), "No no no".AsMemory(), new byte[0], 2023);
        var artist2 = new Artist(guidA2, "Zohan 2".AsMemory(), "No no no".AsMemory(), new byte[0], 2022);
        var artist3 = new Artist(guidA3, "Zohan 3".AsMemory(), "No no no".AsMemory(), new byte[0], 2021);

        var playlist1 = new Playlist(guidP1, "Playlist 1".AsMemory(), "pls1".AsMemory(), new byte[0], 2021);
        var playlist2 = new Playlist(guidP2, "Playlist 2".AsMemory(), "pls2".AsMemory(), new byte[0], 2022);
        var playlist3 = new Playlist(guidP3, "Playlist 3".AsMemory(), "pls3".AsMemory(), new byte[0], 2023);

        var track1 = new Track(guidT1, "/home/jake/Music/a_lot.mp3".AsMemory(), "Track 1".AsMemory(), "t1".AsMemory(), new byte[0], TimeSpan.Zero, 2021); 
        var track2 = new Track(guidT2, "/home/jake/Music/a_lot.mp3".AsMemory(), "Track 2".AsMemory(), "t2".AsMemory(), new byte[0], TimeSpan.Zero, 2022); 
        var track3 = new Track(guidT3, "/home/jake/Music/a_lot.mp3".AsMemory(), "Track 3".AsMemory(), "t3".AsMemory(), new byte[0], TimeSpan.Zero, 2023); 

        var tag1 = new Tag(guidTag1, "Favor".AsMemory(), "red".AsMemory());

        Console.WriteLine("Defining relationships...");

        artist1.AddTrack(ref track1);
        artist1.AddTrack(ref track2);
        artist1.AddPlaylist(ref playlist1);

        artist2.AddTrack(ref track2);
        artist2.AddPlaylist(ref playlist2);
        artist2.AddPlaylist(ref playlist3);
       
        artist3.AddTrack(ref track3);
        artist3.AddPlaylist(ref playlist3);

        playlist1.AddTrack(ref track1);
        playlist1.AddTrack(ref track2);

        playlist2.AddTrack(ref track2);
        playlist2.AddTrack(ref track3);
        playlist3.AddTrack(ref track3);

        artist1.Tags.Add(tag1);
        playlist1.Tags.Add(tag1);
        track1.Tags.Add(tag1);

        Console.WriteLine("Keeping save into database...");

        cube.AddTagObj(tag1).Wait();

        cube.AddArtistObj(artist1).Wait();
        cube.AddArtistObj(artist1).Wait();
        cube.AddArtistObj(artist1).Wait();

        cube.AddPlaylistObj(playlist1).Wait();
        cube.AddPlaylistObj(playlist2).Wait();
        cube.AddPlaylistObj(playlist3).Wait();
        
        cube.AddTrackObj(track1).Wait();
        cube.AddTrackObj(track2).Wait();
        cube.AddTrackObj(track3).Wait();
       
        artist1.Name = "Viber XZ".AsMemory();
        cube.EditArtistObj(artist1);

        playlist1.Name = "Beach Theme".AsMemory();
        playlist1.AddTrack(ref track3);
        cube.EditPlaylistObj(playlist1);

        Console.WriteLine("Hello Mars");
        track3.Name = "Hello Mars".AsMemory();
        cube.EditTrackObj(track3);
    }
    

    public void Dispose()
    {

    }
}
