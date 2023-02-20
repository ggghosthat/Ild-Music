using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;

namespace ShareInstances
{
    public interface IShare
    {        
    }
    
    //Represent Synchronization block instance
    public interface ISynchArea : IShare
    {
        public Guid AreaId { get; }
        public string AreaName { get; }

        #region ToggleMethods
        void Init();
        #endregion

        #region ResourceCollections
        public IList<Artist> existedArtists {get;}
        public IList<Playlist> existedPlaylists { get; }
        public IList<Track> existedTracks { get; }        
        #endregion

        #region AddMethods
        void AddArtistObj(Artist artist);
        void AddTrackObj(Track artist);
        void AddPlaylistObj(Playlist artist);
        #endregion

        #region EditMethods
        void EditArtistObj(Artist artist);
        void EditTrackObj(Track artist);
        void EditPlaylistObj(Playlist artist);
        #endregion

        #region RemoveMethods
        void RemoveArtistObj(Artist artist);
        void RemoveTrackObj(Track artist);
        void RemovePlaylistObj(Playlist artist);
        #endregion

        #region SaveMethods
        void Save();
        void SaveArtists();
        void SaveTracks();
        void SavePlaylists();
        #endregion
    }

    //Represent Player instance
    public interface IPlayer : IShare
    {
        public Guid PlayerId { get; }
        public string PlayerName { get; }

        public ICoreEntity CurrentEntity {get;}

        public bool IsSwipe { get; }
        public bool IsEmpty { get; }
        public bool PlayerState { get; }

        public TimeSpan TotalTime { get; }
        public TimeSpan CurrentTime { get; set; }
 
        public float MaxVolume {get;}
        public float MinVolume {get;}
        public float CurrentVolume {get; set;}

        public void SetInstance(ICoreEntity entity, int index=0);

        public void SetNotifier(Action callBack);


        public Task Play();

        public Task StopPlayer();

        public Task Pause_ResumePlayer();

        public Task RepeatTrack();

        public void DropPrevious();

        public void DropNext();

        public Task ShuffleTrackCollection();

        public Task ChangeVolume(float volume);
    }
    
    public enum PlayerState
    {
        ACTIVE,
        PAUSED
    }
}
