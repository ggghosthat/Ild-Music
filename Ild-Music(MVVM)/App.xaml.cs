using System.Windows;
using Ild_Music_MVVM_.Services;
using MainStage.Stage;

namespace Ild_Music_MVVM_
{ 
    public partial class App : Application
    {
        public static readonly ServiceCenter serviceCenter = new();
        public static readonly Platform mainPlatform = new ();
    }
}
