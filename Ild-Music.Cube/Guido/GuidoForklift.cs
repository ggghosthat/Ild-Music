using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Cube.Guido.Engine;
using Cube.Guido;

namespace Cube;
public class GuidoForklift //Cars from pixar (lol)
{
    private int capacity;

    private static Engine _engine;

    public GuidoForklift(in string dbPath,
                        int capacity = 300)
    {
        _engine = new (dbPath, capacity);

        this.capacity = capacity;
    }

    //check database and table existance
    //in negative case it creates from scratch
    public void ForkliftUp()
    {
        _engine.Start();
    }

    #region CRUD
    //insert new entity
    public async Task AddEntity<T>(T entity) =>
        await _engine.Add<T>(entity);

    //update(edit) existed entity
    public async Task EditEntity<T>(T entity) =>
        await _engine.Edit<T>(entity);

    //delete specific entity by it own id
    public async Task DeleteEntity(Guid entityId,
                                   EntityTag entityTag) =>
        await _engine.Delete(entityId, entityTag);



    public async Task<(IEnumerable<Artist>, IEnumerable<Playlist>, IEnumerable<Track>)> StartLoad(int offset=0)
    {
        return default;
    }

    public async Task<IEnumerable<T>> LoadEntities<T>(int offset)
    {
        return default;
    }
    
    public async Task<IEnumerable<T>> LoadEntitiesById<T>(IEnumerable<CommonInstanceDTO> idCollection)
    {
        return default;
    }



    public async Task<IEnumerable<T>> Search<T>(ReadOnlyMemory<char> searchTerm)
    {
         return default;
    }

    

    //here instead of using generic T have been implemented methods for each type
    //reason is imposibility of implicit casting with T generic type.
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

    //use extend methods when you fetched a bare instance without any dependencies
    //for example playlist without tracks or track without artists. 
    //In this case expend methods will come handy.
    public async Task<Artist> ExtendArtist(Artist artist)
    {
        return default;
    }
   
    public async Task<Playlist> ExtendPlaylist(Playlist playlist)
    {
        return default;
    }

    public async Task<Track> ExtendTrack(Track track)
    {
        return default;
    }

    public async Task<IEnumerable<CommonInstanceDTO>> RequireInstances(EntityTag entitytag, 
                                                                       IEnumerable<Guid> ids)
    {
         return default;
    }


    public async Task<IEnumerable<Guid>> FilterRelates(int tag, ICollection<Guid> relates)
    {
        return default;
    }

    public async Task<IEnumerable<Guid>> FilterTrackRelates(Guid trackId, bool isArtist)
    {
        return default;
    }
    #endregion
}
