using Ild_Music.Core.Instances;

using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using Dapper;

namespace Cube.Guido.Engine.Handlers;

internal sealed class CommandHandler 
{
    private readonly IDbConnection _connection;
    
    public CommandHandler(IDbConnection connection)
    {
        _connection = connection;
    }

    //these methods needs to add instance and their relationships
    public async Task AddArtist(Artist artist) 
    {
        using (IDbConnection connection = _connection)
        {
            connection.Open();

            using(var transaction = connection.BeginTransaction())
            {
                var artistBodyQuery = @"""insert or ignore into artists(AID, Name, Description, Year, Avatar) 
                                                    values (@AID, @Name, @Description, @Year, @Avatar) RETURNING AID""";

                var artistPlaylistQuery = @"""insert into artists_playlists(AID, PID) select @aid, @pid 
                                                where not EXISTS(SELECT 1 from artists_playlists where AID = @aid and PID = @pid)""";
                
                var artistTrackQuery = @"""insert into artists_tracks(AID, TID) select @aid, @tid 
                                            where not EXISTS(SELECT 1 from artists_tracks where AID = @aid and TID = @tid)""";

                var aidRaw = connection.ExecuteScalarAsync<Guid>(artistBodyQuery,
                                                                new{
                                                                    AID = artist.Id,
                                                                    Name = artist.Name.ToString(),
                                                                    Description = artist.Description.ToString(),
                                                                    Year = artist.Year,
                                                                    Avatar = artist.AvatarSource.ToArray()
                                                                },
                                                                transaction);

                foreach(var artistPlaylistRelate in artist.Playlists)
                {
                    await connection.ExecuteAsync(artistPlaylistQuery,
                                                   new{
                                                    aid = aidRaw,
                                                    pid = artistPlaylistQuery
                                                   },
                                                   transaction);
                }

                foreach(var artistTrackRelate in artist.Playlists)
                {
                    await connection.ExecuteAsync(artistTrackQuery,
                                                   new{
                                                    aid = aidRaw,
                                                    tid = artistTrackQuery
                                                   },
                                                   transaction);
                }

                transaction.Commit();
            }
        }
    }

    public async Task AddPlaylist(Playlist playlist) 
    {}
    
    public async Task AddTrack(Track track)
    {}

    public async Task AddTag(Tag tag) 
    {}


    //these methods editting instances and their relationships
    public async Task EditArtist()
    {}

    public async Task EditPlaylist() 
    {}

    public async Task EditTrack() 
    {}

    public async Task EditTag() 
    {}



    //these methods delleting instances and their relationships 
    public async Task DeleteArtist()
    {}

    public async Task DeletePlaylist() 
    {}

    public async Task DeleteTrack() 
    {}

    public async Task DeleteTag() 
    {}

}
