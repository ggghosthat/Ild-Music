using System.Collections.ObjectModel;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    public class MainViewModel : Base.BaseViewModel
    {
        public ObservableCollection<Base.BaseViewModel> ViewModelsCollection { get; set; } = new ();
        public Base.BaseViewModel CurrenttViewModelItem { get; set; } = new StartViewModel();

        public MainViewModel() : base()
        {
        }
    }
}
