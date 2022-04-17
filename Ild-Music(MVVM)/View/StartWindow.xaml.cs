using Ild_Music_MVVM_.ViewModel.VM;
using System.Windows;
using System.Windows.Input;

namespace Ild_Music_MVVM_.View
{
    public partial class StartWindow : Window
    {

        #region Ctors
        public StartWindow()
        {
            DataContext = new MainViewModel();
        }
        #endregion

        #region Methods
       
        private void Btn_MouseRightButtonDown(object sender, RoutedEventArgs e)
        {
        }

        private void PlaylistClick(object sender, RoutedEventArgs e)
        {
        }

       

        private void AddPlaylist_Click(object sender, MouseButtonEventArgs e)
        {
        }

        private void AddTrack_Click(object sender, MouseButtonEventArgs e)
        {
        }

        private void AddArtist_Ckuck(object sender, MouseButtonEventArgs e)
        {
        }


        private void EnterMouse_Artists(object sender, MouseEventArgs e)
        {
           //Artists.Background = new SolidColorBrush(Color.FromRgb(25, 31, 158));
        }

        private void LeaveMouse_Artists(object sender, MouseEventArgs e)
        {
            //Artists.Background = new SolidColorBrush(Color.FromRgb(31, 38, 178));
        }

        private void EnterMouse_Home(object sender, MouseEventArgs e)
        {
            //Home.Background = new SolidColorBrush(Color.FromRgb(25, 31, 158));
        }

        private void LeaverMouse_Home(object sender, MouseEventArgs e)
        {
            //Home.Background = new SolidColorBrush(Color.FromRgb(31, 38, 178));
        }

        private void EnterMouse_Playlists(object sender, MouseEventArgs e)
        {
            //Playlists.Background = new SolidColorBrush(Color.FromRgb(25, 31, 158));
        }

        private void LeaveMouse_Playlists(object sender, MouseEventArgs e)
        {
            //Playlists.Background = new SolidColorBrush(Color.FromRgb(31, 38, 178));
        }

        private void EnterMouse_Tracks(object sender, MouseEventArgs e)
        {
            //Tracks.Background = new SolidColorBrush(Color.FromRgb(25, 31, 158));
        }

        private void LeaveMouse_Tracks(object sender, MouseEventArgs e)
        {
            //Tracks.Background = new SolidColorBrush(Color.FromRgb(31, 38, 178));
        }

        private void Previous_MouseEnter(object sender, MouseEventArgs e)
        {
            //brPrev.Background = new SolidColorBrush(Colors.Aqua);
        }

        private void Previous_MouseLeave(object sender, MouseEventArgs e)
        {
            //brPrev.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void Play_MouseEnter(object sender, MouseEventArgs e)
        {
            //brPlay.Background = new SolidColorBrush(Colors.Aqua);
        }

        private void Play_MouseLeave(object sender, MouseEventArgs e)
        {
            //brPlay.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void Next_MouseEnter(object sender, MouseEventArgs e)
        {
            //brNext.Background = new SolidColorBrush(Colors.Aqua);
        }

        private void Next_MouseLeave(object sender, MouseEventArgs e)
        {
            //brNext.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void Shuffle_MouseEnter(object sender, MouseEventArgs e)
        {
            //brShuffle.Background = new SolidColorBrush(Colors.Aqua);
        }

        private void Shuffle_MouseLeave(object sender, MouseEventArgs e)
        {
            //brShuffle.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void Repeat_MouseEnter(object sender, MouseEventArgs e)
        {
            //brRepeat.Background = new SolidColorBrush(Colors.Aqua);
        }

        private void Repeat_MouseLeave(object sender, MouseEventArgs e)
        {
            //brRepeat.Background = new SolidColorBrush(Colors.Transparent);
        }

     
        

        private void Home_MouseClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void Playlists_MouseClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void Tracks_MouseClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void Artists_MouseClick(object sender, MouseButtonEventArgs e)
        {
        }


        #endregion

        #region IMethods
        //public void EnablePlayer(PlayerTrigger trigger)
        //{
        //    var playerController = (PlayerController)PlayerController;
        //    switch (trigger)
        //    {
        //        case PlayerTrigger.PLAY:
        //            playerController.InitPlayer(initIndex);
        //            break;
        //        case PlayerTrigger.PAUSE:
        //            playerController.Pause_Resume();
        //            break;
        //        case PlayerTrigger.NEXT:
        //            playerController.PlayNext();
        //            break;
        //        case PlayerTrigger.PREVIOUS:
        //            playerController.PlayNext();
        //            break;
        //        case PlayerTrigger.SHUFFLE:
        //            playerController.Shuffle();
        //            break;
        //        case PlayerTrigger.DROP_STAFF:
        //            if (this.inputTracklist != null)
        //                playerController.DropStaff(inputTracklist:inputTracklist, startIndex:initIndex);
        //            break;
        //        case PlayerTrigger.REPEAT:
        //            break;
        //        case PlayerTrigger.SONG_NAME:
        //            break;
        //        case PlayerTrigger.CURRENT_TIME:
        //            break;
        //        case PlayerTrigger.DURATION_TIME:
        //            break;
        //        default:
        //            break;
        //    }
        //}
        #endregion

        #region PlayerMethods
        private void Play_mouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            //var playerController = (PlayerController)PlayerController;

            //switch (playerController.State)
            //{
            //    case NAudio.Wave.PlaybackState.Stopped:
            //        EnablePlayer(PlayerTrigger.PLAY);
            //        btnPlayImage.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "/player_icons/play.png"));
            //        break;
            //    case NAudio.Wave.PlaybackState.Playing:
            //        EnablePlayer(PlayerTrigger.PAUSE);
            //        btnPlayImage.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "/player_icons/pause.png"));
            //        break;
            //    case NAudio.Wave.PlaybackState.Paused:
            //        EnablePlayer(PlayerTrigger.PLAY);
            //        btnPlayImage.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "/player_icons/play.png"));
            //        break;
            //    case null:
            //        btnPlayImage.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "/player_icons/play.png"));
            //        break;
            //    default:
            //        btnPlayImage.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "/player_icons/play.png"));
            //        break;
            //}
        }

        private void Previous_mouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            //var playerController = (PlayerController)PlayerController;

            //if(playerController.Player != null)
            //    EnablePlayer(PlayerTrigger.PREVIOUS);

        }

        private void Next_mouseLeftDown(object sender, MouseButtonEventArgs e)
        {
        //    var playerController = (PlayerController)PlayerController;

        //    if (playerController.Player != null)
        //        EnablePlayer(PlayerTrigger.NEXT);
        }

        private void Shuffle_mouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            //var playerController = (PlayerController)PlayerController;

            //if (playerController.Player != null)
            //    EnablePlayer(PlayerTrigger.SHUFFLE);
        }

        private void Repeat_mouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            //var playerController = (PlayerController)PlayerController;

            //if (playerController.Player != null)
            //    EnablePlayer(PlayerTrigger.REPEAT);
        }
        #endregion
    }
}
