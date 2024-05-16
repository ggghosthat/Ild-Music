using Ild_Music.Core.Contracts;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Instances.Querying;
using Cube.Guido.Agents;
using Cube.Guido.Handlers;

using MediatR;

namespace Cube;
public class GuidoForklift : ICube //Cars from pixar (lol)
{ 
    public string CubeName => "Guido Forklift";
    public Guid CubeId {get; private set;} = Guid.Empty;

    public static List<CommonInstanceDTO>? _artists = new List<CommonInstanceDTO>();
    public static List<CommonInstanceDTO>? _playlists = new List<CommonInstanceDTO>();
    public static List<CommonInstanceDTO>? _tracks = new List<CommonInstanceDTO>();
    public static List<Tag>? _tags = new List<Tag>();

    private static IMediator _mediator = default;
    private readonly static CommandHandler _commandHandler = new ();
    private readonly static QueryHandler _queryHandler = new ();
    private readonly static SearchHandler _searchHandler = new ();

    public GuidoForklift()
    { 
        if (CubeId == Guid.Empty)
            CubeId = Guid.NewGuid();
    }

    public IEnumerable<CommonInstanceDTO>? Artists => _artists;
    public IEnumerable<CommonInstanceDTO>? Playlists => _playlists;
    public IEnumerable<CommonInstanceDTO>? Tracks => _tracks;
    public IEnumerable<Tag>? Tags => _tags;

    public void Init(string allocationPlace, bool isMoveTrackFiles)
    {
        
        WearhouseAgent.ConfigureAgent(allocationPlace, isMoveTrackFiles); 
        ConnectionAgent.ConfigureAgent(allocationPlace);
        ConnectionAgent.SpreadDatabase(); 
    }
    
    public void ConnectMediator(IMediator mediator) =>
        _mediator = mediator;

    
    //insert new entity 
    public async Task AddArtistObj(Artist artist) 
    {
        await _commandHandler.AddArtist(artist);
        
        _artists?.Clear();
        _artists?.AddRange(await _queryHandler.QueryAllArtists());
    }

    public async Task AddPlaylistObj(Playlist playlist) 
    {
        await _commandHandler.AddPlaylist(playlist);
        
        _playlists?.Clear();
        _playlists?.AddRange(await _queryHandler.QueryAllPlaylists());
    }

    public async Task AddTrackObj(Track track) 
    {
        await _commandHandler.AddTrack(track);

        _tracks?.Clear();
        _tracks?.AddRange(await _queryHandler.QueryAllTracks()); 
    }

    public async Task AddTagObj(Tag tag)
    {
        await _commandHandler.AddTag(tag);

        _tags?.Clear();
        _tags?.AddRange(await _queryHandler.QueryAllTags());
    }

    public async Task EditArtistObj(Artist newArtist) 
    {
        await _commandHandler.EditArtist(newArtist);

        _artists?.Clear();
        _artists?.AddRange(await _queryHandler.QueryAllArtists());
    }    

    public async Task EditPlaylistObj(Playlist newPlaylist)
    {
        await _commandHandler.EditPlaylist(newPlaylist);

        _playlists?.Clear();
        _playlists?.AddRange(await _queryHandler.QueryAllPlaylists());
    }

    public async Task EditTrackObj(Track newTrack)
    {
        await _commandHandler.EditTrack(newTrack);

        _tracks?.Clear();
        _tracks?.AddRange(await _queryHandler.QueryAllTracks()); 
    }

    public async Task EditTagObj(Tag newTag)
    {
        await _commandHandler.EditTag(newTag);

        _tags?.Clear();
        _tags?.AddRange(await _queryHandler.QueryAllTags());
    }

    public async Task RemoveArtistObj(Guid artistId) 
    {   
        await _commandHandler.DeleteArtist(artistId);

        _artists?.Clear();
        _artists?.AddRange(await _queryHandler.QueryAllArtists());
    }

    public async Task RemovePlaylistObj(Guid playlistId)
    {
        await _commandHandler.DeletePlaylist(playlistId);

        _playlists?.Clear();
        _playlists?.AddRange(await _queryHandler.QueryAllPlaylists());
    }

    public async Task RemoveTrackObj(Guid trackId) 
    {
        await _commandHandler.DeleteTrack(trackId);

        _tracks?.Clear();
        _tracks?.AddRange(await _queryHandler.QueryAllTracks());
    }

    private async Task RemoveTagObj(Guid tagId)
    {
        await _commandHandler.DeleteTag(tagId);

        _tags?.Clear();
        _tags?.AddRange(await _queryHandler.QueryAllTags());
    } 

    public async Task LoadTopEntities()
    {
        QueryPool pool = await _queryHandler.QueryTopPool();
        _artists = pool.ArtistsDTOs.ToList();
        _playlists = pool.PlaylistsDTOs.ToList();
        _tracks = pool.TracksDTOs.ToList();
    }

    public async Task<IEnumerable<CommonInstanceDTO>> LoadEntities(EntityTag entityTag)
    {
        return entityTag switch
        {
            EntityTag.ARTIST => await _queryHandler.QueryAllArtists(),
            EntityTag.PLAYLIST => await _queryHandler.QueryAllPlaylists(),
            EntityTag.TRACK => await _queryHandler.QueryAllTracks()
        };
    }


    public async Task<IEnumerable<CommonInstanceDTO>> LoadFramedEntities(EntityTag entityTag, int offset, int limit)
    {
        return entityTag switch
        {
            EntityTag.ARTIST => await _queryHandler.QueryArtists(offset, limit),
            EntityTag.PLAYLIST => await _queryHandler.QueryPlaylists(offset, limit),
            EntityTag.TRACK => await _queryHandler.QueryTracks(offset, limit)
        };
    }
    
    public async Task<IEnumerable<Tag>> LoadTags()
    {
        return await _queryHandler.QueryAllTags();
    }

    public async Task<IEnumerable<Tag>> LoadFramedTags (int offset, int limit)
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
        return await _queryHandler.QuerySingleTrack(ref instanceDTO);
    }

    public async Task<Tag> QueryTag(Guid tagId)
    {
        return await _queryHandler.QuerySingleTag(tagId); 
    }

    public async Task<IEnumerable<CommonInstanceDTO>> QueryInstanceDtosFromIds (IEnumerable<Guid> ids, EntityTag entityTag)
    {
        return await _queryHandler.QueryInstanceDtosFromIds(ids, entityTag);
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
            _ => await Task.FromResult<IEnumerable<CommonInstanceDTO>>(Enumerable.Empty<CommonInstanceDTO>())
        };
    }

    public async Task<IEnumerable<Tag>> SearchTag(string searchTerm)
    {
        return await _searchHandler.SearchTags(searchTerm);
    }
}
