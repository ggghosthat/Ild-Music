using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.View.UISubControls.FactorySubControl;
using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

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
        }

        public FactoryContainerViewModel(ICoreEntity instance)
        {
            InitializeSubControls(); 
            subControlService.DropInstance(instance);

            
        }
        #endregion

        #region Private Methods
        private void InitializeSubControls() 
        {
            Factories.Add(new FactorySubControlTab(subControlService.UserSubControls[0], "Artist"));
            Factories.Add(new FactorySubControlTab(subControlService.UserSubControls[1], "Playlist"));
            Factories.Add(new FactorySubControlTab(subControlService.UserSubControls[2], "Track"));
        }
        #endregion

        #region Public Methods
        public void DisplayInstance(ICoreEntity instance)
        {
            if (instance is Artist)
                CurrentFactory = Factories[0];
            if (instance is Playlist)
                CurrentFactory = Factories[1];
            if (instance is Track)
                CurrentFactory = Factories[2];
        }
        #endregion
    }
}
