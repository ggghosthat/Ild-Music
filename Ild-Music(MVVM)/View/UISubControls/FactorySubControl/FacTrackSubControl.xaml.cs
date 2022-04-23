using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacTrackSubControl : UserControl
    {
        //TODO: reset DataContext connection
        private FactoryViewModel factoryViewModel = new ();
        public FacTrackSubControl()
        {
            InitializeComponent();

            DataContext = factoryViewModel;
        }

        private void TrackFactoryClick(object sender, System.Windows.RoutedEventArgs e)
        {
            object[] values = { txtPath.Text, txtName.Text, txtDescription.Text, lvArtistsRoot.Items, lvPlaylistRoot.Items };

            factoryViewModel.CreateTrackInstance(values);
        }
    }
}
