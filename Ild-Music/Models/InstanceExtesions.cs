using ShareInstances;
using ShareInstances.Services.Entities;
using ShareInstances.Services.Storage;
using ShareInstances.PlayerResources;
using ShareInstances.Services.Entities;
using ShareInstances.PlayerResources.Interfaces;

using System.Linq;
using System.Collections.Generic;

namespace Ild_Music.Extensions
{
    public static class InstanceExtensions
    {
        private static SupporterService supporter => (SupporterService)App.Stage.GetServiceInstance("SupporterService");
        
        public static IEnumerable<ICoreEntity> GetInstanceArtist(this ICoreEntity instance)
        {
            if (instance is Playlist playlist)
            {
                return supporter.ArtistsCollection.Where(a => a.Playlists.Contains(playlist.Id)).ToList();
            }
            else if(instance is Track track)
            {
                return supporter.ArtistsCollection.Where(a => a.Tracks.Contains(track.Id)).ToList();
            }
            return null;
        }
    }
}
