using ShareInstances.PlayerResources;

using System.Collections.Generic;
using System.Linq;

namespace ShareInstances.Services.InstanceProducer
{
    internal struct InstanceProducer
    {        
        public Artist ArtistInstance { get; private set; } = null;
        public Playlist PlaylistInstance { get; private set; } = null;
        public Track TrackInstance { get; private set; } = null;

        public InstanceProducer(string name,
                                string description)
        {
            ArtistInstance = new Artist(name: name, description: description);
        }

        public InstanceProducer(string name,
                                string description,
                                IList<Track> tracks,
                                IList<Artist> artists)
        {
            PlaylistInstance = new Playlist(name: name, description: description);
            var playlist = PlaylistInstance;
            
            if (tracks != null && tracks.Count > 0)
                tracks.ToList().ForEach(t => playlist.AddTrack(t));
            
            if (artists != null && artists.Count > 0)
                artists.ToList().ForEach(a => a.AddPlaylist(playlist));
        }

        public InstanceProducer(string pathway,
                                string name,
                                string description,
                                IList<Artist> artists = null)
        {
            TrackInstance = new Track(pathway: pathway, name: name, description: description);
            var track = TrackInstance;

            if (artists != null && artists.Count > 0)
                artists.ToList().ForEach(a => a.AddTrack(track));
        }

        public void Dispose()
        {
            if (ArtistInstance != null)
                ArtistInstance = null;
            if (PlaylistInstance != null)
                PlaylistInstance = null;
            if (TrackInstance != null)
                TrackInstance = null;
        }
    }
}