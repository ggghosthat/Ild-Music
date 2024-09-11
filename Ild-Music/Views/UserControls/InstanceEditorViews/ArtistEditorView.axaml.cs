using Ild_Music.ViewModels;

using System;
using System.Linq;
using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using PropertyChanged;

namespace Ild_Music.Views;

[DoNotNotifyAttribute]
public partial class ArtistEditorView : UserControl
{
    public ArtistEditorView()
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

        if (DataContext is ArtistEditorViewModel artistEditorVM && files.Count() > 0)
            artistEditorVM.SelectAvatar(files.Select(x => x.Path.LocalPath).First());
    }
}
