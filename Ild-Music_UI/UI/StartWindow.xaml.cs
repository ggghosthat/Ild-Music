using Ild_Music.Controllers;
using Ild_Music.Controllers.ControllerServices;
using SynchronizationBlock.Models.SynchArea;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Collections.Generic;
using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using Ild_Music.Controllers.ControllerServices.Interfaces;
using Ild_Music_CORE.Models;

namespace Ild_Music.UI
{
    public partial class StartWindow : Window, IPlayerEnable
    {
        #region PlayerFields
        private PlayerController _playerController;
        IPlayerControll PlayerController => _playerController ?? null;

        private int initIndex = 0;
        private Tracklist inputTracklist;

        private ITrackable _playerStaffTrack;
        private ITrackable _playerStaffTracklist;

        private event Action DropStaff;
        #endregion

        #region SynchBlockFields
        private static Area _mainArea;
        private static ModelFactoryController _modelFactory;
        private static WindowsController _windowsController;
        private static SynchSupporter _supporter;

        private IDictionary<string,string> _tracksStructure = new Dictionary<string, string>();
        private IDictionary<string, string> _playlistsStructure = new Dictionary<string, string>();
        private IDictionary<string, string> _artistsStructure = new Dictionary<string, string>();

        
        internal delegate void RefreshCenteralList();
        internal static event RefreshCenteralList RefreshCentList;
        #endregion



        #region Ctors
        public StartWindow()
        {
            _windowsController = new WindowsController(this);
            _mainArea = new Area();
            _modelFactory = new ModelFactoryController(_mainArea);
            _supporter = new SynchSupporter(_mainArea);

            this.DropStaff += OnDropStaff;

            InitializeComponent();
            DisplayData();
            InitilizeWindowsController();
        }
        #endregion

        #region Methods
        private void InitilizeWindowsController()
        {
            _windowsController.AddWindow(Slider.HOME, this);
            _windowsController.AddWindow(Slider.ADD_ARTIST, new AddArtist(_modelFactory, new ListWindow(_mainArea, _modelFactory, _supporter, _windowsController, EntityState.Artist) ) );
            _windowsController.AddWindow(Slider.ADD_PLAYLIST, new AddPlaylist(_modelFactory, new ListWindow(_mainArea, _modelFactory, _supporter, _windowsController, EntityState.Playlist)) );
            _windowsController.AddWindow(Slider.ADD_TRACK, new AddTrack(_modelFactory, new ListWindow(_mainArea, _modelFactory, _supporter, _windowsController, EntityState.Track)) );

            _windowsController.AddWindow(Slider.TRACKS, new ListWindow(_mainArea, _modelFactory, _supporter, _windowsController, EntityState.Track));
            _windowsController.AddWindow(Slider.ARTISTS, new ListWindow(_mainArea, _modelFactory, _supporter, _windowsController, EntityState.Artist));
            _windowsController.AddWindow(Slider.PLAYLISTS, new ListWindow(_mainArea, _modelFactory, _supporter, _windowsController, EntityState.Playlist));

        }
                
        private void DisplayData()
        {
            _tracksStructure.Clear();
            _playlistsStructure.Clear();
            _artistsStructure.Clear();
            lsTracks.ItemsSource = null;
            crdsPlaylists.Children.Clear();
            crdsArtists.Children.Clear();

            var listTracks = _supporter.ExistedTracks ?? new List<Track>();
            var listPlaylists = _supporter.ExistedPlaylist ?? new List<Tracklist>();
            var listArtists = _supporter.ExistedArtists ?? new List<Artist>();

            listTracks.ToList().ForEach(track => _tracksStructure.Add(track.GetId(), track.Name));
            listPlaylists.ToList().ForEach(playlist => _playlistsStructure.Add(playlist.Id, playlist.Name));
            listArtists.ToList().ForEach(artist => _artistsStructure.Add(artist.Id, artist.Name));


            lsTracks.ItemsSource = _tracksStructure.Values;

            listPlaylists.ToList().ForEach(pl =>
            {
                crdsPlaylists.Children.Add(GeneratePlaylistUI(pl.Name));
            });
                    
            listArtists.ToList().ForEach(artist => 
            {
                crdsArtists.Children.Add(GenerateArtistUI(artist.Name.Substring(0, 1) ) );
            });
        }


