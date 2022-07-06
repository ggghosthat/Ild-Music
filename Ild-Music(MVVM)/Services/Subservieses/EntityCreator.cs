using ShareInstances.PlayerResources;

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

        public void GenerateTrack(string pathway, string name, string description, Artist? artist = null)
        {
            _track = new Track(pathway: pathway, name: name, description: description);
            if (artist != null)
            {
                _track.SetArtist(artist);
            }
            else
            {
                _track.SetArtist(new Artist(name: "Unknown", description: "Unknown artist"));
            }
        }



        public void GetArtist(out Artist? artist)
        {
            artist = _artist ?? null;
        }

        public void GetTrack(out Track? track)
        {
            track = _track ?? null;
        }

        public void GetPlaylist(out Playlist? playlist)
        {
            playlist = _playlist ?? null;
        }
    }
}
