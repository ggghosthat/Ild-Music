using System.Data.SQLite;
using Dapper;

namespace Cube.Guido.Agents;

//this class was being delegated SQLite database 
internal static class ConnectionAgent
{ 
    private static string path;
    private static string connectionString;
    private static int queryLimit;

    public static void ConfigureAgent(
        string allocationPath,
        int pageLimit = 10)
    {
        path = Path.Combine(allocationPath, "storage.db");
        queryLimit = pageLimit;
        connectionString = $"Data Source = {path}";

        CheckFacilityIntegrity();
    }

    public static int QueryLimit => queryLimit;

    //checking out existance of file (relates to SQLite db)
    //create new db file for SQLite using
    private static void CheckFacilityIntegrity()
    {
        if(!File.Exists(path))
            SQLiteConnection.CreateFile(path);
    }

    //returning database connection object for Dapper using
    public static SQLiteConnection GetDbConnection() =>
        new SQLiteConnection(connectionString);

    //initialize database tables in SQLite db
    public static void SpreadDatabase()
    {
        try
        { 
            using (SQLiteConnection connection = GetDbConnection())
            {
                connection.Open();

                //create main tables
                connection.Execute("create table if not exists artists(Id integer primary key, AID, Name varchar, Description varchar, Year integer, Avatar blob)");
                connection.Execute("create table if not exists playlists(Id integer primary key, PID, Name varchar, Description varchar, Year integer, Avatar blob)");
                connection.Execute("create table if not exists tracks(Id integer primary key, TID, Name varchar, Description varchar, Year integer, Avatar blob, Duration integer)");

                //create n:n relates between main tables
                connection.Execute("create table if not exists artists_playlists(Id integer primary key, AID, PID)");
                connection.Execute("create table if not exists artists_tracks(Id integer primary key, AID, TID)");
                connection.Execute("create table if not exists playlists_tracks(Id integer primary key, PID, TID)");
                
                //tags are still staying alone from others
                connection.Execute("create table if not exists tags(Id integer primary key, TagID, Name varchar, Color varchar)");
                connection.Execute("create table if not exists tags_instances(Id integer primary key, TagID, IID, EntityType integer)");

                connection.Execute("create index if not exists idx_artists on artists(Name)");
                connection.Execute("create index if not exists idx_playlists on playlists(Name)");
                connection.Execute("create index if not exists idx_tracks on tracks(Name)");
                connection.Execute("create index if not exists idx_tags on tags(Name)");
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }

    }
}
