using Ild_Music.Core.Instances;
using Ild_Music.Core.Services.Entities;

using System.Collections.Concurrent;

namespace Ild_Music.Core.Helpers;

public class FileHelper
{
    private static Guid _helperId = Guid.NewGuid();
    private static ConcurrentDictionary<ReadOnlyMemory<char>, Track> _musicFiles = null;
    private static FactoryGhost _factoryGhost = null;
    private static IEnumerable<string> _supportedMimeTypes = null;

    public static Guid FILE_HELPER_ID => _helperId;

    public static void SetFactoryGhost(FactoryGhost factoryGhost)
    {
        _factoryGhost = factoryGhost;
    }

    public static void SetMimeTypes(IEnumerable<string> mimeTypes)
    {
        _supportedMimeTypes = mimeTypes;
    }

    public static IEnumerable<Track> BrowseFiles(IEnumerable<string> inputPaths)
	{
        _musicFiles = new();

		Parallel.ForEach(inputPaths,new ParallelOptions { MaxDegreeOfParallelism = 4 },
            (string file) => ProcessFile(file));

        var result = _musicFiles.Values;
        _musicFiles.Clear();
		return result;
	}

    private static void ProcessFile(string file)
    {    
        bool isHelperReady = _factoryGhost != null && _supportedMimeTypes?.Count() > 0;

        if (isHelperReady && File.Exists(file))
        {
            var track = _factoryGhost.CreateTrackBrowsed(file);
            var mimeType = track.MimeType.ToString();

            if (!_supportedMimeTypes.Contains(mimeType))
                return;

            _musicFiles.AddOrUpdate(track.Pathway, track, (ReadOnlyMemory<char> key, Track oldValue) => track);
        }
    }
}