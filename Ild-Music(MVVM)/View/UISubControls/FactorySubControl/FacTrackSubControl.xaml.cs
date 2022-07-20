using System.Linq;
using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.VM.FactoryVM;
using System.Windows.Forms;
using System.Windows.Media;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacTrackSubControl : System.Windows.Controls.UserControl, IFactorySubControl
    {
        private SupporterService supporter;

        public string Header { get; init; } = "Track";

        public FacTrackSubControl()
        {
            InitializeComponent();
            CheckInstance();
        }


        private void CheckInstance()
        {
            var subControlVM = (SubControlViewModel)DataContext;
            supporter = subControlVM.Supporter;

            if (subControlVM.TrackInstance != null)
            {
                txtPath.Text = subControlVM.TrackInstance.Pathway;
                txtName.Text = subControlVM.TrackInstance.Name;
                txtDescription.Text = subControlVM.TrackInstance.Description;

                foreach (var artist in supporter.ArtistSup)
                {
                    artist.Tracks.ToList().ForEach(t =>
                    {
                        if (t.Id.Equals(subControlVM.TrackInstance.Id))
                            lvArtistsRoot.Items.Add(artist.Name);
                    });
                }


                foreach (var playlist in supporter.PlaylistSup)
                {
                    playlist.Tracks.ToList().ForEach(t =>
                    {
                        if (t.Id.Equals(subControlVM.TrackInstance.Id))
                            lvPlaylistsRoot.Items.Add(playlist.Name);
                    });
                }
            }
            else
            {
                txtPath.Foreground = (Brush)new BrushConverter().ConvertFrom("#7e8f8a");
                txtPath.Text = "Click twice to select your track.";
            }
        }




        private void Path2Track(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenFileDialog dialog = new();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtPath.Foreground = (Brush)new BrushConverter().ConvertFrom("AliceBlue");
                txtPath.Text = dialog.FileName;
            }
        }




        private void TrackFactoryClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var subControlVM = (SubControlViewModel)DataContext;

            object[] values = { txtPath.Text, txtName.Text, txtDescription.Text, lvArtistsRoot.SelectedIndex, lvPlaylistsRoot.SelectedIndex};
            
            subControlVM.CreateTrackInstance(values);
        }
    }
}
