using System;
using System.IO;
using System.Linq;
using ShareInstances;
using System.Collections.Generic;
using ShareInstances.PlayerResources;
using SynchronizationBlock.Models.SynchObjects;


namespace SynchronizationBlock.Models.SynchArea
{
    public class Area : ISynchArea
    {
        public Guid AreaId => Guid.NewGuid();
        public string AreaName => "BaseSynch";


        private TrackSynch trackSynch = new TrackSynch();
        private ArtistSynch artistSynch = new ArtistSynch();
        private PlaylistSynch playlistSynch = new PlaylistSynch();


        public IList<Artist> existedArtists => artistSynch.Instances;
        public IList<Playlist> existedPlaylists => playlistSynch.Instances;
        public IList<Track> existedTracks => trackSynch.Instances;

        public event Action OnTrackSynchRefresh;
        public event Action OnPlaylistSynchRefresh;
        public event Action OnArtistSynchRefresh;


        public void Init()
        {
            artistSynch.Deserialize();
            trackSynch.Deserialize();
            playlistSynch.Deserialize();
        }

        public void SetPrefix(string pref)
        {
            if (File.Exists(pref)) 
            {
                trackSynch.Prefix = pref;
                artistSynch.Prefix = pref;
                playlistSynch.Prefix = pref;
            }
        }



        #region add_methods

        public void AddArtistObj(Artist artist)
        {
            artistSynch.AddInstance(artist);
            OnArtistSynchRefresh?.Invoke();
        }

        public void AddTrackObj(Track track)
        {
            trackSynch.AddInstance(track);
            OnTrackSynchRefresh?.Invoke();
        }

        public void AddPlaylistObj(Playlist playlist)
        {
            playlistSynch.AddInstance(playlist);
            OnPlaylistSynchRefresh?.Invoke();
        }
        #endregion

        #region edit_methods
        public void EditArtistObj(Artist artist)
        {
            artistSynch.EditInstance(artist);
            OnArtistSynchRefresh?.Invoke();
        }

        public void EditTrackObj(Track track)
        {
            trackSynch.EditInstance(track);
            OnTrackSynchRefresh?.Invoke();
        }

        public void EditPlaylistObj(Playlist playlist)
        {
            playlistSynch.EditInstance(playlist);
            OnPlaylistSynchRefresh?.Invoke();
        }
        #endregion

        #region remove_methods
        public void RemoveArtistObj(Artist artist)
        {
            artistSynch.RemoveInstance(artist);
            OnArtistSynchRefresh?.Invoke();
        }

        public void RemoveTrackObj(Track track)
        {
            trackSynch.RemoveInstance(track);
            OnTrackSynchRefresh?.Invoke();
        }

        public void RemovePlaylistObj(Playlist playlist)
        {
            playlistSynch.RemoveInstance(playlist);
            OnPlaylistSynchRefresh?.Invoke();
        }
        #endregion

        #region save_methods
        public void Save() 
        {
            artistSynch.Serialize();
            trackSynch.Serialize();
            playlistSynch.Serialize();
        }

        public void SaveArtists() => 
            artistSynch.Serialize();
                
        public void SavePlaylists() =>
            playlistSynch.Serialize();
        
        public void SaveTracks() =>
            trackSynch.Serialize();
        #endregion
    }
}
