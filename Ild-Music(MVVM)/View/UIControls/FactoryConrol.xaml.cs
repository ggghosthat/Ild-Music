using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UIControls
{
    public partial class FactoryConrol : UserControl
    {
        private FactoryService factoryService = (FactoryService)App.serviceCenter.GetService("Factory");
        public FactoryConrol()
        {
            InitializeComponent();
            DataContext = factoryService.FactoryContainerViewModel;
        }
    }
}
