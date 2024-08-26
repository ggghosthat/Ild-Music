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

    private void ListView_DragOver(object sender, DragEventArgs e)
    {
        e.DragEffects = DragDropEffects.Copy;
        e.Handled = true;
    }

    private void ListView_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.Contains(DataFormats.FileNames))
        {
            var files = e.Data.GetFileNames();
            foreach (var file in files)
            {

            }
            e.Handled = true;
        }
    }
}
