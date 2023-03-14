using Ild_Music_MVVM_.Services.Parents;
using Ild_Music_MVVM_.ViewModel.VM;

namespace Ild_Music_MVVM_.Services
{
    public class MainWindowService : Service
    {
        public override string ServiceType { get; init; } = "MainWindowAPI";

        private MainViewModel mainVM;

        public MainViewModel MainWindow 
        {
            get => mainVM;
            set 
            {
                if (mainVM == null)
                    mainVM = value;
            } 
        }
    }
}