        private Button GeneratePlaylistUI(string stuff = "")
        {
            Style style = FindResource("CardButton") as Style;
            var btn = new Button();
            btn.Content = stuff;
            btn.FontSize = 35;
            btn.FontWeight = FontWeight.FromOpenTypeWeight(30);
            btn.Foreground = Brushes.White;
            btn.Style = style;
            btn.Height = 125;
            btn.Width = 125;
            Color c = Color.FromRgb(68,204,234);
            btn.Background = new SolidColorBrush(c);
            btn.Click += PlaylistClick;
            btn.MouseRightButtonDown += Btn_MouseRightButtonDown;
            return btn;
        }

        private void Btn_MouseRightButtonDown(object sender, RoutedEventArgs e)
        {
            var btn = (Button)e.Source;
            _playlistsStructure.ToList().ForEach(entry =>
            {
                if (entry.Value.Equals(btn.Content))
                    _playerStaffTracklist = _supporter.ExistedPlaylist.First(pl => pl.Id.Equals(entry.Key));
            });

            //var playlistContentShow = new PlaylistContentWindow(supporter: _supporter, ref _playerStaffTracklist);
            //playlistContentShow.Show();
        }

        private void PlaylistClick(object sender, RoutedEventArgs e)
        {
            var btn = (Button)e.OriginalSource;
            _playlistsStructure.ToList().ForEach(entry =>
            {
                if (entry.Value.Equals(btn.Content))
                    _playerStaffTracklist = _supporter.ExistedPlaylist.First(pl => pl.Id.Equals(entry.Key));
            });
            DropStaff.Invoke();
        }

        private Button GenerateArtistUI(string stuff = "")
        {
            Style style = FindResource("CardButton") as Style;
            var btn = new Button();
            btn.Content = stuff;
            btn.FontSize = 25;
            btn.FontWeight = FontWeight.FromOpenTypeWeight(30);
            btn.Foreground = Brushes.White;
            btn.Style = style;
            btn.Height = 40;
            btn.Width = 40;
            btn.Margin  = new Thickness(5);
            Color c = Color.FromRgb(68, 204, 234);
            btn.Background = new SolidColorBrush(c);
            

            return btn;
        }

        private void AddPlaylist_Click(object sender, MouseButtonEventArgs e)
        {
            InitStartShow(EntityState.Playlist);
            return;
        }

        private void AddTrack_Click(object sender, MouseButtonEventArgs e)
        {
            lsTracks.Items.Add("ss");
        }

        private void AddArtist_Ckuck(object sender, MouseButtonEventArgs e)
        {
            InitStartShow(EntityState.Artist);
            return;
        }

        private void InitStartShow(EntityState _state) 
        {
            switch (_state)
            {
                case EntityState.Track:
                    var trackWindow = new AddTrack(_modelFactory, (ListWindow)_windowsController.Windows[Slider.TRACKS]);
                    trackWindow.ListRefresh += new AddTrack.RefreshListContent(DisplayData);
                    trackWindow.Show();
                    break;
                case EntityState.Playlist:
                    var playlistWindow = new AddPlaylist(_modelFactory, (ListWindow)_windowsController.Windows[Slider.PLAYLISTS]);
                    playlistWindow.ListRefresh += new AddPlaylist.RefreshListContent(DisplayData);
                    playlistWindow.Show();
                    break;
                case EntityState.Artist:
                    var artistWindow = new AddArtist(_modelFactory, (ListWindow)_windowsController.Windows[Slider.ARTISTS]);
                    artistWindow.ListRefresh += new AddArtist.RefreshListContent(DisplayData);
                    artistWindow.Show();
                    break;
                default:
                    break;
            }
            return;
        }

        private void EnterMouse_Artists(object sender, MouseEventArgs e)
        {
            Artists.Background = new SolidColorBrush(Color.FromRgb(25, 31, 158));
        }

