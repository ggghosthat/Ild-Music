using ShareInstances;
using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;
using LibVLCSharp.Shared;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace IldMusic.VLCSharp
{
    public class VLCSharpPlayer : IPlayer
    {
        #region VLCSharp Instances
        private static readonly LibVLC _vlc = new LibVLC(); 
        private static MediaPlayer _mediaPlayer = new(_vlc);
        #endregion

        #region Player Indentety 
        public Guid PlayerId => Guid.NewGuid();
        public string PlayerName => "VLC Sharp Player";
        #endregion

        #region Player Resources
        public Media CurrentMedia {get; private set;}
        public ICoreEntity CurrentEntity {get; private set;}


        public Track CurrentTrack {get; private set;}
        public Playlist CurrentPlaylist {get; private set;}
        #endregion

        #region Player Triggers
        public bool IsSwipe {get; private set;} = false;
        public bool IsEmpty {get; private set;} = true;
        public bool PlayerState {get; private set;}

        private bool PlayerStopped = false;

        private bool IsTrackPlaylist = true;
        #endregion


        #region Time Presenters
        public TimeSpan TotalTime {get; private set;}
        public TimeSpan CurrentTime {get; set;}
        #endregion


        #region Actions
        private Action notifyAction;
        private event Action ShuffleCollection;
        #endregion

        #region constructor
        public VLCSharpPlayer()
        {
        }
        #endregion


        #region Player Inits
        public void SetInstance(ICoreEntity entity, int index)
        {}

        public void SetTrackInstance(Track track)
        {
            CurrentTrack = track;
            InitPlayer();
            IsEmpty = false;
        }
         
        public void SetPlaylistInstance(Playlist playlist, int index = 0)
        {
            CurrentPlaylist = playlist;
            CurrentTrack = playlist.Tracks[index];

            InitPlayer();

            IsTrackPlaylist = false;
            IsEmpty = false;
            IsSwipe = true;
        }
       

        private void InitPlayer()
        {
            if (CurrentTrack != null)
            {
                if (notifyAction != null)
                    notifyAction?.Invoke();
                SetTrackMedia();
                PlayerState = true;
            }
        }
        #endregion

        #region Player Activity
        public Task Play() 
        {
            return null;
        }
        

        public async Task StopPlayer()
        {
            await Task.Run(() => {
                _mediaPlayer.Stop();

                if (PlayerStopped)
                    return;
                else 
                    _mediaPlayer.Stop();
            });
        }

        private void TogglePlayer()
        {
            if (!_mediaPlayer.IsPlaying)
            {
                Console.WriteLine(CurrentTrack.Name);
                _mediaPlayer.Play();
                
                //_mediaPlayer.Position = 0.88f;

                while(_mediaPlayer.Position < 0.99f);

                _mediaPlayer.Stop();

                if (!IsTrackPlaylist) 
                    TrackEndReached();
            }
            else
                _mediaPlayer.Pause();
        }

        public async Task Pause_ResumePlayer()
        {
            await Task.Run(TogglePlayer);
        }

        public Task ShuffleTrackCollection()
        {
            return null;
        }

        public Task ChangeVolume(float volume)
        {
            return null;
        }

        public void RepeatTrack()
        {}
        #endregion


        #region Move Collection Methods
        public async void DropPrevious()
        {
            await Task.Run (() => {
                if ((IsSwipe) && (!IsEmpty))
                    DropMediaInstance(false);
            });
        }

        public async void DropNext()
        {
            await Task.Run(() => {
                if((IsSwipe) && (!IsEmpty))
                    DropMediaInstance(true);
            });
        }

        private void DropMediaInstance(bool direct)
        {
            _mediaPlayer.Stop();
            if (direct)
                CurrentTrack = CurrentTrack.NextTrack;
            else
                CurrentTrack = CurrentTrack.PreviousTrack;

            SetTrackMedia();
            TogglePlayer();
        }
        #endregion


        #region Media Control Methods
        private void SetTrackMedia()
        {
            if (CurrentMedia != null)
            {
                CurrentMedia.Dispose();
                CurrentMedia = null;
            }
            Console.WriteLine(CurrentTrack.Pathway);
            CurrentMedia = new Media (_vlc, new Uri(CurrentTrack.Pathway));

            _mediaPlayer = new(CurrentMedia);
        }

        private void SetPlaylistMedia()
        {
        }
        #endregion



        #region Notify Action Methods
        public void SetNotifier(Action callback) => 
            notifyAction = callback;

        public void NotifyAboutState()
        {}
        #endregion

        #region Player Triggered Player        
        private void TrackEndReached()
        {
            _mediaPlayer.Stop();
            CurrentTrack = CurrentTrack.NextTrack;
            SetTrackMedia();
            TogglePlayer();
        }
        #endregion
    }
}
