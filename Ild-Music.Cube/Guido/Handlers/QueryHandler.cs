using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Instances.Querying;
using Cube.Guido.Agents;
using Ild_Music.Core.Instances;

using System;
using System.Data;
using Dapper;

namespace Cube.Guido.Handlers;

internal sealed class QueryHandler
{    
    public QueryHandler()
    {}

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
                string commonQuery = @"
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
                            id: Guid.Parse((string)a.AID),
                            name: ((string)a.Name).AsMemory(),
                            avatar: a.Avatar,
                            tag: EntityTag.ARTIST))
                        .ToList();

                    var playlistsDTO = multiQuery.Read()
                        .Select(a => new CommonInstanceDTO( 
                            id: Guid.Parse((string)a.PID),
                            name: ((string)a.Name).AsMemory(),
                            avatar: a.Avatar,
                            tag: EntityTag.PLAYLIST))
                        .ToList(); 

                    var tracksDTO = multiQuery.Read()
                        .Select(a => new CommonInstanceDTO( 
                            id: Guid.Parse((string)a.TID),
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

    public Task<IEnumerable<CommonInstanceDTO>> QueryAllArtists()
    {
        IEnumerable<CommonInstanceDTO> artistsDTOs;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                string artistsPageQuery = "SELECT AID, Name, Avatar FROM artists;";

                artistsDTOs = connection.Query(
                    artistsPageQuery,
                    default,
                    transaction)
                .Select(a => new CommonInstanceDTO( 
                    id: Guid.Parse((string)a.AID),
                    name: ((string)a.Name).AsMemory(),
                    avatar: a.Avatar,
                    tag: EntityTag.ARTIST))
                .ToList();
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(artistsDTOs);
    }

    public Task<IEnumerable<CommonInstanceDTO>> QueryArtists(int offset, int limit)
    {
        IEnumerable<CommonInstanceDTO> artistsDTOs;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                string artistsPageQuery = @"
                    SELECT AID, Name, Avatar FROM artists
                    WHERE Id > @offset AND Id <= @pageLimit;";

                artistsDTOs = connection.Query(
                    artistsPageQuery,
                    new { offset = offset, pageLimit = limit },
                    transaction)
                .Select(a => new CommonInstanceDTO( 
                    id: Guid.Parse((string)a.AID),
                    name: ((string)a.Name).AsMemory(),
                    avatar: a.Avatar,
                    tag: EntityTag.ARTIST))
                .ToList();
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(artistsDTOs);
    }

    public Task<IEnumerable<CommonInstanceDTO>> QueryAllPlaylists()
    {
        IEnumerable<CommonInstanceDTO> playlistsDTOs;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                string playlistsPageQuery = "SELECT PID, Name, Avatar FROM playlists;";
               
                playlistsDTOs = connection.Query(
                    playlistsPageQuery,
                    default,
                    transaction)
                .Select(p => new CommonInstanceDTO( 
                    id: Guid.Parse((string)p.PID),
                    name: ((string)p.Name).AsMemory(),
                    avatar: p.Avatar,
                    tag: EntityTag.PLAYLIST))
                .ToList(); 
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(playlistsDTOs);
    }

    public Task<IEnumerable<CommonInstanceDTO>> QueryPlaylists(int offset, int limit)
    {
        IEnumerable<CommonInstanceDTO> playlistsDTOs;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                string playlistsPageQuery = @"
                    SELECT PID, Name, Avatar FROM playlists
                    WHERE Id > @offset AND Id <= @pageLimit;";
               
                playlistsDTOs = connection.Query(
                    playlistsPageQuery,
                    new { offset = offset, pageLimit = limit },
                    transaction)
                .Select(p => new CommonInstanceDTO( 
                    id: Guid.Parse((string)p.PID),
                    name: ((string)p.Name).AsMemory(),
                    avatar: p.Avatar,
                    tag: EntityTag.PLAYLIST))
                .ToList(); 
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(playlistsDTOs);
    }

    public Task<IEnumerable<CommonInstanceDTO>> QueryAllTracks()
    {
        IEnumerable<CommonInstanceDTO> tracksDTOs;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                string tracksPageQuery = "SELECT TID, Name, Avatar FROM tracks;";
               
                tracksDTOs = connection.Query(
                    tracksPageQuery,
                    default,
                    transaction)
                .Select(t => new CommonInstanceDTO( 
                    id: Guid.Parse((string)t.TID),
                    name: ((string)t.Name).AsMemory(),
                    avatar: t.Avatar,
                    tag: EntityTag.TRACK))
                .ToList();
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(tracksDTOs);
    }

    public Task<IEnumerable<CommonInstanceDTO>> QueryTracks(int offset, int limit)
    {
        IEnumerable<CommonInstanceDTO> tracksDTOs;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                string tracksPageQuery = @"
                    SELECT TID, Name, Avatar FROM tracks
                    WHERE Id > @offset AND Id <= @pageLimit;";
               
                tracksDTOs = connection.Query(
                    tracksPageQuery,
                    new { offset = offset, pageLimit = limit },
                    transaction)
                .Select(t => new CommonInstanceDTO( 
                    id: Guid.Parse((string)t.TID),
                    name: ((string)t.Name).AsMemory(),
                    avatar: t.Avatar,
                    tag: EntityTag.TRACK))
                .ToList();
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(tracksDTOs);
    }

    public Task<IEnumerable<CommonInstanceDTO>> QueryAllTags()
    {
        IEnumerable<CommonInstanceDTO> tags;

        using(IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                string tagsPageQuery = "SELECT TagID, Name, FROM tags;";

                tags = connection.Query(
                    tagsPageQuery,
                    default,
                    transaction)
                .Select(tag => new CommonInstanceDTO(
                    id: Guid.Parse((string)tag.TagId),
                    name: ((string)tag.Name).AsMemory(),
                    avatar: new byte[0],
                    tag: EntityTag.TAG))
                .ToList();
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(tags);
    }

    public Task<IEnumerable<CommonInstanceDTO>> QueryTags(int offset, int limit)
    {
        IEnumerable<CommonInstanceDTO> tags;

        using(IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                string tagsPageQuery = @"
                    SELECT TagID, Name FROM tags
                    WHERE Id > @offset AND Id <= @pageLimit;";

                tags = connection.Query(
                    tagsPageQuery,
                    new { offset = offset, pageLimit = limit },
                    transaction)
                .Select(tag => new CommonInstanceDTO(
                    id: Guid.Parse((string)tag.TagId),
                    name: ((string)tag.Name).AsMemory(),
                    avatar: new byte[0],
                    tag: EntityTag.TAG))
                .ToList();
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(tags);
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
                string artistExtraPropsQuery = @"
                    SELECT Description, Year FROM artists
                    WHERE AID = @aid";
               
                string arttistPlaylistsQuery = @"
                    SELECT PID FROM artists_playlists
                    WHERE AID = @aid";

                string arttistTracksQuery = @"
                    SELECT TID as tid FROM artists_tracks
                    WHERE AID = @aid";

                string arttistTagsQuery = @"
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
                    extraProps.Description.ToCharArray(),
                    instanceDTO.Avatar,
                    (int)extraProps.Year);

                artist.Playlists = connection.Query(
                    arttistPlaylistsQuery,
                    new { aid = artistId },
                    transaction)
                .Where(pid => pid.Value is not null)
                .Select(pid => Guid.Parse((string)pid.Value))
                .ToList();

                artist.Tracks = connection.Query(
                    arttistTracksQuery,
                    new { aid = artistId },
                    transaction)
                .Where(tid => tid.Value is not null)
                .Select(tid => Guid.Parse((string)tid.Value))
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
                string playlistId = instanceDTO.Id.ToString();

                //setting up main and extra properties for artist body
                string artistExtraPropsQuery = @"
                    SELECT Description, Year FROM playlists
                    WHERE PID = @pid";
               
                string playlistArtistsQuery = @"
                    SELECT AID FROM artists_playlists
                    WHERE PID = @pid";

                string playlistTracksQuery = @"
                    SELECT TID FROM playlists_tracks
                    WHERE PID = @pid";

                string playlistTagsQuery = @"
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
                .Where(aid => aid.Value is not null)
                .Select(aid => Guid.Parse((string)aid.Value))
                .ToList();

                playlist.Tracky = connection.Query(
                    playlistTracksQuery,
                    new { pid = playlistId },
                    transaction)
                .Where(tid => tid.Value is not null)
                .Select(tid => Guid.Parse((string)tid.Value))
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
                string trackId = instanceDTO.Id.ToString();

                //setting up main and extra properties for artist body
                string artistExtraPropsQuery = @"
                    SELECT Description, Year, Duration FROM tracks
                    WHERE TID = @tid";
               
                string trackArtistsQuery = @"
                    SELECT AID FROM artists_tracks
                    WHERE TID = @tid";

                string trackPlaylistsQuery = @"
                    SELECT PID FROM playlists_tracks
                    WHERE TID = @tid";

                string trackTagsQuery = @"
                    SELECT t.TagID, t.Name, t.Color, ti.IID 
                    FROM tags_instances as ti
                    INNER JOIN tags as t
                    ON t.TagID = ti.TagID
                    WHERE ti.IID = @tid";

                var extraProps = connection.QueryFirst(
                    artistExtraPropsQuery,
                    new { tid = trackId },
                    transaction);

                track = new (
                    instanceDTO.Id,
                    String.Empty.ToCharArray(),
                    instanceDTO.Name,
                    extraProps.Description.ToCharArray(),
                    instanceDTO.Avatar,
                    TimeSpan.FromSeconds(extraProps.Duration),
                    (int)extraProps.Year);

                track.Artists  = connection.Query(
                    trackArtistsQuery,
                    new { tid = trackId },
                    transaction)                
                .Where(aid => aid.Value is not null)
                .Select(aid => Guid.Parse((string)aid.Value))
                .ToList();

                track.Playlists = connection.Query(
                    trackPlaylistsQuery,
                    new { tid = trackId },
                    transaction)
                .Where(tid => tid.Value is not null)
                .Select(tid => Guid.Parse((string)tid.Value))
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
                string tagIdString = tagId.ToString();

                //setting up main and extra properties for artist body
                string tagQuery = @"
                    SELECT t.TagId as Id, t.Name, t.Color, ti.IID 
                    FROM tags AS t
                    WHERE t.TagID = @tagIdStr";
                
                string tagEntitiesQuery = @"
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
                .Where(aid => aid.Value is not null)
                .Select(aid => Guid.Parse((string)aid.Value))
                .ToList();

                tag.Playlists = connection.Query(
                    tagEntitiesQuery,
                    new { tagIdStr = tagIdString, entityType = 2 },
                    transaction)
                .Where(pid => pid.Value is not null)
                .Select(pid => Guid.Parse((string)pid.Value))
                .ToList();

                tag.Tracks = (connection.Query(
                    tagEntitiesQuery,
                    new { tagIdStr = tagIdString, entityType = 3 },
                    transaction)
                .Where(tid => tid.Value is not null)
                .Select(tid => Guid.Parse((string)tid.Value))
                .ToList());
            }
        }

        return Task.FromResult<Tag>(tag);
    }

    public Task<IEnumerable<CommonInstanceDTO>> QueryInstanceDtosFromIds(
        IEnumerable<Guid> inputIds,
        EntityTag entityTag)
    {
       IEnumerable<CommonInstanceDTO> resultDtos;

       using (IDbConnection connection = ConnectionAgent.GetDbConnection())
       {
           connection.Open();

           using (var transaction = connection.BeginTransaction())
           {
               var inputs = inputIds.Select(i => i.ToString());

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

               string query = $@"SELECT {header} as Id, Name, Avatar FROM {table} WHERE {header} IN @ids";

               resultDtos = connection.Query(
                   query,
                   new {ids = inputs}, 
                   transaction)
                .Select(i => new CommonInstanceDTO(
                   id: Guid.Parse(i.Id),
                   name: ((string)i.Name).AsMemory(),
                   avatar: i.Avatar,
                   tag: entityTag));

           } 

           return Task.FromResult<IEnumerable<CommonInstanceDTO>>(resultDtos);
       }
    }
}
