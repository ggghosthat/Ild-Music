using Ild_Music.Core.Instances;
using Cube.Mapper.Entities;

using System.Data.SQLite;
using Dapper;
namespace Cube.Storage.Guido.Engine;
public class Engine
{
    private string path;
    private int capacity;
    private ReadOnlyMemory<char> _connectionString;

    private Fork fork;
    private Loader loader;
    private Validator validator;
    
    public Engine(string path, int capacity)
    {
        this.path = path;
        this.capacity = capacity;

        var connectionString = $"Data Source = {path}";
        _connectionString = connectionString.AsMemory();

        validator = new(ref connectionString);
        fork = new (ref connectionString);
        loader = new (ref connectionString);
    }

    public void StartEngine()
    {
        try
        {
            if(!File.Exists(path))
            {
                SQLiteConnection.CreateFile(path);
            }
            using (var connection = new SQLiteConnection(_connectionString.ToString()))
            {
                connection.Execute("create table if not exists artists(Id integer primary key, AID text, Name varchar, Description varchar, Year integer, Avatar blob)");
                connection.Execute("create table if not exists playlists(Id integer primary key, PID text, Name varchar, Description varchar, Year integer, Avatar blob)");
                connection.Execute("create table if not exists tracks(Id integer primary key, TID text, Path varchar, Name varchar, Description varchar, Year integer, Avatar blob, Valid integer, Duration integer)");
                

                connection.Execute("create table if not exists artists_playlists(Id integer primary key, AID text, PID text)");
                connection.Execute("create table if not exists artists_tracks(Id integer primary key, AID text, TID text)");
                connection.Execute("create table if not exists playlists_tracks(Id integer primary key, PID text, TID text)");


                connection.Execute("create table if not exists tags(Id integer primary key, TagID text, Name text, Color text)");
                connection.Execute("create table if not exists tags_instances(Id integer primary key, TagID text, IID text)");

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
        await fork.Add<T>(entity); 
    }

    public async Task AddStores(ICollection<Store> stores)
    {
       await fork.AddStores(stores); 
    }

    public async Task Edit<T>(T entity)
    {
        await fork.Edit<T>(entity);
    }

    public async Task EditStores(ICollection<Store> stores)
    {
        await fork.EditStores(stores);
    }

    public async Task Delete<T>(T entity)
    {
       await fork.Delete<T>(entity); 
    }


    public async Task<(IEnumerable<ArtistMap>, IEnumerable<PlaylistMap>, IEnumerable<TrackMap>)> BringAll(int offset, int inputCapacity)
    {
        return await loader.BringAll(offset, inputCapacity);
    }

    public async Task<IEnumerable<T>> Bring<T>(int offset, int inputCapacity)
    {
        return await loader.Bring<T>(offset, inputCapacity);
    }

    public async Task<T> BringSingle<T>(Guid inputId)
    {
        return await loader.BringSingle<T>(inputId);
    }

    public async Task<IEnumerable<T>> BringItemsById<T>(ICollection<Guid> ids)
    {
        return await loader.BringItemsById<T>(ids);
    }

    public async Task<Store> BringStore(int tag, Guid id)
    {
        return await loader.BringStore(tag, id);
    }

    public async Task<IEnumerable<CommonInstanceDTOMap>> RequireInstancesRaw(EntityTag entityTag,
                                                                             IEnumerable<Guid> ids)
    {
        if (ids is null)
            return await loader.RetrieveInstanceDTO(entityTag);
        else return await loader.RetrieveInstanceDTOById(entityTag, ids);
    }

    public async Task<IEnumerable<Guid>> CheckRelates(int tag, ICollection<Guid> guids)
    {
        if(tag == 0)
            return await validator.ValidateArtists(guids);
        else if(tag == 1)
            return await validator.ValidatePlaylists(guids);
        else if(tag == 2)
            return await validator.ValidateTracks(guids);
        else return default;
    } 
}
