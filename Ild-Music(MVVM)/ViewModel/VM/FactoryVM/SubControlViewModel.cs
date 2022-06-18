using Ild_Music_MVVM_.Services;
using System;
using System.Collections.Generic;

namespace Ild_Music_MVVM_.ViewModel.VM.FactoryVM
{
    public class SubControlViewModel : Base.BaseViewModel
    {
        private FactoryService factoryService => (FactoryService)GetService("Factory");

        public static IList<Action<object>> SubConrolActions { get; private set; } = new List<Action<object>>();


        public SubControlViewModel()
        {
        }


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

        public void CreateTrackInstance(object[] values)
        {
            var path = (string)values[0];
            var name = (string)values[1];
            var description = (string)values[2];
            var artistIndex = (int?)values[3] ?? null;
            factoryService.CreateTrack(path, name, description, artistIndex);
        }
    }
}
