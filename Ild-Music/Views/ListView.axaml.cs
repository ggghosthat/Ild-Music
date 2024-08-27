using Ild_Music.ViewModels;

using System;
using Avalonia.Input;
using Avalonia.Controls;
using PropertyChanged;

namespace Ild_Music.Views;

[DoNotNotifyAttribute]
public partial class ListView : UserControl
{
    private bool isListenScrollEvent = true;
    public ListView()
    {
        InitializeComponent();

        AddHandler(DragDrop.DropOverEvent, ListView_DropOver);
        AddHandler(DragDrop.DragEvent, ListView_Drop);
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

    private void ListView_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.Contains(DataFormats.FileNames))
        {
            e.DragEffects = DragDropEffects.Copy;
            e.Handled = true;
        }
    }

    private void ListView_Drag(object sender, DragEventArgs e)
    {
        e.DragEffects = DragDropEffects.Copy;
        e.Handled = true;
    }

    private void ListView_DropOver(object sender, DragEventArgs e)
    {
        // Only allow Copy or Link as Drop Operations.
        e.DragEffects = e.DragEffects & (DragDropEffects.Copy | DragDropEffects.Link);

        // Only allow if the dragged data contains file names.
        if (!e.Data.Contains(DataFormats.FileNames))
            e.DragEffects = DragDropEffects.None;
    }

    private void ListView_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.Contains(DataFormats.FileNames))
        {
            var filePaths = e.Data.GetFileNames();
            var text = string.Join(Environment.NewLine, filePaths);
        }
    }
}
