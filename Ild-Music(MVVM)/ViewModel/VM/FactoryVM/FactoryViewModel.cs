using Ild_Music_MVVM_.Command;
using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.VM;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    public class FactoryViewModel : Base.BaseViewModel
    {
        #region Fields
        private FactoryService factoryService => (FactoryService)base.GetService("Factory");
        

        private CommandDelegater CreateInstance;
        #endregion

        #region constructor
        public FactoryViewModel()
        {
        }
        #endregion


        #region Private methods
        #endregion


        #region Command Methods

        public void CreateArtistInstance(object values)
        {
            //string name, string decription = null
            //factoryService.CreateArtist(name, decription);
        }
        public void CreatePlaylistInstance(object values)
        {
            //string name, string decription = null, IList< object > lsTracks = null
            //factoryService.CreatePlaylist(name, decription);
        }

        public void CreateTrackInstance(object values)
        {
            //string path, string name, string decription = null, int? artistIndex = null
            //factoryService.CreateTrack(path, name, decription, artistIndex);
        }
        #endregion


    }
}
