using ShareInstances.Instances;
using ShareInstances;
using Cube;
using Cube.Storage;

using System;
using System.Linq;
using System.Collections.Generic;
namespace Ild_Music.CLI.Test;
internal static class Utils
{
   

    public static IEnumerable<Artist> GenerateArtists(int count) 
    {
        var results = new List<Artist>(count);
        for(int i=1;i<=count;i++)
        {
            results.Add(new Artist($"Artist {i}".AsMemory(), $"my art {i}".AsMemory(), new byte[0], 2000));
        }
        return results;
    }

    public static IEnumerable<Playlist> GeneratePlaylists(int count) 
    {
        var results = new List<Playlist>(count);
        for(int i=1;i<=count;i++)
        {
            results.Add(new Playlist($"Playlist {i}".AsMemory(), $"my pls {i}".AsMemory(), new byte[0], 2000));
        }

        return results;
    }

    public static IEnumerable<Track> GenerateTracks(int count) 
    {
        var results = new List<Track>(count);
        for(int i=1;i<=count;i++)
        {
            results.Add(new Track("/home/jake/.profile".AsMemory(), "Track 1".AsMemory(), "my trk 1".AsMemory(), new byte[0], TimeSpan.FromMilliseconds(0), 2000));
        }

        return results;
    }
}
