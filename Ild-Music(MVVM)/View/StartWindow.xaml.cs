using Ild_Music_MVVM_.ViewModel.VM;

namespace Ild_Music_MVVM_.View
{
    public partial class StartWindow
    {
        public StartWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
