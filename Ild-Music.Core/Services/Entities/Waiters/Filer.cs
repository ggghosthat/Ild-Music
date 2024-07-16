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

    private static Memory<byte> buffer;

    public Filer()
    {}

	public void WakeUp(FactoryGhost ghost)
	{
        factoryGhost = ghost;
    }

	public async Task BrowseFiles(IEnumerable<string> inputPaths)
	{
		Parallel.ForEach(
			inputPaths,
			new ParallelOptions { MaxDegreeOfParallelism = 4 },
			(string file) =>
        {            
			//containing file-format restriction.
    		//in the nearest release will be allow mp3 format only!!!
            if(File.Exists(file))
			{
				var ext = Path.GetExtension(file.ToString());
	    	    if (ext.Equals(".mp3"))
	        	{
                    var track = factoryGhost.CreateTrackBrowsed(file);
		            MusicFiles.AddOrUpdate(track.Pathway, track, (ReadOnlyMemory<char> key, Track  oldValue) => track);
	       		}
	       	}
        });
	}

	public IList<Track> GetTracks()
	{
		return MusicFiles.Values.ToList();
	}

	public void CleanFiler()
	{
		MusicFiles.Clear();
	}
}
