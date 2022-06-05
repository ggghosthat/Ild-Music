using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UIControls
{
    public partial class ListControl : UserControl
    {
        public ListControl()
        {
            InitializeComponent();
            DataContext = new ListViewModel();
            
        }

    }
}
