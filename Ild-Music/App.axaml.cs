using Ild_Music.ViewModels;
using Ild_Music.Views;
using Ild_Music.Core.Contracts;
using Ild_Music.Core.Configure;
using Ild_Music.Core.Stage;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PropertyChanged;
using System;
using System.Collections;

namespace Ild_Music;

[DoNotNotifyAttribute]
public partial class App : Application
{
    public static Hashtable ViewModelTable = new();
    public static IConfigure Configure;
    public static Stage Stage;

    public App()
    {
        Configure = new Configure("./config.json");
        Stage = new (ref Configure);
        Stage.Build().Wait();
        PopullateViewModelTable();
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

    private void PopullateViewModelTable()
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
    }
}
