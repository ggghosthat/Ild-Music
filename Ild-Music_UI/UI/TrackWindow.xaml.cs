using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ild_Music.UI
{
    /// <summary>
    /// Логика взаимодействия для TrackWindow.xaml
    /// </summary>
    public partial class TrackWindow : Window
    {
        private WindowsController windowsController;

        public TrackWindow()
        {
            windowsController = new WindowsController(this);
            InitializeComponent();
        }

        #region Events 
        private void EnterMouse_Home(object sender, MouseEventArgs e)
        {
            Home.Background = new SolidColorBrush(Color.FromRgb(25, 31, 158));
        }

        private void LeaverMouse_Home(object sender, MouseEventArgs e)
        {
            Home.Background = new SolidColorBrush(Color.FromRgb(31, 38, 178));
        }

        private void EnterMouse_Playlists(object sender, MouseEventArgs e)
        {
            Playlists.Background = new SolidColorBrush(Color.FromRgb(25, 31, 158));
        }

        private void LeaveMouse_Playlists(object sender, MouseEventArgs e)
        {
            Playlists.Background = new SolidColorBrush(Color.FromRgb(31, 38, 178));
        }

        private void EnterMouse_Tracks(object sender, MouseEventArgs e)
        {
            Tracks.Background = new SolidColorBrush(Color.FromRgb(25, 31, 158));
        }

        private void LeaveMouse_Tracks(object sender, MouseEventArgs e)
        {
            Tracks.Background = new SolidColorBrush(Color.FromRgb(31, 38, 178));
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
        }

        private void imgRight_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void imgRight_MouseLeave(object sender, MouseEventArgs e)
        {
        }

        private void leftSkTrack_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void leftSkTrack_MouseLeave(object sender, MouseEventArgs e)
        {
        }

        private void rightSkTrack_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void rightSkTrack_MouseLeave(object sender, MouseEventArgs e)
        {
        }

        private void Previous_MouseEnter(object sender, MouseEventArgs e)
        {
            brPrev.Background = new SolidColorBrush(Colors.Aqua);
        }

        private void Previous_MouseLeave(object sender, MouseEventArgs e)
        {
            brPrev.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void Play_MouseEnter(object sender, MouseEventArgs e)
        {
            brPlay.Background = new SolidColorBrush(Colors.Aqua);
        }

        private void Play_MouseLeave(object sender, MouseEventArgs e)
        {
            brPlay.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void Next_MouseEnter(object sender, MouseEventArgs e)
        {
            brNext.Background = new SolidColorBrush(Colors.Aqua);
        }

        private void Next_MouseLeave(object sender, MouseEventArgs e)
        {
            brNext.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void Shuffle_MouseEnter(object sender, MouseEventArgs e)
        {
            brShuffle.Background = new SolidColorBrush(Colors.Aqua);
        }

        private void Shuffle_MouseLeave(object sender, MouseEventArgs e)
        {
            brShuffle.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void Repeat_MouseEnter(object sender, MouseEventArgs e)
        {
            brRepeat.Background = new SolidColorBrush(Colors.Aqua);
        }

        private void Repeat_MouseLeave(object sender, MouseEventArgs e)
        {
            brRepeat.Background = new SolidColorBrush(Colors.Transparent);
        }

        #endregion

        private void Home_MouseClick(object sender, MouseButtonEventArgs e)
        {
            windowsController.ClickDetector(Slider.HOME, this);
        }

        private void Playlists_MouseClick(object sender, MouseButtonEventArgs e)
        {
            windowsController.ClickDetector(Slider.PLAYLISTS, this);
        }

        private void Tracks_MouseClick(object sender, MouseButtonEventArgs e)
        {
            windowsController.ClickDetector(Slider.TRACKS, this);
        }
    }
}
