namespace Cube.Mapper.Entities;
public record TagMap : IMappable
{
    public string Buid { get; set; }    
    public string Name { get; set; }
    public string Color { get; set; }
}
