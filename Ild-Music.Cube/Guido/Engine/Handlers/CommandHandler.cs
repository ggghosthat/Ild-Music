using Ild_Music.Core.Instances;

using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using Dapper;

namespace Cube.Guido.Engine.Handlers;

internal sealed class CommandHandler 
{
    private readonly IDbConnection _connection;
    
    public CommandHandler(IDbConnection connection)
    {
        _connection = connection;
    }

    //these methods needs to add instance and their relationships
    public Task AddArtist(Artist artist) 
    {
        using (IDbConnection connection = _connection)
        {
            connection.Open();

            using(var transaction = connection.BeginTransaction())
            {
                var artistBodyQuery = @"""insert or ignore into artists(AID, Name, Description, Year, Avatar) 
                                                    values (@AID, @Name, @Description, @Year, @Avatar) RETURNING AID""";

                var artistPlaylistQuery = @"""insert into artists_playlists(AID, PID) select @aid, @pid 
                                                where not EXISTS(SELECT 1 from artists_playlists where AID = @aid and PID = @pid)""";
                
                var artistTrackQuery = @"""insert into artists_tracks(AID, TID) select @aid, @tid 
                                            where not EXISTS(SELECT 1 from artists_tracks where AID = @aid and TID = @tid)""";

                var aidRaw = connection.ExecuteScalar<Guid>(artistBodyQuery,
                                                                new{
                                                                    AID = artist.Id,
                                                                    Name = artist.Name.ToString(),
                                                                    Description = artist.Description.ToString(),
                                                                    Year = artist.Year,
                                                                    Avatar = artist.AvatarSource.ToArray()
                                                                },
                                                                transaction);

                foreach(var artistPlaylistRelate in artist.Playlists)
                {
                    connection.Execute(artistPlaylistQuery,
                                        new{
                                            aid = aidRaw,
                                            pid = artistPlaylistRelate
                                        },
                                        transaction);
                }

                foreach(var artistTrackRelate in artist.Playlists)
                {
                    connection.Execute(artistTrackQuery,
                                        new{
                                            aid = aidRaw,
                                            tid = artistTrackRelate
                                        },
                                        transaction);
                }

                transaction.Commit();
            }
        }

        return Task.CompletedTask;
    }

    public Task AddPlaylist(Playlist playlist) 
    {
        using (IDbConnection connection = _connection)
        {
            connection.Open();

            using(var transaction = connection.BeginTransaction())
            {
                var playlistBodyQuery = @"""insert or ignore into playlists(PID, Name, Description, Year, Avatar) 
                                            values (@PID, @Name, @Description, @Year, @Avatar RETURNING PID""";

                var playlistArtistQuery = @"""insert into artists_playlists(AID, PID) select @aid, @pid 
                                                where not EXISTS(SELECT 1 from artists_playlists where AID = @aid and PID = @pid)""";
                
                var playlistTrackQuery = @"""insert into playlists_tracks(PID, TID) select @tid, @tid 
                                                where not EXISTS(SELECT 1 from playlists_tracks where PID = @pid and TID = @tid)""";

                var pidRaw = connection.ExecuteScalar<Guid>(playlistBodyQuery,
                                                            new{
                                                                PID = playlist.Id,
                                                                Name = playlist.Name.ToString(),
                                                                Description = playlist.Description.ToString(),
                                                                Year = playlist.Year,
                                                                Avatar = playlist.AvatarSource.ToArray()
                                                            },
                                                            transaction);

                foreach(var playlistArtistRelate in playlist.Artists)
                {
                    connection.Execute(playlistArtistQuery,
                                        new{
                                            aid = playlistArtistRelate,
                                            pid = pidRaw
                                        },
                                        transaction);
                }

                foreach(var playlistTrackRelate in playlist.Tracky)
                {
                    connection.Execute(playlistTrackQuery,
                                        new{
                                            pid = pidRaw,
                                            tid = playlistTrackRelate
                                        },
                                        transaction);
                }

                transaction.Commit();
            }
        }

        return Task.CompletedTask;
    }
    
