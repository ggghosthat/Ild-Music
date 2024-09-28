using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Instances.Querying;

namespace Ild_Music.Core.Contracts;

//Represent Cube instance wich interacts with file system
public interface IRepository : IShare
{
    //identifiers
    public Guid CubeId { get; }
    public string CubeName { get; }
    
    public int StartGap => 300;
    public int PageSize { get; }
        
    //main attributes
    public InstancePool InstancePool { get; }
    public IEnumerable<Track> BrowsedTracks { get; }
    
    //intialize method
    public void Init(string alloationPlace, bool isMoveTrackFiles);

    //command methods
    public Task AddArtistObj(Artist artist);
    public Task AddTrackObj(Track artist);
    public Task AddPlaylistObj(Playlist artist);
    public Task AddTagObj(Tag tag);

    public Task EditArtistObj(Artist newArtist);
    public Task EditPlaylistObj(Playlist newPlaylist);
    public Task EditTrackObj(Track newTrack);
    public Task EditTagObj(Tag tag);

    public Task RemoveArtistObj(Guid artistId);
    public Task RemoveTrackObj(Guid trackId);
    public Task RemovePlaylistObj(Guid playlistId);
    public Task RemoveTagObj(Guid tagId);

    //loading (querying) methods
    public Task<Artist> QueryArtist(CommonInstanceDTO instanceDTO);
    public Task<Playlist> QueryPlaylist(CommonInstanceDTO instanceDTO);
    public Task<Track> QueryTrack(CommonInstanceDTO instanceDTO);
    public Task<Tag> QueryTag(Guid tagId);

    public Task LoadStartEntities();
    public Task<IEnumerable<CommonInstanceDTO>> LoadEntities(EntityTag entityTag);
    public Task<IEnumerable<CommonInstanceDTO>> LoadFramedEntities(EntityTag entityTag, int offset, int limit);
    public Task<IEnumerable<CommonInstanceDTO>> LoadTags();
    public Task<IEnumerable<CommonInstanceDTO>> LoadFramedTags(int offset, int limit);
    public Task<IEnumerable<CommonInstanceDTO>> LoadInstancesById(IEnumerable<Guid> ids, EntityTag entityTag);
    public Task<IEnumerable<Track>> LoadTracksById(IEnumerable<Guid> ids);
    public Task<MetricSheet> QueryCapacityMetrics();

    //searching methods
    public Task<IEnumerable<CommonInstanceDTO>> Search(string searchTerm);
    public Task<IEnumerable<CommonInstanceDTO>> SearchInstance(string searchTerm, EntityTag entityTag);
    public Task<IEnumerable<CommonInstanceDTO>> SearchTag(string searchTerm);

    //warehouse Track API
    public string GetTrackPathFromId(Guid id);
    public IEnumerable<string> GetTrackPathsFromId(IEnumerable<Guid> ids);
    public void PlaceTrackFile(Track track);
    public void PlaceTrackFiles(IEnumerable<Track> track);

    //warehouse Avatar API
    public string GetAvatarFromId(Guid instanceId);
    public IDictionary<Guid, string> GetAvatarsFromIds(IEnumerable<Guid> instanceIds);
    public void PlaceAvatar(Guid instanceId, string path);
    public string PlaceAvatar(Guid instanceId, byte[] avatarSource);

    //browsed tracks
    public Task RegisterBrowsedTracks(IEnumerable<Track> tracks);
    public Task EraseBrowsedTracks();
}
