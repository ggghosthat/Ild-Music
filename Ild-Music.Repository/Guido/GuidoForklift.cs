using Ild_Music.Core.Contracts;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Instances.Filing;
using Ild_Music.Core.Instances.Querying;
using Ild_Music.Core.Events;
using Ild_Music.Repository.Guido.Agents;
using Ild_Music.Repository.Guido.Handlers;

namespace Ild_Music.Repository;

public class GuidoForklift : ICube //Cars from pixar (lol)
{ 
    public string CubeName => "Guido Forklift";
    public Guid CubeId {get; private set;} = Guid.Empty;

    private IEventBag _eventBag = default;
    
    private static InstancePool _instancePool;
    private static List<Track> _browsedTracks = [];

    private readonly static CommandHandler _commandHandler = new ();
    private readonly static QueryHandler _queryHandler = new ();
    private readonly static SearchHandler _searchHandler = new ();

    public GuidoForklift()
    { 
        if (CubeId == Guid.Empty)
            CubeId = Guid.NewGuid();
    }
   
    public InstancePool InstancePool => _instancePool;
    
    public IEnumerable<Track> BrowsedTracks => _browsedTracks;
    
    public int PageSize => ConnectionAgent.QueryLimit;


    public void Init(string allocationPlace, bool isMoveTrackFiles)
    {        
        WarehouseAgent.ConfigureAgent(allocationPlace, isMoveTrackFiles); 
        ConnectionAgent.ConfigureAgent(allocationPlace);
        ConnectionAgent.SpreadDatabase(); 
    }
    
    public void InjectEventBag(IEventBag eventBag)
    {
        _eventBag = eventBag;
    }
 
    //insert new entity 
    public async Task AddArtistObj(Artist artist) 
    {
        await _commandHandler.AddArtist(artist);
    }

    public async Task AddPlaylistObj(Playlist playlist) 
    {
        await _commandHandler.AddPlaylist(playlist);
    }

    public async Task AddTrackObj(Track track) 
    {
        await _commandHandler.AddTrack(track);
    }

    public async Task AddTagObj(Tag tag)
    {
        await _commandHandler.AddTag(tag);
    }

    public async Task EditArtistObj(Artist newArtist) 
    {
        await _commandHandler.EditArtist(newArtist);
    }    

    public async Task EditPlaylistObj(Playlist newPlaylist)
    {
        await _commandHandler.EditPlaylist(newPlaylist);
    }

    public async Task EditTrackObj(Track newTrack)
    {
        await _commandHandler.EditTrack(newTrack);
    }

    public async Task EditTagObj(Tag newTag)
    {
        await _commandHandler.EditTag(newTag);
    }

    public async Task RemoveArtistObj(Guid artistId) 
    {   
        await _commandHandler.DeleteArtist(artistId);
    }

    public async Task RemovePlaylistObj(Guid playlistId)
    {
        await _commandHandler.DeletePlaylist(playlistId);
    }

    public async Task RemoveTrackObj(Guid trackId) 
    {
        await _commandHandler.DeleteTrack(trackId);
    }

    public async Task RemoveTagObj(Guid tagId)
    {
        await _commandHandler.DeleteTag(tagId);
    } 

    public async Task LoadStartEntities()
    {
        _instancePool = await _queryHandler.QueryPool();
    }

    public async Task<IEnumerable<CommonInstanceDTO>> LoadEntities(EntityTag entityTag)
    {
        return entityTag switch
        {
            EntityTag.ARTIST => await _queryHandler.QueryAllArtists(),
            EntityTag.PLAYLIST => await _queryHandler.QueryAllPlaylists(),
            EntityTag.TRACK => await _queryHandler.QueryAllTracks(),
            EntityTag.TAG => await _queryHandler.QueryAllTags()
        };
    }

    public async Task<IEnumerable<CommonInstanceDTO>> LoadFramedEntities(EntityTag entityTag, int offset, int limit)
    {
        return entityTag switch
        {
            EntityTag.ARTIST => await _queryHandler.QueryArtists(offset, limit),
            EntityTag.PLAYLIST => await _queryHandler.QueryPlaylists(offset, limit),
            EntityTag.TRACK => await _queryHandler.QueryTracks(offset, limit),
            EntityTag.TAG => await _queryHandler.QueryTags(offset, limit)
        };
    } 
    
