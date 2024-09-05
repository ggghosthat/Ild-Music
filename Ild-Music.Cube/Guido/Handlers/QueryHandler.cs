using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Instances.Querying;
using Cube.Guido.Agents;

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
    public Task<InstancePool> QueryPool()
    {
        var resultPool = new InstancePool();
        int startPageLimit = 300;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                string commonQuery = @"
                    SELECT AID, Name FROM artists
                    WHERE Id > 0 AND Id <= @pageLimit;
			
			        SELECT PID, Name FROM playlists
			        WHERE Id > 0 AND Id <= @pageLimit;
			
			        SELECT TID, Name FROM tracks
			        WHERE Id > 0 AND Id <= @pageLimit;

                    SELECT TagID, Name FROM tags
                    WHERE Id > 0 AND Id <= @pageLimit;";
                
                using(var multiQuery = connection.QueryMultiple(
                        commonQuery,
                        new {pageLimit = startPageLimit},
                        transaction))
                {
                    var artistsDTO = multiQuery.Read()
                        .Select(a => new CommonInstanceDTO( 
                            id: Guid.Parse((string)a.AID),
                            name: ((string)a.Name).AsMemory(),
                            avatarPath: WarehouseAgent.GetAvatarFromId(Guid.Parse((string)a.AID)).AsMemory(),
                            tag: EntityTag.ARTIST))
                        .ToList();

                    var playlistsDTO = multiQuery.Read()
                        .Select(p => new CommonInstanceDTO( 
                            id: Guid.Parse((string)p.PID),
                            name: ((string)p.Name).AsMemory(),
                            avatarPath: WarehouseAgent.GetAvatarFromId(Guid.Parse((string)p.PID)).AsMemory(),
                            tag: EntityTag.PLAYLIST))
                        .ToList(); 

                    var tracksDTO = multiQuery.Read()
                        .Select(t => new CommonInstanceDTO( 
                            id: Guid.Parse((string)t.TID),
                            name: ((string)t.Name).AsMemory(),
                            avatarPath: WarehouseAgent.GetAvatarFromId(Guid.Parse((string)t.TID)).AsMemory(),
                            tag: EntityTag.TRACK))
                        .ToList();

                    var tagsDTO = multiQuery.Read()
                        .Select(a => new CommonInstanceDTO(
                            id: Guid.Parse((string)a.TagID),
                            name: ((string)a.Name).AsMemory(),
                            avatar: new byte[0],
                            tag: EntityTag.TAG))
                        .ToList();

                    resultPool.ArtistsDTOs = artistsDTO;
                    resultPool.PlaylistsDTOs = playlistsDTO;
                    resultPool.TracksDTOs = tracksDTO;
                    resultPool.TagsDTOs = tagsDTO;
                }
            }
        }

        return Task.FromResult<InstancePool>(resultPool);
    }

    public Task<IEnumerable<CommonInstanceDTO>> QueryAllArtists()
    {
        IEnumerable<CommonInstanceDTO> artistsDTOs;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                string artistsPageQuery = "SELECT AID, Name FROM artists;";

                artistsDTOs = connection
                    .Query(artistsPageQuery,default,transaction)
                    .Select(a => new CommonInstanceDTO( 
                        id: Guid.Parse((string)a.AID),
                        name: ((string)a.Name).AsMemory(),
                        avatarPath: WarehouseAgent.GetAvatarFromId(Guid.Parse((string)a.AID)).AsMemory(),
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
                string artistsPageQuery = "SELECT AID, Name FROM artists WHERE Id > "
                    + offset + " AND Id <= " + (offset + limit) + " ;";

                artistsDTOs = connection
                    .Query(artistsPageQuery, default, transaction)
                    .Select(a => new CommonInstanceDTO(
                        id: Guid.Parse((string)a.AID),
                        name: ((string)a.Name).AsMemory(),
                        avatarPath: WarehouseAgent.GetAvatarFromId(Guid.Parse((string)a.AID)).AsMemory(),
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
                string playlistsPageQuery = "SELECT PID, Name FROM playlists;";
               
                playlistsDTOs = connection
                    .Query(playlistsPageQuery, default, transaction)
                    .Select(p => new CommonInstanceDTO( 
                        id: Guid.Parse((string)p.PID),
                        name: ((string)p.Name).AsMemory(),
                        avatarPath: WarehouseAgent.GetAvatarFromId(Guid.Parse((string)p.PID)).AsMemory(),
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
                string playlistsPageQuery = "SELECT PID, Name FROM playlists WHERE Id > " + offset + " AND Id <= " + limit + " ;";
                playlistsDTOs = connection
                    .Query(playlistsPageQuery, default, transaction)
                    .Select(p => new CommonInstanceDTO( 
                        id: Guid.Parse((string)p.PID),
                        name: ((string)p.Name).AsMemory(),
                        avatarPath: WarehouseAgent.GetAvatarFromId(Guid.Parse((string)p.PID)).AsMemory(),
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
                string tracksPageQuery = "SELECT TID, Name FROM tracks;";
               
                tracksDTOs = connection
                    .Query(tracksPageQuery, default, transaction)
                    .Select(t => new CommonInstanceDTO( 
                        id: Guid.Parse((string)t.TID),
                        name: ((string)t.Name).AsMemory(),
                        avatarPath: WarehouseAgent.GetAvatarFromId(Guid.Parse((string)t.TID)).AsMemory(),
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
                string tracksPageQuery = "SELECT TID, Name FROM tracks WHERE Id > " + offset + " AND Id <= " + limit + " ;";
               
                tracksDTOs = connection
                    .Query(tracksPageQuery, default, transaction)
                    .Select(t => new CommonInstanceDTO( 
                        id: Guid.Parse((string)t.TID),
                        name: ((string)t.Name).AsMemory(),
                        avatarPath: WarehouseAgent.GetAvatarFromId(Guid.Parse((string)t.TID)).AsMemory(),
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
                string tagsPageQuery = "SELECT TagID, Name FROM tags;";

                tags = connection.Query(
                    tagsPageQuery,
                    default,
                    transaction)
                .Select(tag => new CommonInstanceDTO(
                    id: Guid.Parse((string)tag.TagID),
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
                string tagsPageQuery = "SELECT TagID, Name FROM tags WHERE Id > " + offset + " AND Id <= " + limit + " ;";

                tags = connection.Query(tagsPageQuery, transaction)
                .Select(tag => new CommonInstanceDTO(
                    id: Guid.Parse((string)tag.TagID),
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
                    instanceDTO.AvatarPath,
                    (int)extraProps.Year);

                var artistPlaylists = connection.Query(
                    arttistPlaylistsQuery,
                    new { aid = artistId },
                    transaction)
                .Where(pid => pid.PID is not null)
                .Select(pid => Guid.Parse((string)pid.PID))
                .ToList();

                artist.Playlists.AddRange(artistPlaylists);

                var artistTracks = connection.Query(
                    arttistTracksQuery,
                    new { aid = artistId },
                    transaction)
                .Where(tid => tid.TID is not null)
                .Select(tid => Guid.Parse((string)tid.TID))
                .ToList();

                artist.Tracks.AddRange(artistTracks);

                var artistsTags = connection.Query<Tag>(
                    arttistTagsQuery,
                    new { aid = artistId },
                    transaction)
                .Select(tag => tag)
                .ToList();

                artist.Tags.AddRange(artistsTags);
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
                    new { pid = playlistId },
                    transaction);

                playlist = new (
                    instanceDTO.Id, 
                    instanceDTO.Name,
                    extraProps.Description.ToCharArray(),
                    instanceDTO.AvatarPath,
                    (int)extraProps.Year);
                
                var playlistArtists = connection.Query(
                    playlistArtistsQuery,
                    new { pid = playlistId },
                    transaction)
                .Where(aid => aid.AID is not null)
                .Select(aid => Guid.Parse((string)aid.AID))
                .ToList();

                playlist.Artists.AddRange(playlistArtists);

                var playlistTracks = connection.Query(
                    playlistTracksQuery,
                    new { pid = playlistId },
                    transaction)
                .Where(tid => tid.TID is not null)
                .Select(tid => Guid.Parse((string)tid.TID))
                .ToList();

                playlist.Tracks.AddRange(playlistTracks);

                var playlistTags = connection.Query<Tag>(
                    playlistTagsQuery,
                    new { pid = playlistId },
                    transaction)
                .Select(tag => tag)
                .ToList();

                playlist.Tags.AddRange(playlistTags);
            }
        }

        return Task.FromResult<Playlist>(playlist);
    }

    public Task<Track> QuerySingleTrack(ref CommonInstanceDTO instanceDTO)
    {
        Track track;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                string trackId = instanceDTO.Id.ToString();

                //setting up main and extra properties for artist body
                string trackExtraPropsQuery = @"
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
                    trackExtraPropsQuery,
                    new { tid = trackId },
                    transaction);
                
                track = new (
                    instanceDTO.Id,
                    WarehouseAgent.GetTrackPathFromIdAsMemory(instanceDTO.Id),
                    instanceDTO.Name,
                    extraProps.Description.ToCharArray(),
                    instanceDTO.AvatarPath,
                    TimeSpan.FromMilliseconds((long)extraProps.Duration),
                    (int)extraProps.Year);

                var trackArtists = connection.Query(
                    trackArtistsQuery,
                    new { tid = trackId },
                    transaction)                
                .Where(aid => aid.AID is not null)
                .Select(aid => Guid.Parse((string)aid.AID))
                .ToList();

                track.Artists.AddRange(trackArtists);
                
                var trackPlaylists = connection.Query(
                    trackPlaylistsQuery,
                    new { tid = trackId },
                    transaction)
                .Where(pid => pid.PID is not null)
                .Select(pid => Guid.Parse((string)pid.PID))
                .ToList();

                track.Playlists.AddRange(trackPlaylists);

                var trackTags = connection.Query<Tag>(
                    trackTagsQuery,
                    new { tid = trackId },
                    transaction)
                .Select(tag => tag)
                .ToList();

                track.Tags.AddRange(trackTags);
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

                string tagQuery = @"
                    SELECT t.TagID, t.Name, t.Color
                    FROM tags AS t
                    WHERE t.TagID = @tagIdStr";
                
                string tagEntitiesQuery = @"
                    SELECT ti.IID
                    FROM tags_instances AS ti
                    WHERE ti.TagID = @tagIdStr AND ti.EntityType = @entityType";

                tag = connection.Query(
                    tagQuery,
                    new { tagIdStr = tagIdString },
                    transaction)
                .Select(t => new Tag(Guid.Parse(t.TagID), t.Name.ToCharArray(), t.Color.ToCharArray()))
                .First();

                connection.Query(
                    tagEntitiesQuery,
                    new { tagIdStr = tagIdString, entityType = 1 },
                    transaction)
                .Where(aid => aid.Value is not null)
                .Select(aid => Guid.Parse((string)aid.IID))
                .ToList()
                .ForEach(aid => tag.Artists.Add(aid));

                connection.Query(
                    tagEntitiesQuery,
                    new { tagIdStr = tagIdString, entityType = 2 },
                    transaction)
                .Where(pid => pid.Value is not null)
                .Select(pid => Guid.Parse((string)pid.IID))
                .ToList()
                .ForEach(aid => tag.Playlists.Add(aid));

                connection.Query(
                    tagEntitiesQuery,
                    new { tagIdStr = tagIdString, entityType = 3 },
                    transaction)
                .Where(tid => tid.Value is not null)
                .Select(tid => Guid.Parse((string)tid.IID))
                .ToList()
                .ForEach(tid => tag.Tracks.Add(tid));
            }
        }

        return Task.FromResult<Tag>(tag);
    }

    public Task<IEnumerable<CommonInstanceDTO>> QueryInstancesById(IEnumerable<Guid> inputIds, EntityTag entityTag)
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

               string query = $@"SELECT {header} as Id, Name FROM {table} WHERE {header} IN @ids";

               resultDtos = connection
                   .Query(query, new {ids = inputs}, transaction)
                   .Select(i => new CommonInstanceDTO(
                        id: Guid.Parse(i.Id),
                        name: ((string)i.Name).ToCharArray(),
                        avatarPath: WarehouseAgent.GetAvatarFromId(Guid.Parse(i.Id)).ToCharArray(),
                        tag: entityTag));
           } 

           return Task.FromResult<IEnumerable<CommonInstanceDTO>>(resultDtos);
       }
    }

    public Task<IEnumerable<Track>> QueryTracksById(IEnumerable<Guid> inputIds)
    {
       IEnumerable<Track> tracks;

       using (IDbConnection connection = ConnectionAgent.GetDbConnection())
       {
           connection.Open();

           using (var transaction = connection.BeginTransaction())
           {
                var inputs = inputIds.Select(i => i.ToString());

                string tracksQuery = @"
                    SELECT t.TID, t.Name, t.Description, t.Year, t.Duration
                    FROM tracks as t
                    WHERE t.TID in @tid";
               
                tracks = connection.Query(
                    tracksQuery,
                    new {tid = inputs},
                    transaction)
                .Select(t => new Track(
                    Guid.Parse(t.TID),
                    WarehouseAgent.GetTrackPathFromId(Guid.Parse(t.TID)).ToCharArray(),
                    t.Name.ToCharArray(),
                    t.Description.ToCharArray(),
                    WarehouseAgent.GetAvatarFromId(Guid.Parse(t.TID)).ToCharArray(),
                    TimeSpan.FromMilliseconds(t.Duration),
                    (int)t.Year));

           } 

           return Task.FromResult<IEnumerable<Track>>(tracks);
       }
    }

    public Task<MetricSheet> QueryCapacityMetrics()
    {
        MetricSheet metricSheet;

        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
           connection.Open();

           using (var transaction = connection.BeginTransaction())
           {
               string scalarArtistsCount = "SELECT count(1) from artists;";
               string scalarPlaylistsCount = "SELECT count(1) from playlists;";
               string scalarTracksCount = "SELECT count(1) from tracks;";
               string scalarTagsCount = "SELECT count(1) from tags;";
                
               int artistsCount = connection.ExecuteScalar<int>(scalarArtistsCount, transaction);
               int playlistsCount = connection.ExecuteScalar<int>(scalarPlaylistsCount, transaction);
               int tracksCount = connection.ExecuteScalar<int>(scalarTracksCount, transaction);
               int tagsCount = connection.ExecuteScalar<int>(scalarTagsCount, transaction);

               metricSheet = new MetricSheet(artistsCount, playlistsCount, tracksCount, tagsCount);
          } 
        }
        return Task.FromResult<MetricSheet>(metricSheet);
    }
}
