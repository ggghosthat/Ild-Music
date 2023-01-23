using NAudio.Wave;
using NAudio;
using System;
using System.Threading.Tasks;
using ShareInstances.PlayerResources;
using System.Diagnostics;

namespace NAudioPlayerCore.Models.Core.Session_Structure
{
    public class NAudioPlaybacker
    {
        private readonly object obj = new object();
        #region Player Properties
        private static WaveOutEvent _device;
        private static AudioFileReader _reader;
        public PlaybackState PlaybackState => _device.PlaybackState;
        #endregion

        #region Current Track Properties
        public Track? CurrentTrack { get; private set; }
        public string Title { get; private set; }
        public float Volume { get; private set; }
        public TimeSpan TotalTime { get; private set; }
        public TimeSpan CurrentTime 
        {
            get 
            {
                if (_reader != null)
                    return _reader.CurrentTime;
                else 
                    return TimeSpan.Zero;
            }
            set => _reader.CurrentTime = value;
        }
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
                while(true)
                {
                    _device?.Stop();
                    if (_device == null && _reader == null)
                        break;
                }
            }

            CurrentTrack = inputTrack;
            Volume = volume ?? 25f;
            BuildPlayer();
        }       
        #endregion

        #region Main Methods
        private void BuildPlayer()
        {
            Title = CurrentTrack.Name;
            TotalTime = CurrentTrack.Duration;

            if (_device == null)
            {
                _device = new();
                _device.PlaybackStopped += OnPlaybackStopped;
            }
            if (_reader == null)
            {
                _reader = new(CurrentTrack.Pathway);
                var wc = new WaveChannel32(_reader);
                wc.PadWithZeroes = false;
                _device.Init(wc);
            }
        }

        public void Play()
        {
            if (CurrentTrack != null && _device != null && _reader != null)
            {            
                _device?.Play();
                Process();
            }
        }

        public void Pause()
        {
            if (_device.PlaybackState == PlaybackState.Paused)
                _device.Play();
            else if (_device.PlaybackState == PlaybackState.Playing)
                _device.Pause();
        }

        public void Stop()
        {
            if (_device != null)
                _device?.Stop();    
        }

        private void Process()
        {
            while (_device.PlaybackState != PlaybackState.Stopped)
            {
                if (!(_reader.CurrentTime.TotalSeconds <= TotalTime.TotalSeconds)
                    break;
            }
            TrackFinished.Invoke();    
        }

        public void Repeat()
        {
            if (CurrentTrack != null)
                _reader.Position = 0;
        }

        public void ResetTime(double resetTime)
        {
            if (CurrentTrack != null)
            {
                _device.Stop();
                _reader.CurrentTime = TimeSpan.FromSeconds(resetTime);
                _device.Play();
            }
        }

        public void OnVolumeChanged(float volume)
        {
            if (_device != null)
                _device.Volume = volume;
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if(_device != null)
            {
                _device.Dispose();
                _device = null;
            }
            if(_reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }
        }
        #endregion
    }
}
