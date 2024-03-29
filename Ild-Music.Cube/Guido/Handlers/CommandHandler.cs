using Ild_Music.Core.Instances;
using Cube.Guido.Agents;

using System.Data;
using Dapper;

namespace Cube.Guido.Handlers;

internal sealed class CommandHandler 
{
    private readonly ReadOnlyMemory<char> _connectionString;
    
    public CommandHandler()
    {
    }

    

    //these methods needs to add instance and their relationships
    public Task AddArtist(Artist artist) 
    {
        //using (IDbConnection connection = new SQLiteConnection(_connectionString.ToString()))
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using(var transaction = connection.BeginTransaction())
            {
                var artistBodyQuery = "insert or ignore into artists(AID, Name, Description, Year, Avatar) values (@AID, @Name, @Description, @Year, @Avatar)";

                connection.Execute(artistBodyQuery,
                                    new{
                                        AID = artist.Id,
                                        Name = artist.Name.ToString(),
                                        Description = artist.Description.ToString(),
                                        Year = artist.Year,
                                        Avatar = artist.AvatarSource.ToArray()
                                    },
                                    transaction);


                if(artist.Playlists is not null && artist.Playlists.Count > 0)
                {
                    var artistPlaylistQuery = "insert into artists_playlists(AID, PID) select @aid, @pid where not EXISTS(SELECT 1 from artists_playlists where AID = @aid and PID = @pid)";
                 
                    foreach(var artistPlaylistRelate in artist.Playlists)
                    {
                        connection.Execute(artistPlaylistQuery,
                                            new{
                                                aid = artist.Id,
                                                pid = artistPlaylistRelate
                                            },
                                            transaction);
                    }
                }


                if(artist.Tracks is not null && artist.Tracks.Count > 0)
                {
                    var artistTrackQuery = "insert into artists_tracks(AID, TID) select @aid, @tid where not EXISTS(SELECT 1 from artists_tracks where AID = @aid and TID = @tid)";
                 
                    foreach(var artistTrackRelate in artist.Tracks)
                    {
                        connection.Execute(artistTrackQuery,
                                            new{
                                                aid = artist.Id,
                                                tid = artistTrackRelate
                                            },
                                            transaction);
                    }
                }

                if(artist.Tags is not null && artist.Tags.Count > 0)
                {
                    var tagArtistQuery = "insert into tags_instances(TagID, IID, EntityType) select @tag_id, @iid, @entity_type where not EXISTS(SELECT 1 from tags_instances where TagID = @tag_id and IID = @iid and EntityType=@entity_type)";
                    
                    foreach(var artistTag in artist.Tags)
                    {
                        connection.Execute(tagArtistQuery,
                                           new{
                                                tag_id = artistTag.Id,
                                                iid = artist.Id,
                                                entity_type = (int)EntityTag.ARTIST
                                            },
                                            transaction);
                    }
                }

                transaction.Commit();
            }
        }

