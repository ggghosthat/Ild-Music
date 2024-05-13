using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Instances.Querying;
using Cube.Guido.Agents;
using Ild_Music.Core.Instances;

using System.Data;
using Dapper;

namespace Cube.Guido.Handlers;

internal sealed class QueryHandler
{    
    public QueryHandler()
    {
    }

    //This method require top items for each type with a predefined count.
    //Please, use this method just for initialization puprose.
    //In other way will increase memory leak.
    public Task<QueryPool> QueryTopPool()
    {
        QueryPool resultPool = new QueryPool();
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var commonQuery = @"
                    SELECT AID, Name, Avatar FROM artists
                    WHERE Id > 0 AND Id <= @pageLimit;
			
			        SELECT PID, Name, Avatar FROM playlists
			        WHERE Id > 0 AND Id <= @pageLimit;
			
			        SELECT TID, Name, Avatar FROM tracks
			        WHERE Id > 0 AND Id <= @pageLimit;";
                
                using(var multiQuery = connection.QueryMultiple(
                        commonQuery,
                        new {pageLimit = ConnectionAgent.QueryLimit},
                        transaction))
                {
                    var artistsDTO = multiQuery.Read()
                        .Select(a => new CommonInstanceDTO( 
                            id: new Guid(a.AID),
                            name: ((string)a.Name).AsMemory(),
                            avatar: a.Avatar,
                            tag: EntityTag.ARTIST))
                        .ToList();

                    var playlistsDTO = multiQuery.Read()
                        .Select(a => new CommonInstanceDTO( 
                            id: new Guid(a.PID),
                            name: ((string)a.Name).AsMemory(),
                            avatar: a.Avatar,
                            tag: EntityTag.PLAYLIST))
                        .ToList(); 

                    var tracksDTO = multiQuery.Read()
                        .Select(a => new CommonInstanceDTO( 
                            id: new Guid(a.TID),
                            name: ((string)a.Name).AsMemory(),
                            avatar: a.Avatar,
                            tag: EntityTag.TRACK))
                        .ToList(); 

                    resultPool.ArtistsDTOs = artistsDTO;
                    resultPool.PlaylistsDTOs = playlistsDTO;
                    resultPool.TracksDTOs = tracksDTO;
                }
            }
        }

        return Task.FromResult<QueryPool>(resultPool);
    }

    public Task<IEnumerable<CommonInstanceDTO>> QueryArtists()
    {
        IEnumerable<CommonInstanceDTO> artistsDTOs;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var artistsPageQuery = @"
                    SELECT AID, Name, Avatar FROM artists
                    WHERE Id > @offset AND Id <= @pageLimit;";

                var limit = ConnectionAgent.ArtistOffset + ConnectionAgent.QueryLimit;
                
                artistsDTOs = connection.Query(
                    artistsPageQuery,
                    new { offset = ConnectionAgent.ArtistOffset, pageLimit = limit },
                    transaction)
                .Select(a => new CommonInstanceDTO( 
                    id: new Guid(a.AID),
                    name: ((string)a.Name).AsMemory(),
                    avatar: a.Avatar,
                    tag: EntityTag.ARTIST))
                .ToList();

                ConnectionAgent.ArtistOffset = limit;
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(artistsDTOs);
    }

    public Task<IEnumerable<CommonInstanceDTO>> QueryPlaylists()
    {
        IEnumerable<CommonInstanceDTO> playlistsDTOs;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var playlistsPageQuery = @"
                    SELECT PID, Name, Avatar FROM playlists
                    WHERE Id > @offset AND Id <= @pageLimit;";
               
                var limit = ConnectionAgent.PlaylistOffset + ConnectionAgent.QueryLimit;

                playlistsDTOs = connection.Query(
                    playlistsPageQuery,
                    new { offset = ConnectionAgent.PlaylistOffset, pageLimit = limit },
                    transaction)
                .Select(p => new CommonInstanceDTO( 
                    id: new Guid(p.PID),
                    name: ((string)p.Name).AsMemory(),
                    avatar: p.Avatar,
                    tag: EntityTag.PLAYLIST))
                .ToList(); 

                ConnectionAgent.PlaylistOffset = limit;
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(playlistsDTOs);
    }

    public Task<IEnumerable<CommonInstanceDTO>> QueryTracks()
    {
        IEnumerable<CommonInstanceDTO> tracksDTOs;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var tracksPageQuery = @"
                    SELECT TID, Name, Avatar FROM tracks
                    WHERE Id > @offset AND Id <= @pageLimit;";
               
                var limit = ConnectionAgent.TrackOffset + ConnectionAgent.QueryLimit;

                tracksDTOs = connection.Query(
                    tracksPageQuery,
                    new { offset = ConnectionAgent.TrackOffset, pageLimit = limit },
                    transaction)
                .Select(t => new CommonInstanceDTO( 
                    id: new Guid(t.TID),
                    name: ((string)t.Name).AsMemory(),
                    avatar: t.Avatar,
                    tag: EntityTag.TRACK))
                .ToList();

                ConnectionAgent.TrackOffset = limit;
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(tracksDTOs);
    }

    public Task<IEnumerable<Tag>> QueryTags()
    {
        IEnumerable<Tag> tags;

        using(IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var tagsPageQuery = @"
                    SELECT TagID, Name, Color FROM tags
                    WHERE Id > @offset AND Id <= @pageLimit;";

                var limit = ConnectionAgent.TagOffset + ConnectionAgent.QueryLimit;

                tags = connection.Query(
                    tagsPageQuery,
                    new { offset = ConnectionAgent.TagOffset, pageLimit = limit },
                    transaction)
                .Select(tag => new Tag(
                    id: new Guid(tag.Id),
                    name: ((string)tag.Name).AsMemory(),
                    color: ((string)tag.Color).AsMemory()))
                .ToList();

                ConnectionAgent.TagOffset = limit;
            }
        }

        return Task.FromResult<IEnumerable<Tag>>(tags);
    }

    public Task<Artist> QuerySingleArtist(ref CommonInstanceDTO instanceDTO)
    {
        Artist artist;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var artistId = instanceDTO.Id.ToString();

                //setting up main and extra properties for artist body
                var artistExtraPropsQuery = @"
                    SELECT Description, Year FROM artists
                    WHERE AID = @aid";
               
                var arttistPlaylistsQuery = @"
                    SELECT PID FROM artists_playlists
                    WHERE AID = @aid";

                var arttistTracksQuery = @"
                    SELECT TID FROM artists_tracks
                    WHERE AID = @aid";

                var arttistTagsQuery = @"
                    SELECT t.TagID, t.Name, t.Color, ti.IID 
                    FROM tags_instances as ti
                    INNER JOIN tags as t
                    ON t.TagID = ti.TagID
                    WHERE ti.IID = @aid";

                var extraProps = connection.QueryFirst(
                    artistExtraPropsQuery,
                    new { aid = artistId },
                    transaction);

                artist = new (
                    instanceDTO.Id, 
                    instanceDTO.Name,
                    extraProps.Description.AsMemory(),
                    instanceDTO.Avatar,
                    extraProps .Year);

                artist.Playlists = connection.Query(
                    arttistPlaylistsQuery,
                    new { aid = artistId },
                    transaction)
                .Select(pid => new Guid(pid))
                .ToList();

                artist.Tracks = connection.Query(
                    arttistTracksQuery,
                    new { aid = artistId },
                    transaction)
                .Select(tid => new Guid(tid))
                .ToList();

                artist.Tags = connection.Query<Tag>(
                    arttistTagsQuery,
                    new { aid = artistId },
                    transaction)
                .Select(tag => tag)
                .ToList();               
            }
        }

        return Task.FromResult<Artist>(artist);
    }

    public Task<Playlist> QuerySinglePlaylist(ref CommonInstanceDTO instanceDTO)
    {
        Playlist playlist;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var playlistId = instanceDTO.Id.ToString();

                //setting up main and extra properties for artist body
                var artistExtraPropsQuery = @"
                    SELECT Description, Year FROM playlists
                    WHERE PID = @pid";
               
                var playlistArtistsQuery = @"
                    SELECT AID FROM artists_playlists
                    WHERE PID = @pid";

                var playlistTracksQuery = @"
                    SELECT TID FROM playlists_tracks
                    WHERE PID = @pid";

                var playlistTagsQuery = @"
                    SELECT t.TagID, t.Name, t.Color, ti.IID 
                    FROM tags_instances as ti
                    INNER JOIN tags as t
                    ON t.TagID = ti.TagID
                    WHERE ti.IID = @pid";


                var extraProps = connection.QueryFirst(
                    artistExtraPropsQuery,
                    new { id = playlistId },
                    transaction);

                playlist = new (
                    instanceDTO.Id, 
                    instanceDTO.Name,
                    extraProps.Description.AsMemory(),
                    instanceDTO.Avatar,
                    extraProps .Year);
                
                playlist.Artists  = connection.Query(
                    playlistArtistsQuery,
                    new { pid = playlistId },
                    transaction)
                .Select(pid => new Guid(pid))
                .ToList();

                playlist.Tracky = connection.Query(
                    playlistTracksQuery,
                    new { pid = playlistId },
                    transaction)
                .Select(tid => new Guid(tid))
                .ToList();

                playlist.Tags = connection.Query<Tag>(
                    playlistTagsQuery,
                    new { pid = playlistId },
                    transaction)
                .Select(tag => tag)
                .ToList();               
            }
        }

        return Task.FromResult<Playlist>(playlist);
    }

    public Task<Track> QuerySingleTrack(ref CommonInstanceDTO instanceDTO)
    {
        Track track;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection .Open();

            using (var transaction = connection.BeginTransaction())
            {
                var trackId = instanceDTO.Id.ToString();

                //setting up main and extra properties for artist body
                var artistExtraPropsQuery = @"
                    SELECT Description, Year, Valid, Duration FROM tracks
                    WHERE TID = @tid";
               
                var trackArtistsQuery = @"
                    SELECT AID FROM artists_tracks
                    WHERE TID = @tid";

                var trackPlaylistsQuery = @"
                    SELECT PID FROM playlists_tracks
                    WHERE TID = @tid";

                var trackTagsQuery = @"
                    SELECT t.TagID, t.Name, t.Color, ti.IID 
                    FROM tags_instances as ti
                    INNER JOIN tags as t
                    ON t.TagID = ti.TagID
                    WHERE ti.IID = @tid";

                var extraProps = connection.QueryFirst(
                    artistExtraPropsQuery,
                    new { pid = trackId },
                    transaction);

                if(!extraProps.Valid)
                    return default;

                track = new (
                    instanceDTO.Id,
                    extraProps .Path,
                    instanceDTO.Name,
                    extraProps.Description.AsMemory(),
                    instanceDTO.Avatar,
                    extraProps.Duration,
                    extraProps .Year);

                track.Artists  = connection.Query(
                    trackArtistsQuery,
                    new { tid = trackId },
                    transaction)
                .Select(aid => new Guid(aid))
                .ToList();

                track.Playlists = connection.Query(
                    trackPlaylistsQuery,
                    new { tid = trackId },
                    transaction)
                .Select(tid => new Guid(tid))
                .ToList();

                track.Tags = connection.Query<Tag>(
                    trackTagsQuery,
                    new { tid = trackId },
                    transaction)
                .Select(tag => tag)
                .ToList();               
            }
        }

        return Task.FromResult<Track>(track);
    }

    public Task<Tag> QuerySingleTag(Guid tagId)
    {
        Tag tag;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var tagIdString = tagId.ToString();

                //setting up main and extra properties for artist body
                var tagQuery = @"
                    SELECT t.TagId as Id, t.Name, t.Color, ti.IID 
                    FROM tags AS t
                    WHERE t.TagID = @tagIdStr";
                
                var tagEntitiesQuery = @"
                    SELECT ti.IID
                    FROM tags_instances AS ti
                    WHERE ti.TagId = @tagIdStr AND ti.EntityType = @entityType";

                tag = connection.QueryFirst<Tag>(
                    tagQuery,
                    new { tagIdStr = tagIdString },
                    transaction);

                tag.Artists = connection.Query(
                    tagEntitiesQuery,
                    new { tagIdStr = tagIdString, entityType = 1 },
                    transaction)
                .Select(aid => new Guid(aid))
                .ToList();

                tag.Playlists = connection.Query(
                    tagEntitiesQuery,
                    new { tagIdStr = tagIdString, entityType = 2 },
                    transaction)
                .Select(pid => new Guid(pid))
                .ToList();

                tag.Tracks = connection.Query(
                    tagEntitiesQuery,
                    new { tagIdStr = tagIdString, entityType = 3 },
                    transaction)
                .Select(tid => new Guid(tid))
                .ToList();
            }
        }

        return Task.FromResult<Tag>(tag);
    }

    public Task<IEnumerable<CommonInstanceDTO>> QueryInstanceDtosFromIds(IEnumerable<Guid> inputIds, EntityTag entityTag)
    {
       IEnumerable<CommonInstanceDTO> resultDtos;

       using (IDbConnection connection = ConnectionAgent.GetDbConnection())
       {
           connection.Open();

           using (var transaction = connection.BeginTransaction())
           {
               var table = entityTag switch
               {
                   EntityTag.ARTIST => "artists",
                   EntityTag.PLAYLIST => "playlists",
                   EntityTag.TRACK => "tracks",
                   _ => default
               };

               var header = entityTag switch
               {
                   EntityTag.ARTIST => "AID",
                   EntityTag.PLAYLIST => "PID",
                   EntityTag.TRACK => "TID",
                   _ => default
               };

               var query = $@"SELECT {header} as Id, Name, Avatar FROM {table} WHERE {header} IN @ids";

               resultDtos = connection.Query(
                   query,
                   new {ids = inputIds}, 
                   transaction)
               .Select( i => new CommonInstanceDTO(
                   id: new Guid(i.Id),
                   name: ((string)i.Name).AsMemory(),
                   avatar: i.Avatar,
                   tag: entityTag));

           } 

           return Task.FromResult<IEnumerable<CommonInstanceDTO>>(resultDtos);
       }
    }
}
