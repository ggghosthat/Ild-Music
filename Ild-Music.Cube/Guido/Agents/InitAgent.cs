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

    public static void ConfigureAgent(string allocationPath,
                                      int pageLimit = 100)
    {

        SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
        path = Path.Combine(allocationPath, "storage.db");
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
                //create main tables
                connection.Execute("create table if not exists artists(Id integer primary key, AID, Name varchar, Description varchar, Year integer, Avatar blob)");
                connection.Execute("create table if not exists playlists(Id integer primary key, PID, Name varchar, Description varchar, Year integer, Avatar blob)");
                connection.Execute("create table if not exists tracks(Id integer primary key, TID, Name varchar, Description varchar, Year integer, Avatar blob, Valid integer, Duration integer)");

                //create n:n relates between main tables
                connection.Execute("create table if not exists artists_playlists(Id integer primary key, AID, PID)");
                connection.Execute("create table if not exists artists_tracks(Id integer primary key, AID, TID)");
                connection.Execute("create table if not exists playlists_tracks(Id integer primary key, PID, TID)");
                
                //tags are still staying alone from others
                connection.Execute("create table if not exists tags(Id integer primary key, TagID, Name varchar, Color varchar)");
                connection.Execute("create table if not exists tags_instances(Id integer primary key, TagID, IID, EntityType integer)");

                //create indexing virtual tables with FTS5 sqlite's extenssion
                connection.Execute("create virtual table if not exists artists_index using fts5 (AID, Name, Description, Year)");
                connection.Execute("create virtual table if not exists playlists_index using fts5 (PID, Name, Description, Year)");
                connection.Execute("create virtual table if not exists tracks_index using fts5 (TID, Name, Description, Year)");
                connection.Execute("create virtual table if not exists tags_index using fts5 (TagId, Name)");
           
                //insert trigger for virtual tables
                connection.Execute("create trigger if not exists insert_artist after insert on artists begin insert into artists_index (AID, Name, Description, Year) values(NEW.AID, NEW.Name, New.Description, New.Year); end;");
                connection.Execute("create trigger if not exists insert_playlist after insert on playlists begin insert into playlists_index (PID, Name, Description, Year) values(NEW.PID, NEW.Name, New.Description, New.Year); end;");
                connection.Execute("create trigger if not exists insert_track after insert on tracks begin insert into tracks_index (TID, Name, Description, Year) values(NEW.TID, NEW.Name, New.Description, New.Year); end;");

                //update triggers for virtual tables
                connection.Execute("create trigger if not exists update_artist after update on artists begin update artists_index set AID=NEW.AID, Name=NEW.Name, Description=NEW.Description, Year=NEW.Year where AID=NEW.AID; end;");
                connection.Execute("create trigger if not exists update_playlist after update on playlists begin update playlists_index set PID=NEW.PID, Name=NEW.Name, Description=NEW.Description, Year=NEW.Year where PID=NEW.PID; end;");
                connection.Execute("create trigger if not exists update_track after update on tracks  begin update tracks_index set TID=NEW.TID, Name=NEW.Name, Description=NEW.Description, Year=NEW.Year where TID=NEW.TID; end;");
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }

    }
}
