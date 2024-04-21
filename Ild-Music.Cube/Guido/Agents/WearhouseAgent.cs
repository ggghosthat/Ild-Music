using Ild_Music.Core.Instances;

namespace Cube.Guido.Agents;

internal static class WearhouseAgent
{
    private static string _wearhousePath;

    private static bool IsMove = false;

    public static void ConfigureAgent(string wearhousePath, bool isMove)
    {
        _wearhousePath = wearhousePath;
        IsMove = isMove;
    }

    public static string GetTrackPathFromId(Guid trackId)
    {
        string trckIdString = trackId.ToString();
        string path = Path.Combine(_wearhousePath, trckIdString);

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
            string path = Path.Combine(_wearhousePath, trckIdString);

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

        string trackId = track.Id.ToString();
        string allocationPath = Path.Combine(_wearhousePath, trackId);
        
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

            string trackId = track.Id.ToString();
            string allocationPath = Path.Combine(_wearhousePath, trackId);
        
            if (IsMove == true)
                File.Move(path, allocationPath);
            else 
                await CopyFromInputToOutputAsync(allocationPath, path);
        });
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
