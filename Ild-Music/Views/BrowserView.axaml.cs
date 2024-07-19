using Ild_Music.ViewModels;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using PropertyChanged;
using System.Linq;

namespace Ild_Music.Views;

[DoNotNotifyAttribute]
public partial class BrowserView : UserControl
{
    public BrowserView()
    {
        InitializeComponent();
    }

    private async void OpenMusicFiles_Clicked(object sender, TappedEventArgs args)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select your music file.",
            AllowMultiple = true
        });

        if (DataContext is BrowserViewModel browserVM && files.Count > 0)
            await browserVM.Browse(files.Select(x => x.Path.LocalPath).ToList());
    }
}
