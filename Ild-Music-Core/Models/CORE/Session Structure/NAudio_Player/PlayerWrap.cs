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
            this._track = track;
            
            InitAudioPlayer();
        }

        public PlayerWrap(Tracklist trackCollection, float volume, int index=0)
        {
            this._tracklist = trackCollection;
            this.tracksCollection = trackCollection.Tracks;
            this.volume = volume;
            this.ShuffleCollection += OnShuffleCollection;
        }
        #endregion

        #region Shuffle_region
        private void OnShuffleCollection()
        {
            _audioPlayer.Stop();
            tracksCollection = null;
            var tmpCollection = Shuffle((IList<object>)tracksCollection);
            tracksCollection = (IList<Track>)tmpCollection;
            InitAudioPlayer(index:0);
        }

        private IList<object> Shuffle(IList<object> collection) 
        {
            return collection.OrderBy(t => Guid.NewGuid()).ToList();
        }
        #endregion

        #region Initial_player_region
        public void InitAudioPlayer(int index)
        {
            if (_audioPlayer != null)
                _audioPlayer.Stop();

            this.current = Collection[index];
            if (current != null)         
            {
                _audioPlayer = new AudioPlayer(current, this.volume);
                _audioPlayer.EndRiched += ReloadNext;
                _audioPlayer.EndRiched += Start;
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
                _audioPlayer.EndRiched += ReloadNext;
                _audioPlayer.EndRiched += Start;
            }
        }

        public void InitAudioPlayer()
        {
            if (_audioPlayer != null)
                _audioPlayer.Stop();

            this.current = this._track;
            if (current != null)
            {
                _audioPlayer = new AudioPlayer(current, this.volume);
                _audioPlayer.EndRiched += ReloadNext;
                _audioPlayer.EndRiched += Start;
            }
        }
        #endregion

        #region player_CLATS_region
        public void Start() 
        {
            _audioPlayer.Play();
        }

        public void StopPlayer()
        {
            _audioPlayer.Stop();
        }

        public void Pause_ResumePlayer() 
        {
            _audioPlayer.Pause();
        }

        public void ReloadNext() 
        {
            if(_audioPlayer != null)
                _audioPlayer.Stop();

            if (current.NextTrack != null)
            {
                InitAudioPlayer(track:(Track)current.NextTrack);
            }
        }

        public void ReloadPrevious()
        {
            if (_audioPlayer != null)
                _audioPlayer.Stop();

            if (current.PreviousTrack != null)
            {
                InitAudioPlayer(current.PreviousTrack);
            }
        }
        
        public void ShuffleTrackCollection() 
        {
            ShuffleCollection?.Invoke();      
        }
        #endregion

        public PlaybackState? GetPlayerState() 
        {
            return _audioPlayer.PlayerState;
        }
    }
}
