using System.Data.SQLite;
using Dapper;

namespace Cube.Guido.Engine;

internal class Validator
{
    private string _connectionString;
    public Validator(ref string connectionString)
    {
        _connectionString = connectionString;
    }
   
    private async Task<IEnumerable<Guid>> PerformValidation(ReadOnlyMemory<char> dapperQuery, ICollection<Guid> input_ids)
    {
        IEnumerable<string> result;
        IEnumerable<string> input = input_ids.Select(guid => guid.ToString());
        using (var connection = new SQLiteConnection(_connectionString.ToString()))
        {                
            await connection.OpenAsync();
            result = await connection.QueryAsync<string>(dapperQuery.ToString(), new {ids =  input} );
        }
        return result.Select(raw => new Guid(raw));
    }

    
    public async Task<IEnumerable<Guid>> ValidateTracks(ICollection<Guid> input_ids)
    {
        ReadOnlyMemory<char> dapperQuery = "select TID from tracks where TID in @ids)".AsMemory();
        return await PerformValidation(dapperQuery, input_ids);
    }

    public async Task<IEnumerable<Guid>> ValidateArtists(ICollection<Guid> input_ids)
    {
        ReadOnlyMemory<char> dapperQuery = "select AID from artists where AID in @ids)".AsMemory();
        return await PerformValidation(dapperQuery, input_ids);
    }

    public async Task<IEnumerable<Guid>> ValidatePlaylists(ICollection<Guid> input_ids)
    {
        ReadOnlyMemory<char> dapperQuery = "select PID from playlists where PID in @ids)".AsMemory();
        return await PerformValidation(dapperQuery, input_ids);
    }

}
