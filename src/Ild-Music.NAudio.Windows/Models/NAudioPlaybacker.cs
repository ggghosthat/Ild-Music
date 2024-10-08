using Ild_Music.Core.Instances;

using NAudio.Wave;

namespace Ild_Music.NAudio.Windows;

public class NAudioPlaybacker
{
    private static WaveOutEvent _device;
    
    private static AudioFileReader _reader;
    
    private const float DEFAULT_VOLUME = 0.5f;

    public event Action TrackFinished;

    public NAudioPlaybacker()
    {}

    public PlaybackState PlaybackState => (_device != null)
        ? _device.PlaybackState
        : PlaybackState.Stopped;

    public bool IsEmpty = true;

    public Track? CurrentTrack { get; private set; }
    
    public ReadOnlyMemory<char> Title { get; private set; }
    
    public float Volume { get; private set; }
    
    public TimeSpan TotalTime { get; private set; } = TimeSpan.FromMilliseconds(1);
    
    public TimeSpan CurrentTime
    {
        get => (_reader != null) ? _reader.CurrentTime : TimeSpan.Zero;
        set => _reader.CurrentTime = value;
    }
    
    public void SetInstance(Track track)
    {   
        CleanPlayer();
        ReadTrack(track);
        BuildPlayer(track.Pathway);
    }

    private void ReadTrack(Track track)
    {
        IsEmpty = false;
        CurrentTrack = track;
        Title = track.Name.ToArray();
        TotalTime = track.Duration;
    }

    private void BuildPlayer(ReadOnlyMemory<char> path)
    {
        if (_device == null)
        {
            _device = new ();
            _device.PlaybackStopped += OnPlaybackStopped;
        }
        if (_reader == null)
        {
            _reader = new (path.ToString());
            _device.Init(_reader);
        }
        
        _device.Volume = Volume;
        IsEmpty = false;
    }

    public Task Toggle()
    {
        bool isActivePlayer = _device != null && _reader != null;

        if (!isActivePlayer)
            return Task.CompletedTask;

        if (_device?.PlaybackState != PlaybackState.Playing)
        {
            bool isPlaying = _reader.CurrentTime.TotalMilliseconds < TotalTime.TotalMilliseconds;

            _device?.Play();
     
            while (isPlaying && isActivePlayer);
        
            _device?.Stop();
            IsEmpty = true;
            TrackFinished?.Invoke();
        }
        else if (_device.PlaybackState == PlaybackState.Playing)
        {
            IsEmpty = false;
            _device?.Pause();
        }

        return Task.CompletedTask;
    }

    public async void Stop()
    {
        await Task.Run( () => 
        {
            _device?.Stop();
            IsEmpty = true;
        });
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
        CleanPlayer();
    }

    private void CleanPlayer()
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