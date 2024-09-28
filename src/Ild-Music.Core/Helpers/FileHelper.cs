using Ild_Music.Core.Instances;
using Ild_Music.Core.Services.Entities;

using System.Collections.Concurrent;

namespace Ild_Music.Core.Helpers;

public class FileHelper
{
    private static Guid _helperId = Guid.NewGuid();
    private static ConcurrentDictionary<ReadOnlyMemory<char>, Track> _musicFiles = null;
    private static FactoryGhost _factoryGhost = null;

    public static Guid FILE_HELPER_ID => _helperId;

    public static void SetFactoryGhost(FactoryGhost factoryGhost)
    {
        _factoryGhost = factoryGhost;

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
        if (_factoryGhost != null && File.Exists(file))
        {
            var track = _factoryGhost.CreateTrackBrowsed(file);
            _musicFiles.AddOrUpdate(track.Pathway, track, (ReadOnlyMemory<char> key, Track oldValue) => track);
        }
    }
}