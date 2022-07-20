using System.Windows;
using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.Base;
using MainStage.Stage;

namespace Ild_Music_MVVM_
{ 
    public partial class App : Application
    {
        public static Platform mainPlatform = new Platform().CallPlatform();
        public static ServiceCenter serviceCenter = new();
    }
}