    public Task AddTrack(Track track)
    {
        using (IDbConnection connection = _connection)
        {
            connection.Open();

            using(var transaction = connection.BeginTransaction())
            {
                var trackBodyQuery = @"""insert or ignore into tracks(TID, Path, Name, Description, Year, Avatar, Valid, Duration) 
                                            values (@TID, @Path, @Name, @Description, @Year, @Avatar, @IsValid, @Duration) RETURNING TID""";

                var trackArtistQuery = @"""insert into artists_tracks(AID, TID) select @aid, @tid 
                                                where not EXISTS(SELECT 1 from artists_tracks where AID = @aid and TID = @tid)""";

                var trackPlaylistQuery = @"""insert into artists_playlists(AID, PID) select @aid, @pid 
                                                where not EXISTS(SELECT 1 from artists_playlists where AID = @aid and PID = @pid)""";

                var tidRaw = connection.ExecuteScalar<Guid>(trackBodyQuery,
                                                            new{
                                                                TID = track.Id,
                                                                Path = track.Pathway.ToString(),
                                                                Name = track.Name.ToString(),
                                                                Description = track.Description.ToString(),
                                                                Year = track.Year,
                                                                Avatar = track.AvatarSource.ToArray()
                                                            },
                                                            transaction);

                foreach(var trackArtistRelate in track.Artists)
                {
                    connection.Execute(trackArtistQuery,
                                       new{
                                            aid = trackArtistRelate,
                                            tid = tidRaw
                                        },
                                        transaction);
                }

                foreach(var trackPlaylistRelate in track.Playlists)
                {
                    connection.Execute(trackPlaylistQuery,
                                        new{
                                            pid = trackPlaylistRelate,
                                            tid = tidRaw
                                        },
                                        transaction);
                }

                transaction.Commit();
            }
        }
        
        return Task.CompletedTask;
    }

    //TODO: correct tag table structure.
    public Task AddTag(Tag tag) 
    {
        using (IDbConnection connection = _connection)
        {
            connection.Open();

            using(var transaction = connection.BeginTransaction())
            {
                var tagBodyQuery = @"""insert or ignore into tags(TagID, Name, Color, EntityType) 
                                            values (@tag_id, @name, @color, @entity_type) RETURNING TagId""";

                var tagInstanceQuery = @"""insert into tags_instances(TagID, IID, EntityType) select @tag_id, @iid, @entity_type 
                                            where not EXISTS(SELECT 1 from tags_instances 
                                            where TagID = @tag_id and IID = @iid and EntityType=@entity_type)""";

            
                var tidRaw = connection.ExecuteScalar<Guid>(tagBodyQuery,
                                                            new{
                                                                tag_id = tag.Id,
                                                                name = tag.Name.ToString(),
                                                                color = tag.Color.ToString(),
                                                            },
                                                            transaction);

                foreach(var tagArtistRelate in tag.Artists)
                {
                    connection.Execute(tagInstanceQuery,
                                       new{
                                            tag_id = tag.Id,
                                            iid = tagArtistRelate,
                                            entity_tag = (int)EntityTag.ARTIST
                                        },
                                        transaction);
                }

                foreach(var tagPlaylistRelate in tag.Playlists)
                {
                    connection.Execute(tagInstanceQuery,
                                       new{
                                            tag_id = tag.Id,
                                            iid = tagPlaylistRelate,
                                            entity_tag = (int)EntityTag.PLAYLIST
                                        },
                                        transaction);
                }
 
                foreach(var tagTrackRelate in tag.Tracks)
                {
                    connection.Execute(tagInstanceQuery,
                                       new{
                                            tag_id = tag.Id,
                                            iid = tagTrackRelate,
                                            entity_tag = (int)EntityTag.TRACK
                                        },
                                        transaction);
                }

                transaction.Commit();
            }
        }
        
        return Task.CompletedTask;

    }


    //these methods editting instances and their relationships
    public async Task EditArtist()
    {}

    public async Task EditPlaylist() 
    {}

    public async Task EditTrack() 
    {}

    public async Task EditTag() 
    {}



    //these methods delleting instances and their relationships 
    public async Task DeleteArtist()
    {}

    public async Task DeletePlaylist() 
    {}

    public async Task DeleteTrack() 
    {}

    public async Task DeleteTag() 
    {}

}
