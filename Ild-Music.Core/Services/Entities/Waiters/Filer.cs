using Ild_Music.Core.Instances;
using Ild_Music.Core.Contracts.Services.Interfaces;

using System.Collections.Concurrent;

namespace Ild_Music.Core.Services.Entities;
//Filer is a special class wich perfome temporary file system loading
//Filer just read special files(music formats, such as .mp3)
public class Filer : IWaiter
{	
    public Guid WaiterId {get; init;} = Guid.NewGuid();
    public ReadOnlyMemory<char> WaiterName {get; init;} = "Filer".AsMemory();

    private static ConcurrentDictionary<ReadOnlyMemory<char>, Track> MusicFiles = new ();

    private static FactoryGhost factoryGhost;

    public Filer()
    {}

	public void WakeUp(FactoryGhost ghost)
	{
        factoryGhost = ghost;
    }

	public IEnumerable<Track> BrowseFiles(IEnumerable<string> inputPaths)
	{
		Parallel.ForEach(inputPaths, new ParallelOptions { MaxDegreeOfParallelism = 4 },
			(string file) => ProcessFile(file));
	
		return MusicFiles.Values;
	}

    private static void ProcessFile(string file)
    {
        if (File.Exists(file))
        {
            var ext = Path.GetExtension(file.ToString());
            if (ext.Equals(".mp3"))
            {
                var track = factoryGhost.CreateTrackBrowsed(file);
                MusicFiles.AddOrUpdate(track.Pathway, track, (ReadOnlyMemory<char> key, Track oldValue) => track);
            }
        }
    }

	public void CleanFiler()
	{
		MusicFiles.Clear();
	}
}
