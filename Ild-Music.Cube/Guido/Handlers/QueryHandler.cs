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
                                    WHERE Id > 0 AND Id < @pageLimit;
			
			                        SELECT PID, Name, Avatar FROM playlists
			                        WHERE Id > 0 AND Id < @pageLimit;
			
			                        SELECT TID, Name, Avatar FROM tracks
			                        WHERE Id > 0 AND Id < @pageLimit;";
                
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
    //1. QueryAllDto(Paged)
    //
    //2. QueryArtistsDto(Paged)
    //3. QueryPlaylistsDto(Paged)
    //4. QueryTrackDto(Paged)
    //5. QueryTagDto(Paged)
    //
    //6. QueryArtistInstance(Single)
    //7. QueryPlaylistInstance(Single)
    //8. QueryTrackInstance(Single)
    //9. QueryTagInstance(Single)
}
