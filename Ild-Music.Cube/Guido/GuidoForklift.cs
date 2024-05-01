using Ild_Music.Core.Contracts;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Instances.Querying;
using Cube.Guido.Agents;
using Cube.Guido.Handlers;

using MediatR;
using System.Linq;

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
     
        if((Artists?.Count() + 1) < (ConnectionAgent.ArtistOffset * ConnectionAgent.QueryLimit))
           _artists?.AddRange(await LoadEntities(EntityTag.ARTIST));
    }

    public async Task AddPlaylistObj(Playlist playlist) 
    {
        await _commandHandler.AddPlaylist(playlist);

        if((Playlists?.Count() + 1) < (ConnectionAgent.PlaylistOffset * ConnectionAgent.QueryLimit))
           _playlists?.AddRange(await LoadEntities(EntityTag.PLAYLIST)); 
    }

    public async Task AddTrackObj(Track track) 
    {
        await _commandHandler.AddTrack(track);

        if((Tracks?.Count() + 1) < (ConnectionAgent.TrackOffset * ConnectionAgent.QueryLimit))
           _tracks?.AddRange(await LoadEntities(EntityTag.TRACK)); 
    }

    public async Task AddTagObj(Tag tag)
    {
        await _commandHandler.AddTag(tag);

        if((Tags?.Count() + 1) < (ConnectionAgent.TagOffset * ConnectionAgent.QueryLimit))
           _tags?.AddRange(await LoadTags());
    }

    public async Task EditArtistObj(Artist newArtist) 
    {
        await _commandHandler.EditArtist(newArtist);

        if((Artists?.Count() + 1) < (ConnectionAgent.ArtistOffset * ConnectionAgent.QueryLimit))
            _artists?.AddRange(await LoadEntities(EntityTag.ARTIST));
    }    

    public async Task EditPlaylistObj(Playlist newPlaylist)
    {
        await _commandHandler.EditPlaylist(newPlaylist);

        if((Playlists?.Count() + 1) < (ConnectionAgent.PlaylistOffset * ConnectionAgent.QueryLimit))
            _playlists?.AddRange(await LoadEntities(EntityTag.PLAYLIST));
    }

    public async Task EditTrackObj(Track newTrack)
    {
        await _commandHandler.EditTrack(newTrack);

        if((Tracks?.Count() + 1) < (ConnectionAgent.TrackOffset * ConnectionAgent.QueryLimit))
            _tracks?.AddRange(await LoadEntities(EntityTag.TRACK)); 
    }

    public async Task EditTagObj(Tag newTag)
    {
        await _commandHandler.EditTag(newTag);

        if((Tags?.Count() + 1) < (ConnectionAgent.TagOffset * ConnectionAgent.QueryLimit))
            _tags?.AddRange(await LoadTags()); 
    }

    public async Task RemoveArtistObj(Guid artistId) 
    {   
        await _commandHandler.DeleteArtist(artistId);

        if((Artists?.Count() + 1) < (ConnectionAgent.ArtistOffset * ConnectionAgent.QueryLimit))
            _artists?.AddRange(await LoadEntities(EntityTag.ARTIST));
    }

    public async Task RemovePlaylistObj(Guid playlistId)
    {
        await _commandHandler.DeletePlaylist(playlistId);

        if((Playlists?.Count() + 1) < (ConnectionAgent.PlaylistOffset * ConnectionAgent.QueryLimit))
            _playlists?.AddRange(await LoadEntities(EntityTag.PLAYLIST));
    }

    public async Task RemoveTrackObj(Guid trackId) 
    {
        await _commandHandler.DeleteTrack(trackId);

        if((Tracks?.Count() + 1) < (ConnectionAgent.TrackOffset * ConnectionAgent.QueryLimit))
            _tracks?.AddRange(await LoadEntities(EntityTag.TRACK)); 
    }

    private async Task RemoveTagObj(Guid tagId)
    {
        await _commandHandler.DeleteTag(tagId);

        if((Tags?.Count() + 1) < (ConnectionAgent.TagOffset * ConnectionAgent.QueryLimit))
            _tags?.AddRange(await LoadTags()); 
    } 

    public async Task LoadUp()
    {
        QueryPool pool = await _queryHandler.QueryTopPool();
        _artists = pool.ArtistsDTOs.ToList();
        _playlists = pool.PlaylistsDTOs.ToList();
        _tracks = pool.TracksDTOs.ToList();
    }

    public async Task QueryNextChunk(EntityTag entityTag)
    {
        switch(entityTag)
        {
            case EntityTag.ARTIST:
                _artists?.AddRange(await LoadEntities(entityTag));
                break;
            case EntityTag.PLAYLIST:
                _playlists?.AddRange(await LoadEntities(entityTag));
                break;
            case EntityTag.TRACK:
                _tracks?.AddRange(await LoadEntities(entityTag));
                break;
            case EntityTag.TAG:
                _tags?.AddRange(await LoadTags());
                break;
        }
    }

    public async Task<IEnumerable<CommonInstanceDTO>> LoadEntities(EntityTag entityTag)
    {
        return entityTag switch
        {
            EntityTag.ARTIST => await _queryHandler.QueryArtists(),
            EntityTag.PLAYLIST => await _queryHandler.QueryArtists(),
            EntityTag.TRACK => await _queryHandler.QueryArtists()
        };
    }
    
    public async Task<IEnumerable<Tag>> LoadTags ()
    {
        return await _queryHandler.QueryTags();
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