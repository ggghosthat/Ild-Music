using Ild_Music.Core.Instances;
using Cube.Mapper.Entities;

using System.Data.SQLite;
using Dapper;
namespace Cube.Storage.Guido.Engine;
internal class Fork
{
    private string _connectionString;

    public Fork(ref string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task Add<T>(T entity)
    {
        ReadOnlyMemory<char> dapperQuery;
        if (entity is ArtistMap artist)
        {
            dapperQuery = "insert or ignore into artists(AID, Name, Description, Year, Avatar) values (@AID, @Name, @Description, @Year, @Avatar)".AsMemory();
            using (var connection = new SQLiteConnection(_connectionString.ToString()))
            {                
                await connection.OpenAsync();
                await connection.ExecuteAsync(dapperQuery.ToString(), artist);     
            }
        }
        else if (entity is PlaylistMap playlist)
        {
            dapperQuery = "insert or ignore into playlists(PID, Name, Description, Year, Avatar) values (@PID, @Name, @Description, @Year, @Avatar)".AsMemory();
            using (var connection = new SQLiteConnection(_connectionString.ToString()))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(dapperQuery.ToString(), playlist );
            }
        }
        else if (entity is TrackMap track)
        {
            dapperQuery = "insert or ignore into tracks(TID, Path, Name, Description, Year, Avatar, Valid, Duration) values (@TID, @Path, @Name, @Description, @Year, @Avatar, @IsValid, @Duration)".AsMemory();
            using (var connection = new SQLiteConnection(_connectionString.ToString()))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(dapperQuery.ToString(), track);
            }
        }
        else if (entity is TagMap tag)
        {
            dapperQuery = "insert or ignore into tags(TagID, Name, Color) values (@Buid, @Name, @Color)".AsMemory();
            using (var connection = new SQLiteConnection(_connectionString.ToString()))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(dapperQuery.ToString(), tag);
            }
        }
    }

    public async Task AddStores(ICollection<Store> stores)
    {
        stores.ToList().ForEach(async store => 
        {  
            if(store.Relates is null || store.Relates.Count == 0)
                return;

            ReadOnlyMemory<char> dapperQuery;
            switch(store.Tag)
            {
                case(1):
                    dapperQuery = "insert into artists_playlists(AID, PID) select @aid, @pid where not EXISTS(SELECT 1 from artists_playlists where AID = @aid and PID = @pid)".AsMemory();
                    using (var connection = new SQLiteConnection(_connectionString.ToString()))
                    {
                        await connection.OpenAsync();
                        foreach(Guid relate in store.Relates)
                        {
                            await connection.ExecuteAsync(dapperQuery.ToString(), new {aid=store.Holder.ToString(), pid=relate.ToString() });
                        }
                    }
                    break;
                case(2):
                    dapperQuery = "insert into artists_tracks(AID, TID) select @aid, @tid where not EXISTS(SELECT 1 from artists_tracks where AID = @aid and TID = @tid)".AsMemory();
                    using (var connection = new SQLiteConnection(_connectionString.ToString()))
                    {
                        await connection.OpenAsync();
                        foreach(Guid relate in store.Relates)
                        {
                            await connection.ExecuteAsync(dapperQuery.ToString(), new {aid=store.Holder.ToString(), tid=relate.ToString() });
                        }
                    }
                    break;
                case(3):
                    dapperQuery = "insert into artists_playlists(AID, PID) select @aid, @pid where not EXISTS(SELECT 1 from artists_playlists where AID = @aid and PID = @pid)".AsMemory();
                    using (var connection = new SQLiteConnection(_connectionString.ToString()))
                    {
                        await connection.OpenAsync();
                        foreach(Guid relate in store.Relates)
                        {
                            await connection.ExecuteAsync(dapperQuery.ToString(), new {aid=relate.ToString(), pid=store.Holder.ToString() });
                        }
                    }
                    break;
                case(4):
                    dapperQuery = "insert into playlists_tracks(PID, TID) select @pid, @tid where not EXISTS(SELECT 1 from playlists_tracks where PID = @pid and TID = @tid)".AsMemory();
                    using (var connection = new SQLiteConnection(_connectionString.ToString()))
                    {
                        await connection.OpenAsync();
                        foreach(Guid relate in store.Relates)
                        {
                            await connection.ExecuteAsync(dapperQuery.ToString(), new {pid=store.Holder.ToString(), tid=relate.ToString() });
                        }
                    }
                    break;
                case(5):
                    dapperQuery = "insert into artists_tracks(AID, TID) select @aid, @tid where not EXISTS(SELECT 1 from artists_tracks where AID = @aid and TID = @tid)".AsMemory();
                    using (var connection = new SQLiteConnection(_connectionString.ToString()))
                    {
                        await connection.OpenAsync();
                        foreach(Guid relate in store.Relates)
                        {
                            await connection.ExecuteAsync(dapperQuery.ToString(), new {aid=relate.ToString(), tid=store.Holder.ToString() });
                        }
                    }
                    break;
                case(6):
                    dapperQuery = "insert into playlists_tracks(PID, TID) select @pid, @tid where not EXISTS(SELECT 1 from playlists_tracks where PID = @pid and TID = @tid)".AsMemory();
                    using (var connection = new SQLiteConnection(_connectionString.ToString()))
                    {
                        await connection.OpenAsync();
                        foreach(Guid relate in store.Relates)
                        {
                            await connection.ExecuteAsync(dapperQuery.ToString(), new {pid=relate.ToString(), tid=store.Holder.ToString() });
                        }
                    }
                    break;
                case(7):
                    dapperQuery = "insert into tags_instances(TagID, IID) select @tagid, @iid where not EXISTS(SELECT 1 from tags_instances where TagID = @tagid and IID = @iid)".AsMemory();
                    using (var connection = new SQLiteConnection(_connectionString.ToString()))
                    {
                        await connection.OpenAsync();
                        foreach(Guid relate in store.Relates)
                        {
                            await connection.ExecuteAsync(dapperQuery.ToString(), new {tagid=relate.ToString(), iid=store.Holder.ToString() });
                        }
                    }
                    break;
                default:break;
            }
        });
    }

    public async Task Edit<T>(T entity)
    {
        ReadOnlyMemory<char> dapperQuery;
        if (entity is ArtistMap artist)
        {
            dapperQuery = "update artists set Name = @Name, Description = @Description, Year = @Year, Avatar = @Avatar where AID = @AID".AsMemory();
            using (var connection = new SQLiteConnection(_connectionString.ToString()))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(dapperQuery.ToString(), artist);
            }
        }
        else if (entity is PlaylistMap playlist)
        {
            dapperQuery = "update playlists set Name = @Name, Description = @Description, Year = @Year, Avatar = @Avatar where PID = @PID".AsMemory();
            using (var connection = new SQLiteConnection(_connectionString.ToString()))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(dapperQuery.ToString(), playlist);
            }

        }
        else if (entity is TrackMap track)
        {
            dapperQuery = "update tracks set Path = @Pathway, Name = @Name, Description = @Description, Year = @Year, Avatar = @Avatar, Valid = @IsValid, Duration = @Duration where TID = @TID".AsMemory();
            using (var connection = new SQLiteConnection(_connectionString.ToString()))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(dapperQuery.ToString(), track);
            }

        }
    }

    public async Task EditStores(ICollection<Store> stores)
    {
       stores.ToList().ForEach(async store => 
        {   
            if(store.Relates is null || store.Relates.Count == 0)
                return;

            ReadOnlyMemory<char> dapperQuery;
            switch(store.Tag)
            {
                case(1):
                    dapperQuery = @"delete from artists_playlists where AID = @aid;
                                   insert into artists_playlists(AID, PID) select @aid, @pid where not EXISTS(SELECT 1 from artists_playlists where AID = @aid and PID = @pid);".AsMemory();
                    using (var connection = new SQLiteConnection(_connectionString.ToString()))
                    {   
                        await connection.OpenAsync();
                        foreach(Guid relate in store.Relates)
                        {
                            await connection.QueryMultipleAsync(dapperQuery.ToString(), new {aid=store.Holder.ToString(), pid=relate.ToString() });
                        }
                    }
                    break;
                case(2):
                    dapperQuery = @"delete from artists_tracks where AID = @aid;
                                    insert into artists_tracks(AID, TID) select @aid, @tid where not EXISTS(SELECT 1 from artists_tracks where AID = @aid and TID = @tid);".AsMemory();
                    using (var connection = new SQLiteConnection(_connectionString.ToString()))
                    {
                        await connection.OpenAsync();
                        foreach(Guid relate in store.Relates)
                        {
                            await connection.QueryMultipleAsync(dapperQuery.ToString(), new {aid=store.Holder.ToString(), tid=relate.ToString() });
                        }
                    }
                    break;
                case(3):
                    dapperQuery = @"delete from artists_playlists where PID = @pid;
                                    insert into artists_playlists(AID, PID) select @aid, @pid where not EXISTS(SELECT 1 from artists_playlists where AID = @aid and PID = @pid);".AsMemory();
                    using (var connection = new SQLiteConnection(_connectionString.ToString()))
                    {
                        await connection.OpenAsync();
                        foreach(Guid relate in store.Relates)
                        {
                            await connection.QueryMultipleAsync(dapperQuery.ToString(), new {aid=relate.ToString(), pid=store.Holder.ToString() });
                        }
                    }
                    break;
                case(4):
                    dapperQuery = @"delete from playlists_tracks where PID = @pid;
                                    insert into playlists_tracks(PID, TID) select @pid, @tid where not EXISTS(SELECT 1 from playlists_tracks where PID = @pid and TID = @tid);".AsMemory();
                    using (var connection = new SQLiteConnection(_connectionString.ToString()))
                    {
                        await connection.OpenAsync();
                        foreach(Guid relate in store.Relates)
                        {
                            await connection.QueryMultipleAsync(dapperQuery.ToString(), new {pid=store.Holder.ToString(), tid=relate.ToString() });
                        }
                    }
                    break;
                case(5):
                    dapperQuery = @"delete from artists_tracks where TID = @tid;
                                    insert into artists_tracks(AID, TID) select @aid, @tid where not EXISTS(SELECT 1 from artists_tracks where AID = @aid and TID = @tid);".AsMemory();
                    using (var connection = new SQLiteConnection(_connectionString.ToString()))
                    {
                        await connection.OpenAsync();
                        foreach(Guid relate in store.Relates)
                        {
                            await connection.QueryMultipleAsync(dapperQuery.ToString(), new {aid=relate.ToString(), tid=store.Holder.ToString() });
                        }
                    }
                    break;
                case(6):
                    dapperQuery = @"delete from playlists_tracks where TID = @tid;
                                    insert into playlists_tracks(PID, TID) select @pid, @tid where not EXISTS(SELECT 1 from playlists_tracks where PID = @pid and TID = @tid);".AsMemory();
                    using (var connection = new SQLiteConnection(_connectionString.ToString()))
                    {
                        await connection.OpenAsync();
                        foreach(Guid relate in store.Relates)
                        {
                            await connection.QueryMultipleAsync(dapperQuery.ToString(), new {pid=relate.ToString(), tid=store.Holder.ToString() });
                        }
                    }
                    break;
                case(7):
                    dapperQuery = @"delete from tags_instances where IID = @iid;
                                    insert into tags_instances(TagID, IID) select @tagid, @iid where not EXISTS(SELECT 1 from tags_instances where TagID = @tagid and IID = @iid);".AsMemory();
                    using (var connection = new SQLiteConnection(_connectionString.ToString()))
                    {
                        await connection.OpenAsync();
                        foreach(Guid relate in store.Relates)
                        {
                            await connection.QueryMultipleAsync(dapperQuery.ToString(), new {tagid=relate.ToString(), iid=store.Holder.ToString() });
                        }
                    }
                    break;
                default:break;
            }
        }); 
    }

    public async Task Delete<T>(T entity)
    {
        ReadOnlyMemory<char> dapperQuery;
        if(entity is Artist artist)
        {
            dapperQuery = @"delete from artists where AID = @aid;
                        delete from artists_playlists where AID = @aid;
                        delete from artists_tracks where AID = @aid;".AsMemory();

            using (var connection = new SQLiteConnection(_connectionString.ToString()))
            {
                await connection.OpenAsync();
                await connection.QueryMultipleAsync(dapperQuery.ToString(), new {aid=artist.Id.ToString()});
            }
        }
        else if(entity is Playlist playlist)
        {
            dapperQuery = @"delete from playlists where PID = @pid;
                            delete from artists_playlists where PID = @pid;
                            delete from playlists_tracks where PID = @pid;".AsMemory();

            using (var connection = new SQLiteConnection(_connectionString.ToString()))
            {
                await connection.OpenAsync();
                await connection.QueryMultipleAsync(dapperQuery.ToString(), new {pid=playlist.Id.ToString()});
            }
        }
        else if(entity is Track track)
        {
            dapperQuery = @"delete from tracks where TID = @tid;
                            delete from playlists_tracks where TID = @tid;
                            delete from artists_tracks where TID = @tid;".AsMemory();

            using (var connection = new SQLiteConnection(_connectionString.ToString()))
            {
                await connection.OpenAsync();
                await connection.QueryMultipleAsync(dapperQuery.ToString(), new {tid=track.Id.ToString()});
            }

        }
    }
}
