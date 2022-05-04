using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacTrackSubControl : UserControl, IFactorySubControl
    {
        private FactoryViewModel FactoryViewModel;
        public string Header { get; init; } = "Track";
        public FacTrackSubControl()
        {
            InitializeComponent();
            this.FactoryViewModel = new FactoryViewModel();
            DataContext = FactoryViewModel;
        }

        private void TrackFactoryClick(object sender, System.Windows.RoutedEventArgs e)
        {
            object[] values = { txtPath.Text, txtName.Text, txtDescription.Text, lvArtistsRoot.Items, lvPlaylistRoot.Items };

            FactoryViewModel.CreateTrackInstance(values);
        }
    }
}
