using Ild_Music_MVVM_.Services.Parents;
using System.Collections.Generic;
using ShareInstances.PlayerResources;

namespace Ild_Music_MVVM_.Services
{
    public class FactoryService : Service
    {
        public override string ServiceType { get; init; } = "Factory";

        private EntityCreator creator = new ();

        private SupporterService supporter;


        private Artist artist;
        private Track track;
        private Playlist playlist;

        public Artist Artist => artist;
        public Track Track => track;
        public Playlist Playlist => playlist;





        public FactoryService(SupporterService supporterService)
        {
            supporter = supporterService;
        }


        public void CreateArtist(string name, string description)
        {
            creator.GenerateArtist(name: name, description: description);
            creator.GetArtist(out artist);
            supporter.AddInstanceObject(artist);
        }

        public void CreateTrack(string pathway, string name, string description, int? artistIndex, int? playlistIndex)
        {
            Artist artist = null;
            Playlist playlist = null;

            if (artistIndex == null && playlistIndex == null)
            {
                creator.GenerateTrack(pathway: pathway, name: name, description: description);
                goto GetTrack;
            }

            if (artistIndex != null)
                artist = supporter.ArtistSup[(int)artistIndex];
            if (playlistIndex != null)
                playlist = supporter.PlaylistSup[(int)playlistIndex];
            

            creator.GenerateTrack(pathway: pathway, name: name, description: description, artist: artist, playlist: playlist);
            
            GetTrack:
            creator.GetTrack(out track);
            supporter.AddInstanceObject(track);
            
        }

        public void CreatePlaylist(string name, string description, IList<object>? tracks = null)
        {
            

            creator.GeneratePlaylist(name: name, description: description);
            creator.GetPlaylist(out playlist);
            
            if (tracks != null)
                playlist.Tracks = (IList<Track>)tracks;
            
            supporter.AddInstanceObject(playlist);
        }
    }
}
