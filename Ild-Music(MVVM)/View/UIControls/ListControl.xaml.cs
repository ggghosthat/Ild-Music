using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UIControls
{
    public partial class ListControl : UserControl
    {
        ListViewModel listVM = new();
        public ListControl()
        {
            InitializeComponent();
            DataContext = listVM;
        }

    }
}
