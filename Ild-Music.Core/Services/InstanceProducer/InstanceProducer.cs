using Ild_Music.Core.Instances;

namespace Ild_Music.Core.Services.InstanceProducer;

internal struct InstanceProducer : IDisposable
{        
    public Artist ArtistInstance { get; private set; } = default!;
    public Playlist PlaylistInstance { get; private set; } = default!;
    public Track TrackInstance { get; private set; } = default!;
    public Tag TagInstance { get; private set; } = default!;

    public InstanceProducer(
        Memory<char> name,
        Memory<char> description, 
        Memory<char> avatarPath,
        int year)
    {
        ArtistInstance = new (
            id: Guid.NewGuid(),
            name: name,
            description: description,
            avatarPath: avatarPath,
            year: year);
    }

    public InstanceProducer(
        Memory<char> name,
        Memory<char> description,
        Memory<char> avatarPath,
        IList<Track> tracks,
        IList<Artist> artists,
        int year)
     {
        var playlist  = new Playlist(
            id: Guid.NewGuid(),
            name: name,
            description: description,
            avatarPath: avatarPath,
            year: year);

        if (tracks != null && tracks.Count > 0)
            tracks.ToList().ForEach(t => playlist.AddTrack(ref t));

        if (artists != null && artists.Count > 0)
            artists.ToList().ForEach(a => 
            {
                a.AddPlaylist(ref playlist);
                playlist.Artists.Add(a.Id);
            });            

        PlaylistInstance = playlist;
    }

    public InstanceProducer(
        Memory<char> pathway,
        Memory<char> name,
        Memory<char> description,
        Memory<char> avatarPath,
        IList<Artist> artists,
        TimeSpan duration,
        int year)
    {
        TrackInstance = new Track(
            id: Guid.NewGuid(),
            pathway: pathway,
            name: name,
            description: description,
            avatarPath: avatarPath,
            duration: duration,
            year: year);

        var track = TrackInstance;

        if (artists != null && artists.Count > 0)
            artists.ToList().ForEach(a => 
            {
                a.AddTrack(ref track);
                track.Artists.Add(a.Id);
            });
    }

    public InstanceProducer(
        Memory<char> name,
        Memory<char> color,
        IList<Artist> artists,
        IList<Playlist> playlists,
        IList<Track> tracks)
    {
        TagInstance = new Tag( Guid.NewGuid(), name, color);
        
        var tag = TagInstance;

        if (artists != null && artists.Count > 0)
        {
            artists.ToList().ForEach(a => 
            {
                a.Tags.Add(tag);
                tag.Artists.Add(a.Id);
            });
        }
        if (playlists != null && playlists.Count > 0)
        {
            playlists.ToList().ForEach(p => 
            {
                p.Tags.Add(tag);
                tag.Playlists.Add(p.Id);
            });
        }
        if (tracks != null && tracks.Count > 0)
        {
            tracks.ToList().ForEach(t => 
            {
                t.Tags.Add(tag);
                tag.Tracks.Add(t.Id);
            });
        }
    }

    public void Dispose()
    {
        ArtistInstance = default!;
        PlaylistInstance = default!; 
        TrackInstance = default!;
        TagInstance = default!;
    }
}
