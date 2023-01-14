using Ild_Music;
using Ild_Music.ViewModels;
using Ild_Music.Views;
using ShareInstances.Services.Interfaces;
using ShareInstances.Services.Entities;
using ShareInstances.Services.Center;
using ShareInstances;
using ShareInstances.Stage;
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
        public static Stage Stage = new Stage();

        public App()
        {
            string playerPath = "E:/ild_music/Ild-Music/Ild-Music-Core/bin/Debug/net6.0";
            string areaPath = "E:/ild_music/Ild-Music/SynchronizationBlock/bin/Debug/net6.0";
            Stage.Init(playerPath, areaPath);
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