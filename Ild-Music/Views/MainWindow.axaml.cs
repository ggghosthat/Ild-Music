using Ild_Music.ViewModels;
using Ild_Music.ViewModels.Base;
using ShareInstances;

using System;
using System.Diagnostics;
using PropertyChanged;
using Avalonia;
using Avalonia.Threading;
using Avalonia.Media;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Interactivity;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.ApplicationLifetimes;


namespace Ild_Music.Views
{
    [DoNotNotifyAttribute]
    public partial class MainWindow : Window
    {
        #region Parts
        private const string PART_TITLEBAR = "DragBar";
        private const string PART_NAVBAR = "NavBar";
        private const string PART_VOLUME_AREA = "VolumeArea";
        private const string PART_VOLUME_BUTTON = "VolumeButton";
        private const string PART_MAIN_GRID = "MainGrid";
        private const string PART_VOLUME_SLIDER = "VolumeSlider";
        private const string PART_TIME_SLIDER = "sldThumby";

        private Grid? titleBar;
        private Grid? navBar;

        private Control volumePopup;
        private Control volumeButton;
        private Control mainGrid;
        private Slider volumeSlider;
        private static Slider sldThumby;
        #endregion

        private static DispatcherTimer timer = new(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, Timer_Tick);
        private static bool isDragging = false;


        public static MainViewModel Context {get; private set;}

        #region Ctor
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            Context = (MainViewModel)DataContext;
            timer.Start();
        }
        #endregion

        #region Dispatcher timer Methods
        

        private static void Timer_Tick(object sender, EventArgs e)
        {
            if ((Context.PlayerState) && !isDragging)
            {
                sldThumby.Minimum = 0;
                sldThumby.Maximum = Context.TotalTime.TotalSeconds;
                sldThumby.Value = Context.CurrentTime.TotalSeconds;
            }
            else if(!Context.PlayerState)
            {                
                sldThumby.Value = sldThumby.Minimum;
                sldThumby.Maximum = 100;
            }
        }
        #endregion

        #region Arrange Maethods
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);
            titleBar = e.NameScope.Get<Grid>(PART_TITLEBAR);
            navBar = e.NameScope.Get<Grid>(PART_NAVBAR);

            volumePopup = (Control)e.NameScope.Get<Border>(PART_VOLUME_AREA);
            volumeButton = (Control)e.NameScope.Get<StackPanel>(PART_VOLUME_BUTTON);
            mainGrid = (Control)e.NameScope.Get<Grid>(PART_MAIN_GRID);
            volumeSlider = e.NameScope.Get<Slider>(PART_VOLUME_SLIDER);
            sldThumby = e.NameScope.Get<Slider>(PART_TIME_SLIDER);
        }


        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            if (Equals(e.Source, titleBar))
                BeginMoveDrag(e);
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            // Get relative position of button, in relation to main grid
            var position = volumeButton.TranslatePoint(new Point(), mainGrid) ??
                           throw new Exception("Cannot get TranslatePoint from Configuration Button");

            // Set margin of popup so it appears bottom left of button
            volumePopup.Margin = new Thickness(
                position.X,
                0,
                0,
                mainGrid.Bounds.Height - position.Y );
        }

        private void OnHideClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            if(Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
                desktopLifetime.Shutdown();
        }
        #endregion


        #region NavBar Pressed Methods
        private void SwitchContext(string vmName)
        {
            var vm = (BaseViewModel)App.ViewModelTable[vmName];

            if (!Context.CurrentVM.NameVM.Equals(vm.NameVM))
                Context.CurrentVM = vm;   
        }
        
        private void OnHomePointerPressed(object? sender, PointerPressedEventArgs e) =>
            SwitchContext(StartViewModel.nameVM);

        private void OnCollectsPointerPressed(object? sender, PointerPressedEventArgs e) =>
            SwitchContext(ListViewModel.nameVM);

        private void OnSettingPointerPressed(object? sender, PointerPressedEventArgs e) =>
            SwitchContext(SettingViewModel.nameVM);

        private void VolumePopupDown(object? sender, PointerPressedEventArgs e) => ((MainViewModel)DataContext).VolumeSliderShowCommand.Execute(null);
        #endregion



    }
}