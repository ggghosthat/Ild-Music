using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows.Forms;
using System.Windows.Media;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacTrackSubControl : System.Windows.Controls.UserControl, IFactorySubControl
    {
        private FactoryContainerViewModel FactoryViewModel;
        public string Header { get; init; } = "Track";
        public FacTrackSubControl()
        {
            InitializeComponent();

            txtPath.Foreground = (Brush)new BrushConverter().ConvertFrom("#7e8f8a");
            txtPath.Text = "Click twice to select your track.";
        }

        private void TrackFactoryClick(object sender, System.Windows.RoutedEventArgs e)
        {
            object[] values = { txtPath.Text, txtName.Text, txtDescription.Text, lvArtistsRoot.Items, lvPlaylistRoot.Items };

            //FactoryViewModel.CreateTrackInstance(values);
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
    }
}
