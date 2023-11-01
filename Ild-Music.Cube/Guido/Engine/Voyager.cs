using ShareInstances.Instances;
using Cube.Mapper.Entities;

using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;
namespace Cube.Storage.Guido;
internal class Voyager
{
    private string _connectionString;
    public Voyager(string path)
    {
        _connectionString = $"Data Source = {path}";;
    }

                                           
    public async Task<IEnumerable<ArtistMap>> SearchArtists(ReadOnlyMemory<char> searchTerm)
    {
       IEnumerable<ArtistMap> artists = null;

       ReadOnlyMemory<char> dapperQuery = $"select AID, Name, Description, Avatar, Year from artists where Name like '%{searchTerm.ToString()}%';".AsMemory();

       using (var connection = new SQLiteConnection(_connectionString.ToString()))
       {
            await connection.OpenAsync();
            artists = await connection.QueryAsync<ArtistMap>(dapperQuery.ToString());

            Console.WriteLine(dapperQuery);
       }
       return artists;
    }

    public async Task<IEnumerable<PlaylistMap>> SearchPlaylists(ReadOnlyMemory<char> searchTerm)
    {
       IEnumerable<PlaylistMap> playlists = null;

       ReadOnlyMemory<char> dapperQuery = $"select PID, Name, Description, Avatar, Year from playlists where Name like '%{searchTerm.ToString()}%';".AsMemory();

       using (var connection = new SQLiteConnection(_connectionString.ToString()))
       {
            await connection.OpenAsync();
            playlists = await connection.QueryAsync<PlaylistMap>(dapperQuery.ToString());
       }

       return playlists;
    }

    public async Task<IEnumerable<TrackMap>> SearchTracks(ReadOnlyMemory<char> searchTerm)
    {
       IEnumerable<TrackMap> tracks = null;

       ReadOnlyMemory<char> dapperQuery = $"select TID, Path, Name, Description, Avatar, Valid, Duration, Year from tracks where Name like '%{searchTerm.ToString()}%';".AsMemory();

       using (var connection = new SQLiteConnection(_connectionString.ToString()))
       {
            await connection.OpenAsync();
            tracks = await connection.QueryAsync<TrackMap>(dapperQuery.ToString());
       }

       return tracks;
    }
}
