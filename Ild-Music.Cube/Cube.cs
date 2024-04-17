using Ild_Music.Core.Contracts;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Statistics;

using MediatR;
namespace Cube;

public class Cube : ICube
{
    public Guid CubeId {get; private set;} = Guid.Empty;

    public string CubeName => "Genezis Cube";

    private IMediator _mediator = default;

    private int artistOffset = 0;
    private int playlistOffset = 0;
    private int trackOffset = 0;

    private int pageCount = 300;
    public int CubePage => pageCount;

    private string dbPath = Path.Combine(Environment.CurrentDirectory, "storage.db");
    private GuidoForklift? guidoForklift = default;

    public IEnumerable<CommonInstanceDTO>? Artists {get; private set;} = default;
    public IEnumerable<CommonInstanceDTO>? Playlists {get; private set;} = default;
    public IEnumerable<CommonInstanceDTO>? Tracks {get; private set;} = default;

    public Cube()
    {
        if(CubeId == Guid.Empty)
            CubeId = Guid.NewGuid();
    }

    public void SetPath(ref string inputPath)
    {
        dbPath = inputPath;
    }

    public void Init()
    {
        guidoForklift = new (dbPath, CubePage);       
        LoadUp().Wait();
    }

    public void ConnectMediator(IMediator mediator)
    {
        _mediator = mediator;
    }

    
    
    #region Command region
    public async Task AddArtistObj(Artist artist) 
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");

        await guidoForklift.AddEntity(artist);
        if((Artists?.Count() + 1) < (artistOffset * CubePage))
        {
           Artists = await guidoForklift.LoadEntities(EntityTag.ARTIST, artistOffset); 
        }
    }

    public async Task AddPlaylistObj(Playlist playlist) 
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");

        await guidoForklift.AddEntity(playlist);
        if((Playlists?.Count() + 1) < (playlistOffset * CubePage))
        {
           Playlists = await guidoForklift.LoadEntities(EntityTag.PLAYLIST, playlistOffset); 
        }
    }

    public async Task AddTrackObj(Track track) 
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");

        await guidoForklift.AddEntity(track);
        if((Tracks?.Count() + 1) < (trackOffset * CubePage))
        {
           Tracks = await guidoForklift.LoadEntities(EntityTag.TRACK, trackOffset); 
        }

    }

    public async Task AddTagObj(Tag tag)
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");
        
        await guidoForklift.AddEntity(tag);
    }



    public async Task EditArtistObj(Artist newArtist) 
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");
  
        await guidoForklift.EditEntity(newArtist);
    }    

    public async Task EditPlaylistObj(Playlist newPlaylist)
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");
  
        await guidoForklift.EditEntity(newPlaylist);
    }

    public async Task EditTrackObj(Track newTrack)
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");
  
        await guidoForklift.EditEntity(newTrack);
    }

    public async Task EditTagObj(Tag newTag)
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");
        
        await guidoForklift.EditEntity(newTag);
    }


    public async Task RemoveArtistObj(Guid artistId) 
    {   
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");
        
        await guidoForklift.DeleteEntity(artistId, EntityTag.ARTIST);
    }

    public async Task RemovePlaylistObj(Guid playlistId)
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");
        
        await guidoForklift.DeleteEntity(playlistId, EntityTag.PLAYLIST);
    }

    public async Task RemoveTrackObj(Guid trackId) 
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");
        
        await guidoForklift.DeleteEntity(trackId, EntityTag.TRACK);

    }

    private async Task RemoveTagObj(Guid tagId)
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");
        
        await guidoForklift.DeleteEntity(tagId, EntityTag.TAG); 
    }
    #endregion

    #region Query region region
    public async Task LoadUp()
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");

        var load = await guidoForklift.StartLoad();
        Artists = load.ArtistsDTOs;
        Playlists = load.PlaylistsDTOs;
        Tracks = load.TracksDTOs;
    }

    public async Task<IEnumerable<CommonInstanceDTO>> LoadEntities(EntityTag entityTag,
                                                                       int offset)
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");

        return await guidoForklift.LoadEntities(entityTag, offset);
    }

    public async Task<IEnumerable<Tag>> LoadTags(int offset)
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");

        return await guidoForklift.LoadTags(offset);
    }

    public async Task<Artist> QueryArtist(CommonInstanceDTO instanceDTO)
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");

        return await guidoForklift.QueryArtist(instanceDTO);
    }

    public async Task<Playlist> QueryPlaylist(CommonInstanceDTO instanceDTO)
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");

        return await guidoForklift.QueryPlaylist(instanceDTO);
    }

    public async Task<Track> QueryTrack(CommonInstanceDTO instanceDTO)
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");

        return await guidoForklift.QueryTrack(instanceDTO);
    }

    public async Task<Tag> QueryTag(Guid tagId)
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");

        return await guidoForklift.QueryTag(tagId);
    }
    #endregion

   
    public async Task<CounterFrame> SnapCounterFrame()
    {
        return default;
    }

    public async Task<IEnumerable<T>> Search<T>(ReadOnlyMemory<char> searchTerm)
    {
        return default;
    }
}
