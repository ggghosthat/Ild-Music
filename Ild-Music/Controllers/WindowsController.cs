using Ild_Music.UI;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Ild_Music
{
    public enum Slider 
    {
        HOME,
        PLAYLISTS,
        TRACKS,
        ARTISTS,
        ADD_ARTIST,
        ADD_TRACK,
        ADD_PLAYLIST
    }

    public class WindowsController
    {
        private Window currentWindow;

        private static IDictionary<Slider,Window> windows = new Dictionary<Slider, Window>();

        public IDictionary<Slider, Window> Windows => windows;



        public WindowsController(Window currentWindow)
        {
            this.currentWindow = currentWindow;
        }



        public void AddWindow(Slider slider, Window window)
        {
            windows.Add(slider, window);
        }

        public void ClickDetector(Slider slider, Window _currentWindow)
        {
            switch (slider)
            {
                case Slider.HOME:
                    HomeSlidebarBehaivour(_currentWindow);
                    break;
                case Slider.PLAYLISTS:
                    PlaylistsSlidebarBehaivour(_currentWindow);
                    break;
                case Slider.TRACKS:
                    TracksSlidebarBehaivour(_currentWindow);
                    break;
                case Slider.ARTISTS:
                    ArtistsSliderBehaivour(_currentWindow);
                    break;
                case Slider.ADD_ARTIST:
                    AddArtistSliderBehaivour(_currentWindow);
                    break;
                case Slider.ADD_TRACK:
                    AddTrackSliderBehaivour(_currentWindow);
                    break;
                case Slider.ADD_PLAYLIST:
                   AddPlaylistSliderBehaivour(_currentWindow);
                    break;
                default:
                    break;
            }
        }

        private void HomeSlidebarBehaivour(Window _window)
        {
            windows[Slider.HOME].WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _window.Visibility = Visibility.Collapsed;
            windows[Slider.HOME].Show();                        
        }

        private void PlaylistsSlidebarBehaivour(Window _window)
        {
            windows[Slider.PLAYLISTS].WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _window.Visibility = Visibility.Collapsed;
            windows[Slider.PLAYLISTS].Show();
        }

        private void TracksSlidebarBehaivour(Window _window)
        {
            windows[Slider.TRACKS].WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _window.Visibility = Visibility.Collapsed;
            windows[Slider.TRACKS].Show();
        }

        private void ArtistsSliderBehaivour(Window _window) 
        {
            windows[Slider.ARTISTS].WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _window.Visibility = Visibility.Collapsed;
            windows[Slider.ARTISTS].Show();
        }

        private void AddArtistSliderBehaivour(Window _window)
        {
            windows[Slider.ADD_ARTIST].WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _window.Visibility = Visibility.Collapsed;
            windows[Slider.ADD_ARTIST].Show();
        }

        private void AddTrackSliderBehaivour(Window _window)
        {
            windows[Slider.ADD_TRACK].WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _window.Visibility = Visibility.Collapsed;
            windows[Slider.ADD_TRACK].Show();
        }

        private void AddPlaylistSliderBehaivour(Window _window)
        {   
            windows[Slider.ADD_PLAYLIST].WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _window.Visibility = Visibility.Collapsed;
            windows[Slider.ADD_PLAYLIST].Show();
        }
    }
}
