using Ild_Music_MVVM_.ViewModel.VM.FactoryVM;
using System.Windows.Forms;
using System.Windows.Media;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacTrackSubControl : System.Windows.Controls.UserControl, IFactorySubControl
    {
        public string Header { get; init; } = "Track";

        public FacTrackSubControl()
        {
            InitializeComponent();

            TrackPathPrecolor();
        }

        private void TrackPathPrecolor()
        {
            txtPath.Foreground = (Brush)new BrushConverter().ConvertFrom("#7e8f8a");
            txtPath.Text = "Click twice to select your track.";
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
