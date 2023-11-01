using ShareInstances.Instances;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ShareInstances.Services.InstanceProducer;
internal struct InstanceProducer : IDisposable
{        
    public Artist ArtistInstance { get; private set; } = default!;
    public Playlist PlaylistInstance { get; private set; } = default!;
    public Track TrackInstance { get; private set; } = default!;

    public InstanceProducer(Memory<char> name,
                            Memory<char> description, 
                            Memory<byte> avatar,
                            int year)
    {
        ArtistInstance = new Artist(name: name,
                                    description: description,
                                    avatarSource: avatar,
                                    year: year);
    }

    public InstanceProducer(Memory<char> name,
                            Memory<char> description,
                            Memory<byte> avatar,
                            int year,
                            IList<Track> tracks,
                            IList<Artist> artists)
    {
        var playlist  = new Playlist(name: name,
                                     description: description,
                                     avatarSource: avatar,
                                     year: year);

        if (tracks != null && tracks.Count > 0)
        {
            tracks.ToList().ForEach(t => playlist.AddTrack(ref t));
        }

        if (artists != null && artists.Count > 0)
        {
            artists.ToList().ForEach(a => 
            {
                a.AddPlaylist(ref playlist);
                playlist.Artists.Add(a.Id);
            });            
        }

        PlaylistInstance = playlist;
    }

    public InstanceProducer(Memory<char> pathway,
                            Memory<char> name,
                            Memory<char> description,
                            Memory<byte> avatar,
                            TimeSpan duration,
                            int year,
                            IList<Artist> artists = null)
    {
        TrackInstance = new Track(pathway: pathway,
                                  name: name,
                                  description: description,
                                  avatarSource: avatar,
                                  duration: duration,
                                  year: year);
        var track = TrackInstance;

        if (artists != null && artists.Count > 0)
        {
            artists.ToList().ForEach(a => 
            {
                a.AddTrack(ref track);
                track.Artists.Add(a.Id);
            });

        }
    }

    public void Dispose()
    {
        ArtistInstance = default!;
        PlaylistInstance = default!; 
        TrackInstance = default!;
    }
}
