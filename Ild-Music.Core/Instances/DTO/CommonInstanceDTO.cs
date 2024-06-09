namespace Ild_Music.Core.Instances.DTO;
public struct CommonInstanceDTO
{
    public Guid Id { get; }
    public ReadOnlyMemory<char> Name { get; }
    public ReadOnlyMemory<byte> Avatar { get; } 
    public EntityTag Tag { get; }

    public CommonInstanceDTO(
        Guid id,
        ReadOnlyMemory<char> name,
        ReadOnlyMemory<byte> avatar,
        EntityTag tag)
    {
        Id = id;
        Name = name;
        Avatar = avatar;
        Tag = tag;
    }
}
