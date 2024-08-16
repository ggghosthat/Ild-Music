using Ild_Music.Core.Instances;

using NAudio.Wave;
using System;

namespace NAudioPlayerCore.Models;
public class NAudioPlaybacker
{
    private readonly object obj = new ();
    private static WaveOutEvent _device;
    private static AudioFileReader _reader;
    private const float DEFAULT_VOLUME = 0.5f;
    public event Action TrackFinished;

    public NAudioPlaybacker()
    {}

    public PlaybackState PlaybackState => (_device != null)
        ?_device.PlaybackState
        :PlaybackState.Stopped;

    public bool IsEmpty = true;

    public Track? CurrentTrack { get; private set; }
    
    public ReadOnlyMemory<char> Title { get; private set; }
    
    public float Volume { get; private set; } = DEFAULT_VOLUME;
    
    public TimeSpan TotalTime { get; private set; }
    
    public TimeSpan CurrentTime
    {
        get 
        {
            return (_reader != null)
                ? _reader.CurrentTime
                : TimeSpan.Zero;
        }
        set => _reader.CurrentTime = value;
    }
    
    public void SetInstance(Track? track)
    {
        if (track == null)
            return;

        do
            _device?.Stop();
        while(_device == null && _reader == null);
        
        IsEmpty = false;
        CurrentTrack = track;
        Title = track?.Name.ToArray();
        TotalTime = track?.Duration ?? default;

        BuildPlayer();
    }

    private void BuildPlayer()
    {
        if (_device == null)
        {
            _device = new();
            _device.PlaybackStopped += OnPlaybackStopped;
        }
        if (_reader == null)
        {
            string path = CurrentTrack?.Pathway.ToString();
            _reader = new(path);
            var wc = new WaveChannel32(_reader);
            wc.PadWithZeroes = false;
            _device.Init(wc);
            _device.Volume = Volume;
        }
    }

    public void Toggle()
    {
        bool isActiveDevice = _device != null;
        bool isActiveReader = _reader != null;

        if (!IsEmpty || !isActiveDevice && !isActiveReader)
            return;

        if (_device.PlaybackState == PlaybackState.Stopped || _device.PlaybackState == PlaybackState.Paused)
        {
            bool isRunningDevice = _device.PlaybackState != PlaybackState.Stopped;
            bool isEndReached = _reader.CurrentTime.TotalMilliseconds == TotalTime.TotalMilliseconds; 

            _device?.Play(); 
            while (isRunningDevice || isEndReached);
            TrackFinished?.Invoke();
        }
        else 
        {
            _device?.Pause();
        }
    }

    public void Stop()
    {
        _device?.Stop();
    }

    public void Repeat()
    {
        if (!IsEmpty)
            _reader.Position = 0;
    }

    public void ResetTime(double resetTime)
    {
        if (!IsEmpty)
        {
            _device.Stop();
            _reader.CurrentTime = TimeSpan.FromSeconds(resetTime);
            _device.Play();
        }
    }

    public void OnVolumeChanged(float volume)
    {
        if (_device != null)
        {
            _device.Volume = volume;
            Volume = volume;
        }
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
        IsEmpty = true;
    }
}