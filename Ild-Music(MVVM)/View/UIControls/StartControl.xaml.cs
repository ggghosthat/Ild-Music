using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UIControls
{
    public partial class StartControl : UserControl
    {
        private ViewModelHolderService vmHolder => (ViewModelHolderService)App.serviceCenter.GetService("VMHolder");

        public StartControl()
        {
            InitializeComponent();
            DataContext = vmHolder.GetViewModel("StartVM");
        }

        private void lsTracks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
