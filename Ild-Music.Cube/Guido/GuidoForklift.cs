using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Cube.Guido.Agents;
using Cube.Guido.Handlers;

namespace Cube;
public class GuidoForklift //Cars from pixar (lol)
{
    private readonly int capacity;

    private readonly CommandHandler _commandHandler;
    private readonly QueryHandler _queryHandler;

    public GuidoForklift(string dbPath,
                         int capacity = 300)
    { 
        this.capacity = capacity;

        ConnectionAgent.ConfigAgent(dbPath);
        ConnectionAgent.ConfigConnection(); 
        
        _commandHandler = new ();
        _queryHandler = new ();
    }

    

    #region CRUD
    //insert new entity
    public async Task AddEntity<T>(T entity)
    {
        if(entity is Artist artist)
            await _commandHandler.AddArtist(artist);
        else if(entity is Playlist playlist)
                await _commandHandler.AddPlaylist(playlist);
        else if(entity is Track track)
            await _commandHandler.AddTrack(track);
        else if(entity is Tag tag)
            await _commandHandler.AddTag(tag);
    }

    //update(edit) existed entity
    public async Task EditEntity<T>(T entity)
    {
        if(entity is Artist artist)
            await _commandHandler.EditArtist(artist);
        else if(entity is Playlist playlist)
            await _commandHandler.EditPlaylist(playlist);
        else if(entity is Track track)
            await _commandHandler.EditTrack(track);
        else if(entity is Tag tag)
            await _commandHandler.EditTag(tag);
    }

    //delete specific entity by it own id
    public async Task DeleteEntity(Guid entityId,
                                   EntityTag entityTag)
    {
        switch(entityTag)
        {
            case (EntityTag.ARTIST) :
                await _commandHandler.DeleteArtist(entityId);
                break;                
            case (EntityTag.PLAYLIST) :
                await _commandHandler.DeletePlaylist(entityId);
                break;
            case (EntityTag.TRACK) : 
                await _commandHandler.DeleteTrack(entityId);
                break; 
            case (EntityTag.TAG) :
                await _commandHandler.DeleteTag(entityId);
                break;
        };

    }


    public async Task<QueryPool> StartLoad()
    {
        return await _queryHandler.QueryTopPool();
    }

    public async Task<IEnumerable<CommonInstanceDTO>> LoadEntities(EntityTag entityTag, 
                                                                   int offset)
    {
        IEnumerable<CommonInstanceDTO> result = entityTag switch
        {
            EntityTag.ARTIST => await _queryHandler.QueryArtists(offset),
            EntityTag.PLAYLIST => await _queryHandler.QueryArtists(offset),
            EntityTag.TRACK => await _queryHandler.QueryArtists(offset)
        };
    
        return result;
    }
    
    public async Task<IEnumerable<T>> LoadEntitiesById<T>(IEnumerable<CommonInstanceDTO> idCollection)
    {
        return default;
    }



    public async Task<IEnumerable<T>> Search<T>(ReadOnlyMemory<char> searchTerm)
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
