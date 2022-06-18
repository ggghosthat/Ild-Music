using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.View.UISubControls.FactorySubControl;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    /// <summary>
    /// In this VM does not use CommandDelegator in some bugs reasons 
    /// maybe in the future it wil be fixed
    /// ~~~~~ keep calm ~~~~~ :)
    /// ---Using For Instance generating processes---
    /// </summary>
    public class FactoryContainerViewModel : Base.BaseViewModel
    {
        private SubControlService subControlService => (SubControlService)GetService("SubControlObserver");

        #region Fields
        public ObservableCollection<FactorySubControlTab> Factories { get; set; } = new ();
        public FactorySubControlTab CurrentFactory { get; set; }
        #endregion

        #region Constructor
        public FactoryContainerViewModel()
        {
            InitializeSubControls();
            CurrentFactory = Factories[0];
        }
        #endregion

        #region Methods
        private void InitializeSubControls() 
        {
            Factories.Add(new FactorySubControlTab(subControlService.UserSubControls[0], "Artist"));
            Factories.Add(new FactorySubControlTab(subControlService.UserSubControls[1], "Playlist"));
            Factories.Add(new FactorySubControlTab(subControlService.UserSubControls[2], "Track"));
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
        #endregion


    }
}
