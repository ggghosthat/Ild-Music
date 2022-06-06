using Ild_Music_MVVM_.ViewModel.VM;

namespace Ild_Music_MVVM_.View
{
    public partial class StartWindow
    {
        ListViewModel ListViewModel = new ListViewModel();
        public StartWindow()
        {
            App.serviceCenter = new Services.ServiceCenter();
            
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void HomeSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.ContentHolder.Content = new StartViewModel();
        }

        private void PlaylistSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.ContentHolder.Content = ListViewModel.CallServiceAndCastLists(ListType.PLAYLISTS);
        }

        private void TracksSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.ContentHolder.Content = ListViewModel.CallServiceAndCastLists(ListType.TRACKS);
        }

        private void ArtistsSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.ContentHolder.Content = ListViewModel.CallServiceAndCastLists(ListType.ARTISTS);
        }
    }
}
