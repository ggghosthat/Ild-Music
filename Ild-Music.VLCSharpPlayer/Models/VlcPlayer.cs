using Ild_Music.Core.Contracts;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Events.Signals;

using System;
using System.Threading.Tasks;
using MediatR;
namespace Ild_Music.VlcPlayer;
public class VlcPlayer : IPlayer
{
    private static readonly VlcPlayerService _playerService = new();

    private IMediator _mediator = default;

    public Guid PlayerId => Guid.NewGuid();
    public string PlayerName => "Vlc Player";
    
    public Track? CurrentTrack {get; private set;} = null;
    public Playlist? CurrentPlaylist { get; private set;} = null;

    #region Player Triggers
    public bool IsEmpty => _playerService.IsEmpty;
    public bool ToggleState => _playerService.ToggleState;
    public int PlaylistPoint {get; private set;} = 0;
    public int PlaylistCount {get; private set;} = 0;

    public bool IsSwipe {get; private set;} = false;
    public bool IsPlaylistLoop {get; set;} = false;
    private bool IsPlaylist = false;
    #endregion

    #region Time Presenters
    public TimeSpan TotalTime => _playerService.TotalTime;
    public TimeSpan CurrentTime 
    {
        get => _playerService.CurrentTime;
        set => _playerService.CurrentTime = value;
    }
    #endregion

    #region Volume Presenters
    public float MaxVolume {get; private set;} = 100;
    public float MinVolume {get; private set;} = 0;
    public float CurrentVolume 
    {
        get => _playerService.CurrentVolume;
        set => _playerService.CurrentVolume = (int)value;
    }
    #endregion

    #region Events
    private event Action ShuffleCollection;
    #endregion

    #region constructor
    public VlcPlayer()
    {}
    #endregion


    #region Player Inits
    public void ConnectMediator(IMediator mediator)
    {
        _mediator = mediator;
    }


    public async Task DropTrack(Track track)
    {            
        CurrentTrack = track;
        await _playerService.SetTrack(track);
        await _mediator.Publish(PlayerSignal.PLAYER_SET_TRACK);
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
        await _mediator.Publish(PlayerSignal.PLAYER_SET_PLAYLIST);
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
        _mediator.Publish(PlayerSignal.PLAYER_OFF);
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
                await _mediator.Publish(PlayerSignal.PLAYER_SHIFT_LEFT);
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
                await _mediator.Publish(PlayerSignal.PLAYER_SHIFT_RIGHT);
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
    #endregion
}
