namespace Ild_Music.Core.Instances.DTO;

public class QueryPool : IDisposable
{
    public IEnumerable<CommonInstanceDTO> ArtistsDTOs { get; set; }

    public IEnumerable<CommonInstanceDTO> PlaylistsDTOs { get; set; }

    public IEnumerable<CommonInstanceDTO> TracksDTOs { get; set; }


    public void Dispose()
    {
        ArtistsDTOs.ToList().Clear();
        PlaylistsDTOs.ToList().Clear();
        TracksDTOs.ToList().Clear();
    }
}
