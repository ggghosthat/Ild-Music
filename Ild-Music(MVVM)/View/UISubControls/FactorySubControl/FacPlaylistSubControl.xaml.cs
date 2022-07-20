using System.Windows;
using System.Linq;
using System.Windows.Controls;
using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.VM.FactoryVM;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacPlaylistSubControl : UserControl, IFactorySubControl
    {
        private SupporterService supporter;

        public string Header { get; init; } = "Playlist";
        public FacPlaylistSubControl()
        {
            InitializeComponent();
            CheckInstance();
        }


        private void CheckInstance()
        {
            var subControlVM = (SubControlViewModel)DataContext;
            supporter = subControlVM.Supporter;

            if (subControlVM.PlaylistInstance != null)
            {
                txtName.Text = subControlVM.PlaylistInstance.Name;
                txtDescription.Text = subControlVM.PlaylistInstance.Description;

                foreach (var artist in supporter.ArtistSup)
                {
                    artist.Playlists.ToList().ForEach(p => 
                    {
                        if (p.Id.Equals(subControlVM.PlaylistInstance.Id))
                            lvArtistsRoot.Items.Add(artist.Name);
                    });
                }
            }
        }

        private void PlaylistFactoryClick(object sender, RoutedEventArgs e)
        {
            var subControlVM = (SubControlViewModel)DataContext;
            object[] values = { txtName.Text, txtDescription.Text, lvArtistsRoot.Items, };
            subControlVM.CreatePlaylistInstance(values);
        }
    }
}
