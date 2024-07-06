using Ild_Music.Core.Instances;

namespace Cube.Guido.Agents;

internal static class WarehouseAgent
{
    private static string _wearhousePath;

    private static bool IsMove = false;

    public static void ConfigureAgent(string wearhousePath, bool isMove)
    {
        _wearhousePath = wearhousePath;
        IsMove = isMove;

        AllocateWearhouse();
    }

    public static string GetTrackPathFromId(Guid trackId)
    {
        string trckIdString = trackId.ToString();
        string path = Path.Combine(_wearhousePath, ".warehouse", "tracks", trckIdString);

        if (File.Exists(path))
            return path;
        else 
            return String.Empty;
    }

    public static IEnumerable<string> GetTrackPathsFromId(IEnumerable<Guid> trackIds)
    {
        return trackIds.Select(trackId => 
        {
            string trckIdString = trackId.ToString();
            string path = Path.Combine(_wearhousePath, ".warehouse", "tracks", trckIdString);

            if (File.Exists(path))
                return path;
            else 
                return String.Empty;
        });
    }

    public async static void PlaceTrackFile(Track track)
    {
        string path = track.Pathway.ToString();

        if (!File.Exists(path))
            return;

        string trackIdString = track.Id.ToString();
        string allocationPath = Path.Combine(_wearhousePath, ".warehouse", "tracks", trackIdString);
        
        if (IsMove == true)
            File.Move(path, allocationPath);
        else 
            await CopyFromInputToOutputAsync(allocationPath, path); 
    }

    public async static void PlaceTrackFiles(IEnumerable<Track> tracks)
    {
        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = 3
        };

        await Parallel.ForEachAsync(tracks, parallelOptions, async(track, token) => 
        {
            string path = track.Pathway.ToString();
            if (!File.Exists(path))
                return;

            string trackIdString = track.Id.ToString();
            string allocationPath = Path.Combine(_wearhousePath, ".warehouse", "tracks", trackIdString);
        
            if (IsMove == true)
                File.Move(path, allocationPath);
            else 
                await CopyFromInputToOutputAsync(allocationPath, path);
        });
    }

    private static void AllocateWearhouse()
    {
        string allocation_root = Path.Combine(_wearhousePath, ".warehouse");
        string track_allocation = Path.Combine(allocation_root, "tracks");
        string avatar_allocation = Path.Combine(allocation_root, "avatars");

        if(!Path.Exists(track_allocation))
            Directory.CreateDirectory(track_allocation);

        if(!Path.Exists(avatar_allocation))
            Directory.CreateDirectory(avatar_allocation);
    }

    private async static ValueTask CopyFromInputToOutputAsync(string outputPath, string inputPath)
    {
        try
        {
            int bytesRead = -1;
            int bufferSize = 1024;
            byte[] buffer = new byte [bufferSize];

            using (FileStream outputFileStream = new FileStream(outputPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            using (FileStream inputFileStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                while ((bytesRead = await inputFileStream.ReadAsync(buffer, 0, bufferSize)) > 0)
                    await outputFileStream.WriteAsync(buffer, 0, bufferSize);
            }
        }
        catch
        {
            throw;
        }
    }
}
