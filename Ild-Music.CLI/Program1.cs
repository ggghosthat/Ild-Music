using ShareInstances.Instances;
using ShareInstances.Statistics;
using ShareInstances;
using Cube;
using Cube.Storage;

using System;
using System.Linq;
using System.Collections.Generic;
namespace Ild_Music.CLI.Test;

class Program13
{
    static string path = "./storage.db";
    static ICube cube = new Cube.Cube();



    public static async Task main123(string[] args)
    {
        cube.SetPath(ref path);
        cube.Init();


        var art1 = new Artist("Artist 1".AsMemory(), "my artist 1".AsMemory(), new byte[0], 2000);
        var art2 = new Artist("Artist 2".AsMemory(), "my artist 2".AsMemory(), new byte[0], 2000);
        var art3 = new Artist("Artist 3".AsMemory(), "my artist 3".AsMemory(), new byte[0], 2000);

       var pls1 = new Playlist("Playlist 1".AsMemory(), "my playlist 1".AsMemory(), new byte[0], 2000);
       var pls2 = new Playlist("Playlist 2".AsMemory(), "my playlist 2".AsMemory(), new byte[0], 2000);
       var pls3 = new Playlist("Playlist 3".AsMemory(), "my playlist 3".AsMemory(), new byte[0], 2000);

       var trc1 = new Track("/home/jake/Documents/skater.xcf".AsMemory(), "Track 1".AsMemory(), "my track 1".AsMemory(), new byte[0], TimeSpan.FromSeconds(0), 2000);
       var trc2 = new Track("/home/jake/Documents/skater.xcf".AsMemory(), "Track 2".AsMemory(), "my track 2".AsMemory(), new byte[0], TimeSpan.FromSeconds(0), 2000);
       var trc3 = new Track("/home/jake/Documents/skater.xcf".AsMemory(), "Track 3".AsMemory(), "my track 3".AsMemory(), new byte[0], TimeSpan.FromSeconds(0), 2000);

       art1.AddPlaylist(ref pls1);
       art2.AddPlaylist(ref pls1);
       art3.AddPlaylist(ref pls3);
       art1.AddPlaylist(ref pls2);

       pls1.AddTrack(ref trc1);
       pls1.AddTrack(ref trc2);
       pls3.AddTrack(ref trc3);
       pls2.AddTrack(ref trc2);

       //await cube.AddArtistObj(art1);
       //await cube.AddArtistObj(art2);
       //await cube.AddArtistObj(art3);

       //await cube.AddPlaylistObj(pls1);
       //await cube.AddPlaylistObj(pls2);
       //await cube.AddPlaylistObj(pls3);

       //await cube.AddTrackObj(trc1);
       //await cube.AddTrackObj(trc2);
       //await cube.AddTrackObj(trc3);


        //if(cube.Artists is not null){
        //var a3 = cube.Artists.Where(a => a.Name.ToString().Equals("Artist 3")).First();
        //Console.WriteLine(a3.Id);
        //await cube.RemoveArtistObj(a3);
       //}
       //

       //if(cube.Playlists is not null)
       //{
       //     var p3 = cube.Playlists.Where(p => p.Name.ToString().Equals("Playlist 3")).First();
       //     Console.WriteLine(p3.Id);
       //     p3.Description = "this is a brand new description.".AsMemory();
       //     await cube.EditPlaylistObj(p3);
       //}
       
        Console.WriteLine("Enter, your search term: ");
        var term = Console.ReadLine();
        var collection = await cube.Search<Track>(term.AsMemory());
        Console.WriteLine(collection.Count());
        foreach (var item in collection)
            Console.WriteLine(item.Name);
    }
}
