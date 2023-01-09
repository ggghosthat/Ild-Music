using System.Collections.Generic;
using ShareInstances;
using ShareInstances.PlayerResources;
using ShareInstances.Services.Interfaces;
using ShareInstances.PlayerResources.Interfaces;

using System;

namespace ShareInstances.Services.Entities
{
    public class SupporterService : IService
    {
        public string ServiceName {get; init;} = "SupporterService";

        //temp solution
        public static ISynchArea SynchArea;

        public IList<Artist> ArtistsCollection => SynchArea.existedArtists;
        public IList<Playlist> PlaylistsCollection => SynchArea.existedPlaylists;
        public IList<Track> TracksCollection => SynchArea.existedTracks;


        public event Action OnArtistsNotifyRefresh = null;
        public event Action OnPlaylistsNotifyRefresh = null;
        public event Action OnTracksNotifyRefresh = null;

        //Initialize and start Synch Area instance 
        public void StartSynchArea(ISynchArea synchArea) 
        {
            SynchArea = synchArea;
            SynchArea.Init();
        }
        
        

        public void AddInstance(ICoreEntity instance)
        {
            if (instance is Artist artist)            
            {
                SynchArea.AddArtistObj(artist);
                SynchArea.SaveArtists();
                OnArtistsNotifyRefresh?.Invoke();
            }
            else if (instance is Playlist playlist)
            {
                SynchArea.AddPlaylistObj(playlist);
                SynchArea.SavePlaylists();
                OnPlaylistsNotifyRefresh?.Invoke();
            }
            else if (instance is Track track)
            {
                SynchArea.AddTrackObj(track);
                SynchArea.SaveTracks();
                OnTracksNotifyRefresh?.Invoke();
            }
        }

        public void EditInstance(ICoreEntity instance)
        {
            if (instance is Artist artist)            
            {
                SynchArea.EditArtistObj(artist);
                SynchArea.SaveArtists();
                OnArtistsNotifyRefresh?.Invoke();
            }
            else if (instance is Playlist playlist)
            {
                SynchArea.EditPlaylistObj(playlist);
                SynchArea.SavePlaylists();
                OnPlaylistsNotifyRefresh?.Invoke();
            }
            else if (instance is Track track)
            {
                SynchArea.EditTrackObj(track);
                SynchArea.SaveTracks();
                OnTracksNotifyRefresh?.Invoke();
            }  
        }

        public void DeleteInstance(ICoreEntity instance)
        {
            if(instance is Track track)
            {
                SynchArea.RemoveTrackObj(track);
                SynchArea.SaveTracks();
                OnArtistsNotifyRefresh?.Invoke();
            }
            else if (instance is Playlist playlist) 
            {
                SynchArea.RemovePlaylistObj(playlist);
                SynchArea.SavePlaylists();
                OnPlaylistsNotifyRefresh?.Invoke();
            }
            else if (instance is Artist artist) 
            {
                SynchArea.RemoveArtistObj(artist);
                SynchArea.SaveArtists();
                OnTracksNotifyRefresh?.Invoke();
            }
        }

        public void DumpState(int i = 0)
        {
            switch(i)
            {
                case 0:
                    SynchArea.Save();
                    break;
                case 1:
                    SynchArea.SaveArtists();
                    break;
                case 2:
                    SynchArea.SavePlaylists();
                    break;
                case 3:
                    SynchArea.SaveTracks();
                    break;
            }
        }
    }
}