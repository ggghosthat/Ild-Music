using Ild_Music_MVVM_.Services.Parents;
using System.Collections.Generic;
using ShareInstances.PlayerResources;
using Ild_Music_MVVM_.ViewModel.VM;

namespace Ild_Music_MVVM_.Services
{
    public class FactoryService : Service
    {
        #region Fields
        public override string ServiceType { get; init; } = "Factory";

        private EntityCreator creator = new ();
        private SupporterService supporter;


        private Artist artist;
        private Track track;
        private Playlist playlist;

        #endregion

        #region Properties
        public Artist Artist => artist;
        public Track Track => track;
        public Playlist Playlist => playlist;

        #endregion

        #region Const
        public FactoryService(SupporterService supporterService) =>
            supporter = supporterService;
        #endregion


        #region Public Methods
        public void CreateArtist(string name, string description)
        {
            creator.GenerateArtist(name: name, description: description);
            creator.GetArtist(out artist);
            supporter.AddInstanceObject(artist);
        }

        public void CreateTrack(string pathway, string name, string description, IList<Artist?> artists, IList<Playlist?> playlists)
        {
            if (artists == null && playlists == null)
            {
                creator.GenerateTrack(pathway: pathway, name: name, description: description);
                goto GetTrack;
            }

            creator.GenerateTrack(pathway: pathway, name: name, description: description, artists: artists, playlists: playlists);
            supporter.SaveAllStates();
            
            GetTrack:
            creator.GetTrack(out track);
            supporter.AddInstanceObject(track);
            
        }

        public void CreatePlaylist(string name, string description, IList<Artist>? artists = null)
        {   
            creator.GeneratePlaylist(name: name, description: description);
            creator.GetPlaylist(out playlist);

            foreach (var artist in artists)
                artist.AddPlaylist(playlist);

            supporter.AddInstanceObject(playlist);
        }

        #endregion
    }
}
