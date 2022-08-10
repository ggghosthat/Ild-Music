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
        #region Fields
        public Guid PlayerId => Guid.NewGuid();
        public string PlayerName { get; } = "NAudio Player ";

        private NAudioPlaybacker _audioPlayer = new();

        private Track _track;
        private Playlist _tracklist;
        private IList<Track> tracksCollection;
        public IList<Track> Collection => tracksCollection ?? null;

        private Track current;

        private float volume;

        private event Action ShuffleCollection;

        public bool IsEmpty { get; private set; } = true;
        public bool IsSwipe { get; private set; } = false;

        public bool PlayerState { get; private set; }

        private Action notifyAction;
        #endregion

        #region ctor
        public NAudioPlayer()
        {
        
        }

        public void SetTrackInstance(Track track)
        {
            _track = track;            
            InitAudioPlayer();
            IsEmpty = false;
        }

        public void SetPlaylistInstance(Playlist trackCollection, int index=0)
        {
            _tracklist = trackCollection;
            tracksCollection = trackCollection.Tracks;
            InitAudioPlayer(index);
            ShuffleCollection += OnShuffleCollection;
            IsEmpty = false;
            IsSwipe = true;
        }

        public void SetNotifier(Action callBack) =>
            notifyAction = callBack;
        
        #endregion

        #region PlayerInitialization
        public void InitAudioPlayer()
        {
            current = _track;
            if (current != null)
            {
                PlayerState = true;
                notifyAction?.Invoke();
                _audioPlayer.SetTrack(current, volume);
                FinishNotify();
            }
        }

        public void InitAudioPlayer(int index)
        {
            if(Collection != null && Collection.Count > 0)
                current = Collection[index];
            if (current != null)
            {
                PlayerState = true;
                notifyAction?.Invoke();
                _audioPlayer.SetTrack(current, volume);
                AutoDrop();
            }
        }
        #endregion

        #region Player_Buttons
        public async Task Play() =>
            await Task.Run(() => _audioPlayer.Play());

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
            var tmp = tracksCollection.OrderBy(t => Guid.NewGuid()).ToList();
            tracksCollection = tmp;
        }
        #endregion

        #region Drop_region
        public async void DropNext() =>
            await Task.Run(() => DropTrack(current.NextTrack));

        public async void DropPrevious() =>
            await Task.Run(() => DropTrack(current.PreviousTrack));

        private void DropTrack(Track track)
        {
            if (_audioPlayer != null)
                _audioPlayer.Stop();

            if (current != null)
                DropTrack2Player(track);
           _audioPlayer.Play();
            
        }

        private void DropTrack2Player(Track track)
        {
            current = track;
            if (current != null)
            {
                PlayerState = true;
                notifyAction?.Invoke();
                _audioPlayer.SetTrack(current, volume);
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
