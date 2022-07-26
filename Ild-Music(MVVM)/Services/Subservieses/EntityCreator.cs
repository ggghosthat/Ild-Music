using ShareInstances.PlayerResources;
using System.Linq;
using System.Collections.Generic;

namespace Ild_Music_MVVM_.Services
{
    class EntityCreator
    {
        private Artist _artist;
        private Track _track;
        private Playlist _playlist;



        public void GenerateArtist(string name, string description)
        {
            _artist = new Artist(name: name, description: description);
        }

        public void GeneratePlaylist(string name, string description)
        {
            _playlist = new Playlist(name: name, description: description);
        }

        public void GenerateTrack(string pathway, string name, string description, IList<Artist> artists = null, IList<Playlist> playlists = null)
        {
            _track = new Track(pathway: pathway, name: name, description: description);

            if (artists != null && artists.Count > 0)
                artists.ToList().ForEach(a => a.AddTrack(_track));
            if (playlists != null && playlists.Count > 0)
                playlists.ToList().ForEach(p => p.AddTrack(_track));                            
        }



        public void GetArtist(out Artist? artist) =>
            artist = _artist ?? null;

        public void GetTrack(out Track? track) =>
            track = _track ?? null;

        public void GetPlaylist(out Playlist? playlist) =>
            playlist = _playlist ?? null;
    }
}
