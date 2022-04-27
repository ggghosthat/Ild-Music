using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacArtistSubControl : UserControl
    {
        private FactoryViewModel FactoryViewModel;

        public FacArtistSubControl()
        {
            InitializeComponent();
            FactoryViewModel = new FactoryViewModel();
            DataContext = FactoryViewModel;
        }

        private void ArtistFactoryClick(object sender, System.Windows.RoutedEventArgs e)
        {
            object[] values = { txtName.Text, txtDescription.Text};
            FactoryViewModel.CreateArtistInstance(values);
        }
    }
}
