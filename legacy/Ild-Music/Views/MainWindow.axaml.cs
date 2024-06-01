using Ild_Music.ViewModels;
using Ild_Music.ViewModels.Base;

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
        private const string PART_TIME_SLIDER_THUMB = "timeThumber";

        private Grid? titleBar;
        private Grid? navBar;

        private Control volumePopup;
        private Control volumeButton;
        private Control mainGrid;
        private Slider volumeSlider;
        #endregion

        public static MainViewModel Context {get; private set;}

        #region Ctor
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            Context = (MainViewModel)DataContext;
        }
        #endregion

        #region Arrange Maethods
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            titleBar = e.NameScope.Get<Grid>(PART_TITLEBAR);
            navBar = e.NameScope.Get<Grid>(PART_NAVBAR);

            volumePopup = (Control)e.NameScope.Get<Border>(PART_VOLUME_AREA);
            volumeButton = (Control)e.NameScope.Get<Border>(PART_VOLUME_BUTTON);
            mainGrid = (Control)e.NameScope.Get<Grid>(PART_MAIN_GRID);
            volumeSlider = e.NameScope.Get<Slider>(PART_VOLUME_SLIDER);

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
            volumePopup.Margin = new Thickness(position.X, 0, 0, (mainGrid.Bounds.Height - position.Y) );
        }

        private void OnHideClick(object sender, PointerPressedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnExpandClick(object sender, PointerPressedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;   
            }
        }

        private void OnCloseClick(object sender, PointerPressedEventArgs e)
        {
            if(Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
                desktopLifetime.Shutdown();
        }
        #endregion


        #region NavBar Pressed Methods
        private void SwitchContext(string vmName)
        {
            var vm = (BaseViewModel)App.ViewModelTable[vmName];

            if (vm != null)
            {
                Context.CurrentVM = vm;   
            }
        }
        
        private void OnHomePointerPressed(object? sender, PointerPressedEventArgs e) =>
            SwitchContext(StartViewModel.nameVM);

        private void OnCollectsPointerPressed(object? sender, PointerPressedEventArgs e) =>
            SwitchContext(ListViewModel.nameVM);

        private void OnSettingPointerPressed(object? sender, PointerPressedEventArgs e) =>
            SwitchContext(SettingViewModel.nameVM);

        private void VolumePopupDown(object? sender, PointerPressedEventArgs e) => 
            ((MainViewModel)DataContext).VolumeSliderShowCommand.Execute(null);
        #endregion
    }
}
