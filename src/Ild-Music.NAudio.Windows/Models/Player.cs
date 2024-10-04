using Ild_Music.Core.Contracts;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Events;
using Ild_Music.Core.Events.Signals;

using NAudio.Wave;

namespace Ild_Music.NAudio.Windows;
public class NAudioPlayer : IPlayer
{
    private IEventBag _eventBag = default;
    private static NAudioPlaybacker _audioPlayer = new();
    private static Action notifyAction;
    public event Action TrackStarted;

    private List<string> supportedAudioMimeTypes;
        
    public NAudioPlayer()
    {}

    public Guid PlayerId => Guid.NewGuid();

    public string PlayerName => "NAudio Player";

    public CurrentEntity CurrentEntity {get; private set;}

    public Track? CurrentTrack { get; private set; }
    
    public Playlist? CurrentPlaylist { get; private set; }
    
    public int PlaylistPoint { get; private set; }
    
    public int PlaylistCount { get; private set; }
        
    public bool IsEmpty => _audioPlayer.IsEmpty;

    public bool IsSwipe { get; private set; } = false;

    public bool IsPlaylist { get; private set;}

    public bool IsPlaylistLoop {get; set;} = false;

    public bool ToggleState => (_audioPlayer.PlaybackState == PlaybackState.Playing)?true:false;
    
    public float MaxVolume {get; private set;} = 1;

    public float MinVolume {get; private set;} = 0;

    public float CurrentVolume
    {
        get => _audioPlayer.Volume;
        set => _audioPlayer.OnVolumeChanged(value);
    }

    public TimeSpan TotalTime => _audioPlayer.TotalTime;

    public TimeSpan CurrentTime
    {
        get => _audioPlayer.CurrentTime;
        set => _audioPlayer.CurrentTime = value;
    }

    public void InjectEventBag(IEventBag eventBag)
    {
        _eventBag = eventBag;
    }
    
    public Task DropTrack(Track track)
    {
        CurrentTrack = track;
        _audioPlayer.SetInstance(track);

        var action = _eventBag?.GetAction((int)PlayerSignal.PLAYER_SET_TRACK);
        action?.DynamicInvoke();
        return Task.CompletedTask;
    }

    public async Task DropPlaylist(Playlist playlist, int index=0)
    {
        IsSwipe = true;
        IsPlaylist = true;
        PlaylistPoint = index;
        CurrentPlaylist = playlist;
        PlaylistCount = playlist.Count;        

        if (PlaylistPoint < playlist.Count)
        {
            var track =  (Track)playlist[PlaylistPoint];
            CurrentTrack = track;
            _audioPlayer.SetInstance(track);
            _audioPlayer.TrackFinished += SkipNext;
        }

        var action = _eventBag?.GetAction((int)PlayerSignal.PLAYER_SET_PLAYLIST);
        action?.DynamicInvoke();
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
        Task.Run(() => _audioPlayer.Toggle());
    }

    public void Stop()
    {
        IsSwipe = false;
        IsPlaylist = false;

        _audioPlayer.Stop();
        CurrentPlaylist?.EraseTracks();

        var action = _eventBag?.GetAction((int)PlayerSignal.PLAYER_OFF);
        action?.DynamicInvoke();
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

    public void SkipPrev()
    {
        if (!IsPlaylist)
            return;

        Task.Run(async () => {
            DropMediaInstance(false);
            await _audioPlayer.Toggle();
            var action = _eventBag?.GetAction((int)PlayerSignal.PLAYER_SHIFT_LEFT);
            action?.DynamicInvoke();
        });
    }

    public void SkipNext()
    {
        if (!IsPlaylist)
            return;

        Task.Run(async () => {
            DropMediaInstance(true);
            await _audioPlayer.Toggle();
            var action = _eventBag?.GetAction((int)PlayerSignal.PLAYER_SHIFT_LEFT);
            action?.DynamicInvoke();
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
        var track = (Track)CurrentPlaylist?[PlaylistPoint];
        CurrentTrack = track;
        _audioPlayer.SetInstance(track);
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
}