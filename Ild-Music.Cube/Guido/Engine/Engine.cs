using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Cube.Guido.Engine.Handlers;

using System.Data.SQLite;
using System.Data;
using Dapper;

namespace Cube.Guido.Engine;

public class Engine
{
    private string path;
    private int capacity;
    private ReadOnlyMemory<char> _connectionString;

    private IDbConnection _connection;
    private CommandHandler _commandHandler; 


    public Engine(string path, int capacity)
    {
        this.path = path;
        this.capacity = capacity;

        var connectionString = $"Data Source = {path}";
        _connection = new SQLiteConnection(connectionString);
        _commandHandler = new(_connection);

    }

    public void Start()
    {
        try
        {
            if(!File.Exists(path))
            {
                SQLiteConnection.CreateFile(path);
            }
            using (var connection = _connection)
            {
                connection.Execute("create table if not exists artists(Id integer primary key, AID, Name varchar, Description varchar, Year integer, Avatar blob)");
                connection.Execute("create table if not exists playlists(Id integer primary key, PID varchar, Name varchar, Description varchar, Year integer, Avatar blob)");
                connection.Execute("create table if not exists tracks(Id integer primary key, TID varchar, Path varchar, Name varchar, Description varchar, Year integer, Avatar blob, Valid integer, Duration integer)");
                

                connection.Execute("create table if not exists artists_playlists(Id integer primary key, AID, PID)");
                connection.Execute("create table if not exists artists_tracks(Id integer primary key, AID, TID)");
                connection.Execute("create table if not exists playlists_tracks(Id integer primary key, PID varchar, TID varchar)");


                connection.Execute("create table if not exists tags(Id integer primary key, TagID varchar, Name varchar, Color varchar)");
                connection.Execute("create table if not exists tags_instances(Id integer primary key, TagID varchar, IID varchar)");

                connection.Execute("create index if not exists artists_index on artists(AID, lower(Name), Year)");
                connection.Execute("create index if not exists playlists_index on playlists(PID, lower(Name), Year)");
                connection.Execute("create index if not exists tracks_index on tracks(TID, Path, lower(Name), Year)");
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

    public async Task Add<T>(T entity)
    {
        //await fork.Add<T>(entity); 
        if(entity is Artist artist)
            await _commandHandler.AddArtist(artist);
    }

    public async Task AddStores(ICollection<object> stores)
    {
       //await fork.AddStores(stores); 
    }

    public async Task Edit<T>(T entity)
    {
        //await fork.Edit<T>(entity);
    }

    public async Task EditStores(ICollection<object> stores)
    {
        //await fork.EditStores(stores);
    }

    public async Task Delete(EntityTag entityTag,
                                Guid entityId)
    {
       //await fork.Delete(entityTag, entityId); 
    }


    public async Task<(IEnumerable<object>, IEnumerable<object>, IEnumerable<object>)> BringAll(int offset, int inputCapacity)
    {
        //return await loader.BringAll(offset, inputCapacity);
        return default;
    }

    public async Task<IEnumerable<T>> Bring<T>(int offset, int inputCapacity)
    {
        //return await loader.Bring<T>(offset, inputCapacity);
        return null;
    }

    public async Task<T> BringSingle<T>(Guid inputId)
    {
        //return await loader.BringSingle<T>(inputId);
        return default;
    }

    public async Task<IEnumerable<T>> BringItemsById<T>(IEnumerable<Guid> ids)
    {
        //return await loader.BringItemsById<T>(ids);
        return null;
    }

    public async Task<object> BringStore(int tag, Guid id)
    {
        //return await loader.BringStore(tag, id);
        return default;
    }

    public async Task<IEnumerable<CommonInstanceDTO>> RequireInstancesRaw(EntityTag entityTag,
                                                                             IEnumerable<Guid> ids)
    {
        return null;
        //if (ids is null)
        //    return await loader.RetrieveInstanceDTO(entityTag);
        //else return await loader.RetrieveInstanceDTOById(entityTag, ids);
    }

    public async Task<IEnumerable<Guid>> CheckRelates(int tag, ICollection<Guid> guids)
    {
        //if(tag == 0)
        //    return await validator.ValidateArtists(guids);
        //else if(tag == 1)
        //    return await validator.ValidatePlaylists(guids);
        //else if(tag == 2)
        //    return await validator.ValidateTracks(guids);
        //else return default;
        return null;
    } 
}
