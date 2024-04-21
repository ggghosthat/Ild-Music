using Ild_Music.Core.Contracts;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Statistics;
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

    private static IMediator _mediator = default;
    private readonly static CommandHandler _commandHandler = new ();
    private readonly static QueryHandler _queryHandler = new ();

    private static int _capacity;

    private int artistOffset = 0;
    private int playlistOffset = 0;
    private int trackOffset = 0;
  

    public GuidoForklift()
    { 
        if (CubeId == Guid.Empty)
            CubeId = Guid.NewGuid();
    }

    public void Init(string allocationPlace, int capacity, bool isMoveTrackFiles)
    {
        _capacity = capacity;
        
        WearhouseAgent.ConfigureAgent(allocationPlace, isMoveTrackFiles); 
        ConnectionAgent.ConfigureAgent(allocationPlace);
        ConnectionAgent.SpreadDatabase(); 
    }
    
    public void ConnectMediator(IMediator mediator) =>
        _mediator = mediator;


    public int CubePage => _capacity;
    public IEnumerable<CommonInstanceDTO>? Artists {get; private set;} = default;
    public IEnumerable<CommonInstanceDTO>? Playlists {get; private set;} = default;
    public IEnumerable<CommonInstanceDTO>? Tracks {get; private set;} = default;
    public IEnumerable<Tag>? Tags {get; set;} = default;

    
    //insert new entity 
    public async Task AddArtistObj(Artist artist) 
    {
        await _commandHandler.AddArtist(artist);
     
        if((Artists?.Count() + 1) < (artistOffset * CubePage))
           Artists = await LoadEntities(EntityTag.ARTIST, artistOffset);
    }

    public async Task AddPlaylistObj(Playlist playlist) 
    {
        await _commandHandler.AddPlaylist(playlist);

        if((Playlists?.Count() + 1) < (playlistOffset * CubePage))
           Playlists = await LoadEntities(EntityTag.PLAYLIST, playlistOffset); 
    }

    public async Task AddTrackObj(Track track) 
    {
        await _commandHandler.AddTrack(track);

        if((Tracks?.Count() + 1) < (trackOffset * CubePage))
           Tracks = await LoadEntities(EntityTag.TRACK, trackOffset); 
    }

    public async Task AddTagObj(Tag tag)
    {
        await _commandHandler.AddTag(tag);
    }


    public async Task EditArtistObj(Artist newArtist) 
    {
        await _commandHandler.EditArtist(newArtist);

        if((Artists?.Count() + 1) < (artistOffset * CubePage))
           Artists = await LoadEntities(EntityTag.ARTIST, artistOffset);
    }    

    public async Task EditPlaylistObj(Playlist newPlaylist)
    {
        await _commandHandler.EditPlaylist(newPlaylist);

        if((Playlists?.Count() + 1) < (playlistOffset * CubePage))
           Playlists = await LoadEntities(EntityTag.PLAYLIST, playlistOffset);
    }

    public async Task EditTrackObj(Track newTrack)
    {
        await _commandHandler.EditTrack(newTrack);

        if((Tracks?.Count() + 1) < (trackOffset * CubePage))
           Tracks = await LoadEntities(EntityTag.TRACK, trackOffset);
    }

    public async Task EditTagObj(Tag newTag)
    {
        await _commandHandler.EditTag(newTag);
    }


    public async Task RemoveArtistObj(Guid artistId) 
    {   
        await _commandHandler.DeleteArtist(artistId);

        if((Artists?.Count() + 1) < (artistOffset * CubePage))
           Artists = await LoadEntities(EntityTag.ARTIST, artistOffset); 
    }

    public async Task RemovePlaylistObj(Guid playlistId)
    {
        await _commandHandler.DeletePlaylist(playlistId);

        if((Playlists?.Count() + 1) < (playlistOffset * CubePage))
           Playlists = await LoadEntities(EntityTag.PLAYLIST, playlistOffset);
    }

    public async Task RemoveTrackObj(Guid trackId) 
    {
        await _commandHandler.DeleteTrack(trackId);

        if((Tracks?.Count() + 1) < (trackOffset * CubePage))
           Tracks = await LoadEntities(EntityTag.TRACK, trackOffset);
    }

    private async Task RemoveTagObj(Guid tagId)
    {
        await _commandHandler.DeleteTag(tagId); 
    } 


    public async Task LoadUp()
    {
        QueryPool pool = await _queryHandler.QueryTopPool();
        Artists = pool.ArtistsDTOs;
        Playlists = pool.PlaylistsDTOs;
        Tracks = pool.TracksDTOs;
    }

    public async Task<IEnumerable<CommonInstanceDTO>> LoadEntities(EntityTag entityTag, int offset)
    {
        return entityTag switch
        {
            EntityTag.ARTIST => await _queryHandler.QueryArtists(offset),
            EntityTag.PLAYLIST => await _queryHandler.QueryArtists(offset),
            EntityTag.TRACK => await _queryHandler.QueryArtists(offset)
        };
    }
    
    public async Task<IEnumerable<Tag>> LoadTags (int offset)
    {
        return await _queryHandler.QueryTags(offset);
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


    public async Task<IEnumerable<T>> Search<T>(ReadOnlyMemory<char> searchTerm)
    {
         return default;
    }

    public async Task<CounterFrame> SnapCounterFrame()
    {
        return default;
    }
}
