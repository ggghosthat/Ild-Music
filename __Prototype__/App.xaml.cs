using System.Windows;
using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.VM;
using MainStage.Stage;

namespace Ild_Music_MVVM_
{
    /// <summary>
    /// While application lo loading 
    /// it initialize Platform instance and critical important services
    /// after finishing thread locking it starts side services initialization asynchronously.
    /// These instances are general for app work.
    /// </summary>
    public partial class App : Application
    {

        public static Platform mainPlatform = new Platform().CallPlatform();
        public static ServiceCenter serviceCenter = new();
        internal static ViewModelHolder vmHolder = new();

    }
}
