using ShareInstances;
using ShareInstances.PlayerResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ild_Music_CORE.Models.Core.Session_Structure
{
    public class NAudioPlayer : IPlayer
    {
        #region Player Indentity Properties
        public Guid PlayerId => Guid.NewGuid();
        public string PlayerName => "NAudio Player ";
        #endregion

        #region Player Resource Properties
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
            get 
            {
                if (_audioPlayer != null)
                    return _audioPlayer.CurrentTime;
                else
                    return TimeSpan.Zero;
            }
            set => _audioPlayer.CurrentTime = value;            
        }
        #endregion

        #region Player State Properties
        public bool IsEmpty { get; private set; } = true;
        public bool IsSwipe { get; private set; } = false;
        public bool PlayerState { get; private set; } = false;
        #endregion

        #region Actions
        private Action notifyAction;
        #endregion

        #region Events
        private event Action ShuffleCollection;
        #endregion

        #region ctor
        public NAudioPlayer()
        {
        
        }

        public void SetTrackInstance(Track track)
        {

            CurrentTrack = track;            
            InitAudioPlayer();
            IsEmpty = false;
        }

        public void SetPlaylistInstance(Playlist playlist, int index=0)
        {
            CurrentPlaylist = playlist;
            Collection = playlist.Tracks;
            InitAudioPlayer(index);
            ShuffleCollection += ShuffleCollection;
            IsEmpty = false;
            IsSwipe = true;
        }

        #endregion

        #region PlayerInitialization
        public void InitAudioPlayer()
        {
            if (CurrentTrack != null)
            {
                PlayerState = true;
                notifyAction?.Invoke(); 
                _audioPlayer.SetTrack(CurrentTrack, volume);
            }
        }

        public void InitAudioPlayer(int index)
        {
            if(Collection != null && Collection.Count > 0)
                CurrentTrack = Collection[index];
            if (CurrentTrack != null)
            {
                PlayerState = true;
                notifyAction?.Invoke(); 
                _audioPlayer.SetTrack(CurrentTrack, volume);
                AutoDrop();
            }
        }


        public void SetNotifier(Action callBack) =>
            notifyAction = callBack;

        #endregion

        #region Player_Buttons
        public async Task Play()
        {
            await Task.Run(() => _audioPlayer.Play());  
            PlayerState = true;
        }

        public async Task StopPlayer()
        {
            await Task.Run(() => _audioPlayer.Stop());
            PlayerState = false;    
        }
        
        public async Task Pause_ResumePlayer()
        {
            switch (_audioPlayer.PlaybackState)
            {
                case NAudio.Wave.PlaybackState.Stopped:
                    await Task.Run(() => _audioPlayer.Play());
                    PlayerState = true;    
                    break;
                case NAudio.Wave.PlaybackState.Paused:
                    await Task.Run(() => _audioPlayer.Pause());
                    PlayerState = true;     
                    break;
                case NAudio.Wave.PlaybackState.Playing:
                    await Task.Run(() => _audioPlayer.Pause());
                    PlayerState = false;    
                    break;
            }
            notifyAction?.Invoke(); 
        }

        public async Task ShuffleTrackCollection() =>
            await Task.Run(() => ShuffleCollection?.Invoke());
        
        public async Task ChangeVolume(float volume) =>
            _audioPlayer.OnVolumeChanged(volume);

        
        #endregion

        #region Shuffle_region
        private void OnShuffleCollection()
        {
            // _audioPlayer.Stop();
            Shuffle();
            InitAudioPlayer(index: 0);
        }

        private void Shuffle()
        {
            var tmp = Collection.OrderBy(t => Guid.NewGuid()).ToList();
            Collection = tmp;
        }
        #endregion

        #region Drop_region
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
                CurrentTrack = CurrentTrack.NextTrack;
            else
                CurrentTrack = CurrentTrack.PreviousTrack;
            
            _audioPlayer.SetTrack(CurrentTrack, volume);
            AutoDrop();
            Pause_ResumePlayer();
        }
        #endregion
    }
}

