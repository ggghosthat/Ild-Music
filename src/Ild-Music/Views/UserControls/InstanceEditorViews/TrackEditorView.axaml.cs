using Ild_Music.ViewModels;

using System;
using System.Linq;
using Avalonia.Input;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using PropertyChanged;

namespace Ild_Music.Views;

[DoNotNotifyAttribute]
public partial class TrackEditorView : UserControl
{
    public TrackEditorView()
    {
        InitializeComponent();
    }

    private async void OpenMusicFile_Clicked(object sender, TappedEventArgs args)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select your music file.",
            AllowMultiple = false
        });

        if (DataContext is TrackEditorViewModel trackEditorVM && files.Count() > 0)
            trackEditorVM.DefineTrackPath(files.Select(x => x.Path.LocalPath).First());
    }

    private async void OpenAvatar_Clicked(object sender, TappedEventArgs args)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select music avatar.",
            AllowMultiple = false
        });

        if (DataContext is TrackEditorViewModel trackEditorVM && files.Count() > 0)
            trackEditorVM.SelectAvatar(files.Select(x => x.Path.LocalPath).First());
    }
}
