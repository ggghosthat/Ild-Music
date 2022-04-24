using Ild_Music_MVVM_.ViewModel.Base;
using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacArtistSubControl : UserControl
    {
        //TODO: reset DataContext connection
        private FactoryViewModel factoryViewModel;
        public FacArtistSubControl()
        {
            InitializeComponent();
            DataContext = factoryViewModel;
        }

        private void ArtistFactoryClick(object sender, System.Windows.RoutedEventArgs e)
        {
            object[] values = { txtName.Text, txtDescription.Text};
            factoryViewModel.CreateArtistInstance(values);
        }
    }
}
