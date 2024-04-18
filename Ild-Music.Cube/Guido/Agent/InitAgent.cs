using System.Data;
using System.Data.SQLite;
using Dapper;

namespace Cube.Guido.Agents;

//this class was being delegated SQLite database 
internal static class ConnectionAgent
{ 
    private static string path;
    private static string connectionString;
    private static int queryLimit;

    public static int QueryLimit => queryLimit;

    public static void ConfigAgent(string dbPath,
                                   int pageLimit = 100)
    {

        SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
        path = dbPath;
        queryLimit = pageLimit;
        connectionString = $"Data Source = {path}";

        CheckFacilityIntegrity();
    }

    //checking out existance of file (relates to SQLite db)
    //create new db file for SQLite using
    private static void CheckFacilityIntegrity()
    {
        if(!File.Exists(path))
            SQLiteConnection.CreateFile(path);
    }

    //returning database connection object for Dapper using
    public static IDbConnection GetDbConnection() =>
        new SQLiteConnection(connectionString);

    //initialize database tables in SQLite db
    public static void SpreadDatabase()
    {
        try
        { 
            using (IDbConnection connection = GetDbConnection())
            {
                connection.Execute("create table if not exists artists(Id integer primary key, AID, Name varchar, Description varchar, Year integer, Avatar blob)");
                connection.Execute("create table if not exists playlists(Id integer primary key, PID, Name varchar, Description varchar, Year integer, Avatar blob)");
                connection.Execute("create table if not exists tracks(Id integer primary key, TID, Path varchar, Name varchar, Description varchar, Year integer, Avatar blob, Valid integer, Duration integer)");
                

                connection.Execute("create table if not exists artists_playlists(Id integer primary key, AID, PID)");
                connection.Execute("create table if not exists artists_tracks(Id integer primary key, AID, TID)");
                connection.Execute("create table if not exists playlists_tracks(Id integer primary key, PID, TID)");

                connection.Execute("create table if not exists tags(Id integer primary key, TagID, Name varchar, Color varchar)");
                connection.Execute("create table if not exists tags_instances(Id integer primary key, TagID, IID, EntityType integer)");

                connection.Execute("create virtual table if not exists artists_index using fts5 (Name, Description, Year)");
                connection.Execute("create virtual table if not exists playlists_index using fts5 (Name, Description, Year)");
                connection.Execute("create virtual table if not exists tracks_index using fts5 (Name, Description, Year)");
                connection.Execute("create virtual table if not exists tags_index using fts5 (Name)");
            
                connection.Execute("create trigger if not exists insert_artist after insert on artists begin insert into artists_index (Name, Description, Year) values(NEW.Name, New.Description, New.Year); end;");
                connection.Execute("create trigger if not exists insert_playlist after insert on playlists begin insert into playlists_index (Name, Description, Year) values(NEW.Name, New.Description, New.Year); end;");
                connection.Execute("create trigger if not exists insert_track after insert on tracks begin insert into tracks_index (Name, Description, Year) values(NEW.Name, New.Description, New.Year); end;");

                connection.Execute("create trigger if not exists update_artist after update on artists begin update artists_index set Name=New.Name, Description=New.Description, Year=New.Year where AID=New.AID; end;");
                connection.Execute("create trigger if not exists update_playlist after update on playlists begin update playlists_index set Name=New.Name, Description=New.Description, Year=New.Year where PID=New.PID; end;");
                connection.Execute("create trigger if not exists update_track after update on tracks  begin update tracks_index set Name=New.Name, Description=New.Description, Year=New.Year where TID=New.TID; end;");
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }

    }
}
