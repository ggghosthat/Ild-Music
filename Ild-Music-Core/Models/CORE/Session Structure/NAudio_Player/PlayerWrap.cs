using Ild_Music_CORE.Models.Core;
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
                _audioPlayer.TrackFinished += DropNext;
                _audioPlayer.TrackFinished += StartPlayer;
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
                _audioPlayer.TrackFinished += DropNext;
                _audioPlayer.TrackFinished += StartPlayer;
            }
        }

        public void InitAudioPlayer(Track track)
        {
            if (_audioPlayer != null)
                _audioPlayer.Stop();

            this.current = track;
            if (current != null)
            {
                _audioPlayer = new AudioPlayer(current, this.volume);
                _audioPlayer.TrackFinished += DropNext;
                _audioPlayer.TrackFinished += StartPlayer;
            }
        }
        #endregion


        #region Player_Buttons
        public void StartPlayer() =>
            _audioPlayer.Play();

        public void StopPlayer() =>
            _audioPlayer.Stop();

        public void Pause_ResumePlayer() =>
            _audioPlayer.Pause();

        public void ShuffleTrackCollection() =>
            ShuffleCollection?.Invoke();

        public PlaybackState? GetPlayerState() =>
            _audioPlayer.PlayerState;


        public void DropNext() 
        {
            if(_audioPlayer != null)
                _audioPlayer.Stop();

            if (current.NextTrack != null)
                InitAudioPlayer(track:current.NextTrack);
            
        }

        public void DropPrevious()
        {
            if (_audioPlayer != null)
                _audioPlayer.Stop();

            if (current.PreviousTrack != null)
                InitAudioPlayer(current.PreviousTrack);
            
        }

        public void ChangeVolume(float volume) =>
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
    }
}
