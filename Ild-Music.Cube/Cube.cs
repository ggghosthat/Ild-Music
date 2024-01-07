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

    public IEnumerable<Artist>? Artists {get; private set;} = default;
    public IEnumerable<Playlist>? Playlists {get; private set;} = default;
    public IEnumerable<Track>? Tracks {get; private set;} = default;

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
        guidoForklift = new (in dbPath, CubePage);       
        guidoForklift.ForkliftUp();
        LoadUp().Wait();
    }

    public void ConnectMediator(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task LoadUp()
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");

        var load = await guidoForklift.StartLoad();
        Artists = load.Item1;
        Playlists = load.Item2;
        Tracks = load.Item3;
    }
    

    public async Task AddArtistObj(Artist artist) 
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");

        await guidoForklift.AddEntity(artist);
        if((Artists?.Count() + 1) < (artistOffset * CubePage))
        {
           Artists = await guidoForklift.LoadEntities<Artist>(artistOffset); 
        }
    }

    public async Task AddPlaylistObj(Playlist playlist) 
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");

        await guidoForklift.AddEntity(playlist);
        if((Playlists?.Count() + 1) < (playlistOffset * CubePage))
        {
           Playlists = await guidoForklift.LoadEntities<Playlist>(playlistOffset); 
        }
    }

    public async Task AddTrackObj(Track track) 
    {
        if(guidoForklift is null)
            throw new NullReferenceException("Could not load up Guido forklift");

        await guidoForklift.AddEntity(track);
        if((Tracks?.Count() + 1) < (trackOffset * CubePage))
        {
           Tracks = await guidoForklift.LoadEntities<Track>(trackOffset); 
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


    public async Task LoadItems<T>()
    {
    }

    public async Task UnloadItems<T>()
    {   
    }



    #region Require-Retrieve region
    public async Task<IEnumerable<CommonInstanceDTO>> RequireInstances(EntityTag entityTag)
    {
        return default;
    }

    public async Task<IEnumerable<CommonInstanceDTO>> RequireInstances(EntityTag entityTag, 
                                                                       IEnumerable<Guid> id)
    {
        return default;
    }


    public async Task<Artist> FetchArtist(Guid artistId)
    {
        return default;
    }

    public async Task<Playlist> FetchPlaylist(Guid playlistId)
    {
        return default;
    }


    public async Task<Track> FetchTrack(Guid trackId)
    {
        return default;
    }

    public async Task<IEnumerable<Artist>> RetrieveArtists(IEnumerable<CommonInstanceDTO> dtos)
    {
        return default;
    }

    public async Task<IEnumerable<Playlist>> RetrievePlaylists(IEnumerable<CommonInstanceDTO> dtos)
    {
        return default;
    }

    public async Task<IEnumerable<Track>> RetrieveTracks(IEnumerable<CommonInstanceDTO> dtos)
    {
        return default;
    }
    #endregion


    public async Task<InspectFrame> CheckArtistRelates(Artist artist)
    {              
        return default;
    }

    public async Task<InspectFrame> CheckPlaylistRelates(Playlist playlist)
    {
        return default;
    }
    
    public async Task<InspectFrame> CheckTrackRelates(Track track)
    {
        return default;
    }

    public async Task<CounterFrame> SnapCounterFrame()
    {
        return default;
    }

    public async Task<IEnumerable<T>> Search<T>(ReadOnlyMemory<char> searchTerm)
    {
        return default;
    }
}
