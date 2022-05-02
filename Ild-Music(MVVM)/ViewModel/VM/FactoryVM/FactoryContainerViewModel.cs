using Ild_Music_MVVM_.Command;
using Ild_Music_MVVM_.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.ViewModel.VM.FactoryVM
{
    public class FactoryContainerViewModel : Base.BaseViewModel
    {
        #region Fields
        private SubControlService subControlService => (SubControlService)base.GetService("SubControlObserver");
        
        public ObservableCollection<UserControl> Factories { get; private set; }
        public UserControl CurrentFactory { get; set; }

        private CommandDelegater switchCommand;
        #endregion

        #region Constructor
        public FactoryContainerViewModel()
        {
            switchCommand = new CommandDelegater(null);
            Factories = new ObservableCollection<UserControl>(subControlService.UserSubControls);
            CurrentFactory = Factories[0];
        }
        #endregion

        #region Methods
        private void GenerateFactories() 
        {
            
        }
        #endregion


        #region Command methods
        private void SwitchFactories([Range(0,2)]int factoryIndex)
        {
            switch (factoryIndex)
            {
                case 0:
                    var artistFactory = Factories[0];
                    CurrentFactory = artistFactory;
                    break;
                case 1:
                    var playlistFactory = Factories[1];
                    CurrentFactory = playlistFactory;
                    break;
                case 2:
                    var trackFactory = Factories[2];
                    CurrentFactory = trackFactory;
                    break;
                default:
                    break;
            }
        }
        #endregion
    }


    public class FactoryEntityVM : Base.BaseViewModel
    {
        public UserControl UserControls { get; set; }
        public string Header { get; set; }

    }
}
