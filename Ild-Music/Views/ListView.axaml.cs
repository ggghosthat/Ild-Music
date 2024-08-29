using Ild_Music.ViewModels;
using Ild_Music.Contracts;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Input;
using Avalonia.Controls;
using PropertyChanged;

namespace Ild_Music.Views;

[DoNotNotifyAttribute]
public partial class ListView : UserControl
{
    private bool isListenScrollEvent = true;

    private const string DROP_AREA_BORRDER = "DropArea";
    private const string DROP_PLACE_BORRDER = "DropPlace";

    private static Border dropArea;
    private static Border dropPlace;

    private static IEnumerable<string> _placedFiles;

    public ListView()
    {
        InitializeComponent();
        dropArea = this.FindControl<Border>(DROP_AREA_BORRDER);
        dropPlace = this.FindControl<Border>(DROP_PLACE_BORRDER);

        AddHandler(DragDrop.DropEvent, ListView_DragLeave);
        AddHandler(DragDrop.DragEnterEvent, ListView_DragOver);
        AddHandler(DragDrop.DragLeaveEvent, ListView_DragLeave);
    }

    private void OnScrollChanged(object sender, PointerWheelEventArgs e)
    {
        if (Math.Abs(e.Delta.Y) == e.Delta.Length && e.Delta.Y < 0 && isListenScrollEvent)
        {
            isListenScrollEvent = false;
            ((ListViewModel)DataContext).ExtendCurrentList().Wait();
            isListenScrollEvent = true;
        }
    }

    private void ListView_DragOver(object sender, DragEventArgs e)
    {
        dropArea.IsVisible = true;
        dropArea.Cursor = new Cursor(StandardCursorType.Hand);
        e.DragEffects = e.DragEffects & (DragDropEffects.Copy | DragDropEffects.Link);

        if (!e.Data.Contains(DataFormats.FileNames))
            e.DragEffects = DragDropEffects.None;

        _placedFiles = GetFiles(e);
        _placedFiles.OrderBy(x => x);
    }

    private void ListView_DragLeave(object sender, DragEventArgs e)
    {
        dropArea.IsVisible = false;
        _placedFiles = Enumerable.Empty<string>();
    }

    private void DropAreaReleaseMouse(object sender, PointerPressedEventArgs e)
    {
        _placedFiles = _placedFiles.GroupBy(f => f).Select(f => f.Key);

        if (DataContext is IFileDropable fileDropable)
            fileDropable.DropFiles(_placedFiles);

        dropArea.IsVisible = false;
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
