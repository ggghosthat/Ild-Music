namespace Ild_Music.Core.Instances;
public struct Playlist
{
	public Guid Id {get; set;}

	public ReadOnlyMemory<char> Name {get; set;} = string.Empty.AsMemory(); 
	public ReadOnlyMemory<char> Description {get; set;} = string.Empty.AsMemory();
    public ReadOnlyMemory<byte> AvatarSource {get; set;} = new byte[0]; 
    public int Year {get; set;} = DateTime.Now.Year;

    private Lazy<List<Track>> Tracks; 
    
    public ICollection<Guid> Artists {get; set;} = new List<Guid>(20);
    public ICollection<Guid> Tracky {get; set;} = new List<Guid>(20);
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();

    //Please, be carefull when you call this property and DO NOT call much this property
    //When playlist contains many track objects, Lazy<T> will init whole list in CLR's heap
    public int Count => Tracks.Value.Count;
	
    public int CurrentIndex {get; set;} = 0;
	public bool IsOrdered { get; private set; } = false;

    public Track this[int i]
    {
        get
        {
            return Tracks.Value[i];
        }
    }

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

        Tracks = new Lazy<List<Track>>();
    }

    #region Collection Manipulation Methods
    public void AddTrack(ref Track track)
    {        
        if(!Tracks.Value.Contains(track))
        {
    	    Tracks.Value.Add(track);
            track.Playlists.Add(Id);

            foreach (var art in Artists)
                track.Artists.Add(art);
            
        }
    }


    public void RemoveTrack(ref Track track)
    {        
    	if(Tracks.Value.Contains(track))
    	{
    		Tracks.Value.Remove(track);
            track.Playlists.Remove(Id);

            foreach (var art in Artists)
                track.Artists.Remove(art);
    	}
    }

    public IEnumerable<Track> GetTracks()
    {
        return Tracks.Value;
    }

    public void RecoverTracks(List<Track> tracks)
    {
        Tracks = new Lazy<List<Track>>(tracks);
    }

    public void DumpTracks()
    {
        if (Tracky?.Count > 0)
            Tracky?.Clear();
        
        if(Tracks is not null)
        {
            foreach(var track in Tracks.Value)
                Tracky.Add(track.Id);

            Tracks.Value.Clear();
        }
    }

    public void EraseTracks()
    {
        Tracks.Value.Clear();
    }
    #endregion


    #region Avatar Manipulation
    public byte[] GetAvatar()
    {
        try
        {
            return AvatarSource.ToArray();
        }
        catch(Exception ex)
        {
            //Speciall logging or throwing logic
            return null;
        }
    }

    public void SetAvatar(string path)
    {
        if(path is not null && File.Exists(path))
        {
            try
            {
                byte[] file = System.IO.File.ReadAllBytes(path);
                AvatarSource = file; 
            }
            catch(Exception ex)
            {
                //Speciall logging or throwing logic
                throw ex;   
            }            
        }
    }
    #endregion
}
