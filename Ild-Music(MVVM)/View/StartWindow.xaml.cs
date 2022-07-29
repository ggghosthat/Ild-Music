using System.Windows;
using Ild_Music_MVVM_.ViewModel.VM;

namespace Ild_Music_MVVM_.View
{
    public partial class StartWindow
    {
        private MainViewModel mainViewModel = new();
        public StartWindow()
        {            
            InitializeComponent();
            DataContext = mainViewModel;
        }

        private void HomeSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            mainViewModel.SetVM(new StartViewModel());

        private void PlaylistSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            mainViewModel.SetVM(new ListViewModel(List.PLAYLISTS));

        private void TracksSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            mainViewModel.SetVM(new ListViewModel(List.TRACKS));

        private void ArtistsSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            mainViewModel.SetVM(new ListViewModel(List.ARTISTS));

        private void PlatformSliderClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            mainViewModel.SetVM(new StageViewModel());





        private void btnMinimizeWindow_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            WindowState = WindowState.Minimized;
        

        private void btnCloseWindow_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            Application.Current.Shutdown();
    }
}
