using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.View.UISubControls.FactorySubControl;
using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;

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
        public static readonly string nameVM = "FactoryContainerVM";

        private TabHolderService TabHolderService => (TabHolderService)GetService("TabHolder");

        #region Fields
        public ObservableCollection<FactorySubControlTab> FactoryTabs { get; set; } = new ();
        public FactorySubControlTab CurrentFactory { get; set; }
        #endregion

        #region Constructor
        public FactoryContainerViewModel() =>
            InitializeSubControls();
        
            
        #endregion

        #region Private Methods
        private void InitializeSubControls() 
        {
            foreach (var tab in TabHolderService.TabHolder.Tabs)
                FactoryTabs.Add(tab);
        }
        #endregion

        #region Public Methods
        public void FreshTabs()
        {
            InitializeSubControls();
            CurrentFactory = FactoryTabs[0];
        }
        public void OrderSingleTab(FactorySubControlTab tab)
        {
            FactoryTabs.Clear();
            FactoryTabs.Add(tab);
            CurrentFactory = FactoryTabs[0];
        }
        #endregion
    }
}
