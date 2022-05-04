using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacPlaylistSubControl : UserControl, IFactorySubControl
    {
        private FactoryViewModel FactoryViewModel;
        public string Header { get; init; } = "Playlist";
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
