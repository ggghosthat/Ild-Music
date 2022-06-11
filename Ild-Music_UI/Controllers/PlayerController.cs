using Ild_Music.Controllers.ControllerServices.Interfaces;
using Ild_Music_CORE.Models;
using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using Ild_Music_CORE.Models.Core.Session_Structure;
using Ild_Music_CORE.Models.Core.Session_Structure.Interfaces;
using NAudio.Wave;
using System;

namespace Ild_Music.Controllers
{

    public enum PlayerTrigger 
    {
        PLAY,
        PAUSE,
        NEXT,
        PREVIOUS,
        SHUFFLE,
        DROP_STAFF,
        REPEAT, 
        SONG_NAME,
        CURRENT_TIME,
        DURATION_TIME
    }

    public class PlayerController : IPlayerControll
    {
        #region Fields
        private PlayerWrap _player;
        private Track _track;
        
        private Tracklist _tracklist;

        public Nullable<PlaybackState> State 
        {
            get 
            {
                if (_player != null)
                    return _player.GetPlayerState();
                else
                    return null;
            }
        }

        public IPlayer Player => _player ?? null;

        private double volume = 25d;
        #endregion

        #region Ctor
        public PlayerController(ITrackable track)
        {
            _track = (Track)track;
            InitPlayer();
        }

        public PlayerController(ITrackable inputTracklist, int startIndex = 0)
        {
            _tracklist = (Tracklist)inputTracklist;
            _tracklist.Order();
            InitPlayer(startIndex);
        }
        #endregion

        public void InitPlayer()
        {
            _player = new PlayerWrap(_track);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
            _player.StartPlayer();
        }

        public void InitPlayer(int index)
        {
            _player = new PlayerWrap(_tracklist,(float)volume);
            _player.InitAudioPlayer(index);
            _player.StartPlayer();
        }


        private void MovePlayer(bool isNext) 
        {
            if (isNext)
                _player.DropNext();
            else
                _player.DropPrevious();

            _player.StartPlayer();
        }


        public void DropStaff(ITrackable inputTracklist, int startIndex = 0)
        {
            _tracklist = (Tracklist)inputTracklist;
            _tracklist.Order();
            InitPlayer(startIndex);
        }

        public void PlayNext()
        {
            MovePlayer(isNext: true);
        }

        public void PlayPrev()
        {
            MovePlayer(isNext: false);
        }

        public void Pause_Resume() 
        {
            _player.Pause_ResumePlayer();
        }

        public void Shuffle() 
        {
            _player.ShuffleTrackCollection();
        }
    }
}
