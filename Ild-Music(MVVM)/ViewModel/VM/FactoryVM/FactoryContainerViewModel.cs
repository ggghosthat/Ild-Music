using Ild_Music_MVVM_.Command;
using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.View.UISubControls.FactorySubControl;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.ViewModel.VM.FactoryVM
{
    public class FactoryContainerViewModel : Base.BaseViewModel
    {
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
            Factories.Add(new FactorySubControlTab(new FacArtistSubControl(),"Artist"));
            Factories.Add(new FactorySubControlTab(new FacPlaylistSubControl(), "Playlist"));
            Factories.Add(new FactorySubControlTab(new FacTrackSubControl(), "Track"));
        }
        #endregion

        
    }
}
