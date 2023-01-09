using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UIControls
{
    public partial class ListControl : UserControl
    {
        private ViewModelHolder vmHolder => App.vmHolder;

        public ListControl()
        {
            InitializeComponent();
            vmHolder.OnViewModelUpdate += () => DataContext = vmHolder.GetViewModel(ListViewModel.nameVM);
        }

    }
}
