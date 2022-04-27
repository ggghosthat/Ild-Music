using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacPlaylistSubControl : UserControl
    {
        private FactoryViewModel FactoryViewModel;
        public FacPlaylistSubControl()
        {
            InitializeComponent();
            this.FactoryViewModel = new FactoryViewModel();
            DataContext = this.FactoryViewModel;
        }

        private void PlaylistFactoryClick(object sender, RoutedEventArgs e)
        {
            object[] values = { txtName.Text, txtDescription.Text, lvArtistsRoot.Items, };
            FactoryViewModel.CreatePlaylistInstance(values);
        }
    }
}
