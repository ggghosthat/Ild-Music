using Ild_Music_MVVM_.ViewModel.VM;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
