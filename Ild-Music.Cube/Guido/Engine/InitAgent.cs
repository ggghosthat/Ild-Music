using System.Data;
using System.Data.SQLite;
using Dapper;

namespace Cube.Guido.Engine.Agents;

//this class was being delegated SQLite database 
internal static class ConnectionAgent
{
    private static string path;
    private static string connectionString;

    public static void ConfigAgent(string dbPath)
    {
        path = dbPath;
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
    public static void ConfigConnection()
    {
        try
        { 
            using (IDbConnection connection = GetDbConnection())
            {
                connection.Execute("create table if not exists artists(Id integer primary key, AID, Name varchar, Description varchar, Year integer, Avatar blob)");
                connection.Execute("create table if not exists playlists(Id integer primary key, PID varchar, Name varchar, Description varchar, Year integer, Avatar blob)");
                connection.Execute("create table if not exists tracks(Id integer primary key, TID varchar, Path varchar, Name varchar, Description varchar, Year integer, Avatar blob, Valid integer, Duration integer)");
                

                connection.Execute("create table if not exists artists_playlists(Id integer primary key, AID, PID)");
                connection.Execute("create table if not exists artists_tracks(Id integer primary key, AID, TID)");
                connection.Execute("create table if not exists playlists_tracks(Id integer primary key, PID varchar, TID varchar)");


                connection.Execute("create table if not exists tags(Id integer primary key, TagID varchar, Name varchar, Color varchar)");
                connection.Execute("create table if not exists tags_instances(Id integer primary key, TagID varchar, IID varchar, EntityType integer)");

                connection.Execute("create index if not exists artists_index on artists(AID, lower(Name), Year)");
                connection.Execute("create index if not exists playlists_index on playlists(PID, lower(Name), Year)");
                connection.Execute("create index if not exists tracks_index on tracks(TID, Path, lower(Name), Year)");
                connection.Execute("create index if not exists tracks_index on tags(TagID, lower(Name), EntityType)");
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }

    }
}
