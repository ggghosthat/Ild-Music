namespace Ild_Music.Core.Instances;

public struct Artist
{	
	public Artist (Guid id,
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

    public Artist (Guid id,
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
	
    public ReadOnlyMemory<char> Name {get; set;}
	
    public ReadOnlyMemory<char> Description {get; set;}
	
    public ReadOnlyMemory<byte> AvatarSource {get; set;}
    
    public ReadOnlyMemory<char> AvatarPath {get; set;}
    
    public int Year {get; set;} = DateTime.Now.Year;

	public List<Guid> Tracks {get; set;} = new List<Guid>();

	public List<Guid> Playlists {get; set;} = new List<Guid>();

	public List<Tag> Tags {get; set;} = new List<Tag>();

	public void AddTrack(ref Track track)
	{
		if(!Tracks.Contains(track.Id))
			Tracks.Add(track.Id);
	}

	public void AddPlaylist(ref Playlist playlist)
	{
		if(!Playlists.Contains(playlist.Id))
		{
			Playlists.Add(playlist.Id);
            Tracks.ToList().AddRange(playlist.Tracks);
		}
	}

	public void DeleteTrack(ref Track track) 
	{
		if (Tracks.Contains(track.Id))
			Tracks.Remove(track.Id);
	}

	public void DeletePlaylist(ref Playlist playlist) 
	{
		if (!Playlists.Contains(playlist.Id))
		{
			Playlists.Remove(playlist.Id);

            foreach (var tid in playlist.Tracks)
                Tracks.Remove(tid);
		}
	}
}