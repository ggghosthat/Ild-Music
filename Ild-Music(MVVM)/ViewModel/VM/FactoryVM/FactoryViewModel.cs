using Ild_Music_MVVM_.Command;
using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.VM;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    public class FactoryViewModel : Base.BaseViewModel
    {

        /// <summary>
        /// In this VM does not use CommandDelegator 'cause of some of reasons of bug
        /// maybe in the future it wil be fixed
        /// ~~~~~ keep calm ~~~~~ :)
        /// </summary>
        #region Fields
        private FactoryService factoryService => (FactoryService)base.GetService("Factory");
        

        #endregion

        #region constructor
        public FactoryViewModel()
        {
        }
        #endregion


        #region Private methods
        #endregion


        #region Command Methods

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
        #endregion


    }
}
