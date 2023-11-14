using Ild_Music.ViewModels;

using Avalonia;
using Avalonia.Input;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;
using System.Collections.ObjectModel;
using PropertyChanged;

namespace Ild_Music.Views
{
    [DoNotNotifyAttribute]
    public partial class StartView : UserControl
    {
        public StartView()
        {
            InitializeComponent();
        	AddHandler(DragDrop.DropEvent, Drop);
        }
        
	    private void Drop(object sender, DragEventArgs e)
	    {
	        if (e.Data.Contains(DataFormats.FileNames))
	    	{    		
	    		var vm = (StartViewModel)DataContext;
	    		vm.BrowseTracks(e.Data.GetFileNames());
	    	}
	    } 
    }
}