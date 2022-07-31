using ShareInstances.PlayerResources.Base;
using ShareInstances.PlayerResources;
using Ild_Music_MVVM_.Services.Parents;
using SynchronizationBlock.Models.SynchArea;
using System.Collections.Generic;

namespace Ild_Music_MVVM_.Services
{
    public enum EntityState { Artist, Track, Playlist}

    public class SupporterService : Service
    {
        public override string ServiceType { get; init; } = "Supporter";

        private Area synchArea = new ();

        public IList<Artist> ArtistSup => synchArea.existedArtists;
        public IList<Track> TrackSup => synchArea.existedTracks;
        public IList<Playlist> PlaylistSup => synchArea.existedPlaylists;


        public SupporterService()
        {
            synchArea.Init();
        }

        #region Public methods
        public void AddInstanceObject(ResourceRoot instanceObject)
        {
            if (instanceObject is Track track)
            {
                synchArea.AddTrackObj(track);
                synchArea.SaveTracks();
            }
            else if (instanceObject is Playlist playlist)
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
                    synchArea.EditPlaylistObj((Playlist)instanceObject);
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
                    synchArea.RemovePlaylistObj((Playlist)instanceObject);
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

        public void SaveAllStates()
        {
            synchArea.Save();
        }
        #endregion
    }
}
