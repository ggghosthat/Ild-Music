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
        public int PlaylistPoint {get; private set;}
        public IList<Track> CurrentPlaylistPool {get; private set;}
        #endregion

        #region Player Triggers
        public bool IsSwipe {get; private set;} = false;
        public bool IsEmpty {get; private set;} = true;
        public bool PlayerState {get; private set;} = false;
        private bool IsTrackPlaylist = true;
        #endregion


        #region Time Presenters
        public TimeSpan TotalTime {get; private set;}
        public TimeSpan CurrentTime 
        {
            get => TimeSpan.FromMilliseconds(_mediaPlayer.Time);
            set => _mediaPlayer.SeekTo(value);
        }
        #endregion

        #region Volume Presenters
        public float MaxVolume {get; private set;} = 100;
        public float MinVolume {get; private set;} = 0;
        public float CurrentVolume 
        {
            get => _mediaPlayer.Volume;
            set => _mediaPlayer.Volume = (int)value;
        }
        #endregion


        #region Actions
        private Action notifyAction;
        #endregion

        #region Events
        private event Action ShuffleCollection;
        public event Action TrackStarted;
        #endregion

        #region constructor
        public VLCSharpPlayer()
        {}
        #endregion


        #region Player Inits
        public void SetNotifier(Action callback)
        {
            notifyAction = callback;
        }

        public void SetInstance(ICoreEntity entity, int index = 0)
        {
            CleanCurrentState();

            CurrentEntity = entity;
            PlaylistPoint = index;
            IsEmpty = false;

            if (CurrentEntity is Playlist playlist)
            {    
                CurrentPlaylistPool = playlist.Tracks;
                IsTrackPlaylist = false;
                IsSwipe = true;
            }

            SetMedia();
        }
       
        private void SetMedia()
        {
            if (CurrentMedia != null)
            {
                CurrentMedia.Dispose();
                CurrentMedia = null;
            }

            if (CurrentEntity is Track track)
            {
                TotalTime = track.Duration;
                TrackStarted?.Invoke();
                CurrentMedia = new Media (_vlc, new Uri(track.Pathway));
                _mediaPlayer = new(CurrentMedia);
            }
            else if (CurrentEntity is Playlist playlist)
            {
                playlist.CurrentIndex = PlaylistPoint;
                TotalTime = CurrentPlaylistPool[PlaylistPoint].Duration;
                TrackStarted?.Invoke();

                CurrentMedia = new Media (_vlc, new Uri(CurrentPlaylistPool[PlaylistPoint].Pathway));
                _mediaPlayer = new(CurrentMedia);
            }
        }
        
        private void CleanCurrentState()
        {
            PlayerState = false;
            notifyAction?.Invoke();
            _mediaPlayer.Stop();

            if (CurrentMedia != null)
            {
                CurrentMedia.Dispose();
                CurrentMedia = null;
            }
            
            CurrentEntity = null;
            PlaylistPoint = 0;
            
            if (CurrentPlaylistPool != null)
            {
                CurrentPlaylistPool.Clear();
                CurrentPlaylistPool = null;
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
            await Task.Run(() => 
            {
                PlayerState = false;
                notifyAction?.Invoke();
                _mediaPlayer.Stop();
            });
        }

        public Task TogglePlayer()
        {
            if (!_mediaPlayer.IsPlaying)
            {
                PlayerState = true;
                notifyAction?.Invoke();
                _mediaPlayer.Play();

                while(_mediaPlayer.Position < 0.99f);

                PlayerState = false;
                notifyAction?.Invoke();
                _mediaPlayer.Stop();

                if (!IsTrackPlaylist)
                {
                    DropMediaInstance(true);
                }
            }
            else
            {
                PlayerState = false;
                notifyAction?.Invoke();
                _mediaPlayer.Pause();
            }
            return null;
        }

        public async Task Pause_ResumePlayer()
        {
           Task.Run(() => TogglePlayer()); 
        }

        public Task RepeatTrack()
        {
            return null;
        }

        public Task ShuffleTrackCollection()
        {
            return null;
        }

        public Task ChangeVolume(float volume)
        {
            return null;
        }
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
            DragPointer(direct);
            SetMedia();
            TogglePlayer();
        }

        private void DragPointer(bool direction)
        {
            if (direction)
            {
                if (PlaylistPoint == CurrentPlaylistPool.Count - 1)
                    PlaylistPoint = 0;
                else
                    PlaylistPoint++;
            }
            else 
            {
                if (PlaylistPoint == 0)
                    PlaylistPoint = CurrentPlaylistPool.Count - 1;
                else
                    PlaylistPoint--;
            }
        }
        #endregion
    }
}
