using ShareInstances;
using ShareInstances.PlayerResources;
using NAudio.Wave;
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
        public string PlayerName { get; } = "NAudio Player " ;

        private NAudioPlaybacker _audioPlayer;

        private Track _track;
        private Playlist _tracklist;
        private IList<Track> tracksCollection;
        public IList<Track> Collection => tracksCollection ?? null;

        private Track current;

        private float volume;

        private event Action ShuffleCollection;

        public bool isEmpty { get; private set; } = true;
        public bool isSwipe { get; private set; } = false;
        #endregion

        #region ctor
        public NAudioPlayer()
        {

        }

        public void StartTrack(Track track)
        {
            _track = track;            
            InitAudioPlayer();
            isEmpty = false;
        }

        public void StartPlaylist(Playlist trackCollection, float volume, int index=0)
        {
            _tracklist = trackCollection;
            tracksCollection = trackCollection.Tracks;
            this.volume = volume;
            InitAudioPlayer(index);
            ShuffleCollection += OnShuffleCollection;
            isEmpty = false;
            isSwipe = true;
        }
        #endregion

        #region PlayerInitialization
        public void InitAudioPlayer()
        {
            if (_audioPlayer != null)
                _audioPlayer.Stop();

            current = _track;
            if (current != null)
            {
                _audioPlayer = new NAudioPlaybacker(current, volume);
                AutoDrop();
            }
        }

        public void InitAudioPlayer(int index)
        {
            if (_audioPlayer != null)
                _audioPlayer.Stop();

            this.current = Collection[index];
            if (current != null)         
            {
                _audioPlayer = new NAudioPlaybacker(current, this.volume); 
                AutoDrop();
            }
        }

        public void InitAudioPlayer(Track track)
        {
            if (_audioPlayer != null)
            {
                _audioPlayer.Stop();
            }

            current = track;
            if (current != null)
            {
                _audioPlayer = new NAudioPlaybacker(current, this.volume);
                AutoDrop();
            }
        }
        #endregion

        #region Player_Buttons
        public async Task Play() =>
            await Task.Run(() => _audioPlayer.Play());

        public async Task StopPlayer() =>
            await Task.Run(() => _audioPlayer.Stop());

        public async Task Pause_ResumePlayer() =>
            await Task.Run(() => _audioPlayer.Pause());

        public async Task ShuffleTrackCollection() =>
            await Task.Run(() => ShuffleCollection?.Invoke());

        public PlaybackState? GetPlayerState() =>
            _audioPlayer.PlayerState;


        

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

            Console.WriteLine(current.Name);
            if (current != null)
            {
                InitAudioPlayer(track);
            }
           _audioPlayer.Play();
            
        }
        #endregion

        #region AutoNextDrop_region
        private void AutoDrop()
        {
            if (_audioPlayer != null)
            {
                _audioPlayer.TrackFinished += DropNext;
                //_audioPlayer.TrackFinished += StartPlayer;
            }
        }
        #endregion
    }
}
