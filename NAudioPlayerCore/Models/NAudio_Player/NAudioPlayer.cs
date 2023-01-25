using ShareInstances;
using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NAudioPlayerCore.Models.Core.Session_Structure
{
    public class NAudioPlayer : IPlayer
    {
        #region Player Indentity Properties
        public Guid PlayerId => Guid.NewGuid();
        public string PlayerName => "NAudio Player ";
        #endregion

        #region Player Resource Properties
        public ICoreEntity CurrentEntity {get; private set;}

        public Track CurrentTrack { get; private set; }
        public Playlist CurrentPlaylist { get; private set; }
        public IList<Track> Collection { get; private set; }
        #endregion

        #region Playbacker Properties
        public NAudioPlaybacker _audioPlayer = new();
        private float volume;
        public TimeSpan TotalTime => _audioPlayer.TotalTime;

        public TimeSpan CurrentTime
        {
            get => _audioPlayer.CurrentTime; 
            set => _audioPlayer.CurrentTime = value; 
        }
        #endregion

        #region Player State Properties
        public bool IsEmpty { get; private set; } = true;
        public bool IsSwipe { get; private set; } = false;
        public bool PlayerState { get; private set; } = false;
        #endregion

        #region Actions
        private static Action notifyAction;
        #endregion

        #region Events
        private event Action ShuffleCollection;
        public event Action CurrentPlaybackStopped;
        #endregion

        #region ctor
        public NAudioPlayer()
        {
        
        }

        public void SetTrackInstance(Track track)
        {
            CurrentEntity = (ICoreEntity)track;            
            InitAudioPlayer();
            IsEmpty = false;
        }

        public void SetPlaylistInstance(Playlist playlist, int index=0)
        {
            CurrentEntity = playlist;
            Collection = playlist.Tracks;
            InitAudioPlayer(index);
            ShuffleCollection += ShuffleCollection;
            IsEmpty = false;
            IsSwipe = true;
        }

        public void SetInstance(ICoreEntity entity, int index=0)
        {
            if (CurrentEntity is Track track)
            {

                CurrentEntity = (ICoreEntity)track;
                InitAudioPlayer();
                IsEmpty = false;
            }
            else if(CurrentEntity is Playlist playlist)
            {
                Collection = playlist.Tracks;
                CurrentEntity = (ICoreEntity)Collection[index];
                InitAudioPlayer(index);

                IsEmpty = false;
                IsSwipe = true;
            }
        }

        #endregion

        #region PlayerInitialization
        public void InitAudioPlayer(bool isTrack)
        {
            if (CurrentEntity is Track track)
            {
                PlayerState = true;
                notifyAction.Invoke();
                _audioPlayer.SetTrack(track, volume);

                if (isTrack)
                {
                    _audioPlayer.TrackFinished += () => 
                    {
                        _audioPlayer.Stop();
                        PlayerState = false;
                        notifyAction.Invoke();
                    };
                }
                else
                {
                    AutoDrop();  
                }
            }
        }

        public void InitAudioPlayer(int index)
        {
            if(Collection != null && Collection.Count > 0)
            {
                notifyAction?.Invoke(); 
                _audioPlayer.SetTrack(Collection[index], volume);
                AutoDrop();
            }
        }

        public void SetNotifier(Action callBack)
        {
            notifyAction = callBack;
        }
        #endregion

        #region Player_Buttons
        public async Task Play()
        {
            await Task.Run(() => _audioPlayer.Play());  
            PlayerState = true;
            notifyAction?.Invoke(); 
        }

        public async Task StopPlayer()
        {
            await Task.Run(() => _audioPlayer.Stop());

            PlayerState = false;    
            notifyAction?.Invoke(); 

        }
        
        public async Task Pause_ResumePlayer()
        {
            switch (_audioPlayer.PlaybackState)
            {
                case NAudio.Wave.PlaybackState.Stopped:
                    PlayerState = true; 
                    notifyAction.Invoke();   
                    await Task.Run(() => _audioPlayer.Play());
                    break;
                case NAudio.Wave.PlaybackState.Paused:
                    PlayerState = true;
                    notifyAction.Invoke();    
                    await Task.Run(() => _audioPlayer.Pause()); 
                    break;
                case NAudio.Wave.PlaybackState.Playing:
                    PlayerState = false;
                    notifyAction.Invoke();    
                    await Task.Run(() => _audioPlayer.Pause());
                    break;
            }
        }

        public async Task ShuffleTrackCollection()
        {
            await Task.Run(() => ShuffleCollection?.Invoke());
        }
        
        public async Task ChangeVolume(float volume)
        {
            _audioPlayer.OnVolumeChanged(volume);
        }

        public async Task RepeatTrack()
        {
            await Task.Run(() => _audioPlayer.Repeat());
        }

        #endregion

        #region Shuffle Methods
        private void OnShuffleCollection()
        {
            Shuffle();
            InitAudioPlayer(index: 0);
        }

        private void Shuffle()
        {
            var tmp = Collection.OrderBy(t => Guid.NewGuid()).ToList();
            Collection = tmp;
        }
        #endregion

        #region Drop Methods
        private void AutoDrop() =>
            _audioPlayer.TrackFinished += DropNext;
        
        public async void DropNext() =>
            await Task.Run(() => {
                if ((IsSwipe) && (!IsEmpty))
                    DropMediaInstance(true);
            });

        public async void DropPrevious() =>
            await Task.Run(() => {
                if ((IsSwipe) && (!IsEmpty))
                    DropMediaInstance(false);
            });
        
        private void DropMediaInstance(bool direct)
        {
            _audioPlayer.TrackFinished -= DropNext;

            if (direct)
                CurrentEntity = (ICoreEntity)(_audioPlayer.CurrentTrack.NextTrack);
            else
                CurrentEntity = (ICoreEntity)(_audioPlayer.CurrentTrack.PreviousTrack);
            
            _audioPlayer.SetInstance(CurrentEntity, volume);
            AutoDrop();
            Pause_ResumePlayer();
        }
        #endregion

        #region Private Methods
        private void TrackFinishedKick()
        {
        }
        #endregion
    }
}

