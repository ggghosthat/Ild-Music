using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using Ild_Music_MVVM_.Services.Parents;
using System.Collections.Generic;

namespace Ild_Music_MVVM_.Services
{
    public class FactoryService : Service
    {
        public override string ServiceType { get; init; } = "Factory";

        private EntityCreator creator = new ();

        private SupporterService supporter;


        private Artist artist;
        private Track track;
        private Tracklist playlist;

        public Artist Artist => artist;
        public Track Track => track;
        public Tracklist Playlist => playlist;





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

        public void CreateTrack(string pathway, string name, string description, int? artistIndex)
        {
            if (artistIndex != null)
            {
                var artist = supporter.ArtistSup[(int)artistIndex];
                creator.GenerateTrack(pathway: pathway, name: name, description: description, artist: artist);
            }
            else
            {
                creator.GenerateTrack(pathway: pathway, name: name, description: description);
            }
            
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
