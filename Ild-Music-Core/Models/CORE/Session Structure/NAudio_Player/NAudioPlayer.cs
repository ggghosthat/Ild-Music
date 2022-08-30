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
        private NAudioPlaybacker _audioPlayer = new();
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
        public bool PlayerState { get; private set; }
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
            ShuffleCollection += OnShuffleCollection;
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
                FinishNotify();
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
            await Task.Run(() => _audioPlayer.Pause());
            switch (_audioPlayer.PlaybackState)
            {
                case NAudio.Wave.PlaybackState.Stopped:
                    PlayerState = false;
                    break;
                case NAudio.Wave.PlaybackState.Paused:
                    PlayerState = false;
                    break;
                case NAudio.Wave.PlaybackState.Playing:
                    PlayerState = true;
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
            _audioPlayer.Stop();
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
        public async void DropNext() =>
            await Task.Run(() => DropTrack(CurrentTrack.NextTrack));

        public async void DropPrevious() =>
            await Task.Run(() => DropTrack(CurrentTrack.PreviousTrack));

        private void DropTrack(Track track)
        {
            if (_audioPlayer != null)
                _audioPlayer.Stop();

            //if (CurrentTrack != null)
            //    DropTrack2Player(track);
            DropTrack2Player(track);
            _audioPlayer.Play();
            
        }

        private void DropTrack2Player(Track track)
        {
            CurrentTrack = track;
            if (CurrentTrack != null)
            {
                PlayerState = true;
                notifyAction?.Invoke();
                _audioPlayer.SetTrack(CurrentTrack, volume);
                AutoDrop();
            }
        }
        #endregion

        #region end
        private void AutoDrop()
        {
            if (_audioPlayer != null)
            {
                _audioPlayer.TrackFinished += () =>
                {
                    PlayerState = false;
                    notifyAction?.Invoke();
                };
                _audioPlayer.TrackFinished += DropNext;
                
                //_audioPlayer.TrackFinished += StartPlayer;
            }
        }

        private void FinishNotify()
        {
            if (_audioPlayer != null)
                _audioPlayer.TrackFinished += () =>
                {
                    PlayerState = false;
                    notifyAction?.Invoke();
                };             
        }
        #endregion
    }
}
