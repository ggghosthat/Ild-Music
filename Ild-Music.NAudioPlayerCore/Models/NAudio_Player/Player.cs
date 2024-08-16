using Ild_Music.Core.Contracts;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Events;
using Ild_Music.Core.Events.Signals;

using NAudio.Wave;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NAudioPlayerCore.Models;
public class NAudioPlayer : IPlayer
{
    private IEventBag _eventBag = default;
    public static NAudioPlaybacker _audioPlayer = new();
    private static Action notifyAction;    
    private event Action ShuffleCollection;
    public event Action TrackStarted;

    public NAudioPlayer()
    {}

    public Guid PlayerId => Guid.NewGuid();
    public string PlayerName => "NAudio Player";

    public CurrentEntity CurrentEntity {get; private set;}
    public Track? CurrentTrack { get; private set; }
    public Playlist? CurrentPlaylist { get; private set; }
    public int PlaylistPoint { get; private set; }
    public int PlaylistCount { get; private set; }
    
    public TimeSpan TotalTime => _audioPlayer.TotalTime;

    public TimeSpan CurrentTime
    {
        get => _audioPlayer.CurrentTime; 
        set => _audioPlayer.CurrentTime = value; 
    }
    
    public bool IsEmpty => _audioPlayer.IsEmpty;

    public bool IsSwipe { get; private set; } = false;

    public bool IsPlaylist { get; private set;}

    public bool IsPlaylistLoop {get; set;} = false;

    public bool ToggleState => (_audioPlayer.PlaybackState == PlaybackState.Stopped)?true:false;
    
    public float MaxVolume {get; private set;} = 1;

    public float MinVolume {get; private set;} = 0;

    public float CurrentVolume 
    {
        get => _audioPlayer.Volume;
        set => _audioPlayer.OnVolumeChanged(value);
    }

    public void InjectEventBag(IEventBag eventBag)
    {
        _eventBag = eventBag;
    }
    
    public Task DropTrack(Track track)
    {
        CurrentTrack = track;
        _audioPlayer.SetInstance(CurrentTrack);
        var action = _eventBag.GetAction((int)PlayerSignal.PLAYER_SET_TRACK);
        action?.DynamicInvoke();
        
        return Task.CompletedTask;
    }

    public Task DropPlaylist(Playlist playlist, int index=0)
    {
        IsSwipe = true;
        IsPlaylist = true;
        PlaylistPoint = index;
        CurrentPlaylist = playlist;
        PlaylistCount = playlist.Count;        

        CurrentTrack = playlist[PlaylistPoint];
        _audioPlayer.SetInstance(CurrentTrack);
        _audioPlayer.TrackFinished += SkipNext;

        var action = _eventBag.GetAction((int)PlayerSignal.PLAYER_SET_PLAYLIST);
        action?.DynamicInvoke();
        return Task.CompletedTask;
}

    public async Task DropNetworkStream(ReadOnlyMemory<char> uri)
    {}

    public void SetNotifier(Action callBack)
    {
        notifyAction = callBack;
    }
 
    public void Toggle()
    {
        Task.Run(() => _audioPlayer.Toggle());
        notifyAction?.Invoke(); 
    }

    public void Stop()
    {
        Task.Run(() => _audioPlayer.Stop());
        notifyAction?.Invoke(); 
    }
    
    public async Task Repeat()
    {}

    public async Task Shuffle()
    {}
       
    public async Task ChangeVolume(float volume)
    {
        _audioPlayer.OnVolumeChanged(volume);
    }

    public async Task RepeatTrack()
    {
        await Task.Run(() => _audioPlayer.Repeat());
    }

    public async void SkipPrev()
    {
        await Task.Run(() => {
            if (IsSwipe && !IsEmpty)
                DropMediaInstance(false);
        });
    }

    public async void SkipNext()
    {
        await Task.Run(() => {
            if (IsSwipe && !IsEmpty)
                DropMediaInstance(true);
        });
    }

    private void DropMediaInstance(bool direct)
    {
        _audioPlayer.TrackFinished -= SkipNext;
        _audioPlayer.Stop();

        notifyAction?.Invoke();
        
        DragPointer(direct);   
        SetMedia();

        notifyAction?.Invoke();
    }

    private void SetMedia()
    {
        CurrentTrack = CurrentPlaylist?[PlaylistPoint];
        _audioPlayer.SetInstance(CurrentTrack);
        Task.Run(() => _audioPlayer.Toggle());
    }

    private void DragPointer(bool direction)
    {
        if (!IsPlaylistLoop)
            return;
     
        if (direction)
        {
            if (PlaylistPoint == PlaylistCount - 1)
                PlaylistPoint = 0;
            else
                PlaylistPoint++;
        }
        else 
        {
            if (PlaylistPoint == 0)
                PlaylistPoint = PlaylistCount - 1;
            else
                PlaylistPoint--;
        }
    }

    private void CleanUpPlayer()
    {
        PlaylistPoint = 0;
        notifyAction?.Invoke();
        
        _audioPlayer.TrackFinished -= SkipNext;
        _audioPlayer.Stop();

    }
}