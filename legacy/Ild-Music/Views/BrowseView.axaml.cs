using Ild_Music.ViewModels;

using Avalonia.Interactivity;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using PropertyChanged;

namespace Ild_Music.Views;
[DoNotNotifyAttribute]
public partial class BrowseView : UserControl
{
    public BrowseView()
    {
        InitializeComponent();
        AddHandler(DragDrop.DropEvent, Drop);
    }

    private void Drop(object sender, DragEventArgs e)
    {
        if (e.Data.Contains(DataFormats.Files))
    	{    		
    		var vm = (BrowseViewModel)DataContext;
    		vm.Browse(e.Data.GetFileNames());
    	}
    }  

    private async void BrowseMusicFile(object sender, RoutedEventArgs args)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        
        var files = await topLevel?.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Your music file",
            AllowMultiple = true,
            FileTypeFilter = new[] { MusicAll }
        });
       if(files.Count >= 1)
       {}
    }
    
    private static FilePickerFileType MusicAll {get;} = new("Music all")
    {
        Patterns = new[] { "*.mp3" },
        MimeTypes = new[] { "audio/*" }
    };
}
