using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using NAudio.Wave;
using System;
using System.Threading.Tasks;

namespace Ild_Music_CORE.Models.Core.Session_Structure
{
    public class AudioPlayer
    {
        private Track currentTrack;


        private WaveOutEvent outputDevice;
        private AudioFileReader _reader;

        private string title;
        private TimeSpan totalTime;

        public event Action TrackFinished;
        private bool isWatch = true;

        private float volume;

        public Track? CurrentTrack => currentTrack ?? null;

        public PlaybackState PlayerState 
        {
            get 
            {
                if (outputDevice != null) return outputDevice.PlaybackState;
                else return PlaybackState.Stopped;
            }
        }

        public AudioPlayer(Track inputTrack, float volume)
        {
            Task.Run(Watch);
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

        public void Play(bool isWatch = true)
        {
            if (outputDevice == null)
            {
                outputDevice = new ();
                outputDevice.Volume = volume;
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (_reader == null)
            {
                _reader = new (currentTrack.Pathway);
                outputDevice.Init(_reader);
            }

            outputDevice.Play();
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
