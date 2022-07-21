using Ild_Music_MVVM_.ViewModel.VM.FactoryVM;
using System.Windows.Controls;
using System;
using ShareInstances.PlayerResources;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacArtistSubControl : UserControl, IFactorySubControl
    {
        #region  Properties
        public string Header { get; init; } = "Artist";
        #endregion

        #region Events
        private event Action OnCheckInstance;
        #endregion

        #region Const
        public FacArtistSubControl()
        {
            InitializeComponent();
            OnCheckInstance += CheckInstance;
        }
        #endregion

        #region Private Methods
        private void CheckInstance() 
        {
            var subControlVM = (SubControlViewModel)DataContext;

            if(subControlVM.Instance is Artist artistInstance)
            {
                txtName.Text = artistInstance.Name;
                txtDescription.Text = artistInstance.Description;
            }
        }

        private void ArtistFactoryClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var subControlVM = (SubControlViewModel)DataContext;

            object[] values = { txtName.Text, txtDescription.Text};
            subControlVM.CreateArtistInstance(values);
        }
        #endregion

        #region Public Methods
        public void InvokeCheckInstance() =>
            OnCheckInstance?.Invoke();
        #endregion
    }
}
