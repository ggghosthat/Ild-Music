namespace Cube.Mapper.Entities;
public record TrackMap : IMappable
{
    public string TID {get; set;}
    public string Path {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}
    public byte[] Avatar {get; set;}
    public int IsValid {get; set;}
    public int Duration {get; set;}
    public int Year {get; set;}
}
