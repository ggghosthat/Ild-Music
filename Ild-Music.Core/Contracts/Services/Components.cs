using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Statistics;

using MediatR;
namespace Ild_Music.Core.Contracts;
public interface IShare{}

//Represent Cube instance wich interacts with file system
public interface ICube : IShare
{
    public Guid CubeId { get; }
    public string CubeName { get; }

    public int CubePage {get;}

    #region ToggleMethods
    void SetPath(ref string inputPath); 
    void Init();
    void ConnectMediator(IMediator mediator);
    #endregion

    #region ResourceCollections
    public IEnumerable<Artist> Artists {get;}
    public IEnumerable<Playlist> Playlists { get; }
    public IEnumerable<Track> Tracks { get; }        
    #endregion

    #region AddMethods
    public Task AddArtistObj(Artist artist);
    public Task AddTrackObj(Track artist);
    public Task AddPlaylistObj(Playlist artist);
    #endregion

    #region EditMethods
    public Task EditArtistObj(Artist artist);
    public Task EditTrackObj(Track artist);
    public Task EditPlaylistObj(Playlist artist);
    #endregion

    #region RemoveMethods
    public Task RemoveArtistObj(Guid artistId);
    public Task RemoveTrackObj(Guid trackId);
    public Task RemovePlaylistObj(Guid playlistId);
    #endregion 

    #region LoadMethods
    public Task LoadItems<T>();
    public Task UnloadItems<T>();
    #endregion
    
    #region InstanceRelatesChecks
    public Task<InspectFrame> CheckArtistRelates(Artist artist);
    public Task<InspectFrame> CheckPlaylistRelates(Playlist playlist);
    public Task<InspectFrame> CheckTrackRelates(Track track);
    #endregion


    #region Instance Request-Retrieve region
    public Task<IEnumerable<CommonInstanceDTO>> RequireInstances(EntityTag entityTag);
    public Task<IEnumerable<CommonInstanceDTO>> RequireInstances(EntityTag entityTag, IEnumerable<Guid> id);

    public Task<IEnumerable<Artist>> RetrieveArtists (IEnumerable<CommonInstanceDTO> dtos);
    public Task<IEnumerable<Playlist>> RetrievePlaylists (IEnumerable<CommonInstanceDTO> dtos);
    public Task<IEnumerable<Track>> RetrieveTracks (IEnumerable<CommonInstanceDTO> dtos);
    #endregion
    
    public Task<CounterFrame> SnapCounterFrame();

    public Task<IEnumerable<T>> Search<T>(ReadOnlyMemory<char> searchTerm); 
}

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

    void ConnectMediator(IMediator mediator);

    public Task DropTrack(Track track);

    public Task DropPlaylist(Playlist playlist, int index=0);

    public Task DropNetworkStream(ReadOnlyMemory<char> uri);

    public void SetNotifier(Action callBack);


    public void Stop();

    public void Toggle();

    public Task Repeat();

    public void SkipPrev();

    public void SkipNext();

    public Task Shuffle();

}