    public async Task<IEnumerable<CommonInstanceDTO>> LoadTags()
    {
        return await _queryHandler.QueryAllTags();
    }

    public async Task<IEnumerable<CommonInstanceDTO>> LoadFramedTags (int offset, int limit)
    {
        return await _queryHandler.QueryTags(offset, limit);
    }

    public async Task<Artist> QueryArtist(CommonInstanceDTO instanceDTO)
    {
        return await _queryHandler.QuerySingleArtist(ref instanceDTO);
    }

    public async Task<Playlist> QueryPlaylist(CommonInstanceDTO instanceDTO)
    {
        return await _queryHandler.QuerySinglePlaylist(ref instanceDTO);
    }

    public async Task<Track> QueryTrack(CommonInstanceDTO instanceDTO)
    {
        var track = _queryHandler.QuerySingleTrack(ref instanceDTO).Result;
        string trackPath = WarehouseAgent.GetTrackPathFromId(track.Id);
        track.Pathway = trackPath.AsMemory();
        return track;
    }

    public async Task<Tag> QueryTag(Guid tagId)
    {
        return await _queryHandler.QuerySingleTag(tagId); 
    }

    public async Task<IEnumerable<CommonInstanceDTO>> LoadInstancesById(IEnumerable<Guid> ids, EntityTag entityTag)
    {
        return await _queryHandler.QueryInstancesById(ids, entityTag);
    }

    public async Task<IEnumerable<Track>> LoadTracksById(IEnumerable<Guid> ids)
    {
        return await _queryHandler.QueryTracksById(ids);
    }

    public async Task<MetricSheet> QueryCapacityMetrics()
    {
        return await _queryHandler.QueryCapacityMetrics();
    }

    public async Task<IEnumerable<CommonInstanceDTO>> Search(string searchTerm)
    {
        return await _searchHandler.Search(searchTerm.ToString());
    }

    public async Task<IEnumerable<CommonInstanceDTO>> SearchInstance(string searchTerm, EntityTag entityTag)
    {
        return entityTag switch
        {
            EntityTag.ARTIST => await _searchHandler.SearchArtists(searchTerm),
            EntityTag.PLAYLIST => await _searchHandler.SearchPlaylists(searchTerm),
            EntityTag.TRACK => await _searchHandler.SearchTracks(searchTerm),
            EntityTag.TAG => await _searchHandler.SearchTags(searchTerm),
            _ => await Task.FromResult<IEnumerable<CommonInstanceDTO>>(Enumerable.Empty<CommonInstanceDTO>())
        };
    }

    public async Task<IEnumerable<CommonInstanceDTO>> SearchTag(string searchTerm)
    {
        return await _searchHandler.SearchTags(searchTerm);
    }

    public string GetTrackPathFromId(Guid id)
    {
        return WarehouseAgent.GetTrackPathFromId(id);
    }

    public IEnumerable<string> GetTrackPathsFromId(IEnumerable<Guid> ids)
    {
        return WarehouseAgent.GetTrackPathsFromIds(ids);
    }

    public void PlaceTrackFile(Track track)
    {
        WarehouseAgent.PlaceTrackFile(track).Wait();
    }

    public void PlaceTrackFiles(IEnumerable<Track> tracks)
    {
        WarehouseAgent.PlaceTrackFiles(tracks).Wait();
    }

    public string GetAvatarFromId(Guid instanceId)
    {
        return WarehouseAgent.GetAvatarFromId(instanceId);
    }

    public IDictionary<Guid, string> GetAvatarsFromIds(IEnumerable<Guid> instanceIds)
    {
        return WarehouseAgent.GetAvatarsFromIds(instanceIds);
    }

    public void PlaceAvatar(Guid instanceId, string path)
    {
        WarehouseAgent.PlaceAvatar(instanceId, path).Wait();
    }

    public string PlaceAvatar(Guid instanceId, byte[] avatarSource)
    {
        return WarehouseAgent.PlaceAvatar(instanceId, avatarSource).Result;
    }

    public async Task RegisterBrowsedTracks(IEnumerable<Track> tracks)
    {
       _browsedTracks.AddRange(tracks); 
    }

    public async Task EraseBrowsedTracks()
    {
        _browsedTracks.Clear();
    }
}