        return Task.CompletedTask;
    }

    public Task AddPlaylist(Playlist playlist) 
    {
        //using (IDbConnection connection = new SQLiteConnection(_connectionString.ToString()))
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using(var transaction = connection.BeginTransaction())
            {
                var playlistBodyQuery = "insert or ignore into playlists(PID, Name, Description, Year, Avatar) values (@PID, @Name, @Description, @Year, @Avatar)";

                connection.Execute(playlistBodyQuery,
                                    new{
                                        PID = playlist.Id,
                                        Name = playlist.Name.ToString(),
                                        Description = playlist.Description.ToString(),
                                        Year = playlist.Year,
                                        Avatar = playlist.AvatarSource.ToArray()
                                    },
                                    transaction);


                if(playlist.Artists is not null && playlist.Artists.Count > 0)
                {
                    var playlistArtistQuery = "insert into artists_playlists(AID, PID) select @aid, @pid where not EXISTS(SELECT 1 from artists_playlists where AID = @aid and PID = @pid)";
                 
                    foreach(var playlistArtistRelate in playlist.Artists)
                    {
                        connection.Execute(playlistArtistQuery,
                                            new{
                                                aid = playlistArtistRelate,
                                                pid = playlist.Id
                                            },
                                            transaction);
                    }
                }

                if(playlist.Tracky is not null && playlist.Tracky.Count > 0)
                {
                    var playlistTrackQuery = "insert into playlists_tracks(PID, TID) select @tid, @tid where not EXISTS(SELECT 1 from playlists_tracks where PID = @pid and TID = @tid)";
                    
                    foreach(var playlistTrackRelate in playlist.Tracky)
                    {
                        connection.Execute(playlistTrackQuery,
                                            new{
                                                pid = playlist.Id,
                                                tid = playlistTrackRelate
                                            },
                                            transaction);
                    }
                }

                if(playlist.Tags is not null && playlist.Tags.Count > 0)
                {
                    var tagPlaylistQuery = "insert into tags_instances(TagID, IID, EntityType) select @tag_id, @iid, @entity_type where not EXISTS(SELECT 1 from tags_instances where TagID = @tag_id and IID = @iid and EntityType=@entity_type)";
                 
                    foreach(var playlistTag in playlist.Tags)
                    {
                        connection.Execute(tagPlaylistQuery,
                                           new{
                                                tag_id = playlistTag.Id,
                                                iid = playlist.Id,
                                                entity_type = (int)EntityTag.PLAYLIST
                                            },
                                            transaction);
                    }
                }

                transaction.Commit();
            }
        }

        return Task.CompletedTask;
    }
    
    public Task AddTrack(Track track)
    {
        //using (IDbConnection connection = new SQLiteConnection(_connectionString.ToString()))
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using(var transaction = connection.BeginTransaction())
            {
                var trackBodyQuery = "insert or ignore into tracks(TID, Path, Name, Description, Year, Avatar, Valid, Duration) values (@TID, @Path, @Name, @Description, @Year, @Avatar, @IsValid, @Duration)";

                connection.Execute(trackBodyQuery,
                                    new{
                                        TID = track.Id,
                                        Path = track.Pathway.ToString(),
                                        Name = track.Name.ToString(),
                                        Description = track.Description.ToString(),
                                        Year = track.Year,
                                        Avatar = track.AvatarSource.ToArray(),
                                        IsValid = track.IsValid,
                                        Duration = track.Duration
                                    },
                                    transaction);


                if(track.Artists is not null && track.Artists.Count > 0)
                {
                    var trackArtistQuery = "insert into artists_tracks(AID, TID) select @aid, @tid where not EXISTS(SELECT 1 from artists_tracks where AID = @aid and TID = @tid)";

                    foreach(var trackArtistRelate in track.Artists)
                    {
                        connection.Execute(trackArtistQuery,
                                           new{
                                                aid = trackArtistRelate,
                                                tid = track.Id
                                            },
                                            transaction);
                    }
                }

                if(track.Playlists is not null && track.Playlists.Count > 0)
                {
                    var trackPlaylistQuery = "insert into playlists_tracks(PID, TID) select @pid, @tid where not EXISTS(SELECT 1 from playlists_tracks where PID = @pid and TID = @tid)";

                    foreach(var trackPlaylistRelate in track.Playlists)
                    {
                        connection.Execute(trackPlaylistQuery,
                                            new{
                                                pid = trackPlaylistRelate,
                                                tid = track.Id
                                            },
                                            transaction);
                    }
                }

                if(track.Tags is not null && track.Tags.Count > 0)
                {
                    var tagTrackQuery = "insert into tags_instances(TagID, IID, EntityType) select @tag_id, @iid, @entity_type where not EXISTS(SELECT 1 from tags_instances where TagID = @tag_id and IID = @iid and EntityType=@entity_type)";

                    foreach(var trackTag in track.Tags)
                    {
                        connection.Execute(tagTrackQuery,
                                           new{
                                                tag_id = trackTag.Id,
                                                iid = track.Id,
                                                entity_type = (int)EntityTag.TRACK
                                            },
                                            transaction);
                    }
                }

                transaction.Commit();
            }
        }
        
        return Task.CompletedTask;
    }

    public Task AddTag(Tag tag) 
    {
        //using (IDbConnection connection = new SQLiteConnection(_connectionString.ToString()))
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using(var transaction = connection.BeginTransaction())
            {
                var tagBodyQuery = "insert or ignore into tags(TagID, Name, Color) values (@tag_id, @name, @color)";
            
                connection.Execute(tagBodyQuery,
                                    new{
                                        tag_id = tag.Id,
                                        name = tag.Name.ToString(),
                                        color = tag.Color.ToString(),
                                    },
                                    transaction);
                

                if(tag.Artists is not null && tag.Artists.Count > 0)
                {
                    var tagInstanceQuery = "insert into tags_instances(TagID, IID, EntityType) select @tag_id, @iid, @entity_type where not EXISTS(SELECT 1 from tags_instances where TagID = @tag_id and IID = @iid and EntityType=@entity_type)";
                    
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
                }

                if(tag.Playlists is not null && tag.Playlists.Count > 0)
                {
                    var tagInstanceQuery = "insert into tags_instances(TagID, IID, EntityType) select @tag_id, @iid, @entity_type where not EXISTS(SELECT 1 from tags_instances where TagID = @tag_id and IID = @iid and EntityType=@entity_type)";
                
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
                }

                if(tag.Tracks is not null && tag.Tracks.Count > 0)
                {
                    var tagInstanceQuery = "insert into tags_instances(TagID, IID, EntityType) select @tag_id, @iid, @entity_type where not EXISTS(SELECT 1 from tags_instances where TagID = @tag_id and IID = @iid and EntityType=@entity_type)";
                
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
                }

                transaction.Commit();
            }
        }
        
        return Task.CompletedTask;

    }


    //these methods editting instances and their relationships
    public Task EditArtist(Artist newArtist)
    {
        //using (IDbConnection connection = new SQLiteConnection(_connectionString.ToString()) )  
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
           connection.Open(); 
           using (var transaction = connection.BeginTransaction())
           {
                var updateArtistQuery = "update artists set Name = @Name, Description = @Description, Year = @Year, Avatar = @Avatar where AID = @AID";
                
                connection.Execute(updateArtistQuery,
                                    new{                            
                                        AID = newArtist.Id,
                                        Name = newArtist.Name.ToString(),
                                        Description = newArtist.Description.ToString(),
                                        Avatar = newArtist.AvatarSource.ToArray(),
                                        Year = newArtist.Year.ToString()
                                    },
                                    transaction);

                if(newArtist.Playlists is not null && newArtist.Playlists.Count > 0)
                {
                    var updateArtistPlaylistQuery = @"delete from artists_playlists where AID = @aid;
                                                      insert into artists_playlists(AID, PID) select @aid, @pid where not EXISTS(SELECT 1 from artists_playlists where AID = @aid and PID = @pid)";
                 
                    foreach(var artistPlaylistRelate in newArtist.Playlists)
                    {
                        connection.Execute(updateArtistPlaylistQuery,
                                            new{
                                                aid = newArtist.Id,
                                                pid = artistPlaylistRelate
                                            },
                                            transaction);
                    }
                }


                if(newArtist.Tracks is not null && newArtist.Tracks.Count > 0)
                {
                    var updateArtistTrackQuery = @"delete from artists_tracks where AID = @aid;
                                                   insert into artists_tracks(AID, TID) select @aid, @tid where not EXISTS(SELECT 1 from artists_tracks where AID = @aid and TID = @tid)";
                 
                    foreach(var artistTrackRelate in newArtist.Tracks)
                    {
                        connection.Execute(updateArtistTrackQuery,
                                            new{
                                                aid = newArtist.Id,
                                                tid = artistTrackRelate
                                            },
                                            transaction);
                    }
                }

                if(newArtist.Tags is not null && newArtist.Tags.Count > 0)
                {
                    var updateTagArtistQuery = @"delete from tags_instances where IID = @iid and EntityType = @entity_type;
                                                 insert into tags_instances(TagID, IID, EntityType) select @tag_id, @iid, @entity_type where not EXISTS(SELECT 1 from tags_instances where TagID = @tag_id and IID = @iid and EntityType=@entity_type)";
                    
                    foreach(var artistTag in newArtist.Tags)
                    {
                        connection.Execute(updateTagArtistQuery,
                                           new{
                                                tag_id = artistTag.Id,
                                                iid = newArtist.Id,
                                                entity_type = (int)EntityTag.ARTIST
                                            },
                                            transaction);
                    }
                }

                transaction.Commit(); 
           }
        }

        return Task.CompletedTask;
    }

    public Task EditPlaylist(Playlist newPlaylist) 
    {
        //using (IDbConnection connection = new SQLiteConnection(_connectionString.ToString()) ) 
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
           connection.Open(); 
           using (var transaction = connection.BeginTransaction())
           {
                var updatePlaylistQuery = "update playlists set Name = @Name, Description = @Description, Year = @Year, Avatar = @Avatar where PID = @PID";
                
                connection.Execute(updatePlaylistQuery,
                                    new{                            
                                        PID = newPlaylist.Id,
                                        Name = newPlaylist.Name.ToString(),
                                        Description = newPlaylist.Description.ToString(),
                                        Avatar = newPlaylist.AvatarSource.ToArray(),
                                        Year = newPlaylist.Year.ToString()
                                    },
                                    transaction);

                if(newPlaylist.Artists is not null && newPlaylist.Artists.Count > 0)
                {
                    var updatePlaylistArtistsQuery = @"delete from artists_playlists where PID = @pid;
                                                       insert into artists_playlists(AID, PID) select @aid, @pid where not EXISTS(SELECT 1 from artists_playlists where AID = @aid and PID = @pid);";
                 
                    foreach(var playlistArtistRelate in newPlaylist.Artists)
                    {
                        connection.Execute(updatePlaylistArtistsQuery,
                                            new{
                                                aid = playlistArtistRelate,
                                                pid = newPlaylist.Id
                                            },
                                            transaction);
                    }
                }


                if(newPlaylist.Tracky is not null && newPlaylist.Tracky.Count > 0)
                {
                    var updateArtistTrackQuery = @"delete from playlists_tracks where PID = @pid;
                                                   insert into playlists_tracks(PID, TID) select @pid, @tid where not EXISTS(SELECT 1 from playlists_tracks where PID = @pid and TID = @tid);";
                 
                    foreach(var playlistTrackRelate in newPlaylist.Tracky)
                    {
                        connection.Execute(updateArtistTrackQuery,
                                            new{
                                                pid = newPlaylist.Id,
                                                tid = playlistTrackRelate
                                            },
                                            transaction);
                    }
                }

                if(newPlaylist.Tags is not null && newPlaylist.Tags.Count > 0)
                {
                    var updateTagPlaylistQuery = @"delete from tags_instances where IID = @iid and EntityType = @entity_type;
                                                   insert into tags_instances(TagID, IID, EntityType) select @tag_id, @iid, @entity_type where not EXISTS(SELECT 1 from tags_instances where TagID = @tag_id and IID = @iid and EntityType=@entity_type)";
                    
                    foreach(var playlistTag in newPlaylist.Tags)
                    {
                        connection.Execute(updateTagPlaylistQuery,
                                           new{
                                                tag_id = playlistTag.Id,
                                                iid = newPlaylist.Id,
                                                entity_type = (int)EntityTag.PLAYLIST
                                            },
                                            transaction);
                    }
                }

                transaction.Commit(); 
           }
        }

        return Task.CompletedTask;
    }

    public Task EditTrack(Track newTrack)
    {
        //using (IDbConnection connection = new SQLiteConnection(_connectionString.ToString()) ) 
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
           connection.Open(); 
           using (var transaction = connection.BeginTransaction())
           {
                var updateTrackQuery = "update tracks set Path = @Pathway, Name = @Name, Description = @Description, Avatar = @Avatar, Year = @Year, Valid = @IsValid, Duration = @Duration where TID = @TID";
                connection.Execute(updateTrackQuery,
                                    new{                            
                                        TID = newTrack.Id,
                                        Pathway = newTrack.Pathway.ToString(),
                                        Name = newTrack.Name.ToString(),
                                        Description = newTrack.Description.ToString(),
                                        Avatar = newTrack.AvatarSource.ToArray(),
                                        Year = newTrack.Year.ToString(),
                                        IsValid = newTrack.IsValid?1:0,
                                        Duration = newTrack.Duration.ToString()
                                    },
                                    transaction);

                if(newTrack.Artists is not null && newTrack.Artists.Count > 0)
                {
                    var updatePlaylistArtistsQuery = @"delete from artists_tracks where TID = @tid;
                                                      insert into artists_tracks(AID, TID) select @aid, @tid where not EXISTS(SELECT 1 from artists_tracks where AID = @aid and TID = @tid);";
                 
                    foreach(var trackArtistRelate in newTrack.Artists)
                    {
                        connection.Execute(updatePlaylistArtistsQuery,
                                            new{
                                                aid = trackArtistRelate,
                                                tid = newTrack.Id
                                            },
                                            transaction);
                    }
                }


                if(newTrack.Playlists is not null && newTrack.Playlists.Count > 0)
                {
                    var updateTrackPlaylistQuery = @"delete from playlists_tracks where TID = @tid;
                                                   insert into playlists_tracks(PID, TID) select @pid, @tid where not EXISTS(SELECT 1 from playlists_tracks where PID = @pid and TID = @tid);";
                 
                    foreach(var trackPlaylistRelate in newTrack.Playlists)
                    {
                        connection.Execute(updateTrackPlaylistQuery,
                                            new{
                                                tid = newTrack.Id,
                                                pid = trackPlaylistRelate
                                            },
                                            transaction);
                    }
                }

                if(newTrack.Tags is not null && newTrack.Tags.Count > 0)
                {
                    var updateTagPlaylistQuery = @"delete from tags_instances where IID = @iid and EntityType = @entity_type;
                                                   insert into tags_instances(TagID, IID, EntityType) select @tag_id, @iid, @entity_type where not EXISTS(SELECT 1 from tags_instances where TagID = @tag_id and IID = @iid and EntityType=@entity_type)";
                    
                    foreach(var trackTag in newTrack.Tags)
                    {
                        connection.Execute(updateTagPlaylistQuery,
                                           new{
                                                tag_id = trackTag.Id,
                                                iid = newTrack.Id,
                                                entity_type = (int)EntityTag.TRACK
                                            },
                                            transaction);
                    }
                }

                transaction.Commit(); 
           }
        }

        return Task.CompletedTask;
    }

    public Task EditTag(Tag newTag)
    {
        //using (IDbConnection connection = new SQLiteConnection(_connectionString.ToString()))   
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using(var transaction = connection.BeginTransaction())
            {
                var updateTagQuery = "update tags set Name = @name, Color = @color where TagID = @tag_id,";
            
                connection.Execute(updateTagQuery,
                                    new{
                                        tag_id = newTag.Id,
                                        name = newTag.Name.ToString(),
                                        color = newTag.Color.ToString(),
                                    },
                                    transaction);
                

                if(newTag.Artists is not null && newTag.Artists.Count > 0)
                {
                    var tagInstanceQuery = @"delete from tags_instances where IID = @iid and EntityType = @entity_type;
                                             insert into tags_instances(TagID, IID, EntityType) select @tag_id, @iid, @entity_type where not EXISTS(SELECT 1 from tags_instances where TagID = @tag_id and IID = @iid and EntityType=@entity_type)";
                    
                    foreach(var tagArtistRelate in newTag.Artists)
                    {
                        connection.Execute(tagInstanceQuery,
                                           new{
                                                tag_id = newTag.Id,
                                                iid = tagArtistRelate,
                                                entity_tag = (int)EntityTag.ARTIST
                                            },
                                            transaction);
                    }
                }

                if(newTag.Playlists is not null && newTag.Playlists.Count > 0)
                {
                    var tagInstanceQuery = @"delete from tags_instances where IID = @iid and EntityType = @entity_type;
                                             insert into tags_instances(TagID, IID, EntityType) select @tag_id, @iid, @entity_type where not EXISTS(SELECT 1 from tags_instances where TagID = @tag_id and IID = @iid and EntityType=@entity_type)";
                
                    foreach(var tagPlaylistRelate in newTag.Playlists)
                    {
                        connection.Execute(tagInstanceQuery,
                                           new{
                                                tag_id = newTag.Id,
                                                iid = tagPlaylistRelate,
                                                entity_tag = (int)EntityTag.PLAYLIST
                                           },
                                           transaction);
                    }
                }

                if(newTag.Tracks is not null && newTag.Tracks.Count > 0)
                {
                    var tagInstanceQuery = @"delete from tags_instances where IID = @iid and EntityType = @entity_type;
                                             insert into tags_instances(TagID, IID, EntityType) select @tag_id, @iid, @entity_type where not EXISTS(SELECT 1 from tags_instances where TagID = @tag_id and IID = @iid and EntityType=@entity_type)";
                
                    foreach(var tagTrackRelate in newTag.Tracks)
                    {
                        connection.Execute(tagInstanceQuery,
                                           new{
                                                tag_id = newTag.Id,
                                                iid = tagTrackRelate,
                                                entity_tag = (int)EntityTag.TRACK
                                            },
                                            transaction);
                    }
                }

                transaction.Commit();
            }
        }
        
        return Task.CompletedTask;

    }



    //these methods delleting instances and their relationships 
    public Task DeleteArtist(Guid artistId)
    {
        //using (IDbConnection connection = new SQLiteConnection(_connectionString.ToString()))
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();
        
            using (var transaction = connection.BeginTransaction())
            {
                var deleteArtistQuery = @"delete from artists where AID = @aid;
                                          delete from artists_tracks where AID = @aid;
                                          delete from artists_playlists where AID = @aid;";
    
                connection.Execute(deleteArtistQuery,
                                   new {aid = artistId},
                                   transaction);

                transaction.Commit();
            }
        }

        return Task.CompletedTask;
    }

    public Task DeletePlaylist(Guid playlistId) 
    {
        //using (IDbConnection connection = new SQLiteConnection(_connectionString.ToString())) 
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();
        
            using (var transaction = connection.BeginTransaction())
            {
                var deletePlaylistQuery = @"delete from playlists where PID = @pid;
                                          delete from artists_playlists where PID = @pid;                                          
                                          delete from playlists_tracks where PID = @pid;";
    
                connection.Execute(deletePlaylistQuery,
                                   new {pid = playlistId},
                                   transaction);

                transaction.Commit();
            }
        }

        return Task.CompletedTask;
    }

    public Task DeleteTrack(Guid trackId) 
    {
        //using (IDbConnection connection = new SQLiteConnection(_connectionString.ToString()))
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();
        
            using (var transaction = connection.BeginTransaction())
            {
                var deleteTrackQuery = @"delete from tracks where TID = @tid;
                                          delete from artists_tracks where TID = @tid;                                          
                                          delete from playlists_tracks where TID = @tid;";
    
                connection.Execute(deleteTrackQuery,
                                   new {tid = trackId},
                                   transaction);

                transaction.Commit();
            }
        }

        return Task.CompletedTask;

    }

    public Task DeleteTag(Guid tagId) 
    {
        //using (IDbConnection connection = new SQLiteConnection(_connectionString.ToString()))
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();
        
            using (var transaction = connection.BeginTransaction())
            {
                var deleteTagQuery = @"delete from tags where TagID = @tag_id;
                                          delete from tags_instances where TagID = @tag_id;";
    
                connection.Execute(deleteTagQuery,
                                   new {tag_id = tagId},
                                   transaction);

                transaction.Commit();
            }
        }

        return Task.CompletedTask;
    }
}
