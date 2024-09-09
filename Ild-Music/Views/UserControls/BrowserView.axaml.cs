using Ild_Music.ViewModels;

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using PropertyChanged;
using System.Linq;
using System.Collections.Generic;

namespace Ild_Music.Views;

[DoNotNotifyAttribute]
public partial class BrowserView : UserControl
{
    private const string DROP_AREA_BORRDER = "BrowseArea";
    private const string DROP_PLACE_BORRDER = "DropPlace";
    private static Border dropArea;
    private static Border dropPlace;
    
    private static IEnumerable<string> _placedFiles;

    public BrowserView()
    {
        InitializeComponent();
        
        dropArea = this.FindControl<Border>(DROP_AREA_BORRDER);
        dropPlace = this.FindControl<Border>(DROP_PLACE_BORRDER);

        AddHandler(DragDrop.DropEvent, ListView_DragLeave);
        AddHandler(DragDrop.DragEnterEvent, ListView_DragOver);
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

    private void ListView_DragOver(object sender, DragEventArgs e)
    {
        dropArea.IsVisible = true;
        dropArea.Cursor = new Cursor(StandardCursorType.Hand);
        e.DragEffects = e.DragEffects & (DragDropEffects.Copy | DragDropEffects.Link);

        if (!e.Data.Contains(DataFormats.FileNames))
            e.DragEffects = DragDropEffects.None;

        _placedFiles = GetFiles(e);
    }

    private void ListView_DragLeave(object sender, DragEventArgs e)
    {
        dropArea.IsVisible = false;
        DropToViewModel();
        _placedFiles = Enumerable.Empty<string>();
    }

    private void DropAreaReleaseMouse(object sender, PointerPressedEventArgs e)
    {
        dropArea.IsVisible = false;
    }

    private void DropToViewModel()
    {
        _placedFiles = _placedFiles.GroupBy(f => f).Select(f=> f.Key);
        
        if (DataContext is BrowserViewModel viewModel)
            viewModel.Browse(_placedFiles);
    }

    private static IEnumerable<string> GetFiles(DragEventArgs dragEvent)
    {
        var filePaths = dragEvent.Data.GetFileNames();

        if (filePaths.Count() > 0)
            return filePaths;
        else 
            return Enumerable.Empty<string>();
    }
}
