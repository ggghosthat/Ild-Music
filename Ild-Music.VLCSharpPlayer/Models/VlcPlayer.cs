using Ild_Music.Core.Contracts;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Events;
using Ild_Music.Core.Events.Entity;
using Ild_Music.Core.Events.Signals;

using System;
using System.Threading.Tasks;

namespace Ild_Music.VlcPlayer;

public class VlcPlayer : IPlayer
{
    private IEventBag _eventBag = default;    
    private bool IsPlaylist = false;

    private static readonly VlcPlayerService _playerService = new();    

    public VlcPlayer()
    {}

    public Guid PlayerId => Guid.NewGuid();
    public string PlayerName => "VLC Player";
    
    public Track? CurrentTrack {get; private set;} = null;
    public Playlist? CurrentPlaylist { get; private set;} = null;

    public bool IsEmpty => _playerService.IsEmpty;
    public bool ToggleState => _playerService.ToggleState;
    public int PlaylistPoint {get; private set;} = 0;
    public int PlaylistCount {get; private set;} = 0;

    public bool IsSwipe {get; private set;} = false;
    public bool IsPlaylistLoop {get; set;} = false;

    public TimeSpan TotalTime => _playerService.TotalTime;
    public TimeSpan CurrentTime 
    {
        get => _playerService.CurrentTime;
        set => _playerService.CurrentTime = value;
    }

    public float MaxVolume {get; private set;} = 100;
    public float MinVolume {get; private set;} = 0;
    public float CurrentVolume 
    {
        get => _playerService.CurrentVolume;
        set => _playerService.CurrentVolume = (int)value;
    }

    private event Action ShuffleCollection;

    public void InjectEventBag(IEventBag eventBag)
    {
        _eventBag = eventBag;
    }

    public async Task DropTrack(Track track)
    {            
        CurrentTrack = track;
        await _playerService.SetTrack(track);
        var action = _eventBag.GetAction((int)PlayerSignal.PLAYER_SET_TRACK);
        action?.DynamicInvoke();
    } 

    public async Task DropPlaylist(Playlist playlist, int index=0)
    { 
        //main playlist params initialization
        PlaylistPoint = index;
        CurrentPlaylist = playlist;
        var startTrack = playlist[PlaylistPoint];
        CurrentTrack = startTrack;

        //side plalist params initialization
        PlaylistCount = playlist.Count;
        IsSwipe = true;
        IsPlaylist = true;

        //player service setting
        _playerService.TrackFinished += async () => await SetNewMediaInstance(true); 
        await _playerService.SetTrack(startTrack);
        var action = _eventBag.GetAction((int)PlayerSignal.PLAYER_SET_PLAYLIST);
        action?.DynamicInvoke();
    }

    public async Task DropNetworkStream(ReadOnlyMemory<char> uri)
    {}

    public void Toggle()
    {
        Task.Run(async () => await _playerService.Toggle());
    }

    public void Stop()
    {
        IsSwipe = false;
        IsPlaylist = false;
        Task.Run(async () => await _playerService.Stop());
        var playerEvent = new PlayerEvent(PlayerSignal.PLAYER_OFF);
    }

    public async Task Repeat()
    {}

    public async Task Shuffle()
    {}


    public void SkipPrev()
    {
        if (IsPlaylist)
        {
            Task.Run(async () => 
            {
                await SetNewMediaInstance(false);
                await _playerService.Toggle();
                var playerEvent = new PlayerEvent(PlayerSignal.PLAYER_SHIFT_LEFT);
            });
        }
    }

    public void SkipNext()
    {
        if(IsPlaylist)
        {
            Task.Run(async () =>
            {
                await SetNewMediaInstance(true);
                await _playerService.Toggle();
                var playerEvent = new PlayerEvent(PlayerSignal.PLAYER_SHIFT_RIGHT);
            });
        }
    }

    private async Task SetNewMediaInstance(bool direct)
    {
        if(IsSwipe && CurrentPlaylist is not null)
        {
            DragPointer(direct);
   
            var newTrack = (Track)CurrentPlaylist?[PlaylistPoint];
            CurrentTrack = newTrack;
            await _playerService.SetTrack(newTrack);
        }
    }

    private void DragPointer(bool direction)
    {
        if (direction)
        {
            if (PlaylistPoint == PlaylistCount - 1)
            {
                if (!IsPlaylistLoop) return;
                else PlaylistPoint = 0;
            }
            else
                PlaylistPoint++;
        }
        else 
        {
            if (PlaylistPoint == 0)
            {
                if (!IsPlaylistLoop) return;
                else PlaylistPoint = PlaylistCount - 1;
            }
            else
            {
                PlaylistPoint--;
            }
        }
    }
}
