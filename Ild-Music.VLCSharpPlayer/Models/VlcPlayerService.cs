using ShareInstances;
using ShareInstances.Instances;

using System;
using System.Threading;
using System.Threading.Tasks;
using LibVLCSharp;
using LibVLCSharp.Shared;
namespace IldMusic.VlcPlayer;
internal class VlcPlayerService
{
    #region VLCSharp Instances
    private static readonly LibVLC _vlc = new(); 
    private static MediaPlayer _mediaPlayer;
    #endregion

    public Track? CurrentTrack {get; private set;} = null;
    private Media currentMedia = null;

    #region Player Triggers
    public bool IsEmpty {get; private set;} = true;
    public bool ToggleState {get; private set;} = false;
    #endregion

    #region Time Presenters
    public TimeSpan TotalTime {get; private set;}
    public TimeSpan CurrentTime 
    {
        get => TimeSpan.FromMilliseconds(_mediaPlayer.Time);
        set => _mediaPlayer.SeekTo(value);
    }
    #endregion

    #region Volume Presenters
    private float maxVolume = 100;
    private float minVolume = 0;
    public float CurrentVolume 
    {
        get => _mediaPlayer.Volume;
        set
        {
            if (value <= minVolume)
                _mediaPlayer.Volume = (int)minVolume;
            else if (value >= maxVolume)
                _mediaPlayer.Volume = (int)maxVolume;
            else
                _mediaPlayer.Volume = (int)value;
        }
    }
    #endregion

    #region Actions
    private Action notifyAction;
    #endregion 

    #region Events
    public event Action TrackFinished;
    public event Func<Task> TrackFinishedAsync;
    #endregion

    public VlcPlayerService()
    {
        _mediaPlayer = new (_vlc);
        _mediaPlayer.Volume = 40;
    }

    public void DefineCallback(Action callback) =>
        notifyAction = callback;

    public async Task SetTrack(Track track)
    {
       Clean();

       CurrentTrack = track;
       currentMedia = new Media(_vlc, new Uri(track.Pathway.ToString()));
       TotalTime = track.Duration;
       _mediaPlayer.Media = currentMedia; 
    }

    public async Task Toggle()
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

    public async void Seek(TimeSpan timePoint)
    {
        _mediaPlayer.SeekTo(timePoint);
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
