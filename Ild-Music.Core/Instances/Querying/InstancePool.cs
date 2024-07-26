using Ild_Music.Core.Instances.DTO;

namespace Ild_Music.Core.Instances.Querying;

public class InstancePool : IDisposable
{
    public IEnumerable<CommonInstanceDTO> ArtistsDTOs { get; set; }

    public IEnumerable<CommonInstanceDTO> PlaylistsDTOs { get; set; }

    public IEnumerable<CommonInstanceDTO> TracksDTOs { get; set; }

    public IEnumerable<CommonInstanceDTO> TagsDTOs { get; set; }

    public void Dispose()
    {
        ArtistsDTOs.ToList().Clear();
        PlaylistsDTOs.ToList().Clear();
        TracksDTOs.ToList().Clear();
        TagsDTOs.ToList().Clear();
    }
}
