using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.VM;
using Ild_Music_MVVM_.ExtensionMethods;

namespace Ild_Music_MVVM_.View
{
    public partial class StartWindow
    {
        private ViewModelHolder vmHolder => App.vmHolder;

        private MainViewModel mainViewModel = new();
        private DispatcherTimer timer = new();

        private bool isDragging = false;

        public StartWindow()
        {
            InitializeComponent();
            DataContext = mainViewModel;
            TimerBoost();
        }

        private void TimerBoost()
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            sldTrackDuration.Value = 0;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        //need for slider steps
        private void Timer_Tick(object sender, EventArgs e)
        {
            sldTrackDuration.Value = sldTrackDuration.Minimum;
            if ((mainViewModel.PlayerEntity != null) && (!mainViewModel.PlayerEntity.IsEmpty) && !isDragging)
            {
                sldTrackDuration.Minimum = 0;
                sldTrackDuration.Maximum = mainViewModel.PlayerEntity.TotalTime.TotalSeconds;
                sldTrackDuration.Value = mainViewModel.PlayerEntity.CurrentTime.TotalSeconds;
            }
        }


        #region Slider
        private void HomeSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            mainViewModel.ResetVM(vmHolder.GetViewModel(StartViewModel.nameVM));

        private void PlatformSliderClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            mainViewModel.ResetVM(vmHolder.GetViewModel(StageViewModel.nameVM));



        private void PlaylistSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            mainViewModel.ResetVM(vmHolder.GetViewModel(ListViewModel.nameVM)
                                        .DefineListVM_Type(List.PLAYLISTS));

        private void TracksSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            mainViewModel.ResetVM(vmHolder.GetViewModel(ListViewModel.nameVM)
                                        .DefineListVM_Type(List.TRACKS));

        private void ArtistsSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            mainViewModel.ResetVM(vmHolder.GetViewModel(ListViewModel.nameVM)
                                        .DefineListVM_Type(List.ARTISTS));

        
        #endregion

        #region Window Manipullation mathods
        private void btnMinimizeWindow_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            WindowState = WindowState.Minimized;
        
        private void btnCloseWindow_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            Application.Current.Shutdown();

        private void DragWindow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                DragMove();
        }
        #endregion

        private void sldTrackDuration_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            isDragging = false;
            mainViewModel.PlayerEntity.CurrentTime = TimeSpan.FromSeconds((int)sldTrackDuration.Value);
        }

        private void sldTrackDuration_DragStarted(object sender, DragStartedEventArgs e)
        {
            isDragging = true;
        }
    }
}

