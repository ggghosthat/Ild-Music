using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Statistics;

using MediatR;
namespace Ild_Music.Core.Contracts;

public interface IShare
{
    //add pub-sub supports for all components
    public void ConnectMediator(IMediator mediator);
}

//Represent Cube instance wich interacts with file system
public interface ICube : IShare
{
    //identifiers
    public Guid CubeId { get; }
    public string CubeName { get; }
    public int CubePage {get;}
    
    //main attributes
    public IEnumerable<CommonInstanceDTO>? Artists {get;}
    public IEnumerable<CommonInstanceDTO>? Playlists { get; }
    public IEnumerable<CommonInstanceDTO>? Tracks { get; }        

    //intialize method
    public void Init(string alloationPlace, int capacity, bool isMoveTrackFiles);

    //command methods
    public Task AddArtistObj(Artist artist);
    public Task AddTrackObj(Track artist);
    public Task AddPlaylistObj(Playlist artist);

    public Task EditArtistObj(Artist newArtist);
    public Task EditPlaylistObj(Playlist newPlaylist);
    public Task EditTrackObj(Track newTrack);

    public Task RemoveArtistObj(Guid artistId);
    public Task RemoveTrackObj(Guid trackId);
    public Task RemovePlaylistObj(Guid playlistId);

    //loading (querying) methods
    public Task LoadUp();
    public Task<IEnumerable<CommonInstanceDTO>> LoadEntities(EntityTag entityTag, int offset);
    public Task<IEnumerable<Tag>> LoadTags(int offset);

    //statistic methods
    public Task<CounterFrame> SnapCounterFrame();

    //searching methods
    public Task<IEnumerable<T>> Search<T>(ReadOnlyMemory<char> searchTerm);
}

//Represent Player instance
public interface IPlayer : IShare
{
    //identifiers
    public Guid PlayerId { get; }
    public string PlayerName { get; }

    //current entity
    public Track? CurrentTrack { get; }
    public Playlist? CurrentPlaylist {get;}

    //state attributes
    public bool IsSwipe { get; }
    public bool IsEmpty { get; }
    public bool ToggleState { get; }
    public int PlaylistPoint {get;}

    //time attributes
    public TimeSpan TotalTime { get; }
    public TimeSpan CurrentTime { get; set; }

    //volume attributes
    public float MaxVolume {get;}
    public float MinVolume {get;}
    public float CurrentVolume {get; set;}

    //set entities methods
    public Task DropTrack(Track track);

    public Task DropPlaylist(Playlist playlist, int index=0);

    public Task DropNetworkStream(ReadOnlyMemory<char> uri);

    //main functionallity
    public void Stop();

    public void Toggle();

    public Task Repeat();

    public void SkipPrev();

    public void SkipNext();

    public Task Shuffle();
}
