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
        App.ViewModelTable.Add(InstanceExplorerViewModel.nameVM, new InstanceExplorerViewModel());
        App.ViewModelTable.Add(ArtistViewModel.nameVM, new ArtistViewModel());
        App.ViewModelTable.Add(PlaylistViewModel.nameVM, new PlaylistViewModel());
        App.ViewModelTable.Add(TrackViewModel.nameVM, new TrackViewModel());
        App.ViewModelTable.Add(TagViewModel.nameVM, new TagViewModel());
        App.ViewModelTable.Add(ArtistEditorViewModel.nameVM, new ArtistEditorViewModel());
        App.ViewModelTable.Add(PlaylistEditorViewModel.nameVM, new PlaylistEditorViewModel());
        App.ViewModelTable.Add(TrackEditorViewModel.nameVM, new TrackEditorViewModel());
        App.ViewModelTable.Add(TagEditorViewModel.nameVM, new TagEditorViewModel());
        App.ViewModelTable.Add(BrowserViewModel.nameVM, new BrowserViewModel());
        App.ViewModelTable.Add(StartViewModel.nameVM, new StartViewModel());
        App.ViewModelTable.Add(ListViewModel.nameVM, new ListViewModel());
    }
}
