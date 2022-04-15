using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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

        public event Action EndRiched;
        private bool isWatch = true;


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
            ReadTrack();
        }

        private void ReadTrack()
        {
            this.title = currentTrack.Name;
            this.totalTime = currentTrack.Duration;
        }



        public void Play(bool isWatch = true)
        {
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (_reader == null)
            {
                _reader = new AudioFileReader(currentTrack.Pathway);
                outputDevice.Init(_reader);
            }

            outputDevice.Play();
        }

        public void Stop()
        {
            if (outputDevice != null)
                outputDevice?.Stop();
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

        private void Watch()
        {
            while (isWatch)
            {
                if (_reader != null) 
                {
                    if (_reader.CurrentTime.Equals(_reader.TotalTime)) 
                    {
                        EndRiched.Invoke();
                        break;
                    }
                }
            }
        }

    }
}
