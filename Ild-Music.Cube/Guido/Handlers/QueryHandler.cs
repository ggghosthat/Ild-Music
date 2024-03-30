using Ild_Music.Core.Instances.DTO;
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
                var commonQuery = @"SELECT AID, Name, Avatar FROM artists
                                    WHERE Id > 0 AND Id <= @pageLimit;
			
			                        SELECT PID, Name, Avatar FROM playlists
			                        WHERE Id > 0 AND Id <= @pageLimit;
			
			                        SELECT TID, Name, Avatar FROM tracks
			                        WHERE Id > 0 AND Id <= @pageLimit;";
                
                using(var multiQuery = connection.QueryMultiple(commonQuery, new {pageLimit = ConnectionAgent.QueryLimit}))
                {
                    var artistsDTO = multiQuery.Read()
                                               .Select(a => new CommonInstanceDTO( id: new Guid(a.AID),
                                                                                   name: ((string)a.Name).AsMemory(),
                                                                                   avatar: a.Avatar,
                                                                                   tag: EntityTag.ARTIST)).ToList(); 
                    var playlistsDTO = multiQuery.Read()
                                                .Select(a => new CommonInstanceDTO( id: new Guid(a.PID),
                                                                                   name: ((string)a.Name).AsMemory(),
                                                                                   avatar: a.Avatar,
                                                                                   tag: EntityTag.ARTIST)).ToList(); 
                    var tracksDTO = multiQuery.Read()
                                                .Select(a => new CommonInstanceDTO( id: new Guid(a.TID),
                                                                                   name: ((string)a.Name).AsMemory(),
                                                                                   avatar: a.Avatar,
                                                                                   tag: EntityTag.ARTIST)).ToList(); 
                    resultPool.ArtistsDTOs = artistsDTO;
                    resultPool.PlaylistsDTOs = playlistsDTO;
                    resultPool.TracksDTOs = tracksDTO;
                }
            }
        }

        return Task.FromResult<QueryPool>(resultPool);
    }

    public Task<IEnumerable<CommonInstanceDTO>> QueryArtists(int offset)
    {
        IEnumerable<CommonInstanceDTO> artistsDTOs;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var artistsPageQuery = @"SELECT AID, Name, Avatar FROM artists
                                         WHERE Id > @offset AND Id <= @pageLimit;";

                var limit = offset + ConnectionAgent.QueryLimit;
                
                artistsDTOs = connection.Query(artistsPageQuery,
                                                   new 
                                                   {
                                                        offset = offset,
                                                        pageLimit = limit
                                                   },
                                                   transaction)
                                            .Select(a => new CommonInstanceDTO( id: new Guid(a.AID),
                                                                                name: ((string)a.Name).AsMemory(),
                                                                                avatar: a.Avatar,
                                                                                tag: EntityTag.ARTIST)).ToList(); 
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(artistsDTOs);

    }

    public Task<IEnumerable<CommonInstanceDTO>> QueryPlaylists(int offset)
    {
        IEnumerable<CommonInstanceDTO> playlistsDTOs;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var playlistsPageQuery = @"SELECT PID, Name, Avatar FROM playlists
                                         WHERE Id > @offset AND Id <= @pageLimit;";
               
                var limit = offset + ConnectionAgent.QueryLimit;

                playlistsDTOs = connection.Query(playlistsPageQuery,
                                                   new 
                                                   {
                                                        offset = offset,
                                                        pageLimit = limit
                                                   },
                                                   transaction)
                                            .Select(p => new CommonInstanceDTO( id: new Guid(p.PID),
                                                                                name: ((string)p.Name).AsMemory(),
                                                                                avatar: p.Avatar,
                                                                                tag: EntityTag.PLAYLIST)).ToList(); 
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(playlistsDTOs);

    }

    public Task<IEnumerable<CommonInstanceDTO>> QueryTracks(int offset)
    {
        IEnumerable<CommonInstanceDTO> tracksDTOs;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var tracksPageQuery = @"SELECT TID, Name, Avatar FROM tracks
                                         WHERE Id > @offset AND Id <= @pageLimit;";
               
                var limit = offset + ConnectionAgent.QueryLimit;

                tracksDTOs = connection.Query(tracksPageQuery,
                                                   new 
                                                   {
                                                        offset = offset,
                                                        pageLimit = limit
                                                   },
                                                   transaction)
                                            .Select(t => new CommonInstanceDTO( id: new Guid(t.TID),
                                                                                name: ((string)t.Name).AsMemory(),
                                                                                avatar: t.Avatar,
                                                                                tag: EntityTag.TRACK)).ToList(); 
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(tracksDTOs);

    }


    public Task<IEnumerable<Tag>> QueryTags(int offset)
    {
        IEnumerable<Tag> tags;

        using(IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var tagsPageQuery = @"SELECT TagID, Name, Color FROM tags
                                    WHERE Id > @offset AND Id <= @pageLimit;";

                var limit = offset + ConnectionAgent.QueryLimit;

                tags = connection.Query(tagsPageQuery,
                                        new 
                                        {
                                            offset = offset,
                                            pageLimit = limit
                                        },
                                        transaction)
                                .Select(tag => new Tag(id: new Guid(tag.Id),
                                                       name: ((string)tag.Name).AsMemory(),
                                                       color: ((string)tag.Color).AsMemory())).ToList();
            }
        }

        return Task.FromResult<IEnumerable<Tag>>(tags);
    }

    //6. QueryArtistInstance(Single)
    public Task<Artist> QuerySingleArtist(ref CommonInstanceDTO instanceDTO)
    {
        Artist artist;
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection .Open();

            using (var transaction = connection.BeginTransaction())
            {

                var artistId = instanceDTO.Id.ToString();

                //setting up main and extra properties for artist body
                var artistExtraPropsQuery = @"SELECT Description, Year FROM artists
                                              WHERE AID = @aid";
                
                var extraProps = connection.QueryFirst(artistExtraPropsQuery,
                                                       new
                                                       {
                                                           aid = artistId
                                                       },
                                                       transaction);

                artist = new Artist(instanceDTO.Id, 
                                        instanceDTO.Name,
                                        extraProps.Description.AsMemory(),
                                        instanceDTO.Avatar,
                                        extraProps .Year);


                var arttistPlaylistsQuery = @"SELECT PID FROM artists_playlists
                                              WHERE AID = @aid";

                var arttistTracksQuery = @"SELECT TID FROM artists_tracks
                                           WHERE AID = @aid";

                var arttistTagsQuery = @"SELECT t.TagID, t.Name, t.Color, ti.IID 
                                        FROM tags_instances as ti
                                        INNER JOIN tags as t
                                        ON t.TagID = ti.TagID
                                         WHERE ti.IID = @aid";


                

                artist.Playlists = connection.Query(arttistPlaylistsQuery,
                                                       new
                                                       {
                                                           aid = artistId
                                                       },
                                                       transaction)
                                                .Select(pid => new Guid(pid))
                                                .ToList();


                artist.Tracks = connection.Query(arttistTracksQuery,
                                                 new
                                                 {
                                                    aid = artistId
                                                 },
                                                 transaction)
                                            .Select(tid => new Guid(tid))
                                            .ToList();

                artist.Tags = connection.Query<Tag>(arttistTagsQuery,
                                                       new
                                                       {
                                                           aid = artistId
                                                       },
                                                       transaction)
                                                .Select(tag => tag)
                                                .ToList();               
            }
        }

        return Task.FromResult<Artist>(artist);
    }

    //7. QueryPlaylistInstance(Single)
    //8. QueryTrackInstance(Single)
    //9. QueryTagInstance(Single)
}
