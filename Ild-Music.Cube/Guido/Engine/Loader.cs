using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Cube.Mapper.Entities;

using System.Data.SQLite;
using Dapper;
namespace Cube.Storage.Guido.Engine;
internal class Loader
{
    private string _connectionString;
    public Loader(ref string connectionString)
    {
        _connectionString = connectionString;
    }

    //retrieve all type instances
    public async Task<(IEnumerable<ArtistMap>, IEnumerable<PlaylistMap>, IEnumerable<TrackMap>)> BringAll(int offset, int capacity)
    {
        IEnumerable<ArtistMap> artists = null;
        IEnumerable<PlaylistMap> playlists = null;
        IEnumerable<TrackMap> tracks = null;

        ReadOnlyMemory<char> dapperQuery = @"select AID, Name, Description, Avatar, Year from artists where Id >= @offset and Id <= @capacity;
                                           select PID, Name, Description, Avatar, Year from playlists where Id >= @offset and Id <= @capacity;
                                           select TID, Path, Name, Description, Avatar, Valid, Duration, Year from tracks where Id >= @offset and Id <= @capacity;".AsMemory();

        using (var connection = new SQLiteConnection(_connectionString.ToString()))
        {
            await connection.OpenAsync();
            using (var multiQuery = await connection.QueryMultipleAsync(dapperQuery.ToString(), new {offset=offset, capacity=capacity}))
            {
                artists = await multiQuery.ReadAsync<ArtistMap>();
                playlists = await multiQuery.ReadAsync<PlaylistMap>();
                tracks = await multiQuery.ReadAsync<TrackMap>(); 
            }
        }

        return (artists, playlists, tracks);
    }

    //retrieve typed instances with offset and capacity
    public async Task<IEnumerable<T>> Bring<T>(int offset, int capacity)
    {
        ReadOnlyMemory<char> dapperQuery;
        IEnumerable<T> result;

        if(typeof(T) == typeof(ArtistMap))
        {
            dapperQuery = "select AID, Name, Description, Avatar, Year from artists where Id >= @offset and Id <= @capacity".AsMemory();
        }
        else if(typeof(T) == typeof(PlaylistMap))
        {
            dapperQuery = "select PID, Name, Description, Avatar, Year from playlists where Id >= @offset and Id <= @capacity".AsMemory();
        }
        else if(typeof(T) == typeof(TrackMap))
        {
            dapperQuery = "select TID, Path, Name, Description, Avatar, Valid, Duration, Year from tracks where Id >= @offset and Id <= @capacity".AsMemory();
        }
        else if(typeof(T) == typeof(TagMap))
        {
            dapperQuery = "select TagID, Name, Color from tags where Id >= @offset and Id <= @capacity".AsMemory();
        }
        else return null;
    
        using (var connection = new SQLiteConnection(_connectionString.ToString()))
        {
            await connection.OpenAsync();
            result = await connection.QueryAsync<T>(dapperQuery.ToString(), new {offset=offset, capacity=capacity});
        }

        return result;
    }

    //retrieve single instance of required type
    public async Task<T> BringSingle<T>(Guid inputId)
    {
        ReadOnlyMemory<char> dapperQuery;
        T result;
        if(typeof(T) == typeof(ArtistMap))
        {
            dapperQuery = "select AID, Name, Description, Avatar, Year from artists where AID = @id".AsMemory();
        }
        else if(typeof(T) == typeof(PlaylistMap))
        {
            dapperQuery = "select PID, Name, Description, Avatar, Year from playlists where PID = @id".AsMemory();
        }
        else if(typeof(T) == typeof(TrackMap))
        {
            dapperQuery = "select TID, Path, Name, Description, Avatar, Valid, Duration, Year from tracks where TID = @id".AsMemory();
        }
        else if(typeof(T) == typeof(TagMap))
        {
            dapperQuery = "select TagID, Name, Color from tags where TagID = @id".AsMemory();
        }
        else throw new Exception("No supported map type!");

        using (var connection = new SQLiteConnection(_connectionString.ToString()))
        {
            await connection.OpenAsync();
            result = await connection.QuerySingleAsync<T>(dapperQuery.ToString(), new {id=inputId.ToString()});
        }
        return result;
    }

    //retrieve required type instances with predefined id
    public async Task<IEnumerable<T>> BringItemsById<T>(IEnumerable<Guid> ids)
    {
        ReadOnlyMemory<char> dapperQuery;
        IEnumerable<T> result;

        if(typeof(T) == typeof(ArtistMap))
        {
            dapperQuery = "select AID, Name, Description, Avatar, Year from artists where AID in @ids".AsMemory();
        }
        else if(typeof(T) == typeof(PlaylistMap))
        {
            dapperQuery = "select PID, Name, Description, Avatar, Year from playlists where PID in @ids".AsMemory();
        }
        else if(typeof(T) == typeof(TrackMap))
        {
            dapperQuery = "select TID, Path, Name, Description, Avatar, Valid, Duration, Year from tracks where TID in @ids".AsMemory();
        }
        else if(typeof(T) == typeof(TagMap))
        {
            dapperQuery = "select TagID, Name, Color from tags where TagID in @ids".AsMemory();
        }
        else throw new Exception("Fuck you, I dont wanna search yo shit!");
        
        using (var connection = new SQLiteConnection(_connectionString.ToString()))
        {
            await connection.OpenAsync();
            result = await connection.QueryAsync<T>(dapperQuery.ToString(), new {ids=ids.Select(i => i.ToString())} );
        }
        return result;
    }

