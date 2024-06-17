using Ild_Music.Core.Instances.DTO;
using Cube.Guido.Agents;
using Ild_Music.Core.Instances;

using System.Data;
using Dapper;

namespace Cube.Guido.Handlers;

internal sealed class SearchHandler
{
    public SearchHandler()
    {}

    public Task<IEnumerable<CommonInstanceDTO>> Search(string searchQuery)
    {
        List<CommonInstanceDTO> resultCollection = new ();
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var inputParam = $"%{searchQuery}%";
                var commonQuery = @"
                    SELECT AID, Name, Avatar FROM artists
                    WHERE Name LIKE @searchWord ;

			        SELECT PID, Name, Avatar FROM playlists
			        WHERE Name LIKE @searchWord ;			
			    
                    SELECT TID, Name, Avatar FROM tracks
			        WHERE Name LIKE @searchWord ;"; 
                               
                using(var multiQuery = connection
                          .QueryMultiple(commonQuery, 
                          new {searchWord = inputParam}) )
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
                string commonQuery = @$"
                    SELECT AID, Name, Avatar FROM artists 
                    WHERE Name LIKE '%{searchQuery}%';";

                var artistsDTO = connection.Query(
                    commonQuery,
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
                var commonQuery = @$"
                    SELECT PID, Name, Avatar FROM playlists	
                    WHERE Name LIKE '%{searchQuery}%';";

                var playlistsDTO = connection.Query(
                    commonQuery,
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
                string commonQuery = @$"
                    SELECT TID, Name, Avatar FROM tracks 
                    WHERE Name LIKE '%{searchQuery}%';";

                var tracksDTO = connection.Query(
                    commonQuery,
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

    public Task<IEnumerable<CommonInstanceDTO>> SearchTags(string searchQuery)
    {
        List<CommonInstanceDTO> resultCollection = new ();
        using (IDbConnection connection = ConnectionAgent.GetDbConnection())
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                string commonQuery = @"
                    SELECT TagID, Name, Color FROM tags 
                    WHERE Name LIKE '%{searchQuery}%';";

                var tagsDTO = connection.Query(
                    commonQuery,
                    transaction)
                .Select(tag => new CommonInstanceDTO(
                    id: Guid.Parse((string)tag.TagId),
                    name: ((string)tag.Name).AsMemory(),
                    avatar: new byte[0],
                    tag: EntityTag.TAG))
                .ToList();

                resultCollection.AddRange(tagsDTO); 
            }
        }

        return Task.FromResult<IEnumerable<CommonInstanceDTO>>(resultCollection);
    }
}
