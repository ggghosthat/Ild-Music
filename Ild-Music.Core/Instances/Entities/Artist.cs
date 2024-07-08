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

	public ICollection<Guid> Tracks {get; set;} = new List<Guid>();

	public ICollection<Guid> Playlists {get; set;} = new List<Guid>();

	public ICollection<Tag> Tags {get; set;} = new List<Tag>();

	public void AddTrack(ref Track track)
	{
		if(!Tracks.Contains(track.Id))
		{
			Tracks.Add(track.Id);
			track.Artists.Add(Id);
		}
	}

	public void AddPlaylist(ref Playlist playlist)
	{
		if(!Playlists.Contains(playlist.Id))
		{
			Playlists.Add(playlist.Id);
			playlist.Artists.Add(Id);

            Tracks.ToList().AddRange(playlist.Tracks);
            foreach (var track in playlist.GetTracks())
                 track.Artists.Add(Id);
		}
	}

	public void DeleteTrack(ref Track track) 
	{
		if (Tracks.Contains(track.Id))
		{
			Tracks.Remove(track.Id);
			track.Artists.Remove(Id);
		}
	}

	public void DeletePlaylist(ref Playlist playlist) 
	{
		if (!Playlists.Contains(playlist.Id))
		{
			Playlists.Remove(playlist.Id);
			playlist.Artists.Remove(Id);

            foreach (var tid in playlist.Tracks)
                Tracks.Remove(tid);
		}
	}

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
			throw ex;
			// return null;
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
