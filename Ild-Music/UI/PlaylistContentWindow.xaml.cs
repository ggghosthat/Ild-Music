using Ild_Music.Controllers.ControllerServices;
using Ild_Music_CORE.Models;
using Ild_Music_CORE.Models.Core;
using System;
using System.Linq;

namespace Ild_Music.UI
{
    /// <summary>
    /// Логика взаимодействия для PlaylistContentWindow.xaml
    /// </summary>
    public partial class PlaylistContentWindow
    {

        private Tracklist _playlist;

        private SynchSupporter _supporter;

        private event Action RefreshContent;

        internal PlaylistContentWindow(SynchSupporter supporter, ref ITrackable playlist)
        {
            RefreshContent += InitLists;
            _playlist = (Tracklist)playlist;
            _supporter = supporter;
            InitializeComponent();
            RefreshContent.Invoke();
        }

        private void InitLists()
        {
            lvContent.ItemsSource = _playlist.Tracks.Select(pl => pl.Name);
            lvAvailable.ItemsSource = _supporter.ExistedTracks.Select(tc => tc.Name);
        }

        private void Delete_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void Delete_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void Delete_MouseClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var index = lvContent.SelectedIndex;
            _playlist.Tracks.RemoveAt(index);
        }

        private void Select_AvaillableTrack(object sender, System.Windows.RoutedEventArgs e)
        {
            var index = lvAvailable.SelectedIndex;
            var track = _supporter.ExistedTracks[index];
            _playlist.Tracks.Add(track);
            RefreshContent.Invoke();
        }

        private void ListViewItem_MouseDoubleClick(object sender, EventArgs e) 
        {
            var index = lvAvailable.SelectedIndex;
            var track = _supporter.ExistedTracks[index];
            _playlist.Tracks.Add(track);
            RefreshContent.Invoke();
        }

        private void btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
