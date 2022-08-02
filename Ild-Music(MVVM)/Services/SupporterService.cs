using ShareInstances.PlayerResources.Base;
using ShareInstances.PlayerResources;
using Ild_Music_MVVM_.Services.Parents;
using SynchronizationBlock.Models.SynchArea;
using System.Collections.Generic;
using ShareInstances.PlayerResources.Interfaces;

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

        public void EditInstanceObject(ICoreEntity instance)
        {
            if (instance is Track track)
            {
                synchArea.EditTrackObj(track);
                synchArea.SaveTracks();
            }
            else if (instance is Playlist playlist)
            {
                synchArea.EditPlaylistObj(playlist);
                synchArea.SavePlaylists();
            }
            else if (instance is Artist artist)
            {
                synchArea.EditArtistObj(artist);
                synchArea.SaveArtists();
            }
        }

        public void RemoveInstanceObject(ICoreEntity instance)
        {

            if(instance is Track track)
            {
                synchArea.RemoveTrackObj(track);
                synchArea.SaveTracks();
            }
            else if (instance is Playlist playlist) 
            {
                synchArea.RemovePlaylistObj(playlist);
                synchArea.SavePlaylists();
            }
            else if (instance is Artist artist) 
            {
                synchArea.RemoveArtistObj(artist);
                synchArea.SaveArtists();
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
