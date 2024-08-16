using Ild_Music.Core.Instances;

using System;
using System.Threading.Tasks;
using LibVLCSharp.Shared;

namespace Ild_Music.VlcPlayer;

internal class VlcPlayerService
{
    private static readonly LibVLC _vlc = new();
    private static MediaPlayer _mediaPlayer;
    private Media currentMedia = null;
    private float maxVolume = 100;
    private float minVolume = 0;

    public VlcPlayerService()
    {
        _mediaPlayer = new (_vlc);
        _mediaPlayer.Volume = 40;
    }

    public Track? CurrentTrack {get; private set;} = null;

    public bool IsEmpty {get; private set;} = true;

    public bool ToggleState {get; private set;} = false;

    public TimeSpan TotalTime {get; private set;}

    public TimeSpan CurrentTime 
    {
        get => TimeSpan.FromMilliseconds(_mediaPlayer.Time);
        set => _mediaPlayer.SeekTo(value);
    }

    public float CurrentVolume 
    {
        get => _mediaPlayer.Volume;
        set => SetVolume(value);
    }

    private Action notifyAction;

    public event Action TrackFinished;
    public event Func<Task> TrackFinishedAsync;

    public void DefineCallback(Action callback)
    {
        notifyAction = callback;
    }

    public Task SetTrack(Track? track)
    {
        Clean();

        if (track != null)
        {
           CurrentTrack = track;
           currentMedia = new (_vlc, new Uri(track?.Pathway.ToString()));
           TotalTime = track?.Duration ?? default;
           _mediaPlayer.Media = currentMedia; 
        }

        return Task.CompletedTask;
    }

    public Task Toggle()
    {
        if (!_mediaPlayer.IsPlaying)
        {
            _mediaPlayer.Play();
            ToggleState = true;
            IsEmpty = false;
            notifyAction?.Invoke();
            
            while(_mediaPlayer.Position < 0.99f);
            
            _mediaPlayer.Stop();
            TrackFinished?.Invoke();
            ToggleState = false;
            IsEmpty = true;
            notifyAction?.Invoke();
        }
        else
        {
            ToggleState = false;
            IsEmpty = false;
            notifyAction?.Invoke();
            _mediaPlayer.Pause();
        }

        return Task.CompletedTask;
    }

    public async Task Stop()
    {
        await Task.Run(() => 
        {
            ToggleState = false;
            IsEmpty = true;
            notifyAction?.Invoke();
            _mediaPlayer.Stop();
        });
    }

    public Task Seek(TimeSpan timePoint)
    {
        _mediaPlayer.SeekTo(timePoint);

       return Task.CompletedTask;
    }

    private void SetVolume(float volume)
    {
        if (volume <= minVolume)
            _mediaPlayer.Volume = (int)minVolume;
        else if (volume >= maxVolume)
            _mediaPlayer.Volume = (int)maxVolume;
        else
            _mediaPlayer.Volume = (int)volume;
    }

    private void Clean()
    {
        if(ToggleState == true)
        {
            ToggleState = false;
            notifyAction?.Invoke();
        }

        _mediaPlayer.Stop();

        if(currentMedia != null)
        {
            currentMedia.Dispose();
            currentMedia = null;
        }

        IsEmpty = true;
    }
}
