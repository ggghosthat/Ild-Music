using Ild_Music.Views;
using Ild_Music.ViewModels;
using Ild_Music.Core.Configure;
using Ild_Music.Core.Exceptions.Flag;
using Ild_Music.Core.Stage;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PropertyChanged;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ild_Music;

[DoNotNotifyAttribute]
public partial class App : Application
{
    public const string CONFIGURATION_FILE = "./config.json";

    private static Configure _configure;
    
    private static Stage _stage;

    private static List<ErrorFlag> _errors = [];

    public App()
    {}
    
    public static bool IsNormalBoot  { get; private set; } = false;

    public static Stage Stage => _stage;
    
    public static Configure Configure => _configure;

    public static Hashtable ViewModelTable { get; set; } = new();

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {    
            var splashScreen = new SplashScreen()
            {
                DataContext = new SplashScreenViewModel(),
            };

            desktop.MainWindow = splashScreen;
            splashScreen.Show();
            
            await Task.Run(() => StageBuildChainExecute());

            var mainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };

            desktop.MainWindow = mainWindow;
            mainWindow.Show();
            splashScreen.Close();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static bool ParseConfigurationFile()
    {
        _configure = new (CONFIGURATION_FILE);
        _configure.Parse();
        return _configure.CheckErrors(_errors);
    }

    private static bool BuildStage()
    {
        _stage = new (_configure);
        _stage.Build().Wait();
        return _stage.CheckErrors(_errors);
    }

    private static void StageBuildChainExecute()
    {
        if (ParseConfigurationFile() || BuildStage())
            IsNormalBoot = false;
        else 
            IsNormalBoot = true;
        
        PrepareViewModelTable();
    }


    private static void SuccededLoadingViewModelTableInitialization()
    {
        var instanceExplorerVm = new InstanceExplorerViewModel();
        App.ViewModelTable.Add(instanceExplorerVm.ViewModelId, instanceExplorerVm);
        var artistVm = new ArtistViewModel();
        App.ViewModelTable.Add(artistVm.ViewModelId, artistVm);
        var playlistVm = new PlaylistViewModel();
        App.ViewModelTable.Add(playlistVm.ViewModelId, playlistVm);
        var trackVm = new TrackViewModel();
        App.ViewModelTable.Add(trackVm.ViewModelId, trackVm);
        var tagVm = new TagViewModel();
        App.ViewModelTable.Add(tagVm.ViewModelId, tagVm);
        var artistEditorVm = new ArtistEditorViewModel();
        App.ViewModelTable.Add(artistEditorVm.ViewModelId, artistEditorVm);
        var playlistEditorVm = new PlaylistEditorViewModel();
        App.ViewModelTable.Add(playlistEditorVm.ViewModelId, playlistEditorVm);
        var trackEditorVm = new TrackEditorViewModel();
        App.ViewModelTable.Add(trackEditorVm.ViewModelId, trackEditorVm);
        var tagEditorVm = new TagEditorViewModel();
        App.ViewModelTable.Add(tagEditorVm.ViewModelId, tagEditorVm);
        var browserVm = new BrowserViewModel();
        App.ViewModelTable.Add(browserVm.ViewModelId, browserVm);
        var startVm = new StartViewModel();
        App.ViewModelTable.Add(startVm.ViewModelId, startVm);
        var listVm = new ListViewModel();
        App.ViewModelTable.Add(listVm.ViewModelId, listVm);
        var aboutVm = new AboutViewModel();
        App.ViewModelTable.Add(aboutVm.ViewModelId, aboutVm);
    }

    private static void CrashedLoadingViewModelTableInitialization()
    {
        var failedBootViewModel = new FailedBootViewModel();
        failedBootViewModel.SetErrors(_errors);
        App.ViewModelTable.Add(failedBootViewModel.ViewModelId, failedBootViewModel);
    }

    private static void PrepareViewModelTable()
    {
        if (IsNormalBoot)
            SuccededLoadingViewModelTableInitialization();
        else
            CrashedLoadingViewModelTableInitialization();
    }
}