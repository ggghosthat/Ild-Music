using Ild_Music.Core.Contracts;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Events;
using Ild_Music.Core.Events.Signals;

namespace Ild_Music.VlcPlayer.Linux;

public class VlcPlayer : IPlayer
{
    private IEventBag _eventBag;
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
        IsSwipe = true;
        IsPlaylist = true;
        PlaylistPoint = index;
        CurrentPlaylist = playlist;
        PlaylistCount = playlist.Count;

        _playerService.TrackFinished += MovePlaylistForward;
        var action = _eventBag.GetAction((int)PlayerSignal.PLAYER_SET_PLAYLIST);
        action?.DynamicInvoke();
        
        CurrentTrack = playlist[PlaylistPoint];
        await _playerService.SetTrack(CurrentTrack);
    }

    public async Task DropNetworkStream(ReadOnlyMemory<char> uri)
    {}

    public Task<IEnumerable<string>> GetSupportedMimeTypes()
    {
        var result = new List<string>()
        {
            "audio/wav",         // WAV
            "audio/x-wav",        // WAV
            "audio/x-aiff",      // AIFF
            "sound/aiff",        // AIFF
            "audio/x-pn-aiff",   // AIFF with LPCM audio
            "audio/mpeg",        // MP3
            "audio/mp3",         // MP3
            "audio/x-ms-wma",    // WMA
            "audio/mp4",         // AAC/MP4
            "audio/x-m4a",        // AAC/MP4
            "audio/ogg",         // OGG Vorbis
            "audio/flac",        // FLAC
            "audio/x-gsm",       // GSM
            "audio/x-midi",      // MIDI
            "audio/midi",        // MIDI
            "audio/x-mod",       // Module Music Formats
            "audio/vnd.qcelp",   // Qualcomm PureVoice
            "audio/x-rmf"        // Rich Music Format
        };

        return Task.FromResult<IEnumerable<string>>(result);
    }

    public void Toggle()
    {
        Task.Run(async () => await _playerService.Toggle());
    }

    public void Stop()
    {
        IsSwipe = false;
        IsPlaylist = false;
        CurrentPlaylist?.EraseTracks();
        Task.Run(async () => await _playerService.Stop());
        var action = _eventBag.GetAction((int)PlayerSignal.PLAYER_OFF);
        action?.DynamicInvoke();
    }

    public async Task Repeat()
    {}

    public async Task Shuffle()
    {}

    public void SkipPrev()
    {
        if (!IsPlaylist)
            return;

        Task.Run(async () => 
        {
            MovePlaylistBack();
            await _playerService.Toggle();
            var action = _eventBag.GetAction((int)PlayerSignal.PLAYER_SHIFT_LEFT);
            action?.DynamicInvoke();
        });
    }

    public void SkipNext()
    {
        if(!IsPlaylist)
            return;
        
        Task.Run(async () =>
        {
            MovePlaylistForward();
            await _playerService.Toggle();
            var action = _eventBag.GetAction((int)PlayerSignal.PLAYER_SHIFT_RIGHT);
            action?.DynamicInvoke();
        });
    }

    private void MovePlaylistBack()
    {
        if(IsSwipe && CurrentPlaylist != null)
        {
            DragPointer(false);
            SetPlaylistTrack();
            _playerService.SetTrack(CurrentTrack).Wait();
        }
    }

    private void MovePlaylistForward()
    {
        if(IsSwipe && CurrentPlaylist != null)
        {
            DragPointer(true);
            SetPlaylistTrack();
            _playerService.SetTrack(CurrentTrack).Wait();
        }
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

    private void SetPlaylistTrack()
    {
        CurrentTrack = (Track)CurrentPlaylist?[PlaylistPoint];        
    }
}
