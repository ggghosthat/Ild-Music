using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Statistics;

using MediatR;
namespace Ild_Music.Core.Contracts;
public interface IShare
{
    void ConnectMediator(IMediator mediator);
}

//Represent Cube instance wich interacts with file system
public interface ICube : IShare
{
    public Guid CubeId { get; }
    public string CubeName { get; }

    public int CubePage {get;}

    #region ToggleMethods
    void SetPath(ref string inputPath); 
    void Init();
    #endregion

    #region ResourceCollections
    public IEnumerable<CommonInstanceDTO>? Artists {get;}
    public IEnumerable<CommonInstanceDTO>? Playlists { get; }
    public IEnumerable<CommonInstanceDTO>? Tracks { get; }        
    #endregion

    #region Command methods
    public Task AddArtistObj(Artist artist);
    public Task AddTrackObj(Track artist);
    public Task AddPlaylistObj(Playlist artist);

    public Task EditArtistObj(Artist newArtist);
    public Task EditPlaylistObj(Playlist newPlaylist);
    public Task EditTrackObj(Track newTrack);

    public Task RemoveArtistObj(Guid artistId);
    public Task RemoveTrackObj(Guid trackId);
    public Task RemovePlaylistObj(Guid playlistId);
    #endregion 

    #region Query methods
    public Task LoadUp();
    public Task<IEnumerable<CommonInstanceDTO>> LoadEntities(EntityTag entityTag, int offset);
    public Task<IEnumerable<Tag>> LoadTags(int offset);
    #endregion
       
    #region Statistic data
    public Task<CounterFrame> SnapCounterFrame();
    #endregion

    #region Search
    public Task<IEnumerable<T>> Search<T>(ReadOnlyMemory<char> searchTerm);
    #endregion
}

//Represent Player instance
public interface IPlayer : IShare
{
    #region Identifiers
    public Guid PlayerId { get; }
    public string PlayerName { get; }
    #endregion

    #region Current entity
    public Track? CurrentTrack { get; }
    public Playlist? CurrentPlaylist {get;}
    #endregion

    #region State attributes
    public bool IsSwipe { get; }
    public bool IsEmpty { get; }
    public bool ToggleState { get; }
    public int PlaylistPoint {get;}
    #endregion

    #region Time attributes
    public TimeSpan TotalTime { get; }
    public TimeSpan CurrentTime { get; set; }
    #endregion

    #region Volume attributes
    public float MaxVolume {get;}
    public float MinVolume {get;}
    public float CurrentVolume {get; set;}
    #endregion

    #region Set entities methods
    public Task DropTrack(Track track);

    public Task DropPlaylist(Playlist playlist, int index=0);

    public Task DropNetworkStream(ReadOnlyMemory<char> uri);
    #endregion

    #region main functionallity
    public void Stop();

    public void Toggle();

    public Task Repeat();

    public void SkipPrev();

    public void SkipNext();

    public Task Shuffle();
    #endregion
}
