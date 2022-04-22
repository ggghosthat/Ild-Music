using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public partial class FacContainerSubControl : UserControl
    {
        private FactoryViewModel factoryVM = new FactoryViewModel();
        public FacContainerSubControl()
        {
            InitializeComponent();
            DataContext = factoryVM;
        }
    }
}
