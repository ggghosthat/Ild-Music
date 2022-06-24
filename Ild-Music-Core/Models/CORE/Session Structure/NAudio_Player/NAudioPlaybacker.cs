using NAudio.Wave;
using System;
using System.Threading.Tasks;
using ShareInstances.PlayerResources;

namespace Ild_Music_CORE.Models.Core.Session_Structure
{
    public class NAudioPlaybacker
    {
        Task task;

        private Track currentTrack;

        private WaveOutEvent outputDevice;
        private AudioFileReader _reader;

        private string title;
        private TimeSpan totalTime;

        public event Action TrackFinished;
        private bool isWatch = true;

        private float volume = 25;

        public Track? CurrentTrack => currentTrack ?? null;

        public PlaybackState PlayerState => (outputDevice is null) ? PlaybackState.Stopped : outputDevice.PlaybackState;



        public NAudioPlaybacker(Track inputTrack, float volume)
        {
            currentTrack = inputTrack;
            this.volume = volume;
            ReadTrack();
        }




        #region Main Methods
        private void ReadTrack()
        {
            title = currentTrack.Name;
            totalTime = currentTrack.Duration;
        }

        public void Play()
        {
            if (outputDevice == null)
            {
                outputDevice = new ();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (_reader == null)
            {
                _reader = new (currentTrack.Pathway);
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
                
                if (!(_reader.CurrentTime < totalTime) || outputDevice.PlaybackState == PlaybackState.Stopped)
                {                 
                    TrackFinished?.Invoke();
                    break;
                }         
                continue;
                
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

        //These method control timespan of current track
        //if timespan riched to total time Finish event raizing up
        private void Watch()
        {
            while (isWatch)
            {
                if (_reader != null) 
                {
                    if (_reader.CurrentTime.Equals(_reader.TotalTime)) 
                    {
                        TrackFinished.Invoke();
                        break;
                    }
                }
            }
        }

    }
}
