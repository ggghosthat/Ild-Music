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


        private void SwitchViewModel(ViewModelCollection collection)
        {
            switch (collection)
            {
                case ViewModelCollection.START:
                    break;
                case ViewModelCollection.LIST:
                    break;
                case ViewModelCollection.TRACK:
                    break;
                case ViewModelCollection.FACTORY:
                    break;
                default:
                    break;
            }
        }
    }
}
