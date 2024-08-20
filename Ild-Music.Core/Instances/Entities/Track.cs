using TagLib;

namespace Ild_Music.Core.Instances;

public struct Track
{
    public Track(Guid id,
        ReadOnlyMemory<char> pathway,
        ReadOnlyMemory<char> name,
        ReadOnlyMemory<char> description,
        ReadOnlyMemory<byte> avatarSource,
        TimeSpan duration,
        int year)
    {
        Id = id; 
        Pathway = pathway;
        Name = name;
        Description = description;
        AvatarSource = avatarSource;      
        Duration = duration;
        Year = year;
    }

    public Track(Guid id,
        ReadOnlyMemory<char> pathway,
        ReadOnlyMemory<char> name,
        ReadOnlyMemory<char> description,
        ReadOnlyMemory<char> avatarPath,
        TimeSpan duration,
        int year)
    {
        Id = id; 
        Pathway = pathway;
        Name = name;
        Description = description;
        AvatarPath = avatarPath; 
        Duration = duration;
        Year = year;
    }
	
    public Guid Id { get; set; }

	public ReadOnlyMemory<char> Pathway { get; set; } = string.Empty.AsMemory();

	public ReadOnlyMemory<char> Name { get; set; } = string.Empty.AsMemory();

	public ReadOnlyMemory<char> Description { get; set; } = string.Empty.AsMemory();

    public ReadOnlyMemory<byte> AvatarSource { get; set; } = new byte[0];

    public ReadOnlyMemory<char> AvatarPath { get; set; }
    
    public int Year { get; set; } = DateTime.Now.Year;

    public bool IsValid { get; private set; } = false;
	
    public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(0);

    public List<Guid> Artists { get; set; } = new List<Guid>(20);

    public List<Guid> Playlists { get; set; } = new List<Guid>(20);
	
    public List<Tag> Tags { get; set; } = new List<Tag>();
}
