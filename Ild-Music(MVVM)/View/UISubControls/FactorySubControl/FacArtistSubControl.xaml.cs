using Ild_Music_MVVM_.ViewModel.VM.FactoryVM;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacArtistSubControl : UserControl, IFactorySubControl
    {
        public string Header { get; init; } = "Artist";

        public FacArtistSubControl()
        {
            InitializeComponent();
        }

        private void ArtistFactoryClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var subControlVM = (SubControlViewModel)DataContext;

            object[] values = { txtName.Text, txtDescription.Text};
            subControlVM.CreateArtistInstance(values);
        }
    }
}
