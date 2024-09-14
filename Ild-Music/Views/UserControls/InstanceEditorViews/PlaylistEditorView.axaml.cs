using Ild_Music.ViewModels;

using System;
using System.Linq;
using Avalonia.Input;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using PropertyChanged;

namespace Ild_Music.Views;

[DoNotNotifyAttribute]
public partial class PlaylistEditorView : UserControl
{
    public PlaylistEditorView()
    {
        InitializeComponent();
    }

    private async void OpenAvatar_Clicked(object sender, TappedEventArgs args)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select music avatar.",
            AllowMultiple = false
        });

        if (DataContext is PlaylistEditorViewModel playlistEditorVM && files.Count() > 0)
            playlistEditorVM.SelectAvatar(files.Select(x => x.Path.LocalPath).First());
    }
}
