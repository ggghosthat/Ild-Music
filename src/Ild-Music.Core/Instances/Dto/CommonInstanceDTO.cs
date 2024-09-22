namespace Ild_Music.Core.Instances.DTO;
public struct CommonInstanceDTO
{
    public Guid Id { get; }
    public ReadOnlyMemory<char> Name { get; }
    public ReadOnlyMemory<byte> Avatar { get; } 
    public ReadOnlyMemory<char> AvatarPath { get; }
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

    public CommonInstanceDTO(
        Guid id,
        ReadOnlyMemory<char> name,
        ReadOnlyMemory<char> avatarPath,
        EntityTag tag)
    {
        Id = id;
        Name = name;
        AvatarPath = avatarPath;
        Tag = tag;
    }
}
