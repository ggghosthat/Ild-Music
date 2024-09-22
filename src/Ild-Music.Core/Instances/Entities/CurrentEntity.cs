namespace Ild_Music.Core.Instances;
public record struct CurrentEntity(Guid Id, 
                                   Memory<char> Name,
                                   Memory<char> Description,
                                   Memory<byte> Avatar,
                                   bool IsTrack=true);
