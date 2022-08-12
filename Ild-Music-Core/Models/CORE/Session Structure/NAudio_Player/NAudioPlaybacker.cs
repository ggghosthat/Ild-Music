using NAudio.Wave;
using System;
using System.Threading.Tasks;
using ShareInstances.PlayerResources;

namespace Ild_Music_CORE.Models.Core.Session_Structure
{
    public class NAudioPlaybacker
    {
        private Track currentTrack;
        public Track? CurrentTrack => currentTrack;

        private WaveOutEvent outputDevice;
        private AudioFileReader _reader;
        public PlaybackState PlaybackState => outputDevice.PlaybackState;

        private string title;
        private float volume = 25;
        public TimeSpan TotalTime { get; private set; }
        public TimeSpan CurrentTime { get; private set; }

        public event Action TrackFinished;

        
        public NAudioPlaybacker()
        {
        }

        public void SetTrack(Track inputTrack, float volume)
        {
            if (outputDevice != null || _reader != null)
            {
                Task.Run(() => outputDevice?.Stop());

                while(true)
                {
                    if (outputDevice == null && _reader == null)
                        break;
                }
            }

            currentTrack = inputTrack;
            this.volume = volume;
            ReadTrack();
        }

        #region Main Methods
        private void ReadTrack()
        {
            title = currentTrack.Name;
            TotalTime = currentTrack.Duration;
        }

        public void Play()
        {
            if (outputDevice == null)
            {
                outputDevice = new();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (_reader == null)
            {
                _reader = new(currentTrack.Pathway);
                outputDevice.Init(_reader);
            }
            outputDevice.Play();
            Process();
        }

        public void Pause()
        {
            if (outputDevice.PlaybackState == PlaybackState.Paused)
                outputDevice.Play();
            else if (outputDevice.PlaybackState == PlaybackState.Playing)
                outputDevice.Pause();
            else
                return;
        }

        public void Stop()
        {
            if (outputDevice != null)
                outputDevice?.Stop();
        }

        private void Process()
        {
            while (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                CurrentTime = _reader.CurrentTime;
                if (!(_reader.CurrentTime < TotalTime) || outputDevice.PlaybackState == PlaybackState.Stopped)
                {
                    TrackFinished?.Invoke();
                    break;
                }
            }
        }

        public void OnVolumeChanged(float volume)
        {
            if (outputDevice != null)
                outputDevice.Volume = volume;
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (outputDevice != null) 
            {
                outputDevice.Dispose();
                outputDevice = null;
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
