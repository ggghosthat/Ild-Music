using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacPlaylistSubControl : UserControl
    {
        //TODO: reset DataContext connection
        private FactoryViewModel factoryViewModel;
        public FacPlaylistSubControl(FactoryViewModel factoryViewModel)
        {
            InitializeComponent();
            this.factoryViewModel = factoryViewModel;
            DataContext = factoryViewModel;
        }

        private void PlaylistFactoryClick(object sender, RoutedEventArgs e)
        {
            object[] values = { txtName.Text, txtDescription.Text, lvArtistsRoot.Items, };
            factoryViewModel.CreatePlaylistInstance(values);
        }
    }
}
