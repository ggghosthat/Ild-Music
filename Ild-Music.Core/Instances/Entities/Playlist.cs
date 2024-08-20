using Ild_Music.Core.Services.Entities;

using System.Linq;

namespace Ild_Music.Core.Instances;
public struct Playlist
{
    public Playlist(Guid id,
        ReadOnlyMemory<char> name, 
        ReadOnlyMemory<char> description,
        ReadOnlyMemory<byte> avatarSource,
        int year)
    {
        Id = id;
        Name = name;
        Description = description;
        AvatarSource = avatarSource;
        Year = year;
    }
    
    public Playlist(Guid id,
        ReadOnlyMemory<char> name, 
        ReadOnlyMemory<char> description,
        ReadOnlyMemory<char> avatarPath,
        int year)
    {
        Id = id;
        Name = name;
        Description = description;
        AvatarPath = avatarPath;
        Year = year;
    }

	public Guid Id {get; set;}

	public ReadOnlyMemory<char> Name {get; set;} = string.Empty.AsMemory();

	public ReadOnlyMemory<char> Description {get; set;} = string.Empty.AsMemory();
    
    public ReadOnlyMemory<byte> AvatarSource {get; set;} = new byte[0]; 
   
    public ReadOnlyMemory<char> AvatarPath {get; set;} 

    public int Year {get; set;} = DateTime.Now.Year;
    
    public List<Guid> Artists { get; set; } = new List<Guid>(20);
    
    public List<Guid> Tracks { get; set; } = new List<Guid>(20);
    
    public List<Tag> Tags { get; set; } = new List<Tag>();
    
    public List<Track> TrackLine { get; private set; } = new List<Track>();

    public int Count => Tracks.Count;
	
    public int CurrentIndex {get; set;} = 0;

    public bool IsOrdered { get; private set; } = false;

    public Track? this[int i]
    {
        get => TrackLine[i];
    }
   
    public void AddTrack(ref Track track)
    {        
        if(!Tracks.Contains(track.Id))
        {
    	    Tracks.Add(track.Id);

            foreach (var art in Artists)
                track.Artists.Add(art);            
        }
    }

    public void RemoveTrack(ref Track track)
    {        
    	if(Tracks.Contains(track.Id))
    	{
    		Tracks.Remove(track.Id);

            foreach (var art in Artists)
                track.Artists.Remove(art);
    	}
    }

    public void LoadTrackLine(SupportGhost supportGhost)
    {
        var tracks = supportGhost.LoadTracksById(Tracks).Result;
        TrackLine.AddRange(tracks);
    }

    public void EraseTracks()
    {
        TrackLine.Clear();
    }
}
