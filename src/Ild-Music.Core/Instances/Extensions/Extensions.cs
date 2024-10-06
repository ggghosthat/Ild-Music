using Ild_Music.Core.Instances.DTO;

namespace Ild_Music.Core.Instances;

public static class Extenssions
{

    public static CommonInstanceDTO ToCommonDTO(this Artist artist) =>
        new CommonInstanceDTO(artist.Id, artist.Name, artist.AvatarSource, EntityTag.TRACK);
    
    public static CommonInstanceDTO ToCommonDTO(this Playlist playlist) =>
        new CommonInstanceDTO(playlist.Id, playlist.Name, playlist.AvatarSource, EntityTag.TRACK);

    public static CommonInstanceDTO ToCommonDTO(this Track track) =>
        new CommonInstanceDTO(track.Id, track.Name, track.AvatarSource, EntityTag.TRACK);
}
