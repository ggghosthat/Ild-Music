namespace Ild_Music.Core.Instances;

public struct Tag
{
    public Guid Id { get; set; }
    public ReadOnlyMemory<char> Name { get; set; }
    public ReadOnlyMemory<char> Color { get; set; }

    public IList<Guid> Artists { get; set; } = new List<Guid>();
    public IList<Guid> Playlists { get; set; } = new List<Guid>();
    public IList<Guid> Tracks { get; set; } = new List<Guid>();


    public Tag (Guid id,
                ReadOnlyMemory<char> name,
				ReadOnlyMemory<char> color)
	{
        Id = id;
		Name = name;
        Color = color;
	}
 
}
