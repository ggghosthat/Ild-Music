using System.Windows;
using System.Windows.Controls;
using Ild_Music_MVVM_.ViewModel.VM.FactoryVM;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacPlaylistSubControl : UserControl, IFactorySubControl
    {
        public string Header { get; init; } = "Playlist";
        public FacPlaylistSubControl()
        {
            InitializeComponent();
        }

        private void PlaylistFactoryClick(object sender, RoutedEventArgs e)
        {
            var subControlVM = (SubControlViewModel)DataContext;
            object[] values = { txtName.Text, txtDescription.Text, lvArtistsRoot.Items, };
            subControlVM.CreatePlaylistInstance(values);
        }
    }
}
