using System;
using System.Threading.Tasks;
using ShareInstances.PlayerResources;

namespace ShareInstances
{
    //Represent Synchronization block instance
    public interface ISynchArea
    {
        public Guid AreaId { get; }
        public string AreaName { get; }

        #region AddMethods
        public void AddArtistObj(Artist artist);
        public void AddTrackObj(Track artist);
        public void AddPlaylistObj(Playlist artist);
        #endregion

        #region EditMethods
        public void EditArtistObj(Artist artist);
        public void EditTrackObj(Track artist);
        public void EditPlaylistObj(Playlist artist);
        #endregion

        #region RemoveMethods
        public void RemoveArtistObj(Artist artist);
        public void RemoveTrackObj(Track artist);
        public void RemovePlaylistObj(Playlist artist);
        #endregion

        #region SaveMethods
        public void Save();
        public void SaveArtists();
        public void SaveTracks();
        public void SavePlaylists();
        #endregion
    }

    //Represent Player instance
    public interface IPlayer
    {
        public Guid PlayerId { get; }
        public string PlayerName { get; }

        public bool IsSwipe { get; }
        public bool IsEmpty { get; }

        public bool PlayerState { get; }

        public void SetTrackInstance(Track track);

        public void SetPlaylistInstance(Playlist trackCollection, int index = 0);

        public void SetNotifier(Action callBack);

        public Task Play();

        public Task StopPlayer();

        public Task Pause_ResumePlayer();

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