        private void LeaveMouse_Artists(object sender, MouseEventArgs e)
        {
            Artists.Background = new SolidColorBrush(Color.FromRgb(31, 38, 178));
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

        private void LsTracksDoubleClick(object sender, MouseEventArgs e)
        {
            var name = lsTracks.SelectedItem;
            //var id = _tracksStructure.ToList().ForEach(track => 
            //{
            //    if (track.Value.Equals(name))
            //        return track.Key;
            //});

            var tmp = _tracksStructure.ToList().First(track => track.Value.Equals(name));
            var track = _supporter.ExistedTracks.ToList().Where(track => track.Id.Equals(tmp.Key));
            
            //Change this solution
            _playerController = new PlayerController((ITrackable)track);
        }

        

        private void Home_MouseClick(object sender, MouseButtonEventArgs e)
        {
            _windowsController.ClickDetector(Slider.HOME,this);
        }

        private void Playlists_MouseClick(object sender, MouseButtonEventArgs e)
        {
            _windowsController.ClickDetector(Slider.PLAYLISTS,this);
        }

        private void Tracks_MouseClick(object sender, MouseButtonEventArgs e)
        {
            _windowsController.ClickDetector(Slider.TRACKS,this);
        }

        private void Artists_MouseClick(object sender, MouseButtonEventArgs e)
        {
            _windowsController.ClickDetector(Slider.ARTISTS,this);
        }


        #endregion

        #region IMethods
        public void EnablePlayer(PlayerTrigger trigger)
        {
            var playerController = (PlayerController)PlayerController;
            switch (trigger)
            {
                case PlayerTrigger.PLAY:
                    playerController.InitPlayer(initIndex);
                    break;
                case PlayerTrigger.PAUSE:
                    playerController.Pause_Resume();
                    break;
                case PlayerTrigger.NEXT:
                    playerController.PlayNext();
                    break;
                case PlayerTrigger.PREVIOUS:
                    playerController.PlayNext();
                    break;
                case PlayerTrigger.SHUFFLE:
                    playerController.Shuffle();
                    break;
                case PlayerTrigger.DROP_STAFF:
                    if (this.inputTracklist != null)
                        playerController.DropStaff(inputTracklist:inputTracklist, startIndex:initIndex);
                    break;
                case PlayerTrigger.REPEAT:
                    break;
                case PlayerTrigger.SONG_NAME:
                    break;
                case PlayerTrigger.CURRENT_TIME:
                    break;
                case PlayerTrigger.DURATION_TIME:
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region PlayerMethods
        private void Play_mouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var playerController = (PlayerController)PlayerController;

            switch (playerController.State)
            {
                case NAudio.Wave.PlaybackState.Stopped:
                    EnablePlayer(PlayerTrigger.PLAY);
                    btnPlayImage.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "/player_icons/play.png"));
                    break;
                case NAudio.Wave.PlaybackState.Playing:
                    EnablePlayer(PlayerTrigger.PAUSE);
                    btnPlayImage.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "/player_icons/pause.png"));
                    break;
                case NAudio.Wave.PlaybackState.Paused:
                    EnablePlayer(PlayerTrigger.PLAY);
                    btnPlayImage.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "/player_icons/play.png"));
                    break;
                case null:
                    btnPlayImage.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "/player_icons/play.png"));
                    break;
                default:
                    btnPlayImage.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "/player_icons/play.png"));
                    break;
            }
        }

        private void Previous_mouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var playerController = (PlayerController)PlayerController;

            if(playerController.Player != null)
                EnablePlayer(PlayerTrigger.PREVIOUS);

        }

        private void Next_mouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var playerController = (PlayerController)PlayerController;

            if (playerController.Player != null)
                EnablePlayer(PlayerTrigger.NEXT);
        }

        private void Shuffle_mouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var playerController = (PlayerController)PlayerController;

            if (playerController.Player != null)
                EnablePlayer(PlayerTrigger.SHUFFLE);
        }

        private void Repeat_mouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            var playerController = (PlayerController)PlayerController;

            if (playerController.Player != null)
                EnablePlayer(PlayerTrigger.REPEAT);
        }
        #endregion

        private void OnDropStaff() 
        {
            _playerController.DropStaff(_playerStaffTracklist);
        }
    }
}
