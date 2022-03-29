using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для PlaylistWindow.xaml
    /// </summary>
    public partial class PlaylistWindow : Window
    {
        private WindowsController windowsController;

        public PlaylistWindow()
        {
            windowsController = new WindowsController(this);
            InitializeComponent();
            GenerateGrid();
        }

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

        private Button GenerateButton(string stuff = "")
        {
            Style style = FindResource("CardButton") as Style;
            var btn = new Button();
            btn.Content = stuff;
            btn.FontSize = 35;
            btn.FontWeight = FontWeight.FromOpenTypeWeight(30);
            btn.Foreground = Brushes.White;
            btn.Style = style;
            btn.Height = 80;
            btn.Width = 80;
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.VerticalAlignment = VerticalAlignment.Center;
            Color c = Color.FromRgb((byte)new Random().Next(256), (byte)new Random().Next(256), (byte)new Random().Next(256));
            btn.Background = new SolidColorBrush(c);
            customGrid.Children.Add(btn);
            return btn;
        }

        private void GenerateGrid() 
        {
            var lsBtns = new List<Button>();
            for(int i = 0; i < 28;i++)
               lsBtns.Add(GenerateButton());

            int x = (lsBtns.Count - 1) / 8;


            for (int j = 0; j < x; j++)
            {
                var lsK = lsBtns.GetRange(0, 8);
                lsBtns.RemoveRange(0, 8);

                customGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100) });

                for (int i = 0; i < lsK.Count; i++)
                {
                    customGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100) });
                    Grid.SetRow(lsK[i], j);
                    Grid.SetColumn(lsK[i], i);
                }

                if (lsBtns.Count > 0)
                {
                    customGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100) });
                    for (int k = 0; k < lsBtns.Count; k++) 
                    {
                        Grid.SetRow(lsBtns[k],x);
                        Grid.SetColumn(lsBtns[k], k);
                    }
                }
            }
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
