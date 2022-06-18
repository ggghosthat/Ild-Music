using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacArtistSubControl : UserControl, IFactorySubControl
    {
        private FactoryContainerViewModel FactoryViewModel;

        public string Header { get; init; } = "Artist";

        public FacArtistSubControl()
        {
            InitializeComponent();
        }

        private void ArtistFactoryClick(object sender, System.Windows.RoutedEventArgs e)
        {
            object[] values = { txtName.Text, txtDescription.Text};
            //FactoryViewModel.CreateArtistInstance(values);
        }
    }
}
