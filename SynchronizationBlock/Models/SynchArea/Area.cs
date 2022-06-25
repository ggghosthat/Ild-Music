using System;
using System.IO;
using ShareInstances;
using System.Collections.Generic;
using ShareInstances.PlayerResources;
using SynchronizationBlock.Models.SynchObjects;


namespace SynchronizationBlock.Models.SynchArea
{
    public class Area : ISynchArea
    {
        public Guid PlayerId => Guid.NewGuid();
        public string PlayerName => "BaseSynch";


        private TrackSynch<Track> trackSynch = new TrackSynch<Track>();
        private ArtistSynch<Artist> artistSynch = new ArtistSynch<Artist>();
        private PlaylistSynch<Playlist> playlistSynch = new PlaylistSynch<Playlist>();


        public IList<Artist> existedArtists => artistSynch.Instances;
        public IList<Track> existedTracks => trackSynch.Instances;
        public IList<Playlist> existedPlaylists => playlistSynch.Instances;



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

        public void AddArtistObj(Artist artist) =>        
            artistSynch.AddInstance(artist);
        
        public void AddTrackObj(Track track) =>        
            trackSynch.AddInstance(track);
        
        public void AddPlaylistObj(Playlist playlist) =>
            playlistSynch.AddInstance(playlist);
        #endregion

        #region edit_methods
        public void EditArtistObj(Artist artist) =>        
            artistSynch.EditInstance(artist);
        
        public void EditTrackObj(Track track) =>
            trackSynch.EditInstance(track);
        
        public void EditPlaylistObj(Playlist playlist) => 
            playlistSynch.EditInstance(playlist);
        #endregion

        #region remove_methods
        public void RemoveArtistObj(Artist artist) => 
            artistSynch.RemoveInstance(artist);
        
        public void RemoveTrackObj(Track track) =>
            trackSynch.RemoveInstance(track);
        
        public void RemovePlaylistObj(Playlist playlist) => 
            playlistSynch.RemoveInstance(playlist);
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
