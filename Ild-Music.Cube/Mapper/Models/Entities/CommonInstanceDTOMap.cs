using Ild_Music.Core.Instances;
namespace Cube.Mapper.Entities;
public record CommonInstanceDTOMap : IMappable
{
    public string ID { get; set; }
    public string Name { get; set; }
    public byte[] Avatar { get; set; }
    public EntityTag Tag { get; set; }
}
