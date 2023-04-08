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
        AddHandler(DragDrop.DragOverEvent, DragOver);
    }

    private void DragOver(object sender, DragEventArgs e)
    {}

    private void Drop(object sender, DragEventArgs e)
    {
        if (e.Data.Contains(DataFormats.FileNames))
    	{    		
    		var vm = (BrowseViewModel)DataContext;
    		vm.Browse(e.Data.GetFileNames());
    	}
    }   
}