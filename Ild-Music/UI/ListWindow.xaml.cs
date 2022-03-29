using Ild_Music.Controllers;
using Ild_Music.Controllers.ControllerServices;
using SynchronizationBlock.Models.SynchArea;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Linq;
using Ild_Music_CORE.Models.Interfaces;
using Ild_Music_CORE.Models.CORE.Tracklist_Structure;
using Ild_Music_CORE.Models.Core;
using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using Ild_Music_CORE.Models;
using System;

namespace Ild_Music.UI
{

    public enum LvSelectionMode 
    {
        Artist, 
        Playlist,
        Track 
    };


    /// <summary>
    /// Логика взаимодействия для ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {

        private static WindowsController windowsController;
        private static ModelFactoryController _modelFactory;
        private static SynchSupporter _supporter;
        private static Area _area;

        internal SynchSupporter SynchSupporter => _supporter;

        private EntityState _state;
        public EntityState State 
        {
            get => _state;
            set => _state = value;
        }


        private IList<Artist> _link_artists = new List<Artist>();
        public IList<Artist> LinkArtists
        {
            get => _link_artists;
            set => _link_artists = value;
        }

        private IList<Tracklist> _link_playlists = new List<Tracklist>();
        public IList<Tracklist> LinkPlaylists
        {
            get => _link_playlists;
            set => _link_playlists = value;
        }

        private IList<Track> _link_tracks = new List<Track>();
        public IList<Track> LinkTracks
        {
            get => _link_tracks;
            set => _link_tracks = value;
        }


        internal delegate void ArtistsDetect();
        internal event ArtistsDetect ArtistsDetected;

        internal delegate void PlaylistDetect();
        internal event PlaylistDetect PlaylistDetected;


        //Constructor for presenting whole existed staff
        internal ListWindow(Area area,
                            ModelFactoryController modelFactory, 
                            SynchSupporter supporter,
                            WindowsController winController,
                            EntityState state)
        {
            _area = area;
            windowsController = winController;
            _supporter = supporter;
            _modelFactory = modelFactory;
            _state = state;
            InitializeComponent();
            SetList();
        }

        
        #region Executable Functions

        internal void SetList()
        {
            switch (_state)
            {
                case EntityState.Artist:
                    lblPlaylistName.Text = "Your artists";
                    _supporter.RefreshList(EntityState.Artist);
                    lvList.ItemsSource = _supporter.ExistedArtists;
                    break;
                case EntityState.Playlist:
                    lblPlaylistName.Text = "Your playlists";
                    _supporter.RefreshList(EntityState.Playlist);
                    lvList.ItemsSource = _supporter.ExistedPlaylist;
                    break;
                case EntityState.Track:
                    lblPlaylistName.Text = "Your tracks";
                    _supporter.RefreshList(EntityState.Track);
                    lvList.ItemsSource = _supporter.ExistedTracks;
                    break;
                default:
                    break;
            }
            return;
        }

        private void Add_MouseClick(object sender, MouseButtonEventArgs e)
        {
                InitializeNewStaff();
        }

        private void InitializeNewStaff()
        {
            switch (_state)
            {
                case EntityState.Track:
                    var trackWindow = new AddTrack(_modelFactory, this);
                    trackWindow.Show();
                    break;
                case EntityState.Playlist:
                    var playlistWindow = new AddPlaylist(_modelFactory, this);
                    playlistWindow.Show();
                    break;
                case EntityState.Artist:
                    var artistWindow = new AddArtist(_modelFactory, this);
                    artistWindow.Show();
                    break;
                default:
                    break;
            }
        }


        private void Delete_MouseClick(object sender, MouseButtonEventArgs e)
        {
            var index = lvList.SelectedIndex;
            switch (_state)
            {
                case EntityState.Track:
                    var track = _supporter.ExistedTracks[index];
                    _supporter.RemoveInstanceObject(track, EntityState.Track);
                    _supporter.RefreshList(EntityState.Track);
                    SetList();
                    break;
                case EntityState.Playlist:
                    var playlist = _supporter.ExistedPlaylist[index];
                    _supporter.RemoveInstanceObject(playlist, EntityState.Playlist);
                    _supporter.RefreshList(EntityState.Playlist);
                    SetList();
                    break;
                case EntityState.Artist:
                    var artist = _supporter.ExistedArtists[index];
                    _supporter.RemoveInstanceObject(artist, EntityState.Artist);
                    _supporter.RefreshList(EntityState.Artist);
                    SetList();
                    break;
                default:
                    break;
            }
        }

