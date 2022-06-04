using Ild_Music_MVVM_.Command;
using System.Collections.ObjectModel;
using Ild_Music_MVVM_.View;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    public class MainViewModel : Base.BaseViewModel
    {
        public ObservableCollection<Base.BaseViewModel> ViewModelsCollection { get; set; } = new ();
        public Base.BaseViewModel CurrenttViewModelItem { get; set; } = new StartViewModel();


        public CommandDelegater SlideItemsClickCommand { get; }
        public MainViewModel() : base()
        {

        }


        private void SlideItemsClick() 
        {
            StartWindow1 window = new StartWindow1();
           
        }
        
    }
}
