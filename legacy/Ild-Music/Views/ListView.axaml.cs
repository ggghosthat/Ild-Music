using Ild_Music.ViewModels;

using Avalonia;
using Avalonia.Input;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace Ild_Music.Views
{
    [DoNotNotifyAttribute]
    public partial class ListView : UserControl
    {
        public ListView()
        {
            InitializeComponent();
        	AddHandler(DragDrop.DropEvent, Drop);
        }

        private void Drop(object sender, DragEventArgs e)
	    {
	        if (e.Data.Contains(DataFormats.FileNames))
	    	{    		
	    		var vm = (ListViewModel)DataContext;
	    		vm.BrowseTracks(e.Data.GetFileNames());
	    	}
	    } 
    }
}