        private void Edit_MouseClick(object sender, MouseButtonEventArgs e)
        {
            var new_name = txtName.Text;
            Edit_Name(new_name);
        }

        public ListWindow BootListInit(EntityState state)
        {
            this._state = state;
            SetList();
            return this;
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var items = lvList.SelectedItems;

            foreach (var item in items)
            {
                var index = lvList.Items.IndexOf(item);
                SelectEntity(index);
            }

            btn.Visibility = Visibility.Collapsed;
            this.Close();
        }

        private void SelectEntity(int index)
        {
            switch (_state)
            {
                case EntityState.Track:
                    _link_tracks.Add(_supporter.ExistedTracks[index]);
                    ArtistsDetected.Invoke();
                    break;
                case EntityState.Playlist:
                    _link_playlists.Add(_supporter.ExistedPlaylist[index]);
                    PlaylistDetected.Invoke();
                    break;
                case EntityState.Artist:
                    _link_artists.Add(_supporter.ExistedArtists[index]);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Event Functions
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

        private void Edit_MouseEnter(object sender, MouseEventArgs e)
        {
            brEdit.Background = new SolidColorBrush(Colors.Aqua);
        }

        private void Edit_MouseLeave(object sender, MouseEventArgs e)
        {
            brEdit.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void Add_MouseEnter(object sender, MouseEventArgs e)
        {
            brAdd.Background = new SolidColorBrush(Colors.Aqua);
        }

        private void Add_MouseLeave(object sender, MouseEventArgs e)
        {
            brAdd.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void Delet_MouseEnter(object sender, MouseEventArgs e)
        {
            brDelete.Background = new SolidColorBrush(Colors.Aqua);
        }

        private void Delet_MouseLeave(object sender, MouseEventArgs e)
        {
            brDelete.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void EnterMouse_Artists(object sender, MouseEventArgs e)
        {
            Artists.Background = new SolidColorBrush(Color.FromRgb(25, 31, 158));
        }

        private void LeaveMouse_Artists(object sender, MouseEventArgs e)
        {
            Artists.Background = new SolidColorBrush(Color.FromRgb(31, 38, 178));
        }

        private void Home_MouseClick(object sender, MouseButtonEventArgs e)
        {
            windowsController.ClickDetector(Slider.HOME,this);
        }

        private void Playlists_MouseClick(object sender, MouseButtonEventArgs e)
        {
            windowsController.ClickDetector(Slider.PLAYLISTS,this);
        }

        private void Tracks_MouseClick(object sender, MouseButtonEventArgs e)
        {
            windowsController.ClickDetector(Slider.TRACKS,this);
        }

        private void Artists_MouseClick(object sender, MouseButtonEventArgs e)
        {
            windowsController.ClickDetector(Slider.ARTISTS,this);
        }
                
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            switch (_state) 
            {
                case EntityState.Artist:
                    var index = lvList.SelectedIndex;
                    var item = _supporter.ExistedArtists[index];
                    GetArtistContent(item);
                    break;
                case EntityState.Playlist:
                    break;
                case EntityState.Track:
                    break;
                default:
                    break;
            }
        }


        private void GetArtistContent(Artist artist) 
        {
            _supporter.RefreshList(EntityState.Artist);
            var tracks = artist.TracksCollection;
            var playlists = artist.Tracks;

            if (tracks != null && tracks.Count > 0)
                lvList.ItemsSource = tracks.Select(t => t.Name);

            if (playlists != null && playlists.Count > 0)
                lvList.ItemsSource = playlists.Select(p => p.Name);
        }


        private void Edit_Name(string new_name) 
        {
            if (new_name != string.Empty) 
            {
                var index  = lvList.SelectedIndex;

                switch (_state)
                {
                    case EntityState.Artist:
                        var item_artist = _supporter.ExistedArtists[index];
                        item_artist.Name = new_name;
                        _supporter.EditInstanceObject(item_artist,EntityState.Artist);
                        break;
                    case EntityState.Playlist:
                        var item_playlist = _supporter.ExistedPlaylist[index];
                        item_playlist.Name = new_name;
                        _supporter.EditInstanceObject(item_playlist, EntityState.Playlist);
                        break;
                    case EntityState.Track:
                        var item_track = _supporter.ExistedTracks[index];
                        item_track.Name = new_name;
                        _supporter.EditInstanceObject(item_track, EntityState.Playlist);
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

    }
}
