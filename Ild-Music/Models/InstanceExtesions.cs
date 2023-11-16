using Ild_Music.Core.Instances;

using System.Linq;
using System.Collections.Generic;

namespace Ild_Music.Extensions;
public static class InstanceExtensions
{
    private static SupporterService supporter => (SupporterService)App.Stage.GetServiceInstance("SupporterService");
     
    public static IEnumerable<Artist> GetTrackArtist(this Track track)
    {
        return supporter.ArtistsCollection.Where(a => a.Tracks.Contains(track.Id)).ToList();
    }
}

