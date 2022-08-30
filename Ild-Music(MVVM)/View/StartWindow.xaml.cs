using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.VM;

namespace Ild_Music_MVVM_.View
{
    public partial class StartWindow
    {
        private ViewModelHolderService vmHolder => (ViewModelHolderService)App.serviceCenter.GetService("VMHolder");

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

        private void Timer_Tick(object sender, EventArgs e)
        {
            sldTrackDuration.Value = sldTrackDuration.Minimum;
            if ((mainViewModel.PlayerEntity != null) && (!mainViewModel.PlayerEntity.IsEmpty) && !isDragging)
            {
                sldTrackDuration.Minimum = 0;
                sldTrackDuration.Maximum = mainViewModel.PlayerEntity.TotalTime.TotalSeconds;
                sldTrackDuration.Value = mainViewModel.PlayerEntity.CurrentTime.TotalSeconds;
                Debug.WriteLine($"V -{sldTrackDuration.Value}");
            }
        }


        #region Sliders
        private void HomeSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            mainViewModel.SetVM(vmHolder.GetViewModel("StartVM"));

        private void PlaylistSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            mainViewModel.SetVM(new ListViewModel(List.PLAYLISTS));

        private void TracksSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            mainViewModel.SetVM(new ListViewModel(List.TRACKS));

        private void ArtistsSlideClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            mainViewModel.SetVM(new ListViewModel(List.ARTISTS));

        private void PlatformSliderClick(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            mainViewModel.SetVM(new StageViewModel());
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

            //var x = sldTrackDuration.Value * mainViewModel.PlayerEntity.CurrentTime.TotalSeconds;
            var x = sldTrackDuration.Value;
            mainViewModel.PlayerEntity.CurrentTime = TimeSpan.FromSeconds((int)x);
        }

        private void sldTrackDuration_DragStarted(object sender, DragStartedEventArgs e)
        {
            isDragging = true;
        }
    }
}

