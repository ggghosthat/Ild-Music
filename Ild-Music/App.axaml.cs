using Ild_Music.ViewModels;
using Ild_Music.Views;
using Ild_Music.Core.Contracts;
using Ild_Music.Core.Configure;
using Ild_Music.Core.Stage;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PropertyChanged;
using System.Collections;

namespace Ild_Music;

[DoNotNotifyAttribute]
public partial class App : Application
{
    public static Hashtable ViewModelTable;
    public static IConfigure Configure;
    public static Stage Stage;

    public App()
    {
        Configure = new Configure("./config.json");
        Stage = new (ref Configure);
        Stage.Build().Wait();
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
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}