using Ild_Music.Core.Instances;
using Ild_Music.Core.Services.InstanceProducer;
using Ild_Music.Core.Services.Entities;

using System;
using System.Linq;
using System.Collections.Generic;

namespace Ild_Music.Extensions;
public static class InstanceExtensions
{
    private static SupportGhost supporter => (SupportGhost)App.Stage.GetServiceInstance("SupporterService");
     
    public static IEnumerable<Artist> GetTrackArtist(this Track track)
    {
        return supporter.ArtistsCollection.Where(a => a.Tracks.Contains(track.Id)).ToList();
    } 
}

