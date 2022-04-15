using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using SynchronizationBlock.Models.SynchArea;
using System.Collections.ObjectModel;

namespace Ild_Music_MVVM_.Services
{
    public enum EntityState { Artist, Track, Playlist}

    public class SupporterService
    {
        private Area synchArea;
        private EntityState state;

        public ObservableCollection<Artist> ArtistsSup { get; set; } = new ObservableCollection<Artist>();
        public ObservableCollection<Track> TrackSup { get; set; } = new ObservableCollection<Track>();
        public ObservableCollection<Tracklist> PlaylistSup { get; set; } = new ObservableCollection<Tracklist>();


        public SupporterService(Area area)
        {
            synchArea = area;
            synchArea.Init();
        }


        public void AddInstanceObject(ResourceRoot instanceObject)
        {
            if (instanceObject is Track track)
            {
                synchArea.AddTrackObj(track);
                synchArea.SaveTracks();
            }
            else if (instanceObject is Tracklist playlist)
            {
                synchArea.AddPlaylistObj(playlist);
                synchArea.SavePlaylists();
            }
            else if (instanceObject is Artist artist)
            {
                synchArea.AddArtistObj(artist);
                synchArea.SaveArtists();
            }
        }

        public void EditInstanceObject(ResourceRoot instanceObject, EntityState entityState)
        {
            switch (entityState)
            {
                case EntityState.Track:
                    synchArea.EditTrackObj((Track)instanceObject);
                    synchArea.SaveTracks();
                    break;
                case EntityState.Playlist:
                    synchArea.EditPlaylistObj((Tracklist)instanceObject);
                    synchArea.SavePlaylists();
                    break;
                case EntityState.Artist:
                    synchArea.EditArtistObj((Artist)instanceObject);
                    synchArea.SaveArtists();
                    break;
                default:
                    break;
            }
        }

        public void RemoveInstanceObject(ResourceRoot instanceObject, EntityState entityState)
        {
            switch (entityState)
            {
                case EntityState.Track:
                    synchArea.RemoveTrackObj((Track)instanceObject);
                    synchArea.SaveTracks();
                    break;
                case EntityState.Playlist:
                    synchArea.RemovePlaylistObj((Tracklist)instanceObject);
                    synchArea.SavePlaylists();
                    break;
                case EntityState.Artist:
                    synchArea.RemoveArtistObj((Artist)instanceObject);
                    synchArea.SaveArtists();
                    break;
                default:
                    break;
            }
        }

        public void RefreshList()
        {
            synchArea.Init();
        }

        public void SaveSingleSatate(EntityState state)
        {
            switch (state)
            {
                case EntityState.Track:
                    synchArea.SaveTracks();
                    break;
                case EntityState.Playlist:
                    synchArea.SavePlaylists();
                    break;
                case EntityState.Artist:
                    synchArea.SaveArtists();
                    break;
                default:
                    break;
            }
        }

        public void SaveState()
        {
            synchArea.Save();
        }
    }
}
