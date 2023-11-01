using System;

namespace Cube.Mapper.Entities;
public record PlaylistMap : IMappable
{
    public string PID {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}
    public byte[] Avatar {get; set;}
    public int Year {get; set;}
}
