using Ild_Music_MVVM_.Services;
using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;
using System.Collections.Generic;

namespace Ild_Music_MVVM_.ViewModel.VM.FactoryVM
{
    public class SubControlViewModel : Base.BaseViewModel
    {
        private FactoryService factoryService => (FactoryService)GetService("Factory");
        public SupporterService Supporter => (SupporterService)GetService("Supporter");


        public Artist ArtistInstance { get; private set; } = null;
        public Playlist PlaylistInstance { get; private set; } = null;
        public Track TrackInstance { get; private set; } = null;


        public void CreateArtistInstance(object[] values)
        {
            var name = (string)values[0];
            var description = (string)values[1];
            factoryService.CreateArtist(name, description);
        }

        public void CreatePlaylistInstance(object[] values)
        {
            var name = (string)values[0];
            var description = (string)values[1];
            var tracks = (IList<object>)values[2] ?? null;
            factoryService.CreatePlaylist(name, description, tracks);
        }

        public void CreateTrackInstance(object? value)
        {
            var values = (object[])value;
            var path = (string)values[0];
            var name = (string)values[1];
            var description = (string)values[2];
            var artistIndex = (int?)values[3] ?? null;
            var playlistIndex = (int?)values[4] ?? null;
            factoryService.CreateTrack(path, name, description, artistIndex, playlistIndex);
        }

        public void DropInstance(ICoreEntity instance)
        {
            if (instance is Artist)
                ArtistInstance = (Artist)instance;
            if (instance is Playlist)
                PlaylistInstance = (Playlist)instance;
            if (instance is Track)
                TrackInstance = (Track)instance;
        }
    }
}
