using Ild_Music.Core.Instances;
using Ild_Music.Core.Services.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
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
}
