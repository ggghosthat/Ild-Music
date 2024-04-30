using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Instances.Querying;
using Cube.Guido.Agents;
using Ild_Music.Core.Instances;

using System.Data;
using Dapper;

namespace Cube.Guido.Handlers;

internal sealed class SearchHandler
{
    public SearchHandler()
    {
    }

    public Task<IEnumerable<CommonInstanceDTO>> Search(string searchQuery)
    {
        List<CommonInstanceDTO> resultCollection = new ();
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                IEnumerable<string> artistsId = null;
                IEnumerable<string> playlistsId = null;
                IEnumerable<string> tracksId = null;
                IEnumerable<string> tagsId = null;

                var commonSearchQuery = @"
                    SELECT AID FROM artists_index MATCH @query;
                    SELECT PID FROM playlists_index MATCH @query;
                    SELECT TID FROM tracks_index MATCH @query;
                    SELECT TagId FROM tags_index MATCH @query;";
                
                var commonQuery = @"
                    SELECT AID, Name, Avatar FROM artists
                    WHERE AID IN @aids;

			        SELECT PID, Name, Avatar FROM playlists
			        WHERE PID IN @pids;			
			    
                    SELECT TID, Name, Avatar FROM tracks
			        WHERE TID IN @tids;                                    
                    
                    SELECT TagID, Name, Color FROM tags
                    WHERE TagID IN @tagids;";

                using (var multiple = connection
                           .QueryMultiple(commonSearchQuery,
                           new {query=searchQuery}))
                {
                    artistsId = multiple.Read<string>();
                    playlistsId = multiple.Read<string>();
                    tracksId = multiple.Read<string>();
                    tagsId = multiple.Read<string>();
                }
                               
                using(var multiQuery = connection
                          .QueryMultiple(commonQuery, 
                          new {aids = artistsId,
                               pids = playlistsId,
                               tids = tracksId,
                               tagids = tagsId}))
                {
                    var artistsDTO = multiQuery.Read()
                        .Select(a => new CommonInstanceDTO( 
                            id: new Guid(a.AID),
                            name: ((string)a.Name).AsMemory(),
                            avatar: a.Avatar,
                            tag: EntityTag.ARTIST))
                        .ToList();

                    resultCollection.AddRange(artistsDTO); 

                    var playlistsDTO = multiQuery.Read()
                        .Select(p => new CommonInstanceDTO( 
                            id: new Guid(p.PID),
                            name: ((string)p.Name).AsMemory(),
                            avatar: p.Avatar,
                            tag: EntityTag.PLAYLIST))
                        .ToList(); 

                    resultCollection.AddRange(playlistsDTO);

                    var tracksDTO = multiQuery.Read()
                        .Select(t => new CommonInstanceDTO( 
                            id: new Guid(t.TID),
                            name: ((string)t.Name).AsMemory(),
                            avatar: t.Avatar,
                            tag: EntityTag.TRACK))
                        .ToList();

                    resultCollection.AddRange(tracksDTO);
                }
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(resultCollection);
    }

    public Task<IEnumerable<CommonInstanceDTO>> SearchArtists(string searchQuery)
    {
        List<CommonInstanceDTO> resultCollection = new ();
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var commonSearchQuery = @"SELECT AID FROM artists_index MATCH @query;";                
                var commonQuery = @"SELECT AID, Name, Avatar FROM artists WHERE AID IN @aids;";

                var artistsId = connection.Query<string>(
                    commonSearchQuery,
                    new {query=searchQuery},
                    transaction);
                               
                var artistsDTO = connection.Query(
                    commonQuery,
                    new {aids = artistsId},
                    transaction)
                .Select(a => new CommonInstanceDTO( 
                    id: new Guid(a.AID),
                    name: ((string)a.Name).AsMemory(),
                    avatar: a.Avatar,
                    tag: EntityTag.ARTIST)).ToList();

                resultCollection.AddRange(artistsDTO);  
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(resultCollection);
    }

    public Task<IEnumerable<CommonInstanceDTO>> SearchPlaylists(string searchQuery)
    {
        List<CommonInstanceDTO> resultCollection = new ();
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {

                var commonSearchQuery = @"SELECT PID FROM playlists_index MATCH @query;";                
                var commonQuery = @"SELECT PID, Name, Avatar FROM playlists	WHERE PID IN @pids;	";

                var playlistsId = connection.Query<string>(
                    commonSearchQuery,
                    new {query=searchQuery},
                    transaction);
                               
                var playlistsDTO = connection.Query(
                    commonQuery,
                    new {pids = playlistsId},
                    transaction)                
                .Select(p => new CommonInstanceDTO( 
                    id: new Guid(p.PID),
                    name: ((string)p.Name).AsMemory(),
                    avatar: p.Avatar,
                    tag: EntityTag.PLAYLIST))
                .ToList(); 

                resultCollection.AddRange(playlistsDTO); 
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(resultCollection);
    }

    public Task<IEnumerable<CommonInstanceDTO>> SearchTracks(string searchQuery)
    {
        List<CommonInstanceDTO> resultCollection = new ();
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {

                var commonSearchQuery = @"SELECT TID FROM tracks_index MATCH @query;";                
                var commonQuery = @"SELECT TID, Name, Avatar FROM tracks WHERE TID IN @tids;";

                var tracksId = connection.Query<string>(
                    commonSearchQuery,
                    new {query=searchQuery},
                    transaction);
                               
                var tracksDTO = connection.Query(
                    commonQuery,
                    new {tids = tracksId},
                    transaction) 
                .Select(t => new CommonInstanceDTO( 
                    id: new Guid(t.TID),
                    name: ((string)t.Name).AsMemory(),
                    avatar: t.Avatar,
                    tag: EntityTag.TRACK))
                .ToList();

                resultCollection.AddRange(tracksDTO);                
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(resultCollection);
    }

    public Task<IEnumerable<Tag>> SearchTags(string searchQuery)
    {
        List<Tag> resultCollection = new ();
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var commonSearchQuery = @"SELECT AID FROM artists_index MATCH @query;";                
                var commonQuery = @"SELECT TagID, Name, Color FROM tags WHERE TagID IN @tagids;";

                var tagsId = connection.Query<string>(
                    commonSearchQuery,
                    new {query=searchQuery},
                    transaction);
                               
                var tagsDTO = connection.Query(
                    commonQuery,
                    new {tagids = tagsId},
                    transaction)
                .Select(tag => new Tag(
                    id: new Guid(tag.Id),
                    name: ((string)tag.Name).AsMemory(),
                    color: ((string)tag.Color).AsMemory()))
                .ToList();

                resultCollection.AddRange(tagsDTO); 
            }
        }

        return Task.FromResult<IEnumerable<Tag>>(resultCollection);
    }
}
