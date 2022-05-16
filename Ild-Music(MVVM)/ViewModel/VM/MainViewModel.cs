using Ild_Music_MVVM_.Command;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    public class MainViewModel : Base.BaseViewModel
    {
        public ObservableCollection<Base.BaseViewModel> ViewModelsCollection { get; set; } = new ();
        public Base.BaseViewModel CurrenttViewModelItem { get; set; } = new StartViewModel();


        public CommandDelegater HoverUpCommand { get; }
        public CommandDelegater HoverDownCommand { get; }
        public MainViewModel() : base()
        {
            HoverUpCommand = new CommandDelegater(HoverUp);
            HoverDownCommand = new CommandDelegater(HoverDown);
        }



        private void HoverUp(object input)
        {
            if (input is StackPanel stack)
                stack.Background = new SolidColorBrush(Color.FromRgb(25, 31, 158));
        }
        private void HoverDown(object input)
        {
            if (input is StackPanel stack)
                stack.Background = new SolidColorBrush(Color.FromRgb(31, 38, 178));
        }
        
    }
}
