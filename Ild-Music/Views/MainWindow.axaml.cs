using Avalonia;
using Avalonia.Media;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Interactivity;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.ApplicationLifetimes;
using Ild_Music.ViewModels;
using Ild_Music.ViewModels.Base;

using System;
using System.Diagnostics;
using PropertyChanged;

namespace Ild_Music.Views
{
    [DoNotNotifyAttribute]
    public partial class MainWindow : Window
    {
        #region Parts
        private const string PART_TITLEBAR = "DragBar";
        private const string PART_NAVBAR = "NavBar";

        private Grid? titleBar;
        private Grid? navBar;
        #endregion


        public MainViewModel Context {get; private set;}

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

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);
            titleBar = e.NameScope.Get<Grid>(PART_TITLEBAR);
            navBar = e.NameScope.Get<Grid>(PART_NAVBAR);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            if (Equals(e.Source, titleBar))
                BeginMoveDrag(e);
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
        
        private void OnHomePointerPressed(object? sender, PointerPressedEventArgs e)
        {
            SwitchContext(StartViewModel.nameVM);
        }

        private void OnCollectsPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            SwitchContext(ListViewModel.nameVM);
            // if (!context.CurrentVM.GetType().Equals(typeof(ListViewModel)))
            //     context.CurrentVM = new ListViewModel();
        }

        private void OnSettingPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            SwitchContext(SettingViewModel.nameVM);
            // if (!context.CurrentVM.GetType().Equals(typeof(SettingViewModel)))
            //     context.CurrentVM = new SettingViewModel();   
        }
        #endregion
    }
}