    //retrieve item dependent store by instance id and required case
    public async Task<Store> BringStore(int tag, Guid id)
    {
        ReadOnlyMemory<char> dapperQuery;
        Store store = new Store(tag:tag);
        switch(tag)
        {
            case (1):
                dapperQuery = @"select AID, PID from artists_playlists where AID = @id;".AsMemory();
                var apPairs = await PairsObtain<ApPair>(dapperQuery, id);
                store.Holder = new Guid(apPairs.First().AID);
                store.Relates = apPairs.Select(x => new Guid(x.PID)).ToList();
                break;
            case (2):
                dapperQuery = @"select PID, AID from artists_playlists where PID = @id;".AsMemory();
                var paPairs = await PairsObtain<ApPair>(dapperQuery, id);
                store.Holder = new Guid(paPairs.First().PID);
                store.Relates = paPairs.Select(x => new Guid(x.AID)).ToList();
                break;
            case (3):
                dapperQuery = @"select AID, TID from artists_tracks where AID = @id;".AsMemory();
                var atPairs = await PairsObtain<AtPair>(dapperQuery, id);
                store.Holder = new Guid(atPairs.First().AID);
                store.Relates = atPairs.Select(x => new Guid(x.TID)).ToList();
                break;
            case (4):
                dapperQuery = @"select TID, AID from artists_tracks where TID = @id;".AsMemory();
                var taPairs = await PairsObtain<AtPair>(dapperQuery, id);
                store.Holder = new Guid(taPairs.First().TID);
                store.Relates = taPairs.Select(x => new Guid(x.AID)).ToList();
                break;
            case (5):
                dapperQuery = @"select PID, TID from playlists_tracks where PID = @id;".AsMemory();
                var ptPairs = await PairsObtain<PtPair>(dapperQuery, id);
                store.Holder = new Guid(ptPairs.First().PID);
                store.Relates = ptPairs.Select(x => new Guid(x.TID)).ToList();
                break;
            case (6):
                dapperQuery = @"select TID, PID from playlists_tracks where TID = @id;".AsMemory();
                var tpPairs = await PairsObtain<PtPair>(dapperQuery, id);
                store.Holder = new Guid(tpPairs.First().TID);
                store.Relates = tpPairs.Select(x => new Guid(x.PID)).ToList();
                break;
            case (7):
                dapperQuery = "select TagID, IID from tags_instances where TagID = @id;".AsMemory();
                var tagPairs = await PairsObtain<TagPair>(dapperQuery, id);
                store.Holder = new Guid(tagPairs.First().TagId);
                store.Relates = tagPairs.Select(x => new Guid(x.IID)).ToList();
                break;
            default: break;
        }

        return store;
    }
 
    public async Task<IEnumerable<CommonInstanceDTOMap>> RetrieveInstanceDTO(EntityTag entityTag)
    {
        IEnumerable<CommonInstanceDTOMap> result = default!;
        ReadOnlyMemory<char> dapperQuery = entityTag switch
        {
            EntityTag.ARTIST =>
                dapperQuery = "select AID, Name, Avatar, Year from artists;".AsMemory(),
            EntityTag.PLAYLIST => 
                dapperQuery = "select PID, Name, Avatar, Year from playlists;".AsMemory(),
            EntityTag.TRACK =>
                dapperQuery = "select TID, Name, Avatar, Year from tracks;".AsMemory()
        };

        using (var connection = new SQLiteConnection(_connectionString.ToString()))
        {
            await connection.OpenAsync();
            result = await connection.QueryAsync<CommonInstanceDTOMap>(dapperQuery.ToString());
        }
        return result;
    }

    public async Task<IEnumerable<CommonInstanceDTOMap>> RetrieveInstanceDTOById(EntityTag entityTag,
                                                                             IEnumerable<Guid> ids)
    {
        IEnumerable<CommonInstanceDTOMap> result = default!;
        ReadOnlyMemory<char> dapperQuery = entityTag switch
        {
            EntityTag.ARTIST =>
                dapperQuery = "select AID, Name, Avatar, Year from artists where AID in @ids;".AsMemory(),
            EntityTag.PLAYLIST => 
                dapperQuery = "select PID, Name, Avatar, Year from playlists where PID in @ids;".AsMemory(),
            EntityTag.TRACK =>
                dapperQuery = "select TID, Name, Avatar, Year from tracks where TID in @ids;".AsMemory()
        };

        using (var connection = new SQLiteConnection(_connectionString.ToString()))
        {
            await connection.OpenAsync();
            result = await connection.QueryAsync<CommonInstanceDTOMap>(dapperQuery.ToString(),
                                                                    new { ids=ids.Select(i => i.ToString()) });
        }

        return result;
    }


    //perform obtaining depended pair 
    private async Task<IEnumerable<T>> PairsObtain<T>(ReadOnlyMemory<char> dapperQuery, Guid id)
    {
        if(typeof(T) == typeof(ApPair) || typeof(T) == typeof(AtPair) || typeof(T) == typeof(PtPair))
        {
            IEnumerable<T> obtained;
            using (var connection = new SQLiteConnection(_connectionString.ToString()))
            {
                obtained = await connection.QueryAsync<T>(dapperQuery.ToString(), new {id=id.ToString()} );            
            }
            
            return obtained;
        }
        else throw new Exception("Not supported Pair type. There are only support for ap, at and pt pair type.");
    }
}
