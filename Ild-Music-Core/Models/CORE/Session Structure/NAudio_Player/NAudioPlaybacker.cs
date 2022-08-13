using NAudio.Wave;
using System;
using System.Threading.Tasks;
using ShareInstances.PlayerResources;

namespace Ild_Music_CORE.Models.Core.Session_Structure
{
    public class NAudioPlaybacker
    {
        #region Player Properties
        private WaveOutEvent _device;
        private AudioFileReader _reader;
        public PlaybackState PlaybackState => _device.PlaybackState;
        #endregion

        #region Current Track Properties
        public Track? CurrentTrack { get; private set; }
        public string Title { get; private set; }
        public float Volume { get; private set; }
        public TimeSpan TotalTime { get; private set; }
        public TimeSpan CurrentTime { get; private set; }
        #endregion

        #region Events
        public event Action TrackFinished;
        #endregion


        #region Ctor
        public NAudioPlaybacker()
        {
        }

        public void SetTrack(Track inputTrack, float? volume)
        {
            if (_device != null || _reader != null)
            {
                Task.Run(() => _device?.Stop());

                while(true)
                {
                    if (_device == null && _reader == null)
                        break;
                }
            }

            CurrentTrack = inputTrack;
            Volume = volume ?? 25f;
            ReadTrack();
        }
        #endregion

        #region Main Methods
        private void ReadTrack()
        {
            Title = CurrentTrack.Name;
            TotalTime = CurrentTrack.Duration;
        }

        public void Play()
        {
            if (_device == null)
            {
                _device = new();
                _device.PlaybackStopped += OnPlaybackStopped;
            }
            if (_reader == null)
            {
                _reader = new(CurrentTrack.Pathway);
                _device.Init(_reader);
            }
            _device.Play();
            Process();
        }

        public void Pause()
        {
            if (_device.PlaybackState == PlaybackState.Paused)
                _device.Play();
            else if (_device.PlaybackState == PlaybackState.Playing)
                _device.Pause();
            else
                return;
        }

        public void Stop()
        {
            if (_device != null)
                _device?.Stop();
        }

        private void Process()
        {
            while (_device.PlaybackState == PlaybackState.Playing)
            {
                CurrentTime = _reader.CurrentTime;
                if (!(_reader.CurrentTime < TotalTime) || _device.PlaybackState == PlaybackState.Stopped)
                {
                    TrackFinished?.Invoke();
                    break;
                }
            }
        }

        public void OnVolumeChanged(float volume)
        {
            if (_device != null)
                _device.Volume = volume;
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (_device != null) 
            {
                _device.Dispose();
                _device = null;
            }
            if (_reader != null) 
            {
                _reader.Dispose();
                _reader = null;
            }

        }
        #endregion
    }
}
