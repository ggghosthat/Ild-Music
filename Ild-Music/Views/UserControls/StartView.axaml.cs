using Ild_Music.Contracts;
using Ild_Music.ViewModels;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Ild_Music.Views;

[DoNotNotifyAttribute]
public partial class StartView : UserControl
{
    
    private const string DROP_AREA_BORRDER = "DropArea";
    private const string DROP_PLACE_BORRDER = "DropPlace";
    private static Border dropArea;
    private static Border dropPlace;

    private static IEnumerable<string> _placedFiles;
    public StartView()
    {
        InitializeComponent();
        dropArea = this.FindControl<Border>(DROP_AREA_BORRDER);
        dropPlace = this.FindControl<Border>(DROP_PLACE_BORRDER);

        AddHandler(DragDrop.DropEvent, ListView_DragLeave);
        AddHandler(DragDrop.DragEnterEvent, ListView_DragOver);
        AddHandler(DragDrop.DragLeaveEvent, ListView_DragLeave);
    }

    private void ListView_DragOver(object sender, DragEventArgs e)
    {
        dropArea.IsVisible = true;
        dropArea.Cursor = new Cursor(StandardCursorType.Hand);
        e.DragEffects = e.DragEffects & (DragDropEffects.Copy | DragDropEffects.Link);

        if (!e.Data.Contains(DataFormats.FileNames))
            e.DragEffects = DragDropEffects.None;

        _placedFiles = GetFiles(e);
        DropToViewModel();
    }

    private void ListView_DragLeave(object sender, DragEventArgs e)
    {
        dropArea.IsVisible = false;
        _placedFiles = Enumerable.Empty<string>();
    }

    private void DropAreaReleaseMouse(object sender, PointerPressedEventArgs e)
    {
        dropArea.IsVisible = false;
    }

    private void DropToViewModel()
    {
        _placedFiles = _placedFiles.GroupBy(f => f).Select(f=> f.Key);
        
        if (DataContext is StartViewModel viewModel)
            viewModel.DropFiles(_placedFiles);
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