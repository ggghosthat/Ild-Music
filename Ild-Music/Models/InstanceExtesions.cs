using Ild_Music.Core.Instances;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Contracts.Services.Interfaces;

using System.Linq;
using System.Collections.Generic;

namespace Ild_Music.Extensions;
public static class InstanceExtensions
{
    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
     
    public static IEnumerable<Artist> GetTrackArtist(this Track track)
    {
        return supporter.ArtistsCollection.Where(a => a.Tracks.Contains(track.Id)).ToList();
    } 
}

