using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UIControls
{
    public partial class ListControl : UserControl
    {
        private ViewModelHolderService vmHolder => (ViewModelHolderService)App.serviceCenter.GetService("VMHolder");

        public ListControl()
        {
            InitializeComponent();
            DataContext = vmHolder.GetViewModel(ListViewModel.NameVM) ?? new ListViewModel();
        }

    }
}
