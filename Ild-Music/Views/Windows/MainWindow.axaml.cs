using Ild_Music.ViewModels;
using Ild_Music.ViewModels.Base;

using System;
using PropertyChanged;
using Avalonia;
using Avalonia.Media;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.Interactivity;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.ApplicationLifetimes;


namespace Ild_Music.Views;

[DoNotNotifyAttribute]
public partial class MainWindow : Window
{
    private const string PART_TITLEBAR = "DragBar";
    private const string PART_NAVBAR = "NavBar";
    private const string PART_VOLUME_AREA = "VolumeArea";
    private const string PART_VOLUME_BUTTON = "VolumeButton";
    private const string PART_SEARCH_AREA = "SearchArea";
    private const string PART_SEARCH_BAR = "SearchBar";
    private const string PART_SEARCH_BOX = "SearchBox";
    private const string PART_CURRENT_INSTANCE_TRAY = "CurrentInstanceTray";
    private const string PART_CURRENT_INSTANCE_AREA = "CurrentInstanceArea";
    private const string PART_MAIN_GRID = "MainGrid";
    private const string PART_VOLUME_SLIDER = "VolumeSlider";
    private const string PART_TIME_SLIDER = "sldThumby";
    private const string PART_TIME_SLIDER_THUMB = "timeThumber";

    private Grid? titleBar;
    private Grid? navBar;

    private Control volumePopup;
    private Control volumeButton;
    private Slider volumeSlider;

    private Control searchPopup;
    private Control searchBar;
    private TextBox searchBox;

    private Control currentInstancePopup;
    private Grid currentInstanceTray;

    private Control mainGrid;

    public static MainWindowViewModel Context {get; private set;}

    public MainWindow()
    {
        InitializeComponent();
    }

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
        volumeSlider = e.NameScope.Get<Slider>(PART_VOLUME_SLIDER);

        searchPopup = (Control)e.NameScope.Get<Border>(PART_SEARCH_AREA);
        searchBar = (Control)e.NameScope.Get<Border>(PART_SEARCH_BAR);
        searchBox= e.NameScope.Get<TextBox>(PART_SEARCH_BOX);
        
        currentInstancePopup = (Control)e.NameScope.Get<Border>(PART_CURRENT_INSTANCE_AREA);
        currentInstanceTray = e.NameScope.Get<Grid>(PART_CURRENT_INSTANCE_TRAY);

        mainGrid = (Control)e.NameScope.Get<Grid>(PART_MAIN_GRID);
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

        var voulumeButtonPosition = volumeButton.TranslatePoint(new Point(), mainGrid) ??
            throw new Exception("Cannot get TranslatePoint");

        var searchBarPosition = searchBar.TranslatePoint(new Point(), mainGrid) ??
            throw new Exception("Cannot get TranslatePoint");

        var currentInstanceTrayPosition = currentInstanceTray.TranslatePoint(new Point(), mainGrid) ??
            throw new Exception("Cannot get TranslatePoint");

        var volumePopupGap = (mainGrid.Bounds.Height - voulumeButtonPosition.Y);
        var searchPopupGap = (mainGrid.Bounds.Height - (searchBarPosition.Y + searchBar.Bounds.Height + searchPopup.Bounds.Height));
        var currentInstancePopupGap = (mainGrid.Bounds.Height - currentInstanceTrayPosition.Y) + 10;

        
        Dispatcher.UIThread.Post( () => 
        {
            volumePopup.Margin = new Thickness(voulumeButtonPosition.X, 0, 0, volumePopupGap);
            searchPopup.Margin = new Thickness(searchBarPosition.X, 0, 0, searchPopupGap);
            currentInstancePopup.Margin = new Thickness(mainGrid.Bounds.Width - currentInstancePopup.Width, 0, 0, currentInstancePopupGap);
        });
    }

    private void OnHideClick(object sender, PointerPressedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void OnExpandClick(object sender, PointerPressedEventArgs e)
    {
        if (WindowState == WindowState.Normal)
            WindowState = WindowState.Maximized;
        else if (WindowState == WindowState.Maximized)
            WindowState = WindowState.Normal;
    }

    private void OnCloseClick(object sender, PointerPressedEventArgs e)
    {
        if(Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            desktopLifetime.Shutdown();
    }

    private void VolumePopupDown(object? sender, PointerPressedEventArgs e)
    {
        ((MainWindowViewModel)DataContext).VolumeSliderShowCommand.Execute(null);
    }

    private void HomeNavBarClicked(object? sender, PointerPressedEventArgs e)
    {
        var viewModel = (MainWindowViewModel)DataContext;        
        viewModel.DefineNewPresentItem(StartViewModel.viewModelId);
    }
    
    private void ListNavBarClicked(object? sender, PointerPressedEventArgs e)
    {
        var viewModel = (MainWindowViewModel)DataContext;
        viewModel.DefineNewPresentItem(ListViewModel.viewModelId);
    }
    
    private void BrowseNavBarClicked(object? sender, PointerPressedEventArgs e)
    {
        var viewModel = (MainWindowViewModel)DataContext;
        viewModel.DefineNewPresentItem(BrowserViewModel.viewModelId);
    }

    private void SearchBarTyped(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            ((MainWindowViewModel)DataContext).SearchCommand.Execute(null);
        else if (e.Key == Key.Up)
            ((MainWindowViewModel)DataContext).SearchItemUp();
        else if (e.Key == Key.Down)
            ((MainWindowViewModel)DataContext).SearchItemDown();
        else if (e.Key == Key.Escape)
            ((MainWindowViewModel)DataContext).SearchAreaHideCommand.Execute(null);
    }
}
