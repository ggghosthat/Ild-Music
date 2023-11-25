using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Services.InstanceProducer;

namespace Ild_Music.Core.Instances;
public static class Extesions
{
    //Search for the single item
	public static Track ToTrackEntity(this Guid guid, IList<Track> store)
    {
        return store.ToList().FirstOrDefault(i => guid.Equals(i.Id));
    }

    public static Artist ToArtistEntity(this Guid guid, IList<Artist> store)
    {
        return store.ToList().FirstOrDefault(i => guid.Equals(i.Id));
    }

    public static Playlist ToArtistEntity(this Guid guid, IList<Playlist> store)
    {
        return store.ToList().FirstOrDefault(i => guid.Equals(i.Id));
    }


    //Search for list of entities in the input store
    public static List<Track> ToTrackEntities(this IList<Guid> guids, IList<Track> store)
	{
    	return store.ToList()
                    .FindAll(delegate(Track item) { return guids.Contains(item.Id); });
	}

    public static List<Artist> ToTrackEntities(this IList<Guid> guids, IList<Artist> store)
	{
    	return store.ToList()
                    .FindAll(delegate(Artist item) { return guids.Contains(item.Id); });
	}

    public static List<Playlist> ToTrackEntities(this IList<Guid> guids, IList<Playlist> store)
	{
    	return store.ToList()
                    .FindAll(delegate(Playlist item) { return guids.Contains(item.Id); });
	}


    public static Playlist ComposePlaylist(this IList<Track> tracks)
    {
        var producer = new InstanceProducer($"Playlist: { DateTime.Now.ToString("h_mm") }".ToCharArray(), 
                                            "".ToCharArray(),
                                            new byte[0],
                                            2000,
                                            tracks,
                                            null);

        return producer.PlaylistInstance;
    }

    public static IEnumerable<CommonInstanceDTO> ToCommonDTO(this IEnumerable<Artist> collection)
    {
       foreach(var artist in collection)
       {
           yield return new CommonInstanceDTO(artist.Id,
                                              artist.Name,
                                              artist.AvatarSource,
                                              EntityTag.ARTIST);
       }
    }

    public static IEnumerable<CommonInstanceDTO> ToCommonDTO(this IEnumerable<Playlist> collection)
    {
       foreach(var playlist in collection)
       {
           yield return new CommonInstanceDTO(playlist.Id,
                                              playlist.Name,
                                              playlist.AvatarSource,
                                              EntityTag.ARTIST);
       }
    }

    public static IEnumerable<CommonInstanceDTO> ToCommonDTO(this IEnumerable<Track> collection)
    {
       foreach(var track in collection)
       {
           yield return new CommonInstanceDTO(track.Id,
                                              track.Name,
                                              track.AvatarSource,
                                              EntityTag.ARTIST);
       }
    }
}
