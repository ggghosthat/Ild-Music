using Ild_Music.Controllers.ControllerServices;
using Ild_Music_CORE.Models.CORE.Tracklist_Structure;
using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using SynchronizationBlock.Models.SynchArea;
using Ild_Music_CORE.Models.Core;

namespace Ild_Music.Controllers
{
    public class ModelFactoryController
    {
        private EntitiesFactory _factory = new EntitiesFactory();
        private Area _synchArea;


        private Artist artist;
        private Track track;
        private Tracklist playlist;


        public Artist Artist => artist;
        public Track Track => track;
        public Tracklist Playlist => playlist;


        public ModelFactoryController(Area area)
        {
            _synchArea = area;
        }

        public void CreateArtist(string name, string description)
        {
            _factory.GenerateArtist(name:name, description:description);
            _factory.GetArtist(out this.artist);
            _synchArea.AddArtistObj(this.artist);
            _synchArea.SaveArtists();
        }

        public void CreateTrack(string pathway, string name, string description )
        {
            _factory.GenerateTrack(pathway: pathway, name:name, description: description);
            _factory.GetTrack(out this.track);
            _synchArea.AddTrackObj(this.track);
            _synchArea.SaveTracks();
        }

        public void CreatePlaylist(string name, string description)
        {
            _factory.GeneratePlaylist(name: name, description: description);
            _factory.GetPlaylist(out this.playlist);
            _synchArea.AddPlaylistObj(this.playlist);
            _synchArea.SavePlaylists();
        }
    }
}
