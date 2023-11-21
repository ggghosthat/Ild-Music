using Ild_Music.ViewModels;

using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;
using Avalonia.Input.Raw;
using Avalonia.VisualTree;
using PropertyChanged;
using System;
using System.IO;
using System.Linq;

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
        if (e.Data.Contains(DataFormats.FileNames))
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
            FileTypeFilter = new("Music All") 
            {
                Patterns = new[] { "*.mp3" },
                MimeTypes = new[] { "audio/*" }
            }
        });
       if(files.Count >= 1)
       {}
    }
}
