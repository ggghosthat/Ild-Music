using ShareInstances.Instances;
using ShareInstances.Contracts.Services.Interfaces;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace ShareInstances.Services.Entities;
//Filer is a special class wich perfome temporary file system loading
//Filer just read special files(music formats, such as .mp3)
public class Filer : IWaiter
{	
    public Guid WaiterId {get; init;} = Guid.NewGuid();
    public ReadOnlyMemory<char> WaiterName {get; init;} = "Filer".AsMemory();

    private static ConcurrentDictionary<ReadOnlyMemory<char>, Track> MusicFiles = new ();

    private static FactoryGhost factoryGhost;

    private static Memory<byte> buffer;

	public Filer(FactoryGhost ghost)
	{
        factoryGhost = ghost;
    }

	public async Task BrowseFiles(IEnumerable<Memory<char>> inputPaths)
	{
		Parallel.ForEach(inputPaths,
						 new ParallelOptions { MaxDegreeOfParallelism = 4 },
						 (Memory<char> file) =>
        {            
			//containing file-format restriction.
    		//in the nearest release will be allow mp3 format only!!!
            if(File.Exists(file.ToString()))
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
