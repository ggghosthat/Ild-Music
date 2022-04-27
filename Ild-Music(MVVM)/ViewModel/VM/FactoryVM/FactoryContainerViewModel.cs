using Ild_Music_MVVM_.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.ViewModel.VM.FactoryVM
{
    public class FactoryContainerViewModel : Base.BaseViewModel
    {
        #region Fields
        public ObservableCollection<UserControl> Factories { get; set; } = new();
        public UserControl CurrentFactory { get; set; }

        private CommandDelegater switchCommand;
        #endregion

        #region Constructor
        public FactoryContainerViewModel()
        {
            switchCommand = new CommandDelegater(null);
        }
        #endregion

        #region Command methods
        private void SwitchFactories([Range(0,2)]int factoryIndex)
        {
            switch (factoryIndex)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
