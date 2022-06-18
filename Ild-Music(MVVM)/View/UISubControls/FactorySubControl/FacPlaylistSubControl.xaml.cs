using System;
using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacPlaylistSubControl : UserControl, IFactorySubControl
    {
        private FactoryContainerViewModel FactoryViewModel;
        public string Header { get; init; } = "Playlist";
        public FacPlaylistSubControl()
        {
            InitializeComponent();
        }

        private void PlaylistFactoryClick(object sender, RoutedEventArgs e)
        {
            object[] values = { txtName.Text, txtDescription.Text, lvArtistsRoot.Items, };
            //FactoryViewModel.CreatePlaylistInstance(values);
        }
    }
}
