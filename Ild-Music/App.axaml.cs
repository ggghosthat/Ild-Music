using Ild_Music;
using Ild_Music.ViewModels;
using Ild_Music.Views;
using ShareInstances.Services.Interfaces;
using ShareInstances.Services.Entities;
using ShareInstances.Services.Center;
using ShareInstances;
using ShareInstances.Stage;
using ShareInstances.Configure;

using System.Diagnostics;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PropertyChanged;
using System.Collections;

namespace Ild_Music
{
    [DoNotNotifyAttribute]
    public partial class App : Application
    {
        public static Hashtable ViewModelTable;
        public static IConfigure Configure;
        public static Stage Stage = new();

        public App()
        {
            //string playerPath = "E:/ild_music/Ild-Music/NAudioPlayerCore/bin/Debug/net6.0";
            //string areaPath = "E:/ild_music/Ild-Music/SynchronizationBlock/bin/Debug/net6.0";
            //Stage.Init(playerPath, areaPath);

            Configure = new Configure("Configuration/configuration.json");
            Stage.Init(Configure.Players, Configure.Synches);
            ViewModelTable = new Hashtable();
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
