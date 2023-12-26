namespace Ild_Music.Core.Instances;

public struct Tag
{
    public Guid Id { get; set; }
    public ReadOnlyMemory<char> Name { get; set; }
    public ReadOnlyMemory<char> Color { get; set; }

    public Tag (Guid id,
                   ReadOnlyMemory<char> name,
				   ReadOnlyMemory<char> color)
	{
        Id = id;
		Name = name;
        Color = color;
	}
 
}
