using Ild_Music_MVVM_.ViewModel.VM;

namespace Ild_Music_MVVM_.View
{
    public partial class StartWindow
    {
        public StartWindow()
        {            
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void HomeSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            ContentHolder.Content = new StartViewModel();

        private void PlaylistSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            ContentHolder.Content =  new ListViewModel(List.PLAYLISTS);

        private void TracksSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e) => 
            ContentHolder.Content = new ListViewModel(List.TRACKS);

        private void ArtistsSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e) => 
            ContentHolder.Content = new ListViewModel(List.ARTISTS);

        private void PlatformSliderClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            ContentHolder.Content = new StageViewModel();
    }
}
