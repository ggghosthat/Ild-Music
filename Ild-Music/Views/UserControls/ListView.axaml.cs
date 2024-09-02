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
    private const string TABBER_NAME = "Tabs";

    private static Border dropArea;
    private static Border dropPlace;
    private static ListBox tabber;
    private static IEnumerable<string> _placedFiles;

    public ListView()
    {
        InitializeComponent();
        dropArea = this.FindControl<Border>(DROP_AREA_BORRDER);
        dropPlace = this.FindControl<Border>(DROP_PLACE_BORRDER);

        tabber = this.FindControl<ListBox>(TABBER_NAME);

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

    private void TabberPointerPressed(object sender, PointerPressedEventArgs e)
    {
        var viewModel = (ListViewModel)DataContext;
        viewModel.DisplayProvidersAsync().Wait();
    }

    private void DropToViewModel()
    {
        _placedFiles = _placedFiles.GroupBy(f => f).Select(f=> f.Key);
        
        if (DataContext is ListViewModel viewModel)
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
