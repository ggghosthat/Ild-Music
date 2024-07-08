namespace Ild_Music.Core.Instances;
public struct Playlist
{
    private Lazy<List<Track>> _tracks;

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
        _tracks = new Lazy<List<Track>>();
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
        _tracks = new Lazy<List<Track>>();
    }

	public Guid Id {get; set;}

	public ReadOnlyMemory<char> Name {get; set;} = string.Empty.AsMemory();

	public ReadOnlyMemory<char> Description {get; set;} = string.Empty.AsMemory();
    
    public ReadOnlyMemory<byte> AvatarSource {get; set;} = new byte[0]; 
   
    public ReadOnlyMemory<char> AvatarPath {get; set;} 

    public int Year {get; set;} = DateTime.Now.Year;
    
    public ICollection<Guid> Artists {get; set;} = new List<Guid>(20);
    
    public ICollection<Guid> Tracks {get; set;} = new List<Guid>(20);
    
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public int Count => _tracks.Value.Count;
	
    public int CurrentIndex {get; set;} = 0;

    public bool IsOrdered { get; private set; } = false;

    public Track this[int i]
    {
        get => _tracks.Value[i];
    }
   
    public void AddTrack(ref Track track)
    {        
        if(!_tracks.Value.Contains(track))
        {
    	    _tracks.Value.Add(track);
            track.Playlists.Add(Id);

            foreach (var art in Artists)
                track.Artists.Add(art);            
        }
    }

    public void RemoveTrack(ref Track track)
    {        
    	if(_tracks.Value.Contains(track))
    	{
    		_tracks.Value.Remove(track);
            track.Playlists.Remove(Id);

            foreach (var art in Artists)
                track.Artists.Remove(art);
    	}
    }

    public IEnumerable<Track> GetTracks()
    {
        return _tracks.Value;
    }

    public void RecoverTracks(List<Track> tracks)
    {
        _tracks = new Lazy<List<Track>>(tracks);
    }

    public void DumpTracks()
    {
        if (Tracks?.Count > 0)
            Tracks?.Clear();
        
        if(_tracks is not null)
        {
            foreach(var track in _tracks.Value)
                Tracks?.Add(track.Id);

            _tracks.Value.Clear();
        }
    }

    public void EraseTracks()
    {
        _tracks.Value.Clear();
    }

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
}
