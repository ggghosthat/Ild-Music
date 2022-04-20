using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UIControls
{
    public partial class FactoryConrol : UserControl
    {
        public FactoryConrol()
        {
            InitializeComponent();
            DataContext = new FactoryViewModel();
        }
    }
}
