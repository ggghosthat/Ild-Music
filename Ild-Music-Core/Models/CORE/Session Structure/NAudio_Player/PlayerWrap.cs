using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using Ild_Music_CORE.Models.Core.Session_Structure.Interfaces;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ild_Music_CORE.Models.Core.Session_Structure
{
    public class PlayerWrap : IPlayer
    {
        #region Fields
        public Guid PlayerId => Guid.NewGuid();

        private AudioPlayer _audioPlayer;

        private Track _track;
        private Tracklist _tracklist;
        private IList<Track> tracksCollection;
        public IList<Track> Collection => tracksCollection ?? null;
        private Track current;

        private float volume;

        private event Action ShuffleCollection;
        #endregion

        #region ctor
        public PlayerWrap(Track track)
        {
            _track = track;            
            InitAudioPlayer();
        }

        public PlayerWrap(Tracklist trackCollection, float volume, int index=0)
        {
            _tracklist = trackCollection;
            tracksCollection = trackCollection.Tracks;
            this.volume = volume;
            InitAudioPlayer(index);
            ShuffleCollection += OnShuffleCollection;
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
                _audioPlayer = new AudioPlayer(current, volume);
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
                _audioPlayer = new AudioPlayer(current, this.volume); 
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
                _audioPlayer = new AudioPlayer(current, this.volume);
                AutoDrop();
            }
        }
        #endregion

        #region Player_Buttons
        public async void StartPlayer() =>
            await Task.Run(() => _audioPlayer.Play());

        public async void StopPlayer() =>
            await Task.Run(() => _audioPlayer.Stop());

        public async void Pause_ResumePlayer() =>
            await Task.Run(() => _audioPlayer.Pause());

        public async void ShuffleTrackCollection() =>
            await Task.Run(() => ShuffleCollection?.Invoke());

        public PlaybackState? GetPlayerState() =>
            _audioPlayer.PlayerState;


        

        public async void ChangeVolume(float volume) =>
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
