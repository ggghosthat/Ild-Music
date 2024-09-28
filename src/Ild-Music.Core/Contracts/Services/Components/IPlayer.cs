using Ild_Music.Core.Instances;

namespace Ild_Music.Core.Contracts;

//Represent Player instance
public interface IPlayer : IShare
{
    public Guid PlayerId { get; }
    
    public string PlayerName { get; }

    public Track? CurrentTrack { get; }
    
    public Playlist? CurrentPlaylist {get;}

    public bool IsSwipe { get; }

    public bool IsEmpty { get; }
    
    public bool ToggleState { get; }
    
    public int PlaylistPoint {get;}
    
    public TimeSpan TotalTime { get; }
    
    public TimeSpan CurrentTime { get; set; }
    
    public float MaxVolume {get;}
    
    public float MinVolume {get;}
    
    public float CurrentVolume {get; set;}
    
    public Task DropTrack(Track track);

    public Task DropPlaylist(Playlist playlist, int index=0);

    public Task DropNetworkStream(ReadOnlyMemory<char> uri);

    public Task<IEnumerable<string>> GetSupportedMimeTypes();

    public void Stop();

    public void Toggle();

    public void SkipPrev();

    public void SkipNext();
    
    public Task Repeat();

    public Task Shuffle();
}
